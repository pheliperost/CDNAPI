using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace XUnitTestCDNAPI.Utils
{
    public static class PrivateMethodHelper
    {
        public static object InvokePrivateMethod(this object instance, string methodName, params object[] parameters)
        {
            var methodInfo = instance.GetType().GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            return methodInfo.Invoke(instance, parameters);
        }
    }
}
