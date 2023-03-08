//---------------------------------------------------------------------
// <copyright file="CustomizedFormat.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//---------------------------------------------------------------------

namespace ODataCustomizePayloadFormat.Models;

public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Color FavoriteColor { get; set; }

    public IList<string> Tokens { get; set; }

    public int Amount { get; set; }

    public virtual Address HomeAddress { get; set; }

    public virtual IList<Address> FavoriteAddresses { get; set; }
}

public enum Color
{
    Red,
    Green,
    Blue
}