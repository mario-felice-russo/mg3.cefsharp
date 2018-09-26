using CefSharp.ModelBinding;
using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace Bridges
{
    /*
    public class MethodInterceptorLogger : IMethodInterceptor
    {
        public object Intercept(Func<object> method, string methodName)
        {
            if (method.Method.GetParameters().Length > 0)
                foreach (ParameterInfo pi in method.Method.GetParameters())
                {
                    Console.WriteLine("{0}", pi.Name);
                }

            object result = method();
            Debug.WriteLine("MethodInterceptorLogger -> " + methodName);
            return result;
        }
    }
    */
}
