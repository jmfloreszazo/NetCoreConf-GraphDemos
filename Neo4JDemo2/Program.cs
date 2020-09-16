using System;
using Neo2JDemo2Library;

namespace Neo4JDemo2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var greeter = new HelloWorldExample("bolt://localhost:7687", "neo4j", "12345"))
            {
                greeter.PrintGreeting("hello, world");
            }
        }
    }
}
