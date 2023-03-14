using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AuthServices;
using Microsoft.Identity.Client;
using Plugin.CurrentActivity;

namespace Collaborate.Microsoft.MAUI;

[Activity(Theme = "@style/Maui.SplashTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density,
    SupportsPictureInPicture = true,
    HardwareAccelerated = true,
    LaunchMode = LaunchMode.SingleInstance,
    Exported = true,
    ResizeableActivity = true,
    AllowEmbedded = true)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle bundle)
    {
        //TabLayoutResource = Resource.Layout.Tabbar;
        //ToolbarResource = Resource.Layout.Toolbar;

        base.OnCreate(bundle);
        CrossCurrentActivity.Current.Init(this,bundle);
        AuthService.ParentWindow = this;

        //Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
        //global::Xamarin.Forms.Forms.Init(this, bundle);

        //CrossCurrentActivity.Current.Init(this, bundle);
        //// CrossCurrentActivity.Current.Activity is NULL
        //CrossCurrentActivity.Current.Activity = this;
        //// CrossCurrentActivity.Current.Activity is still NULL

        //var formsApp = new App(new AndroidInitializer());
        //_registryContainer = formsApp.Container;
        //LoadApplication(formsApp);
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
        AuthenticationContinuationHelper.SetAuthenticationContinuationEventArgs(requestCode, resultCode, data);
    }
}


