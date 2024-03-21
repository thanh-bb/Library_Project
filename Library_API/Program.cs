using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Library_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Authentication.Models;
using Library_API;
using Library_API.Helpter;
using Library_API.Service;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// Connect to the database
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

//send email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();


// JWT Configuration
var jwtSettings = builder.Configuration.GetSection("JWTSetting").Get<JWTSetting>();
builder.Services.Configure<JWTSetting>(builder.Configuration.GetSection("JWTSetting"));

var authKey = jwtSettings.SecurityKey;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

// Access the DbContext within the ConfigureServices method
var dbContext = builder.Services.BuildServiceProvider().GetService<LibraryContext>();

// Refresh Token
builder.Services.AddSingleton<IRefreshTokenGenerator>(provider => new RefreshTokenGenerator(dbContext));


builder.Services.AddHostedService<PhieuMuonWorker>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
    = new DefaultContractResolver());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Enable CORS
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

//images
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});

app.UseHttpsRedirection();


// Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
