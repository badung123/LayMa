using LayMa.Api;
using LayMa.Api.Services;
using LayMa.Core.ConfigOptions;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Interface;
using LayMa.Core.Model.Auth;
using LayMa.Data;
using LayMa.Data.Repositories;
using LayMa.Data.SeedWorks;
using LayMa.WebAPI.Filters;
using LayMa.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//					  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");
//add cors
var LaymaCorsPolicy = "LayMaCorsPolicy";

builder.Services.AddCors(o => o.AddPolicy(LaymaCorsPolicy, builder =>
{
	var origins = configuration["AllowedOrigins"].Split(",");
	builder.AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins(origins)
        .AllowCredentials();
}));

//Config DB Context and ASP.NET Core Identity
builder.Services.AddDbContext<LayMaContext>(options =>
                options.UseSqlServer(connectionString));

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<LayMaContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

// Add services to the container.
builder.Services.AddScoped(typeof(IRepository<,>), typeof(RepositoryBase<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//Auto mapper
builder.Services.AddAutoMapper(typeof(RegisterRequest));
// Business services and repositories
var services = typeof(ShortLinkRepository).Assembly.GetTypes()
    .Where(x => x.GetInterfaces().Any(i => i.Name == typeof(IRepository<,>).Name)
    && !x.IsAbstract && x.IsClass && !x.IsGenericType);

foreach (var service in services)
{
    var allInterfaces = service.GetInterfaces();
    var directInterface = allInterfaces.Except(allInterfaces.SelectMany(t => t.GetInterfaces())).FirstOrDefault();
    if (directInterface != null)
    {
        builder.Services.Add(new ServiceDescriptor(directInterface, service, ServiceLifetime.Scoped));
    }
}

//Authen and author
builder.Services.Configure<JwtTokenSettings>(configuration.GetSection("JwtTokenSettings"));
builder.Services.Configure<MediaSettings>(configuration.GetSection("MediaSettings"));
builder.Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
builder.Services.Configure<NotiSettings>(configuration.GetSection("NotiSettings"));
builder.Services.Configure<WhiteListIPGetCode>(configuration.GetSection("WhiteListIPGetCode"));
builder.Services.AddScoped<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddScoped<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IShortLinkService, ShortLinkService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddScoped<RoleManager<IdentityRole<Guid>>, RoleManager<IdentityRole<Guid>>>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomOperationIds(apiDesc =>
    {
        return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
    });
    c.SwaggerDoc("AdminAPI", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API for Administrators",
        Description = "API for CMS core domain. This domain keeps track of campaigns, campaign rules, and campaign execution."
    });
    c.ParameterFilter<SwaggerNullableParameterFilter>();
});
builder.Services.AddAuthentication(o =>
{
	o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
	cfg.RequireHttpsMetadata = false;
	cfg.SaveToken = true;
	cfg.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateLifetime = true,
		ClockSkew = TimeSpan.FromSeconds(0),
		ValidIssuer = configuration["JwtTokenSettings:Issuer"],
		ValidAudience = configuration["JwtTokenSettings:Issuer"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtTokenSettings:Key"]))
	};
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
	   o.TokenLifespan = TimeSpan.FromHours(3));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("AdminAPI/swagger.json", "Admin API");
        c.DisplayOperationId();
        c.DisplayRequestDuration();
    });
}
app.UseStaticFiles();
app.UseCors(LaymaCorsPolicy);

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
//Seeding data
app.MigrateDatabase();

app.Run();
