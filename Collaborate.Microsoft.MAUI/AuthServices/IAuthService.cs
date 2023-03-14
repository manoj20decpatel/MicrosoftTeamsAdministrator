namespace AuthServices;
using Microsoft.Identity.Client;
using System.Threading;
using System.Threading.Tasks;

public interface IAuthService
{
	Task<AuthenticationResult?> SignInInteractively(CancellationToken cancellationToken);
	Task<AuthenticationResult?> AcquireTokenSilent(CancellationToken cancellationToken);
	Task LogoutAsync(CancellationToken cancellationToken);
}
