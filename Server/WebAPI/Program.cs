using EfcRepositories.Repositories;
using FileRepositories.Repositories;
using RepositoryContracts;

namespace WebAPI;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Register repository implementations:
        //builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();
        //builder.Services.AddScoped<IForumRepository, ForumFileRepository>();
        //builder.Services.AddScoped<IPostRepository, PostFileRepository>();
        //builder.Services.AddScoped<IUserProfileRepository, UserProfileFileRepository>();
        //builder.Services.AddScoped<IUserRepository, UserFileRepository>();
        
        // Shifted to the EFC based DB repositories instead:
        builder.Services.AddScoped<ICommentRepository, CommentEfcRepository>();
        builder.Services.AddScoped<IForumRepository, ForumEfcRepository>();
        builder.Services.AddScoped<IPostRepository, PostEfcRepository>();
        builder.Services.AddScoped<IUserProfileRepository, UserProfileEfcRepository>();
        builder.Services.AddScoped<IUserRepository, UserEfcRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}