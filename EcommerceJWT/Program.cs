
using System.Collections.ObjectModel;
using DataAccess.Context;
using DataAccess.Entity;
using DataAccess.Repository;
using Duende.IdentityServer.Models;

using EcommerceJWT.Mapping;
using Interfaces.IManagers;
using Interfaces.IRepository;
using Interfaces.IServices;
using Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.TokenHelper;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Services;



var builder = WebApplication.CreateBuilder(args);


//logging to db


var sinkOptions = new MSSqlServerSinkOptions
{
    TableName = "Logs",
    AutoCreateSqlTable = true
};

var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn("UserId", System.Data.SqlDbType.NVarChar),
        new SqlColumn("Action", System.Data.SqlDbType.NVarChar)
    }
};




//configure logging to file

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10, shared: true)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: sinkOptions,
        columnOptions: columnOptions
    )
    .CreateLogger();

builder.Host.UseSerilog();

// IDENTITY SERVER CONFIGURATION

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddIdentityServer()
    .AddInMemoryClients(new[]
    {
        new Client
        {
            ClientId = "myClient",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            ClientSecrets = new List<Secret>{new Secret("secret".Sha256()) } ,
            AllowedScopes = { "myApi", "openid", "profile", "offline_access" },
             AllowOfflineAccess = true
        }
    })
    .AddAspNetIdentity<AppUser>()
    .AddInMemoryApiScopes(new[]
    {
        new ApiScope("myApi", "My API", new[] { "role" })
    })
    .AddInMemoryIdentityResources(new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
       new IdentityResource("roles", new[] { "role" })
    })
    .AddDeveloperSigningCredential(); // dev only


//configure your api to trust server configuration
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:7141"; // IdentityServer URL
        options.Audience = "myApi"; // must match scope
    });


//integrate providers optional when UI is there
builder.Services.AddAuthentication()
    .AddGoogle("Google", options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddOpenIdConnect("AzureAD", options =>
    {
        options.Authority = "https://login.microsoftonline.com/{tenantId}/v2.0";
        options.ClientId = builder.Configuration["Authentication:AzureAd:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:AzureAd:ClientSecret"];
        options.ResponseType = "code";
        options.CallbackPath = "/signin-oidc";
        options.SaveTokens = true;
    });


builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));



//JWT AUTHENTICATION CONFIG
//var key = builder.Configuration["Jwt:Key"];
//var issuer = builder.Configuration["Jwt:Issuer"];
//var audience = builder.Configuration["Jwt:Audience"];
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = issuer,
//        ValidAudience = audience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
//    };
//});


builder.Services.AddScoped<TokenHelper>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IProductManager, ProductManager>();
builder.Services.AddScoped<IOrderManager, OrderManager>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseIdentityServer();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
