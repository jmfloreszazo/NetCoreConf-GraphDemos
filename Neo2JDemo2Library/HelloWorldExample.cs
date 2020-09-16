using System;
using System.Linq;
using Neo4j.Driver;

namespace Neo2JDemo2Library
{
    public class HelloWorldExample : IDisposable
    {
        private readonly IDriver _driver;

        public HelloWorldExample(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public void PrintGreeting(string message)
        {
            using (var session = _driver.Session())
            {
                var greeting = session.WriteTransaction(tx =>
                {
                    var result = tx.Run("CREATE (a:Greeting) " +
                                        "SET a.message = $message " +
                                        "RETURN a.message + ', from node ' + id(a)",
                        new { message });
                    return result.Single()[0].As<string>();
                });
                Console.WriteLine(greeting);
            }
        }

        public void Dispose()
        {
            _driver?.Dispose();
        }

        public static void Main()
        {

        }
    }
}
