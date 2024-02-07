using Octokit;

namespace Giteiro.Controllers;

// This controller is out of scope for now, since you need to authorize the token to have access to organizations.
// With the legacy token you can grant access to organizations without approval.
public class OauthController
{
    public static IResult GetAuthenticate()
    {
        string[] scopes = { "user", "repo" };

        // Generate a random state for CSRF protection
        string state = GenerateRandomString();

        string authorizationUrl = "https://github.com/login/oauth/authorize?" +
            "client_id=" + Environment.GetEnvironmentVariable("clientId") + "&" +
            "scope=" + string.Join(",", scopes) + "&" +
            "state=" + state;

        return Results.Redirect(authorizationUrl);
    }

    public static async Task<string> GetCallbackAsync(string code)
    {
        var request = new OauthTokenRequest(Environment.GetEnvironmentVariable("clientId"), Environment.GetEnvironmentVariable("clientSecret"), code);
        var client = new GitHubClient(new ProductHeaderValue("Giteiro"));
        var tokenResponse = await client.Oauth.CreateAccessToken(request);

        var githubClient = new GitHubClient(new ProductHeaderValue("Giteiro"))
        {
            Credentials = new Credentials(tokenResponse.AccessToken)
        };

        var currentUser = await githubClient.User.Current();

        Console.WriteLine("Hello, {0}!", currentUser.Name);
        return $@"Hello {currentUser.Name}
                Your token is {tokenResponse.AccessToken}";
    }

    static string GenerateRandomString()
    {
        return Guid.NewGuid().ToString("N");
    }
}