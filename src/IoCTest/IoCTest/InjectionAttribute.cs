using System;
using System.Collections.Generic;
using System.Text;

namespace IoCTest
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class InjectionAttribute : Attribute
    {
    }
}
