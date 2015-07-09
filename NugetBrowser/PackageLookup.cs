using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NugetBrowser.NugetBrowser.Api;

namespace NugetBrowser
{
    /// <summary>
    /// Nuget Package search operations
    /// </summary>
    public class PackageLookup
    {
        private readonly V2FeedContext _context;

        public PackageLookup(V2FeedContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the most popular packages in a given time period
        /// </summary>
        /// <param name="startDate">Start time for the package search</param>
        /// <param name="endDate">End time for the package search</param>
        /// <param name="numResults">Top N packages to display, as sorted by download counts</param>
        /// <returns>numResults most popular packages in the given time period</returns>
        public Task<IEnumerable<PackageReleaseInfo>> SearchReleasesByTime(DateTime startDate, DateTime endDate, int numResults = 10)
        {

           
            var packageQuery = (DataServiceQuery<PackageReleaseInfo>)_context.Packages
                .Where(p => p.Created > startDate && p.Created < endDate)
                .OrderByDescending(p => p.VersionDownloadCount)
                .Take(numResults)
                .Select(p => new PackageReleaseInfo
                {
                    Id = p.Id,
                    Version = p.Version,
                    DownloadCount = p.VersionDownloadCount
                });

            var task = Task<IEnumerable<PackageReleaseInfo>>.Factory.FromAsync(packageQuery.BeginExecute, packageQuery.EndExecute,
                packageQuery);

            return task;
        }

    }
}
