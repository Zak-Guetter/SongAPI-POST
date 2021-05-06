using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Amazon.Lambda.Core;
using System.Dynamic;
using Newtonsoft.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Assignment9
{
    public class Function
    {
        static readonly HttpClient client = new HttpClient();
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// 


        private static AmazonDynamoDBClient storageClient = new AmazonDynamoDBClient();
        private string tableName = "assignment11";

        public async Task<string> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            Table items = Table.LoadTable(storageClient, tableName);
            /**dynamic data = new ExpandoObject();
            Dictionary<string, string> dict = (Dictionary<string, string>)input.QueryStringParameters;
            string something = await client.GetStringAsync("https://api.nytimes.com/svc/books/v3/lists/current/" + dict.First().Value + ".json?api-key=iXIGuGPUAFHQ5FFAnHAdkrJJ0at4fkLV");
            dynamic objects = JsonConvert.DeserializeObject<ExpandoObject>(something);
            return objects;
            This is Assignment 9's code that gets data from the api*/
            //Task<ExpandoObject>


            Dictionary<string, string> dict = (Dictionary<string, string>)input.QueryStringParameters;
            string something = await client.GetStringAsync("https://api.lyrics.ovh/v1/" + dict.First().Key + "/" + dict.First().Value);
            dynamic objects = JsonConvert.DeserializeObject<ExpandoObject>(something);



                 
            
           
            Dictionary<string, AttributeValue> myDictionary = new Dictionary<string, AttributeValue>();
            myDictionary.Add("song", new AttributeValue() { S = dict.First().Value });
            myDictionary.Add("lyrics", new AttributeValue() { S = objects.lyrics });
            PutItemRequest myRequest = new PutItemRequest(tableName, myDictionary);
            PutItemResponse res = await storageClient.PutItemAsync(myRequest);
            

            return "Song: " + dict.First().Value + "\nLyrics: " + objects.lyrics;

        }

        
    }
}
