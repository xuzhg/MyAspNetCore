//-----------------------------------------------------------------------------
// <copyright file="Person.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

namespace UntypedApp.Models;

public class Person
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Gender Gender { get; set; }

    public object Data { get; set; } //=> Edm.Untyped

    public IList<object> Infos { get; set; } = new List<object>(); // Collection(Edm.Untyped)

    public IDictionary<string, object> DynamicContainer { get; set; } // dynamic property container

   // public Address HomeAddress { get; set; }
}
