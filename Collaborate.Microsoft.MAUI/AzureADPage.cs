namespace MauiAuth;
using AuthServices;
using Microsoft.Graph;
using Microsoft.Graph.Models;

public class AzureADPage : AuthPage
{
	public AzureADPage(AuthService authService) : base(authService)
	{
		Title = "Explore Seameless Integration";
	}
}

public class TeamsChannelPage : ChannelPage
{
    public TeamsChannelPage(IAuthService authService, GraphServiceClient client, ChannelCollectionResponse channels, string teamId) : base(authService, client, channels, teamId)
    {
        Title = "Selected Team Channels";
    }
}