using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocoptNet;
using NugetBrowser.NugetBrowser.Api;

namespace NugetBrowser
{
    class Program
    {

        private const string _usage = @"NugetBrowser.  Utility for querying information about packages in a new nuget feed

Usage:
    NugetBrowser.exe popularByYear <year> [--num=<numPackages>] [--api=<url>]
    NugetBrowser.exe (-h | --help)
    NugetBrowser.exe --version

Options:
    -h --help               Show this screen.
    --version               Show version.
    --num=<numPackages>     Number of packages to display [default: 10].
    --api=<url>             Path to Nuget feed api [default: https://www.nuget.org/api/v2].
";

        private static void Main(string[] args)
        {
            var arguments = new Docopt().Apply(_usage, args, version: "NugetBrowser 0.1", exit: true);

            // getting the most popular packages in a year
            if (arguments["popularByYear"].IsTrue)
            {
                int year;
                int numToDisplay;
                string serviceUrl = arguments["--api"].ToString();

                int lowestYear = 2010; // Nuget didn't exist till 2010
                int nextYear = DateTime.Now.Year + 1;
                if (!(int.TryParse(arguments["<year>"].ToString(), out year) && year > lowestYear && year < nextYear))
                {
                    Console.Error.WriteLine("Enter a valid year [{0}-{1}]", lowestYear, nextYear - 1);
                    return;
                }

                if (!(int.TryParse(arguments["--num"].ToString(), out numToDisplay) && numToDisplay > 0))
                {
                    Console.Error.WriteLine("Enter a valid number of packages to display.  1 or more.");
                    return;
                }


                var task = PopularPackagesByYear(serviceUrl, year, numToDisplay);
                task.Wait();
            }
            
        }

        private static async Task PopularPackagesByYear(string serviceUrl, int year, int numPackages)
        {
            var context = new V2FeedContext(new Uri(serviceUrl));

            Console.WriteLine("Looking up the top {0} most popular packages published in the year {1}.  Please wait...", numPackages, year);


            var searchApi = new PackageLookup(context);

            for (var i = 1; i < 13; ++i)
            {
                var startDate = new DateTime(year, i, 1);
                var endDate = startDate.AddMonths(1);

                var packagesInMonth = (await searchApi.SearchReleasesByTime(startDate, endDate, numPackages)).ToList();

                Console.WriteLine("------------------------------------");
                Console.WriteLine("{0:MMMM yyyy}", startDate);
                Console.WriteLine("------------------------------------");

                foreach (var p in packagesInMonth)
                {
                    Console.WriteLine("{0} {1} [{2} downloads]", p.Id, p.Version, p.DownloadCount);
                }
            }
        }


    }
}
