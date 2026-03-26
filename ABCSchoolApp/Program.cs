using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ABCSchoolApp;
using ABCSchoolApp.Infrastructure;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(builder.Configuration.GetSection(nameof(ApiSettings)).Get<ApiSettings>()!);

await builder.Build().RunAsync();
