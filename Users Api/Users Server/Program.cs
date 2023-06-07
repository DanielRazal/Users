global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using System.Text;
global using dotenv.net;
global using Microsoft.EntityFrameworkCore;
global using Users_Server.Context;
global using Users_Server.Token;
global using Users_Server.Repositories;
global using Users_Server.Services;
global using Users_Server.Models;
global using Users_Server.Hubs;


var builder = WebApplication.CreateBuilder(args);

DotEnv.Load();

builder.Services.AddCors(setup =>
{
    setup.AddPolicy("CorsPolicy", options =>
    {
        options.AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()
            .WithOrigins(builder.Configuration["Cors:Angular"]!)
            .AllowCredentials();
            
    });
});

var configuration = builder.Configuration;
configuration.AddUserSecrets<Program>();

var key = Encoding.ASCII.GetBytes(configuration["TokenKey"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddDbContext<UsersDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnectionString")));

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUploadPhotos, UploadPhotos>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();


builder.Services.AddControllers();
builder.Services.AddSignalR();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My User Api",
        Version = "v1",
        Description = "An ASP.NET Core Web API for Users",
        Contact = new OpenApiContact
        {
            Name = "Daniel Razal",
            Email = "mr.danielrazal@gmail.com",
            Url = new Uri("https://github.com/DanielRazal")
        }
    });
});

var app = builder.Build();


//Remove all the data from database
// using (var scope = app.Services.CreateScope())
// {
//    var ctx = scope.ServiceProvider.GetRequiredService<UsersDBContext>();
//    ctx.Database.EnsureDeleted();
//    ctx.Database.EnsureCreated();
// }

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "My User Api V1");
    });
}

app.UseCors("CorsPolicy");

app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<UserHub>("/userhub");
});

app.Run();