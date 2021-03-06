using Newtonsoft.Json.Linq;
using Refinitiv.DataPlatform.Core;
using Refinitiv.DataPlatform.Delivery;
using Refinitiv.DataPlatform.Delivery.Request;
using System;

namespace _3._2._03_Endpoint_Symbology
{
    // **********************************************************************************************************************
    // 3.2.03-Endpoint-Symbology
    // The following example demonstrates the Symbology API showcasing a wide variety of conversions.
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        const string symbolLookupEndpoint = "https://api.refinitiv.com/discovery/symbology/v1/lookup";

        static void Main(string[] args)
        {
            try
            {
                // Create the platform session.
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    var endpoint = DeliveryFactory.CreateEndpoint(new Endpoint.Params().Session(session)
                                                                                     .Url(symbolLookupEndpoint));

                    // RIC to multiple identifiers
                    Console.WriteLine("\nRIC to multiple identifiers...");
                    Display(endpoint.SendRequest(new Endpoint.Request.Params().WithMethod(Endpoint.Request.Method.POST)
                                                                              .WithBodyParameters(new JObject()
                                                                              {
                                                                                  ["from"] = new JArray(
                                                                                                  new JObject()
                                                                                                  {
                                                                                                      ["identifierTypes"] = new JArray("RIC"),
                                                                                                      ["values"] = new JArray("MSFT.O")
                                                                                                  }),
                                                                                  ["to"] = new JArray(
                                                                                                  new JObject()
                                                                                                  {
                                                                                                      ["identifierTypes"] = new JArray("ISIN", "LEI", "ExchangeTicker")
                                                                                                  }),
                                                                                  ["type"] = "auto"
                                                                              })));

                    // Legal Entity Identifier (LEI) to multiple RICs
                    Console.WriteLine("LEI to multiple RICs...");
                    Display(endpoint.SendRequest(new Endpoint.Request.Params().WithMethod(Endpoint.Request.Method.POST)
                                                                              .WithBodyParameters(new JObject()
                                                                              {
                                                                                  ["from"] = new JArray(
                                                                                                  new JObject()
                                                                                                  {
                                                                                                      ["identifierTypes"] = new JArray("LEI"),
                                                                                                      ["values"] = new JArray("INR2EJN1ERAN0W5ZP974")
                                                                                                  }),
                                                                                  ["to"] = new JArray(
                                                                                                  new JObject()
                                                                                                  {
                                                                                                      ["identifierTypes"] = new JArray("RIC")
                                                                                                  }),
                                                                                  ["type"] = "auto"
                                                                              })));

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        private static void Display(IEndpointResponse response)
        {
            if (response.IsSuccess)
            {
                Console.WriteLine(response.Data.Raw);
            }
            else
            {
                Console.WriteLine(response.Status);
            }
            Console.Write("\nHit <enter> to continue...");
            Console.ReadLine();
        }
    }
}
