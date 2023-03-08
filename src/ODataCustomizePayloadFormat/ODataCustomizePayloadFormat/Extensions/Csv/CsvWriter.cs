//---------------------------------------------------------------------
// <copyright file="CsvWriter.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using System.Text;
using Microsoft.AspNetCore.OData.Formatter.Wrapper;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace ODataCustomizePayloadFormat.Extensions.Csv;

public class CsvWriter : ODataWriter
{
    private ODataItemWrapper _topLevelItem = null;
    private Stack<ODataItemWrapper> _itemsStack = new Stack<ODataItemWrapper>();

    private CsvOutputContext _context;

    public CsvWriter(CsvOutputContext context, IEdmStructuredType structuredType)
    {
        _context = context;
    }

    public override void Flush() => _context.Flush();

    public override void WriteEnd()
    {
        _itemsStack.Pop();

        if (_itemsStack.Count == 0)
        {
            // we finished the process, let's write the value into stream
            WriteItems();
            Flush();
        }
    }

    public override Task WriteStartAsync(ODataResourceSet resourceSet)
    {
        ODataResourceSetWrapper resourceSetWrapper = new ODataResourceSetWrapper(resourceSet);

        if (_topLevelItem == null)
        {
            _topLevelItem = resourceSetWrapper;
        }
        else
        {
            // It must be under nested resource info
            ODataNestedResourceInfoWrapper parentNestedResourceInfo = (ODataNestedResourceInfoWrapper)_itemsStack.Peek();
            parentNestedResourceInfo.NestedItems.Add(resourceSetWrapper);
        }

        _itemsStack.Push(resourceSetWrapper);

        return Task.CompletedTask;
    }

    public override Task WriteStartAsync(ODataResource resource)
    {
        ODataResourceWrapper resourceWrapper = new ODataResourceWrapper(resource);
        if (_topLevelItem == null)
        {
            _topLevelItem = resourceWrapper;
        }
        else
        {
            ODataItemWrapper parentItem = _itemsStack.Peek();

            if (parentItem is ODataResourceSetWrapper parentResourceSet)
            {
                parentResourceSet.Resources.Add(resourceWrapper);
            }
            else
            {
                ODataNestedResourceInfoWrapper parentNestedResource = (ODataNestedResourceInfoWrapper)parentItem;
                parentNestedResource.NestedItems.Add(resourceWrapper);
            }
        }

        _itemsStack.Push(resourceWrapper);
        return Task.CompletedTask;
    }

    public override Task WriteStartAsync(ODataNestedResourceInfo nestedResourceInfo)
    {
        ODataNestedResourceInfoWrapper nestedResourceInfoWrapper = new ODataNestedResourceInfoWrapper(nestedResourceInfo);
        ODataResourceWrapper parentResource = (ODataResourceWrapper)_itemsStack.Peek();
        parentResource.NestedResourceInfos.Add(nestedResourceInfoWrapper);
        _itemsStack.Push(nestedResourceInfoWrapper);
        return Task.CompletedTask;
    }

    private void WriteItems()
    {
        if (_topLevelItem == null)
        {
            return;
        }

        IList<string> headers = null;
        ODataResourceSetWrapper topResourceSet = _topLevelItem as ODataResourceSetWrapper;
        ODataResourceWrapper topResource = _topLevelItem as ODataResourceWrapper;

        // For simplicity, NOT consider the inheritance
        if (topResourceSet != null)
        {
            ODataResourceWrapper firstResource = topResourceSet.Resources.FirstOrDefault();
            headers = BuildHeaders(firstResource);
        }
        else if (topResource != null)
        {
            headers = BuildHeaders(topResource);
        }

        if (headers == null)
        {
            return;
        }

        // write the head
        WriteHeader(headers);

        if (topResourceSet != null)
        {
            foreach (var resource in topResourceSet.Resources)
            {
                WriteResource(headers, resource);
            }
        }
        else if (topResource != null)
        {
            WriteResource(headers, topResource);
        }
    }

    private static IList<string> BuildHeaders(ODataResourceWrapper topResource)
    {
        if (topResource == null)
        {
            return null;
        }

        IList<string> headers = new List<string>();
        foreach (var property in topResource.Resource.Properties)
        {
            headers.Add(property.Name);
        }

        foreach (var nestedProperty in topResource.NestedResourceInfos)
        {
            headers.Add(nestedProperty.NestedResourceInfo.Name);
        }

        return headers;
    }

    private void WriteHeader(IList<string> headers)
    {
        int index = 0;
        foreach (var header in headers)
        {
            if (index == 0)
            {
                index = 1;
                _context.Writer.Write("{0}", header);
            }
            else
            {
                _context.Writer.Write(",{0}", header);
            }
        }

        _context.Writer.WriteLine();
    }

    private void WriteResource(IList<string> headers, ODataResourceWrapper resource)
    {
        int index = 0;
        foreach (var header in headers)
        {
            if (index != 0)
            {
                _context.Writer.Write(",");
            }
            ++index;

            string propertyName = header;
            ODataProperty property = resource.Resource.Properties.SingleOrDefault(p => p.Name == propertyName);
            if (property != null)
            {
                string propertyValueString = GetValueString(property.Value);
                _context.Writer.Write(propertyValueString);
                continue;
            }

            ODataNestedResourceInfoWrapper nestedResourceInfoWrapper = resource.NestedResourceInfos.SingleOrDefault(n => n.NestedResourceInfo.Name == propertyName);
            if (nestedResourceInfoWrapper != null)
            {
                StringBuilder sb = new StringBuilder();
                WriteResourceString(sb, nestedResourceInfoWrapper);
                _context.Writer.Write("\"");
                _context.Writer.Write(sb.ToString());
                _context.Writer.Write("\"");
                continue;
            }
        }

        _context.Writer.WriteLine();
    }

    private string GetValueString(object value)
    {
        if (value is ODataEnumValue enumValue)
        {
            return enumValue.Value;
        }
        else if (value is ODataCollectionValue collectionValue)
        {
            StringBuilder sb = new StringBuilder("\"[");
            int index = 0;
            foreach (var item in collectionValue.Items)
            {
                string itemStr = GetValueString(item);
                if (index != 0)
                {
                    sb.Append(",");
                }

                sb.Append(itemStr);
                ++index;
            }
            sb.Append("]\"");

            return sb.ToString();
        }
        else
        {
            return value.ToString();
        }
    }

    private void WriteResourceString(StringBuilder sb, ODataNestedResourceInfoWrapper nestedResourceInfoWrapper)
    {
        foreach (ODataItemWrapper childItem in nestedResourceInfoWrapper.NestedItems)
        {
            ODataResourceSetWrapper resourceSetWrapper = childItem as ODataResourceSetWrapper;
            if (resourceSetWrapper != null)
            {
                sb.Append("[");
                int index = 0;
                foreach (var resource in resourceSetWrapper.Resources)
                {
                    if (index != 0)
                    {
                        sb.Append(",");
                    }
                    ++index;
                    WriteResourceString(sb, resource);
                }
                sb.Append("]");

                continue;
            }

            ODataResourceWrapper resourceWrapper = childItem as ODataResourceWrapper;
            if (resourceWrapper != null)
            {
                WriteResourceString(sb, resourceWrapper);
            }
        }
    }

    private void WriteResourceString(StringBuilder sb, ODataResourceWrapper resourceWrapper)
    {
        sb.Append("{");
        int index = 0;
        foreach (var property in resourceWrapper.Resource.Properties)
        {
            if (index != 0)
            {
                sb.Append(",");
            }
            ++index;
            sb.Append(property.Name);
            sb.Append("=");
            sb.Append(GetValueString(property.Value));
        }

        foreach (var nestedProperty in resourceWrapper.NestedResourceInfos)
        {
            sb.Append(nestedProperty.NestedResourceInfo.Name);
            sb.Append("=");
            WriteResourceString(sb, nestedProperty);
        }

        sb.Append("}");
    }

    #region Synchronization not used
    public override void WriteStart(ODataResource resource)
        => throw new NotImplementedException();
    public override void WriteStart(ODataResourceSet resourceSet)
        => throw new NotImplementedException();
    public override void WriteStart(ODataNestedResourceInfo nestedResourceInfo)
        => throw new NotImplementedException();
    public override void WriteEntityReferenceLink(ODataEntityReferenceLink entityReferenceLink)
        => throw new NotImplementedException();
    #endregion
}
