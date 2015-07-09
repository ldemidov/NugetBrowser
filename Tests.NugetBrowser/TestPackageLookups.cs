using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NugetBrowser;
using NugetBrowser.NugetBrowser.Api;
using Xunit;
using Xunit.Abstractions;

namespace Tests.NugetBrowser
{
    public class TestPackageLookups
    {
        private readonly V2FeedContext _context;
        private readonly ITestOutputHelper _output;

        public TestPackageLookups(ITestOutputHelper output)
        {
            _context = new V2FeedContext(new Uri("https://www.nuget.org/api/v2"));
            _output = output;
        }


        [Theory]
        [InlineData(2014, 1, 10)]
        [InlineData(2012, 3, 10)]
        [InlineData(2015, 6, 10)]
        public async void get_packages_in_year(int year, int month, int numResults)
        {

            var pl = new PackageLookup(_context);

            var startTime = new DateTime(year, month, 1);
            var endTime = startTime.AddMonths(1);
            var results = (await pl.SearchReleasesByTime(startTime, endTime, numResults)).ToList();

            
            _output.WriteLine(string.Format("{0:d} to {1:d}: {2} results", startTime, endTime, results.Count));

            // check result counts
            Assert.True(results.Count <= 50);

            // check ordering
            if (results.Count > 0)
            {
                Assert.True(Enumerable.Range(0, results.Count-1)
                    .All(index => results[index].DownloadCount >= results[index + 1].DownloadCount));    
            }
            

        }

    }
}
