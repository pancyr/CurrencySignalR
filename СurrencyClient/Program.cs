using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace СurrencyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chatHub").Build();
            
            connection.StartAsync().Wait();
            connection.On("ReceiveMessage", (string responseBody) => {
                Dictionary<string, Currency> root = JsonConvert.DeserializeObject<Dictionary<string, Currency>>(responseBody);
                Console.Clear();
                int i = 1;
                foreach (KeyValuePair<string, Currency> kvp in root)
                {
                    Console.Write(String.Format("{0} : {1}          ", kvp.Key, kvp.Value.Last));
                    if (i == 5)
                    {
                        Console.WriteLine();
                        i = 1;
                    }
                    else i++;
                }
            });

            Console.ReadKey();
        }
    }
}
