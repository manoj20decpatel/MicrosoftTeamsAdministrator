using System.Threading.Channels;

namespace AuthServices;

public static class Constants
{
	public static readonly string ClientId = "cc1954c8-a020-44b1-bfc7-43dbd8ae01a5"; 
	public static readonly string[] Scopes = { "openid", "offline_access", 
		"Team.ReadBasic.All", "TeamSettings.Read.All", "TeamSettings.ReadWrite.All", 
		"User.Read.All", "Directory.Read.All", "User.ReadWrite.All", "Directory.ReadWrite.All",
    "ChannelSettings.Read.All", "Channel.ReadBasic.All", "ChannelSettings.ReadWrite.All",
            "ChannelMessage.Send", "Group.ReadWrite.All"};
	
	public static readonly string TenantName = "CaravanPluto";
	public static readonly string TenantId = $"{TenantName}.onmicrosoft.com";
	public static readonly string SignInPolicy = "B2C_1_Client";
	public static readonly string AuthorityBase = $"https://{TenantName}.b2clogin.com/tfp/{TenantId}/";
	public static readonly string AuthoritySignIn = $"{AuthorityBase}{SignInPolicy}";
	public static readonly string IosKeychainSecurityGroups = "com.microsoft.adalcache";
}