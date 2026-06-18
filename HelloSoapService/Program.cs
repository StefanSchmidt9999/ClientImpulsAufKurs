using HelloSoapService.Components;
using HelloSoapService.Services;
using HelloSoapClient = HelloSoapService.Services.HelloSoapService;

var builder = WebApplication.CreateBuilder(args);

// Blazor Standard
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient + SOAP Service
builder.Services.AddHttpClient<HelloSoapClient>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{

    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();