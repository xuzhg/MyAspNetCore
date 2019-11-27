using System;
using System.Collections.Generic;
using System.Text;

namespace IoCTest
{
    [AttributeUsage(AttributeTargets.Constructor |
                 AttributeTargets.Property |
                 AttributeTargets.Method,
                 AllowMultiple = false)]
    public class InjectionAttribute : Attribute
    {
    }
}
