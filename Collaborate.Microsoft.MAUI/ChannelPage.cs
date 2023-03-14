namespace MauiAuth;
using AuthServices;
using Collaborate.Microsoft.MAUI;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Graph;
using Microsoft.Graph.Admin.ServiceAnnouncement.Messages.Archive;
using Microsoft.Graph.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Windows.Input;

public abstract class ChannelPage : ContentPage
{
    private readonly string teamId;
    private GraphServiceClient client;
    private readonly ICommand updateCommand;
    Color[] colorList = { Colors.LightSkyBlue, Colors.LightCoral, Colors.LightCyan,
            Colors.LightGray, Colors.LightGreen, Colors.LightPink,
            Colors.LightSalmon, Colors.LightGoldenrodYellow, Colors.LightYellow};

    protected ChannelPage(IAuthService authService, GraphServiceClient client, ChannelCollectionResponse channels, string teamId)
    {
        this.client = client;
        Button backButton = new()
        {
            Text = "Navigate To All Teams",
            Command = new AsyncRelayCommand(OnBackClick)
        };
        var border = new Border()
        {
            //Content = container,
            StrokeShape = new RoundRectangle() { CornerRadius = new CornerRadius(10) },
            StrokeThickness = 4,
            Stroke = Brush.CadetBlue
        };
        var edge = new Border()
        {
            //Content = container,
            StrokeShape = new RoundRectangle() { CornerRadius = new CornerRadius(10) },
            StrokeThickness = 4,
            Stroke = Brush.CadetBlue
        };
        Button btnArchive = new()
        {
            Text = "Archive Team",
            Command = new AsyncRelayCommand(OnArchiveClick)
        };
        Button btnUnArchive = new()
        {
            Text = "Un-Archive Team",
            Command = new AsyncRelayCommand(OnUnArchiveClick)
        };
        var channelGrid = new Grid
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

        VerticalStackLayout child = new VerticalStackLayout()
        {
            Children =
            {
                border,
                backButton,
                channelGrid,
                btnArchive,
                edge,
                btnUnArchive
            }
        };

        Content = child;

        int column = 0;
        int rows = 0;
        Random rand = new Random();

        for (var i = 0; i < channels.Value.Count; i += 3)
        {
            foreach (var channel in channels.Value.Skip(i).Take(3))
            {
                int index = rand.Next(colorList.Length);
                updateCommand = new ModelCommand<string>(OnChannelClick, channel.Id);
                Button btnTeam = new()
                {
                    Text = channel.DisplayName,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Command = updateCommand,
                    BackgroundColor = colorList[index],
                    TextColor = Colors.DarkBlue,
                    BorderColor = Colors.WhiteSmoke,
                    HeightRequest = 80,
                    AutomationId = channel.Id
                };
                ToolTipProperties.SetText(btnTeam, channel.Description);

                // Row 0
                channelGrid.Add(new Border()
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

        this.teamId = teamId;
    }

    private async void OnChannelClick(string channelId)
    {
        try
        {
            string msg = await DisplayPromptAsync("Send Message", "What's your message?");
            if (string.IsNullOrEmpty(msg ))
                return;

            var requestBody = new ChatMessage
            {
                Body = new ItemBody
                {
                    Content = msg,
                },
            };

            var result = await client.Teams[teamId].Channels[channelId].Messages.PostAsync(requestBody);

            await Toast.Make("Message Sent").Show();
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async Task OnArchiveClick()
    {
        try
        {
          Microsoft.Graph.Teams.Item.Archive.ArchivePostRequestBody body = new();
            await client.Teams[teamId].Archive.PostAsync(body);
            await Toast.Make("Teams Archived").Show();
            OnBackClick();
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async Task OnUnArchiveClick()
    {
        try
        {
            //Microsoft.Graph.Teams.Item.UnArchive.unArchivePostRequestBody body = new();
            await client.Teams[teamId].Unarchive.PostAsync();
            await Toast.Make("Teams Un-archived").Show();
            //OnBackClick();
        }
        catch (Exception ex)
        {
            await Toast.Make(ex.Message).Show();
        }
    }

    private async Task OnBackClick()
    {
        Navigation.RemovePage(this);
    }
}