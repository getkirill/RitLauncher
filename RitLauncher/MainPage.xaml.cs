using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;

namespace RitLauncher;

public partial class MainPage : ContentPage
{
    private List<string> _ritVersions = new List<string>() { "branch-master" };

    private List<string> FriendlyRitVersions
    {
        get
        {
            return _ritVersions.Select(x =>
            {
                if (x.StartsWith("branch"))
                {
                    return $"Latest - branch {x.Substring(6)}";
                }

                return "COULD NOT PARSE VERSION, REPORT TO YOUR LOCAL PROGRAMMER";
            }).ToList();
        }
    }

    private int CurrentRitVersion
    {
        get => FriendlyRitVersions.IndexOf(VersionPicker.SelectedItem as string);
    }

    private bool CurrentRitVersionDownloaded
    {
        // TODO: MORE PLATFORMS
        get => File.Exists(Path.Combine(FileSystem.AppDataDirectory, _ritVersions[CurrentRitVersion], "Rit.exe"));
    }

    public void DownloadRit()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // TODO: MORE PLATFORMS
            var bits = RuntimeInformation.OSArchitecture;
            var installPath = Path.Combine(FileSystem.AppDataDirectory, _ritVersions[CurrentRitVersion]);
            var downloadPath = Path.Combine(installPath, "Rit.zip");
            // TODO: HttpClient
            using (var client = new WebClient())
            {
                if (bits == Architecture.X64)
                {
                    client.DownloadFile(
                        "https://nightly.link/GuglioIsStupid/Rit/workflows/windows-build.yaml/master/Rit-Win64.zip",
                        downloadPath
                    );
                }
                else if (bits == Architecture.X86)
                {
                    client.DownloadFile(
                        "https://nightly.link/GuglioIsStupid/Rit/workflows/windows-build.yaml/master/Rit-Win32.zip",
                        downloadPath
                    );
                }
                else
                {
                    DisplayAlert("Error", $"Architecture unsupported: {bits.ToString()}", "OK");
                    return;
                }
            }
            var archive = ZipFile.Open(downloadPath, ZipArchiveMode.Read);
            archive.ExtractToDirectory(installPath);
        }
        else
        {
            DisplayAlert("Error", "Unsupported operating system", "OK");
        }
    }

    public MainPage()
    {
        InitializeComponent();
        var versions = new List<string>();
        versions.Add("Latest - branch master");
        versions.Add("Latest - tag v0.0.1");
        VersionPicker.ItemsSource = versions;
        VersionPicker.SelectedItem = versions[0];
    }
}