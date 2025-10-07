using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using BlogApp.Web.Client.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// MudBlazor servislerini ekleyin
builder.Services.AddMudServices();

// Authentication servislerini ekleyin
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<JwtAuthenticationStateProvider>());

// HttpClient servisi
builder.Services.AddScoped<AuthHeaderHandler>();
builder.Services.AddScoped(sp =>
{
    var authHeaderHandler = sp.GetRequiredService<AuthHeaderHandler>();
    authHeaderHandler.InnerHandler = new HttpClientHandler();
    var apiBaseApiAddress = new Uri("http://localhost:8081/");

    return new HttpClient(authHeaderHandler) { BaseAddress = apiBaseApiAddress };
});

await builder.Build().RunAsync();