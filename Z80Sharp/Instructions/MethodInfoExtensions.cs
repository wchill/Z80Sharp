using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Z80Sharp.Instructions
{
    public static class MethodInfoExtensions
    {
        public static T[] GetAttributes<T>(this MethodInfo action) where T : Attribute
        {
            return action.GetCustomAttributes(true).OfType<T>().ToArray();
        }
    }
}
