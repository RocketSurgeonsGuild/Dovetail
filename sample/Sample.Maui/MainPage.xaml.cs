using Microsoft.Extensions.DependencyInjection;

namespace Sample.Maui;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
		// Registered by GreetingJoint, with its text from the embedded appsettings files.
		GreetingLabel.Text = IPlatformApplication.Current?.Services.GetService<Greeting>()?.Text;
	}

	private void OnCounterClicked(object? sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}
