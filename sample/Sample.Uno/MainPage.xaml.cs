namespace Sample.Uno;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        // Registered by GreetingJoint, with its text from the appsettings files.
        GreetingText.Text = ( Application.Current as App )?.Host?.Services.GetService<Greeting>()?.Text;
    }
}
