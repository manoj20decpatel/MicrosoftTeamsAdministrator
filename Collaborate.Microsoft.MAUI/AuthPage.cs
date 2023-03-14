namespace MauiAuth;
using AuthServices;
using Collaborate.Microsoft.MAUI;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Client;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Windows.Input;

public abstract class AuthPage : ContentPage
{
   
    private readonly IAuthService authService;
    private readonly Button loginButton;
    private readonly Button logoutButton;

    private readonly Button getTeams;
    private GraphServiceClient client;
    private TeamsChannelPage channelPage;
    private Grid grid;
    private ICommand updateCommand;
    private VerticalStackLayout mainForm;
    Color[] colorList = { Colors.LightSkyBlue, Colors.LightCoral, Colors.LightCyan,
            Colors.LightGray, Colors.LightGreen, Colors.LightPink,
            Colors.LightSalmon, Colors.LightGoldenrodYellow, Colors.LightYellow};

    protected AuthPage(IAuthService authService)
    {
        this.authService = authService;
        loginButton = new Button
        {
            Text = "Login",
            Command = new AsyncRelayCommand(OnLoginClicked)
        };
        var startArea = new Border()
        {
            //Content = container,
            StrokeShape = new RoundRectangle() { CornerRadius = new CornerRadius(10) },
            StrokeThickness = 4,
            Stroke = Brush.CadetBlue
        };
        var border = new Border()
        {
            //Content = container,
            StrokeShape = new RoundRectangle() { CornerRadius = new CornerRadius(10) },
            StrokeThickness = 4,
            Stroke = Brush.CadetBlue
        };
        logoutButton = new Button
        {
            Text = "Logout",
            IsVisible = false,
            Command = new AsyncRelayCommand(OnLogoutClicked)
        };
        getTeams = new Button
        {
            Text = "Get My Teams",
            IsVisible = false,
            Command = new AsyncRelayCommand(OnFetchTeams)
        };
        grid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition{ Height = new GridLength(10, GridUnitType.Star) },
                new RowDefinition{ Height = new GridLength(10, GridUnitType.Star) },
                new RowDefinition{ Height = new GridLength(10, GridUnitType.Star) }
            },
            ColumnDefinitions =
            {
                new Microsoft.Maui.Controls.ColumnDefinition(),
                new Microsoft.Maui.Controls.ColumnDefinition(),
                new Microsoft.Maui.Controls.ColumnDefinition()
            }
        };

        mainForm = new VerticalStackLayout
        {
            Children =
            {
                loginButton,
                startArea,
                logoutButton,
                  border,
                getTeams,
                grid

            }
        };

        Content = mainForm;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var result = await authService.AcquireTokenSilent(CancellationToken.None);
        await GetResult(result);
    }

    private async Task GetResult(AuthenticationResult? result)
    {
        var token = result?.IdToken;
        if (token != null)
        {
            var handler = new JwtSecurityTokenHandler();
            var data = handler.ReadJwtToken(token);
            if (data != null)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Welcome {data.Claims.FirstOrDefault(x => x.Type.Equals("name"))?.Value}");
                await Toast.Make(stringBuilder.ToString()).Show();
                loginButton.IsVisible = false;
                logoutButton.IsVisible = true;
                getTeams.IsVisible = true;

                token = result?.AccessToken;
                client = GetGraphClient(result?.AccessToken);
            }
        }
    }

    private GraphServiceClient GetGraphClient(string token)
    {
        var accessTokenProvider = new BaseBearerTokenAuthenticationProvider(new TokenProvider(token));
        return new GraphServiceClient(accessTokenProvider);
    }

    private async Task OnFetchTeams()
    {
        try
        {
            grid.IsVisible = true;
            TeamCollectionResponse teams = await client.Me.JoinedTeams.GetAsync();
            int column = 0;
            int rows = 0;
            Random rand = new Random();

            for (var i = 0; i < teams.Value.Count; i += 3)
            {
                foreach (var team in teams.Value.Skip(i).Take(3))
                {
                    if(team.IsArchived.Value) continue;

                    int index = rand.Next(colorList.Length);
                    updateCommand = new ModelCommand<string>(OnTeamClick, team.Id);
                    Button btnTeam = new Button
                    {
                        Text = team.DisplayName,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Command = updateCommand,
                        // Command = new AsyncRelayCommand(OnTeamClick),
                        //CommandParameter = team,
                        BackgroundColor = colorList[index],
                        TextColor = Colors.DarkBlue,
                        BorderColor = Colors.WhiteSmoke,
                        HeightRequest = 70,
                        AutomationId = team.Id
                    };
                    ToolTipProperties.SetText(btnTeam, team.Description);

                    // Row 0
                    grid.Add(new Border()
                    {

                        Content = btnTeam,
                        StrokeShape = new RoundRectangle() { CornerRadius = new CornerRadius(10), Fill = colorList[index] },
                        StrokeThickness = 4,
                        Stroke = colorList[index]
                    }, column, rows);

                    column++;
                }
                column = 0;
                rows++;
            }
            await Toast.Make("Loaded all teams").Show();

        }
        catch (MsalClientException ex)
        {
            await Toast.Make(ex.Message).Show();
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }



    private async void OnTeamClick(string id)
    {
        try
        {
            var relatedChannels = await client.Teams[id].AllChannels.GetAsync();
            await showTeamChannelsAsync(relatedChannels, id);
        }
        catch (MsalClientException ex)
        {
            await Toast.Make(ex.Message).Show();
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async Task showTeamChannelsAsync(ChannelCollectionResponse channels, string teamId)
    {
        try
        {
            channelPage = new TeamsChannelPage(authService, client, channels, teamId);
            await Navigation.PushAsync(channelPage);
            await Toast.Make("Loaded all channels").Show();
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }


    private async Task OnLoginClicked()
    {
        try
        {
            var result = await authService.SignInInteractively(CancellationToken.None);
            await GetResult(result);
        }
        catch (MsalClientException ex)
        {
            await Toast.Make(ex.Message).Show();
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async Task OnLogoutClicked()
    {
        await authService.LogoutAsync(CancellationToken.None);
        loginButton.IsVisible = true;
        logoutButton.IsVisible = false;
        getTeams.IsVisible = false;
        grid.IsVisible = false;
    }
}