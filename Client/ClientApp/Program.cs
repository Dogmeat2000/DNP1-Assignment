using ClientApp.Auth;
using ClientApp.Components;
using ClientApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace ClientApp;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Define a CORS policy
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()     // Allow requests from any origin
                    .AllowAnyMethod()       // Allow any HTTP method (GET, POST, etc.)
                    .AllowAnyHeader();      // Allow any headers
            });
        });
        
        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        
        // Dependency injection:
        builder.Services.AddScoped<IForumService, HttpForumService>();
        builder.Services.AddScoped<IPostService, HttpPostService>();
        builder.Services.AddScoped<ICommentService, HttpCommentService>();
        builder.Services.AddScoped<IUserService, HttpUserService>();
        builder.Services.AddScoped<IUserProfileService, HttpUserProfileService>();
        builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthProvider>();
        builder.Services.AddScoped(sp => new HttpClient {
            BaseAddress = new Uri("http://localhost:5107") // Remember to update this address:port, so it corresponds with the HTTPS address/port in the WEBAPI.
        });
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();
        
        // Use the CORS policy
        app.UseCors("AllowAll");

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}