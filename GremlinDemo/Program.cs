using System;
using System.Collections.Generic;
using Gremlin.Net.Driver;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;

namespace GremlinDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostname = "";
            var port = 443;
            string authKey = "";
            string database = "";
            string collection = "";

            var gremlinServer = new GremlinServer(hostname, port, enableSsl: true,
                username: "/dbs/" + database + "/colls/" + collection,
                password: authKey);

            using (var gremlinClient = new GremlinClient(gremlinServer, new GraphSON2Reader()
                , new GraphSON2Writer(), GremlinClient.GraphSON2MimeType))
            {
                var gremlinQueries = new List<string>
                {
                    "g.V().drop()",
                    "g.addV('person').property('id', 'P1').property('Name', 'Dinesh').property('email', 'dinesh@paidpiper.com').property('pk', 'pk')",
                    "g.addV('person').property('id', 'P2').property('Name', 'Gilfoyle').property('email', 'dinesh@paidpiper.com').property('pk', 'pk')",
                    "g.V('P1').addE('knows').to(g.V('P2'))",
                    "g.addV('Employer').property('id', 'E1').property('Name', 'Pied Piper').property('Founded Amount', '$50K').property('pk', 'pk')",
                    "g.V('P1').addE('EmployerBy').to(g.V('E1'))",
                    "g.V('P2').addE('EmployerBy').to(g.V('E1'))",
                };
                foreach (var query in gremlinQueries)
                {
                    var resultSet = gremlinClient.SubmitAsync<dynamic>(query).Result;
                    if (resultSet.Count > 0)
                    {
                        Console.WriteLine("Result:");
                        foreach (var result in resultSet)
                        {
                            string output = JsonConvert.SerializeObject(result);
                            Console.WriteLine($"{output}");
                        }
                        Console.WriteLine();
                    }
                }

                var countQuery = "g.V('P1').out('knows').haslabel('person').count()";
                var resultCount = gremlinClient.SubmitAsync<dynamic>(countQuery).Result;
                if (resultCount.Count > 0)
                {
                    Console.WriteLine("Result:");
                    foreach (var result in resultCount)
                    {
                        string output = JsonConvert.SerializeObject(result);
                        Console.WriteLine($"{output}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
