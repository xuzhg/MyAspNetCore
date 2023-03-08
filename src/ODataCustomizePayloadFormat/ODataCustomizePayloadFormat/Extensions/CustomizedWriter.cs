//---------------------------------------------------------------------
// <copyright file="CustomizedWriterBase.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.AspNetCore.OData.Formatter.Wrapper;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace ODataCustomizePayloadFormat.Extensions;

public abstract class CustomizedWriter : ODataWriter
{
    private ODataItemWrapper _topLevelItem = null;
    private Stack<ODataItemWrapper> _itemsStack = new Stack<ODataItemWrapper>();

    protected CustomizedWriter(CustomizedOutputContext context, IEdmStructuredType structuredType)
    {
        Context = context;
        StructuredType = structuredType;
    }

    protected ODataItemWrapper TopLevelItem => _topLevelItem;

    protected Stack<ODataItemWrapper> ItemsStack => _itemsStack;

    protected IEdmStructuredType StructuredType { get; }

    protected CustomizedOutputContext Context { get; }

    public override void Flush()
    {
        Context.Flush();
    }

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

    protected abstract void WriteItems();

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

    #region Synchronization not used
    public override void WriteStart(ODataResource resource) => throw new NotImplementedException();
    public override void WriteStart(ODataResourceSet resourceSet) => throw new NotImplementedException();
    public override void WriteStart(ODataNestedResourceInfo nestedResourceInfo) => throw new NotImplementedException();
    public override void WriteEntityReferenceLink(ODataEntityReferenceLink entityReferenceLink) => throw new NotImplementedException();
    #endregion
}
