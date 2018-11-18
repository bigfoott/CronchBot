using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tweetinvi;

namespace CronchBot
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("credentials.txt"))
            {
                File.WriteAllText("credentials.txt", "CONSUMER_KEY\nCONSUMER_SECRET\nACCESS_TOKEN\nACCESS_TOKEN_SECRET");
                Console.WriteLine("Please replace the placeholders in 'credentials.txt' with the values for your twitter account.\n\nPress any key to continue...");
                Console.Read();
                return;
            }
            string[] creds = File.ReadAllLines("credentials.txt");
            Auth.SetUserCredentials(creds[0], creds[1], creds[2], creds[3]);
            
            if (!File.Exists("cronch.txt"))
                File.WriteAllText("cronch.txt", "cronch\nCRONCH\nC R O N C H\nCRRRONCH\ncronch.\ncronch...\nCRONCHCRONCHCRONCH");
            
            var array = File.ReadAllLines("cronch.txt").Where(l => l != "").ToList();
            var r = new Random();
            
            bool hasPosted = false;
            while (true)
            {
                Thread.Sleep(1000);
                if (DateTime.Now.Minute % 30 == 0)
                {
                    if (hasPosted) continue;
                    hasPosted = true;
                    int amount = r.Next(1, 10);
                    string tweet = "";
                    for (int i = 0; i < amount; i++)
                    {
                        string phrase = array[r.Next(0, array.Count - 1)];
                        if (phrase.Length + tweet.Length > 280)
                            break;
                        tweet += phrase + " ";
                    }
                    tweet = tweet.TrimEnd();
                    Tweet.PublishTweet(tweet);
                    Console.WriteLine("Posted tweet at " + DateTime.Now);
                }
                else if (hasPosted) hasPosted = false;
            }
        }
    }
}
