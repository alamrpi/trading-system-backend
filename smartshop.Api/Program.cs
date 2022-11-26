using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using smartshop.Business;
using System.Text;
using NLog;
using NLog.Web;
using smartshop.Common.Constants;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();

    // Api Versioning service added
    builder.Services.AddApiVersioning(opt =>
    {
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.DefaultApiVersion = new ApiVersion(1, 0);
        opt.ReportApiVersions = true;
    });

    builder.Services.AddVersionedApiExplorer(opt => opt.GroupNameFormat = "'v'VVV");

    // Authentication configure 
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AuthConfigures:Issuer"],
            ValidAudience = builder.Configuration["AuthConfigures:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthConfigures:Token"])),
        };
    });

    builder.Services.AddAuthorization(opt =>
    {
        opt.AddPolicy(AuthPolicies.APP_SETTING, policy => policy.RequireRole(new List<string> { Roles.ADMIN }));
        opt.AddPolicy(AuthPolicies.SUPER_ADMIN, policy => policy.RequireRole(new List<string> { Roles.SUPER_ADMIN }));
        opt.AddPolicy(AuthPolicies.ACCOUNTANT, policy => policy.RequireRole(new List<string> { Roles.ADMIN, Roles.ACCOUNTANT }));
        opt.AddPolicy(AuthPolicies.SALES_MANAGEMENT, policy => policy.RequireRole(new List<string> { Roles.ADMIN, Roles.SALES_MANAGEMENT }));
        opt.AddPolicy(AuthPolicies.PURCHASE_MANAGEMENT, policy => policy.RequireRole(new List<string> { Roles.ADMIN, Roles.PURCHASE_MANAGEMENT }));
        opt.AddPolicy(AuthPolicies.SALES_MANAGEMENT, policy => policy.RequireRole(new List<string> { Roles.ADMIN, Roles.SALES_MANAGEMENT }));
        opt.AddPolicy(AuthPolicies.PRODUCT_MANAGEMENT, policy => policy.RequireRole(new List<string> { Roles.ADMIN, Roles.PRODUCT_MANAGEMENT }));
        opt.AddPolicy(AuthPolicies.STOCK_INVENTORY, policy => policy.RequireRole(new List<string> { Roles.ADMIN, Roles.STOCK_INVENTORY }));
        opt.AddPolicy(AuthPolicies.HR_MANAGEMENT, policy => policy.RequireRole(new List<string> { Roles.ADMIN, Roles.HR_MANAGEMENT }));
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "JWT Authentication",
            Description = "Enter JWT Bearer token **_only_**",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer", // must be lower case
            BearerFormat = "JWT",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
        c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement { { securityScheme, new string[] { } } });

        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Shop Api V1", Version = "v1" });
    });

    // Configure routing services
    builder.Services.AddRouting(options => options.LowercaseUrls = true);

    // Register Services
    builder.Services.AddBusinessLayer(builder.Configuration);
   

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}

    app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseStaticFiles();
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}
