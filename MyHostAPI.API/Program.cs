using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MyHostAPI.Common.Configurations;
using MyHostAPI.Common.Filters;
using MyHostAPI.Configuration;
using MyHostAPI.Configuration.Middlewares;
using MyHostAPI.Data.Seeder;
using SendGrid.Extensions.DependencyInjection;
using Serilog;
using System.Text;
using Quartz;
using MyHostAPI.SceduledServices.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MappingProfiles());
});

var mapper = config.CreateMapper();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register services directly with Autofac here. Don't
// call builder.Populate(), that happens in AutofacServiceProviderFactory.
builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule(new AppModule(builder.Configuration)));
builder.Services.AddSingleton(mapper);
builder.Services.AddTransient<Seeder>();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    // Register the job, loading the schedule from configuration
    q.AddJobAndTrigger<RemoveUnconfirmedUsersJob>(builder.Configuration);
});

builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
        .RequireAuthenticatedUser().Build());
});

builder.Services.AddAuthentication(x =>
   {
       x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
       x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
   })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettings:JWTKey"])),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = configuration["JwtSettings:ValidIssuer"],
            ValidAudience = configuration["JwtSettings:ValidAudience"]
        };
    });

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers(options => { options.Filters.Add<ExceptionFilter>(); });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSendGrid(options =>
{
    options.ApiKey = builder.Configuration
    .GetSection(SendGridEmailSettingsSection.Name).GetValue<string>("APIKey");

});

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("X-Pagination");
}));

builder.Services.Configure<StorageAccountSection>(configuration.GetSection(StorageAccountSection.Name));

builder.Services.Configure<SendGridEmailSettingsSection>(configuration.GetSection(SendGridEmailSettingsSection.Name));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");


var seeder = app.Services.GetService<Seeder>();

if (seeder != null)
    await seeder.SeedAsync();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
