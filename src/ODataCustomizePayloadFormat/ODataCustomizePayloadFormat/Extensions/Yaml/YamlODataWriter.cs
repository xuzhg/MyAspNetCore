//---------------------------------------------------------------------
// <copyright file="YamlODataWriter.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using Microsoft.AspNetCore.OData.Formatter.Wrapper;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace ODataCustomizePayloadFormat.Extensions.Yaml;

public class YamlODataWriter : CustomizedWriter
{
    protected const string IndentationString = "  ";

    public YamlODataWriter(CustomizedOutputContext context, IEdmStructuredType structuredType)
        : base(context, structuredType)
    {
    }

    protected override void WriteItems()
    {
        if (TopLevelItem == null)
        {
            return;
        }

        // For simplicity, NOT consider the inheritance
        if (TopLevelItem is ODataResourceSetWrapper topResourceSet)
        {
            WriteResourceSet(0, topResourceSet);
        }
        else if (TopLevelItem is ODataResourceWrapper topResource)
        {
            WriteResource(0, topResource);
        }
    }

    /// <summary>
    /// When coming into this method, the cursor is stop at the first place to write the resource set.
    /// </summary>
    private void WriteResourceSet(int indentLevel, ODataResourceSetWrapper resourceSetWrapper)
    {
        if (resourceSetWrapper.Resources.Count == 0)
        {
            Context.Writer.WriteLine("[ ]"); // write the empty
            return;
        }

        // @odata.count: 4  if we have the count
        if (resourceSetWrapper.ResourceSet.Count != null)
        {
            Context.Writer.Write("@odata.count: ");
            Context.Writer.WriteLine(resourceSetWrapper.ResourceSet.Count.Value);
        }

        bool first = true;
        foreach (var resource in resourceSetWrapper.Resources)
        {
            if (!first)
            {
                Context.Writer.WriteLine();
                WriteIndentation(indentLevel);
            }
            else
            {
                first = false;
            }

            Context.Writer.Write("- ");
            WriteResource(indentLevel + 1, resource);
        }
    }

    /// <summary>
    /// When coming into this method, the cursor is stop at the first place to write the resource.
    /// </summary>
    private void WriteResource(int indentLevel, ODataResourceWrapper resourceWrapper)
    {
        bool first = true;
        foreach (var property in resourceWrapper.Resource.Properties)
        {
            if (!first)
            {
                Context.Writer.WriteLine();
                WriteIndentation(indentLevel);
            }
            else
            {
                first = false;
            }

            Context.Writer.Write($"{property.Name}: ");

            WriteValue(property.Value, indentLevel);
        }

        foreach (var property in resourceWrapper.NestedResourceInfos)
        {
            if (!first)
            {
                Context.Writer.WriteLine();
                WriteIndentation(indentLevel);
            }
            else
            {
                first = false;
            }

            Context.Writer.Write($"{property.NestedResourceInfo.Name}: ");

            WriteNestedProperty(indentLevel + 1, property);
        }

        Context.Writer.WriteLine();
    }

    private void WriteNestedProperty(int indentLevel, ODataNestedResourceInfoWrapper nestedResourceInfoWrapper)
    {
        foreach (ODataItemWrapper childItem in nestedResourceInfoWrapper.NestedItems)
        {
            ODataResourceSetWrapper resourceSetWrapper = childItem as ODataResourceSetWrapper;
            if (resourceSetWrapper != null)
            {
                // if it's empty collection, write [] and return;
                if (resourceSetWrapper.Resources.Count == 0)
                {
                    Context.Writer.WriteLine("[ ]"); // write the empty
                    continue;
                }

                Context.Writer.WriteLine();
                WriteIndentation(indentLevel);
                WriteResourceSet(indentLevel, resourceSetWrapper);
                continue;
            }

            ODataResourceWrapper resourceWrapper = childItem as ODataResourceWrapper;
            if (resourceWrapper != null)
            {
                Context.Writer.WriteLine();
                WriteIndentation(indentLevel);
                WriteResource(indentLevel, resourceWrapper);
            }
        }
    }

    protected void WriteValue(object value, int indentLevel)
    {
        Type valueType = value.GetType();
        valueType = Nullable.GetUnderlyingType(valueType) ?? valueType;

        if (value is ODataEnumValue enumValue)
        {
            Context.Writer.Write(enumValue.Value);
        }
        else if (value is ODataCollectionValue collectionValue)
        {
            if (collectionValue.Items.Count() == 0)
            {
                Context.Writer.Write("[ ]");
            }
            else
            {
                Context.Writer.WriteLine();
                bool first = true;
                foreach (var item in collectionValue.Items)
                {
                    if (!first)
                    {
                        Context.Writer.WriteLine();
                    }
                    else
                    {
                        first = false;
                    }

                    WriteIndentation(indentLevel + 1);
                    Context.Writer.Write("- ");
                    WriteValue(item, indentLevel + 1);
                }
            }
        }
        else if (valueType == typeof(int))
        {
            Context.Writer.Write((int)value);
        }
        else if (valueType == typeof(bool))
        {
            Context.Writer.Write((bool)value);
        }
        else if (valueType == typeof(string))
        {
            Context.Writer.Write(value.ToString());
        }
        else
        {
            throw new NotImplementedException("I don't have time to implement all. You can add more if you need more.");
        }
    }

    public virtual void WriteIndentation(int indentLevel)
    {
        for (var i = 0; i < indentLevel; i++)
        {
            Context.Writer.Write(IndentationString);
        }
    }
}
