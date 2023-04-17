//---------------------------------------------------------------------
// <copyright file="ProtobufWriter.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using Bookstores;
using Google.Protobuf;
using Microsoft.AspNetCore.OData.Formatter.Wrapper;
using Microsoft.OData;
using Microsoft.OData.Edm;

namespace ODataProtobufExample.Protobuf;

public class ProtobufWriter : ODataWriter
{
    private ODataItemWrapper _topLevelItem = null;
    private Stack<ODataItemWrapper> _itemsStack = new Stack<ODataItemWrapper>();

    private ProtobufOutputContext _context;

    public ProtobufWriter(ProtobufOutputContext context, IEdmStructuredType structuredType)
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

      //  Shelf shelf = new Shelf();
      //  shelf.WriteTo()
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

        // For simplicity, NOT consider the inheritance
        if (_topLevelItem is ODataResourceSetWrapper topResourceSet)
        {
            WriteldResourceSet(topResourceSet);
        }
        else if (_topLevelItem is ODataResourceWrapper topResource)
        {
            WriteResource(topResource);
        }
    }

    private void WriteldResourceSet(ODataResourceSetWrapper resourceSetWrapper)
    {
        if (resourceSetWrapper.ResourceSet.TypeName.Contains("Shelf"))
        {
            IList<Shelf> shelves = BuildShelves(resourceSetWrapper);
            foreach (var shelf in shelves)
            {
                shelf.WriteTo(_context.MessageStream);
            }
        }
        else
        {
            IList<Book> books = BuildBooks(resourceSetWrapper);
            foreach (Book book in books)
            {
                book.WriteTo(_context.MessageStream);
            }
        }
    }

    private void WriteResource(ODataResourceWrapper resourceWrapper)
    {
        if (resourceWrapper.Resource.TypeName.Contains("Shelf"))
        {
            Shelf shelf = BuildShelf(resourceWrapper);
            shelf.WriteTo(_context.MessageStream);
        }
        else
        {
            Book book = BuildBook(resourceWrapper);
            book.WriteTo(_context.MessageStream);
        }
    }

    /*
    private static MethodInfo GetMethodInfo(string typeName)
    {
        // just for simplicity using the following implementation
        if (typeName.Contains("Collection"))
        {
            // collection
            if (typeName.Contains("Shelf"))
            {
                return typeof(ProtobufWriter).GetMethod("BuildResourceSet").MakeGenericMethod(typeof(Shelf));
            }
            else
            {
                return typeof(ProtobufWriter).GetMethod("BuildResourceSet").MakeGenericMethod(typeof(Book));
            }
        }
        else
        {
            if (typeName.Contains("Shelf"))
            {
                return typeof(ProtobufWriter).GetMethod("BuildResource").MakeGenericMethod(typeof(Shelf));
            }
            else
            {
                return typeof(ProtobufWriter).GetMethod("BuildResource").MakeGenericMethod(typeof(Book));
            }
        }
    }*/

    /// <summary>
    /// When coming into this method, the cursor is stop at the first place to write the resource set.
    /// </summary>
    /*
    private IList<T> BuildResourceSet<T>(ODataResourceSetWrapper resourceSetWrapper)
    {
        Type listType = typeof(List<>).MakeGenericType(typeof(T));

        IList<T> list = Activator.CreateInstance(listType) as IList<T>;

        foreach (var resource in resourceSetWrapper.Resources)
        {
            MethodInfo methodInfo = GetMethodInfo(resource.Resource.TypeName);

            T item = (T)methodInfo.Invoke(this, new object[] { resource });

            list.Add(item);
        }

        return list;
    }*/

    private IList<Shelf> BuildShelves(ODataResourceSetWrapper resourceSetWrapper)
    {
        IList<Shelf> shelves = new List<Shelf>();
        foreach (var resource in resourceSetWrapper.Resources)
        {
            shelves.Add(BuildShelf(resource));
        }

        return shelves;
    }

    private IList<Book> BuildBooks(ODataResourceSetWrapper resourceSetWrapper)
    {
        IList<Book> books = new List<Book>();
        foreach (var resource in resourceSetWrapper.Resources)
        {
            books.Add(BuildBook(resource));
        }

        return books;
    }

    private Shelf BuildShelf(ODataResourceWrapper resourceWrapper)
    {
        Shelf shelf = new Shelf();
        Type resourceType = typeof(Shelf);
        foreach (var property in resourceWrapper.Resource.Properties)
        {
            PropertyInfo propertyInfo = resourceType.GetProperty(property.Name);

            object propertyValue = GetPropertyValue(property.Value);

            propertyInfo.SetValue(shelf, propertyValue);
        }

        foreach (var nestedProperty in resourceWrapper.NestedResourceInfos)
        {
            if (nestedProperty.NestedResourceInfo.Name == "Books")
            {
                IList<Book> books = BuildNestedBooks(nestedProperty);
                if (books != null)
                {
                    foreach (var book in books)
                    {
                        shelf.Books.Add(book);
                    }
                }
            }
        }

        return shelf;
    }

    private Book BuildBook(ODataResourceWrapper resourceWrapper)
    {
        Book book = new Book();
        Type resourceType = typeof(Book);
        foreach (var property in resourceWrapper.Resource.Properties)
        {
            PropertyInfo propertyInfo = resourceType.GetProperty(property.Name);

            object propertyValue = GetPropertyValue(property.Value);

            propertyInfo.SetValue(book, propertyValue);
        }

        // so far, we don't have book nested property

        return book;
    }

    private IList<Book> BuildNestedBooks(ODataNestedResourceInfoWrapper nestedResourceInfoWrapper)
    {
        foreach (ODataItemWrapper childItem in nestedResourceInfoWrapper.NestedItems)
        {
            ODataResourceSetWrapper resourceSetWrapper = childItem as ODataResourceSetWrapper;
            if (resourceSetWrapper != null)
            {
                return BuildBooks(resourceSetWrapper);
            }

            //ODataResourceWrapper resourceWrapper = childItem as ODataResourceWrapper;
            //if (resourceWrapper != null)
            //{
            //    books.Add(BuildBook(resourceWrapper));
            //}
        }

        return null;
    }

    protected object GetPropertyValue(object value)
    {
        Type valueType = value.GetType();
        valueType = Nullable.GetUnderlyingType(valueType) ?? valueType;

        if (value is ODataEnumValue enumValue)
        {
            // Context.Writer.Write(enumValue.Value);
            ;
        }
        else if (value is ODataCollectionValue collectionValue)
        {
            ;
        }
        else if (valueType == typeof(int))
        {
            return (int)value;
        }
        else if (valueType == typeof(long))
        {
            return (long)value;
        }
        else if (valueType == typeof(bool))
        {
            return (bool)value;
        }
        else if (valueType == typeof(string))
        {
            return value.ToString();
        }

        throw new NotImplementedException("I don't have time to implement all. You can add more if you need more.");
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
