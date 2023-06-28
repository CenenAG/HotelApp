using HotelAppLibrary.Data;
using HotelAppLibrary.Databases;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


var configbuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

IConfiguration config = configbuilder.Build();

// Add services to the container.
builder.Services.AddRazorPages();

string dbChoice = config.GetValue<string>("DatabaseChoice").ToLower();
if (dbChoice == "sql")
{
    builder.Services.AddTransient<IDatabaseData, SqlData>();
}
else if (dbChoice == "sqlite")
{
    builder.Services.AddTransient<IDatabaseData, SqliteData>(); 
}
else
{
    //fallback /default value
    builder.Services.AddTransient<IDatabaseData, SqlData>();
}

builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<ISqliteDataAccess, SqliteDataAccess>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
