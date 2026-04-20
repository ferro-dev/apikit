using System.Linq;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

class Build : NukeBuild
{
  public static int Main () => Execute<Build>(x => x.Compile);

  [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
  readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

  [Parameter("Collect code coverage. Default is 'true'")] readonly bool? Cover = true;

  [Parameter("Coverage threshold. Default is 80%")] readonly int Threshold = 80;

  [Parameter("Target NuGet server to publish our packages")] readonly string TargetNuGetServer;

  [Parameter("API key for pushing our NuGet packages")] readonly string NuGetApiKey;

  [GitVersion(Framework = "net10.0", NoFetch = true)][CanBeNull] readonly GitVersion GitVersion;

  [Solution] readonly Solution Solution;
  [GitRepository] readonly GitRepository GitRepository;

  AbsolutePath SourceDirectory => RootDirectory / "src";
  AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";
  AbsolutePath NuGetArtifactsDirectory => ArtifactsDirectory / "nuget";

  Target Clean => _ => _
  .Before(Restore)
  .Executes(() =>
  {
      ArtifactsDirectory.CreateOrCleanDirectory();
      NuGetArtifactsDirectory.CreateOrCleanDirectory();
  });

  Target Restore => _ => _
  .Executes(() =>
  {
      DotNetRestore(s => s
    .SetProjectFile(Solution));
  });

  Target Compile => _ => _
  .DependsOn(Restore)
  .Executes(() =>
  {
      DotNetBuild(s =>
      {
    s = s
          .SetProjectFile(Solution)
          .SetConfiguration(Configuration)
          .EnableNoRestore();

    if (GitVersion != null)
    {
          s = s
      .SetAssemblyVersion($"{GitVersion.Major}.{GitVersion.Minor}.0")
      .SetFileVersion(GitVersion.MajorMinorPatch)
      .SetInformationalVersion(GitVersion.InformationalVersion);
    }

    return s;
      });
  });

  Target Test => _ => _
  .DependsOn(Compile)
  .Executes(() =>
  {
      DotNetTest(s => s
    .SetConfiguration(Configuration)
    .SetNoBuild(InvokedTargets.Contains(Compile))
    .SetProjectFile(Solution)
    .EnableNoBuild()
    .SetLoggers("trx")
    .SetProperty("CollectCoverage", Cover)
    .SetProperty("CoverletOutput", ArtifactsDirectory / "coverage" / "coverage")
    .SetProperty("Threshold", Threshold)
    .SetProperty("ThresholdType", "line")
    .SetProperty("CoverletOutputFormat", "cobertura")
    .SetResultsDirectory(ArtifactsDirectory / "tests"));

      // Merge per-TFM cobertura reports into a single union (was this line hit in any TFM?).
      var coverageDir = ArtifactsDirectory / "coverage";
      ReportGenerator(s => s
    .SetReports(coverageDir / "coverage.*.cobertura.xml")
    .SetTargetDirectory(coverageDir)
    .SetReportTypes(ReportTypes.Cobertura));

      (coverageDir / "Cobertura.xml").Move(coverageDir / "coverage.cobertura.xml", ExistsPolicy.FileOverwrite);
  });

  Target Pack => _ => _
  .After(Test)
  .DependsOn(Compile)
  .Executes(() =>
  {
      DotNetPack(s =>
      {
    s = s
          .SetProject(SourceDirectory / "ApiKit" / "ApiKit.csproj")
          .SetConfiguration(Configuration)
          .EnableNoBuild()
          .EnableNoRestore()
          .SetOutputDirectory(NuGetArtifactsDirectory);

    if (GitVersion != null)
          s = s.SetVersion(GitVersion.NuGetVersionV2);

    return s;
      });
  });

  Target Push => _ => _
  .After(Pack)
  .Requires(() => OnPublishBranch())
  .Requires(() => NuGetApiKey)
  .Requires(() => TargetNuGetServer)
  .Requires(() => Configuration.Equals(Configuration.Release))
  .Executes(() =>
  {
      NuGetArtifactsDirectory.GlobFiles("*.nupkg")
    .Where(f => !f.Name.EndsWith("symbols.nupkg"))
    .ForEach(targetPath =>
    {
          DotNetNuGetPush(s => s
      .SetSource(TargetNuGetServer)
      .SetApiKey(NuGetApiKey)
      .SetTargetPath(targetPath)
      .EnableSkipDuplicate());
    });
  });

  bool OnPublishBranch()
  => GitRepository.IsOnDevelopBranch() ||
       (GitRepository.Branch?.StartsWithOrdinalIgnoreCase("release-") ?? false) ||
       GitRepository.IsOnMainBranch() ||
       IsTagBuild();

  static bool IsTagBuild()
  {
    var githubRef = System.Environment.GetEnvironmentVariable("GITHUB_REF") ?? "";
    return githubRef.StartsWith("refs/tags/v");
  }
}
