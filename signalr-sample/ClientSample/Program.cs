using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace ClientSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();

            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/itemsHub")
                .Build();

            connection.On("UpdateList", () => GetItems(client));
            
            connection.StartAsync().GetAwaiter().GetResult();

            Console.WriteLine("Listening on itemsHub, press ENTER to exit.");
            Console.Read();
        }

        private static void GetItems(HttpClient client)
        {
            var response = client.GetAsync("https://localhost:5001/Items").GetAwaiter().GetResult();
            var list = DeserializeResponse<List<string>>(response);

            Console.WriteLine("Updated list: ");
            list.ForEach(i => Console.WriteLine($"- {i}"));
            Console.WriteLine();
        }
        
        private static T DeserializeResponse<T>(HttpResponseMessage response)
        {
            var responseAsString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<T>(responseAsString);
        }
    }
}