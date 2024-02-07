using dotenv.net;
using Giteiro.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:42069");

var app = builder.Build();

DotEnv.Fluent()
      .WithEnvFiles(".env")
      .Load();

app.MapGet("/", () => "Hello World!");
app.MapGet("/authenticate", OauthController.GetAuthenticate);
app.MapGet("/callback", OauthController.GetCallbackAsync);

app.Run();
