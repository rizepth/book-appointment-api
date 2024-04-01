using BookAppointment.Core;
using BookAppointment.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BookAppointment.API.Swagger;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using BookAppointment.API.Common.TypeConverters;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using BookAppointment.API;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new AutofacModule(config));
});

// Add services to the container.
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllers()
        .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
            opt.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
        });
builder.Services.AddAutoMapper(typeof(Program));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new OpenApiString(DateTime.Today.ToString("yyyy-MM-dd"))
    });
    opt.MapType<TimeSpan>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "time",
        Example = new OpenApiString(DateTime.Today.ToString("hh:mm"))
    });
}
);
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(
    c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Booking Appointment API V1");
        c.RoutePrefix = String.Empty;
    }
);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
