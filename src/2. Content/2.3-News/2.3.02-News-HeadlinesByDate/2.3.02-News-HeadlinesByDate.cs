using Refinitiv.DataPlatform.Content.News;
using Refinitiv.DataPlatform.Core;
using System;
using System.Linq;

namespace _2._3._02_News_HeadlinesByDate
{
    // **********************************************************************************************************************
    // 2.3.02-News-HeadlinesByDate
    // The News interfaces provide options to query for headlines within a specified time period.  The following example 
    // demonstrates the behavior and how to retrieve headlines based on a time period.
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
                    if (session.Open() == Session.State.Opened)
                    {
                        // ***************************************************************************************************************
                        // Note: Each request below specifies a count of zero (0) which implies all available headlines within the query.
                        // ***************************************************************************************************************

                        // Use date specified within query: "Apple daterange:'2020-06-01,2020-06-07'"
                        Console.WriteLine("\nRetrieve all headlines for query: 'Apple daterange'...");
                        DisplayHeadlines(Headlines.Definition().Query(@"Apple daterange:""2020-06-01,2020-06-07""")
                                                               .Count(0)
                                                               .Sort(Headlines.SortOrder.oldToNew)
                                                               .GetData());

                        // Use date specifier within query - last 5 days
                        Console.WriteLine("Retrieve all headlines for query: 'Apple last 5 days'...");
                        DisplayHeadlines(Headlines.Definition().Query("Apple last 5 days")
                                                               .Count(0)
                                                               .Sort(Headlines.SortOrder.oldToNew)
                                                               .GetData());

                        // Same as previous except show each page response from the platform
                        Console.WriteLine("Same as previous except show each page response...");
                        DisplayHeadlines(Headlines.Definition().Query("Apple last 5 days")
                                                               .Count(0)
                                                               .Sort(Headlines.SortOrder.oldToNew)
                                                               .OnPageResponse((p, headlines) => Console.Write($"{headlines.Data.Headlines.Count}, "))
                                                               .GetData());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n**************\nFailed to execute: {e.Message}\n{e.InnerException}\n***************");
            }
        }

        static void DisplayHeadlines(IHeadlinesResponse headlines)
        {
            if (headlines.IsSuccess)
            {
                Console.WriteLine($"Retrieved a total of {headlines.Data.Headlines.Count} headlines.  Small sample:");
                foreach (var headline in headlines.Data.Headlines.Take(5))
                    Console.WriteLine($"\t{headline.CreationDate}\t{headline.HeadlineTitle}");
            }
            else
                Console.WriteLine($"Issue retrieving headlines: {headlines.Status}");

            Console.WriteLine("\n************************************************************************\n");
        }
    }
}
