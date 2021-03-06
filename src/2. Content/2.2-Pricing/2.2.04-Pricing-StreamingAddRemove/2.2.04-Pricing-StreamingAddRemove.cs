using Refinitiv.DataPlatform.Content.Pricing;
using Refinitiv.DataPlatform.Core;
using Refinitiv.DataPlatform.Delivery.Stream;
using System;

namespace _2._2._04_Pricing_StreamingAddRemove
{
    // **********************************************************************************************************************
    // 2.2.04-Pricing-StreamingAddRemove
    // The following example demonstrates how add to or remove items from your streaming cache.  Items added will be
    // automatically opened if the stream is already opened.
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

                            // Dump the cache to show the current items we're watching
                            DumpCache(stream);

                            // Add 2 new currencies...
                            stream.AddItems("JPY=", "MXN=");

                            // Dump cache again...
                            DumpCache(stream);

                            // Remove 2 different currencies...
                            stream.RemoveItems("CAD=", "GBP=");

                            // Final dump
                            DumpCache(stream);

                            // Close streams
                            Console.WriteLine("\nClosing opened streams...");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        private static void DumpCache(IStreamingPricesRequest stream)
        {
            Console.WriteLine("\n*************************************Current cached items**********************************");

            foreach (var entry in stream)
                Console.WriteLine($"{entry.Key}: {entry.Value["DSPLY_NAME"]}");
        }
    }
}
