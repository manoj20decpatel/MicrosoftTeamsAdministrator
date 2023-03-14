using MauiAuth;

namespace Collaborate.Microsoft.MAUI;

public partial class MainPage : ContentPage
{
	int count = 0;
    private readonly AzureADPage _azureAdPage;
    private readonly AzureB2CPage _azureB2CPage;

    public MainPage(AzureADPage azureAdPage, AzureB2CPage azureB2CPage)
	{
        _azureAdPage = azureAdPage;
        _azureB2CPage = azureB2CPage;
        InitializeComponent();
    }

    async void AzureADPageClicked(object? sender, EventArgs args)
    {
		try
		{
            await Navigation.PushAsync(_azureAdPage);
        }
		catch (Exception)
		{

			//throw;
		}
       
    }

    async void AzureB2CPageClicked(object? sender, EventArgs args)
    {
        await Navigation.PushAsync(_azureB2CPage);
    }

 //   private void OnCounterClicked(object sender, EventArgs e)
	//{
	//	count++;

	//	if (count == 1)
	//		CounterBtn.Text = $"Clicked {count} time";
	//	else
	//		CounterBtn.Text = $"Clicked {count} times 1";

	//	SemanticScreenReader.Announce(CounterBtn.Text);
	//}
}

