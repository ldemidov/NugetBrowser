# NugetBrowser 0.1

This is a proof-of-concept console application for browsing and querying Nuget feeds using the OData Nuget package feed endpoint. 
Currently the only available operation is to see the most popular packages published in a given year.  
This utility may be extended in the future.  The complete operation options are listed below.

```
NugetBrowser.  Utility for querying information about packages in a new nuget feed

Usage:
    NugetBrowser.exe popularByYear <year> [--num=<numPackages>] [--api=<url>]
    NugetBrowser.exe (-h | --help)
    NugetBrowser.exe --version

Options:
    -h --help               Show this screen.
    --version               Show version.
    --num=<numPackages>     Number of packages to display [default: 10].
    --api=<url>             Path to Nuget feed api [default: https://www.nuget.org/api/v2].
```

