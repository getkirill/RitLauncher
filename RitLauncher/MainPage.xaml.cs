namespace RitLauncher;

public partial class MainPage : ContentPage
{
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