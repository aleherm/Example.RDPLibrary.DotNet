using Refinitiv.DataPlatform.Content.Pricing;
using Refinitiv.DataPlatform.Core;
using Refinitiv.DataPlatform.Delivery.Stream;
using System;
using System.Threading;

namespace _2._2._02_Pricing_StreamingCache
{
    // **********************************************************************************************************************
    // 2.2.02-Pricing-StreamingCache
    // The following example demonstrates the basic usage of the Streaming Cache interfaces.  When defining a streaming cache,
    // users can optionally pull out live prices from the cache at their leisure.  The interface will automatically manage
    // streaming updates as market conditions change and keep the internal cache fresh.  
    //
    // Note: To configure settings for your environment, visit the following files within the .Solutions folder:
    //      1. Configuration.Session to specify the access channel into the platform. Default: RDP (PlatformSession).
    //      2. Configuration.Credentials to define your login credentials for the specified access channel.
    // **********************************************************************************************************************
    class Program
    {
        static void Main(string[] _)
        {
            try
            {
                // Create a session into the platform...
                using (ISession session = Configuration.Sessions.GetSession())
                {
                    // Open the session
                    session.Open();

                    // Create a streaming price interface for a list of instruments
                    using (var stream = StreamingPrices.Definition("EUR=", "CAD=", "GBP=").Fields("DSPLY_NAME", "BID", "ASK")
                                                                                          .OnStatus((o, item, status) => Console.WriteLine(status)))
                    {
                        if (stream.Open() == Stream.State.Opened)
                        {

                            // Retrieve a snapshot of the whole cache.  The interface also supports the ability to pull out specific items and fields.
                            var snapshot = stream.GetSnapshot();

                            // Print out the contents of the snapshot
                            foreach (var entry in snapshot)
                                DisplayPriceData(entry.Value);

                            // Print out values directly within the live cache
                            Console.WriteLine($"\nDirect cache access => cache[CAD=][ASK] = {stream["CAD="]["ASK"]}");

                            // Pull out a reference to a live item...
                            Console.WriteLine("\nShow change in a live cache item.");
                            var item = stream["GBP="];

                            // Display the change in values from the live cached item...
                            int iterations = 5;
                            for (var i = 0; i < iterations; i++)
                            {
                                Console.WriteLine($"\n{iterations-i} iterations remaining.  Sleeping for 5 seconds...");
                                Thread.Sleep(5000);
                                DisplayPriceData(item);
                            }

                            // Close streams
                            Console.WriteLine("\nClosing open streams...");
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        private static void DisplayPriceData(IPriceData data)
        {
            if (data != null)
            {
                Console.WriteLine($"\nPrices for item: {data.ItemName}");

                // Print 1 field
                Console.WriteLine($"\n{data.ItemName}[DSPLY_NAME]: {data["DSPLY_NAME"]}");

                // Print the contents of one item
                Console.WriteLine($"{data.ItemName} contents: {data.Fields()}");
            }
            else
                Console.WriteLine("\n**********Error displaying price data**********");
        }
    }
}
