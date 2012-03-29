using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        private const int Iterations = 100;
        private static readonly CountdownEvent CountdownEvent = new CountdownEvent(Iterations);

        static void Main()
        {
            SendEmails("http://localhost:7269/Home/SendEmail");
            SendEmails("http://localhost:7269/Home/SendEmailAsync");
            Console.Read();
        }

        private static void SendEmails(string syncUrl)
        {
            CountdownEvent.Reset();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (var i = 0; i < Iterations; i++)
            {
                var webRequest = WebRequest.Create(syncUrl);
                webRequest.BeginGetResponse(DownloadComplete, webRequest);
            }

            CountdownEvent.Wait();
            stopwatch.Stop();
            Console.WriteLine("Elapsed time: {0}.{1} seconds", stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds);
        }

        private static void DownloadComplete(IAsyncResult ar)
        {
            var webRequest = (WebRequest)ar.AsyncState;

            try
            {
                var response = webRequest.EndGetResponse(ar);
                var readToEnd = new StreamReader(response.GetResponseStream()).ReadToEnd();
                //Console.WriteLine(readToEnd);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }
            finally
            {
                CountdownEvent.Signal();
            }
        }
    }
}
