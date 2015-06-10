using System;
using System.Collections.Generic;
using System.Text;

namespace IftarUniversal.Service
{
    public class HelloService : IHelloService
    { 
        public string SayHello()
        {
            return "Hello World";
        }
    }
}
