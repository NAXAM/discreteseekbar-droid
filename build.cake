#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

// Cake Addins
#addin nuget:?package=Cake.FileHelpers&version=2.0.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var VERSION = "1.0.0";
var NUGET_SUFIX = "";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var solutionPath = "./Naxam.DiscreteSeekBar.Droid.sln";
var artifacts = new [] {
    new Artifact {
        AssemblyInfoPath = "./Naxam.DiscreteSeekBar.Droid/Properties/AssemblyInfo.cs",
        NuspecPath = "./discreteseekbar.nuspec",
        DownloadUrl = "https://dl.bintray.com/acappelli/maven/com/acappelli/discreteSeekBar/1.0/discreteSeekBar-1.0.aar",
        JarPath = "./Naxam.DiscreteSeekBar.Droid/Jars/library-debug.aar"
    }
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Downloads")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        var downloadUrl = string.Format(artifact.DownloadUrl, VERSION);
        var jarPath = string.Format(artifact.JarPath, VERSION);

        DownloadFile(downloadUrl, jarPath);
    }
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./packages");

    var nugetPackages = GetFiles("./*.nupkg");

    foreach (var package in nugetPackages)
    {
        DeleteFile(package);
    }
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionPath);
});

Task("Build")
    // .IsDependentOn("Downloads")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild(solutionPath, settings => settings.SetConfiguration(configuration));
});

Task("UpdateVersion")
    .Does(() => 
{
    foreach(var artifact in artifacts) {
        ReplaceRegexInFiles(artifact.AssemblyInfoPath, "\\[assembly\\: AssemblyVersion([^\\]]+)\\]", string.Format("[assembly: AssemblyVersion(\"{0}\")]", VERSION));
    }
});

Task("Pack")
    .IsDependentOn("UpdateVersion")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var artifact in artifacts) {
        NuGetPack(artifact.NuspecPath, new NuGetPackSettings {
            Version = VERSION+NUGET_SUFIX,
            Dependencies = new []{
                new NuSpecDependency {
                    Id = "Xamarin.Android.Support.v4",
                    Version = "27.0.2"
                }
            },
            ReleaseNotes = new [] {
                $"DiscreteSeekBar for Android v{VERSION}"
            }
        });
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

class Artifact {
    public string AssemblyInfoPath { get; set; }

    public string SolutionPath { get; set; }

    public string DownloadUrl  { get; set; }

    public string JarPath { get; set; }

    public string NuspecPath { get; set; }
}