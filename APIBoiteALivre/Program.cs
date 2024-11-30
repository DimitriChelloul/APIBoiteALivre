using APIBoiteALivre.Filtre;
using BLL;
using DAL;
using Domain.DTO.Utilisateur.Requetes;
using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(options =>
{

    options.Filters.Add(typeof(ApiExceptionsFiltre));

});

//Add the FluentValidators in the IOC
//builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddValidatorsFromAssemblyContaining<AjoutUtilisateurRequeteDTOValidateur>();
builder.Services.AddValidatorsFromAssemblyContaining<ModificationUtilisateurRequeteValidateur>();
builder.Services.AddValidatorsFromAssemblyContaining<AuthentificationDTORequetevalidator>();

builder.Services.AddBLL(options => { });

//ADD the DAL in the IOC
builder.Services.AddDAL(options =>
{
    //options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var server = Environment.GetEnvironmentVariable("DbServer");
    var user = Environment.GetEnvironmentVariable("DbUser");
    var password = Environment.GetEnvironmentVariable("DbPassWord");
    var port = Environment.GetEnvironmentVariable("DbPort");

    //Console.WriteLine($"DbServer: {server}, DbUser: {user}, DbPassWord: {password}, DbPort: {port}");

    options.ConnectionString =
       $"Server={server};Database=Boite a livre;Uid={user};Pwd={password};Charset=utf8;Port={port};SslMode=none";

    Console.WriteLine($"ConnectionString: {options.ConnectionString}");

    Enum.TryParse(builder.Configuration.GetValue<string>("DBType"), out DBType dbType);
    options.DBType = dbType;
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidIssuer = builder.Configuration["JWTIssuer"],
            ValidAudience = builder.Configuration["JWTAudience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSecret"])),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "APIBoiteALivre",
        Version = "v1"
        //Add infos document        
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, true);

    options.AddSecurityDefinition("jwt", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.OAuth2,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Flows = new OpenApiOAuthFlows()
        {
            Password = new OpenApiOAuthFlow()
            {
                TokenUrl = new Uri("/APIBoiteALivre/authentification/swagger", UriKind.Relative)
            }
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "jwt" }
            },
            new string[] {}
        }
    });

});

builder.Services.AddFluentValidationRulesToSwagger();


var app = builder.Build();

////////////////////////////////////////////
/////      Pipeline middleware !
//// Configure the HTTP request pipeline !!!
////////////////////////////////////////////


if (app.Environment.IsDevelopment())
{
    //Here all the middlewares that are only for development environment

    // [DOCUMENTATION] Enable middleware to serve generated Swagger as a JSON endpoint. (Development only)
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("v1/swagger.json", "My API V1");
    });
}

// Configure the HTTP request pipeline.

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
