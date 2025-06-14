using Blazor_Markedsplads.Components;
using Blazor_Markedsplads.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Blazor_Markedsplads.Models;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<DBService>();
builder.Services.AddScoped<ListingService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<LoginService>();
//builder.Services.AddScoped<CartService>();
builder.Services.AddSingleton<CartService>();
builder.Services.AddSingleton<SessionService>();

builder.Services.AddRazorPages(); 
builder.Services.AddServerSideBlazor();

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
