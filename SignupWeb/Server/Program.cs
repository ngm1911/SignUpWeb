using Carter;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SignupWeb.Server.DatabaseContext;
using SignupWeb.Server.Models;
using System.Text.Json.Serialization;
using Logger = Serilog.Log;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
//builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EgbLoyaltyDb>(options =>
{
    var connection = builder.Configuration.GetSection("Connection").Get<Connection>();
    var connectInUse = connection.Connects.Single(x => x.InUse);
    var sConnB = new SqlConnectionStringBuilder()
    {
        DataSource = connectInUse.DataSource,
        InitialCatalog = "EGBLoyalty",
        UserID = connectInUse.User,
        Password = connectInUse.Password,
        TrustServerCertificate = true,
        IntegratedSecurity = false,
        ConnectTimeout = 60,
    };
    options.UseSqlServer(sConnB.ConnectionString);
});

builder.Services.AddHealthChecks();
builder.Services.AddCarter();
builder.Services.Configure<JsonOptions>(op =>
{
    op.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                                                | JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
AddSerilog();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
//app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseHealthChecks("/users/PingServer");

app.MapCarter();

app.Run();

void AddSerilog()
{
    string template = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{TraceId}] [{UserId}] {Message:lj}{NewLine}{Exception}";
    Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.Logger(lc => lc
                        .WriteTo.Async(a =>
                        {
                            a.File($"Logs/{DateTime.Now:yyyy}/{DateTime.Now:MM}/{DateTime.Now:dd}/.txt",
                            outputTemplate: template,
                            rollingInterval: RollingInterval.Day,
                            fileSizeLimitBytes: 10485760,
                            rollOnFileSizeLimit: true,
                            shared: true);
                        }))
                    .CreateLogger();

}