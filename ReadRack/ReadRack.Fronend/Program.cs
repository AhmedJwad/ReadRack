using Blazored.Modal;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ReadRack.Fronend;
using ReadRack.Fronend.AuthenticationProviders;
using ReadRack.Fronend.Repositories;
using ReadRack.Fronend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7186/") });
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7186/") });
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddSweetAlert2();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazorBootstrap();
await builder.Build().RunAsync();
