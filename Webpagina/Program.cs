using Appdata2.Components;
using SimpleMqtt; 

var builder = WebApplication.CreateBuilder(args);


var SqlConnectionString = "aei-sql2.avans.nl,1443"; 
builder.Services.AddSingleton<IUserRepository, SqlUserRepository> (o => new SqlUserRepository(SqlConnectionString));

// Create a simple MQTT client with the necessary connection details
var simpleMqttClient = new SimpleMqttClient(new()
{
    Host = "feddf277fe95453a995e24724824bab7.s1.eu.hivemq.cloud", 
    Port = 8883,
    ClientId = "webapp",
    TimeoutInMs = 5_000, 
    UserName = "hivemq.webclient.1732790125285", //TODO: replace with user-secrets
    Password = "d9,GrNIM.X17Dsu*6#xo" // TODO: replace with user-secrets
});

// Register the Simple MQTT client as an object in the dependency injection container
builder.Services.AddSingleton(simpleMqttClient); 

// Configure a MQTT Message Processing Service (that runs continuously in the background)
builder.Services.AddHostedService<MqttMessageProcessingService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
