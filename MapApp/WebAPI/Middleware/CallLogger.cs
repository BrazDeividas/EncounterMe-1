﻿using Castle.DynamicProxy;
using System.IO;
using System.Linq;

namespace WebAPI.Middleware
{
    public class CallLogger : IInterceptor
    {
        TextWriter _output;

        //Temporary debug class
        public CallLogger(TextWriter output)
        {
            _output = output;
        }

        public void Intercept(IInvocation invocation)
        {
            _output.Write("Calling method {0} with parameters {1}... ",
              invocation.Method.Name,
              string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            invocation.Proceed();

            _output.WriteLine("Done: result was {0}.", invocation.ReturnValue);
        }
    }
}
