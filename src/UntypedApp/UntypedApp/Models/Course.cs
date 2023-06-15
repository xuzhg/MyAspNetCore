//-----------------------------------------------------------------------------
// <copyright file="Course.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

namespace UntypedApp.Models;

public class Course
{
    public string Title { get; set; }

    private string AliasName { get => Title + "_alias"; }

    public IList<int> Credits { get; set; }
}
