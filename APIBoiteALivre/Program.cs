using BLL;
using DAL;
using Domain.DTO.Utilisateur.Requetes;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Microsoft.OpenApi.Models;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using APIBoiteALivre.Filtre;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(options =>
{

    options.Filters.Add(typeof(ApiExceptionsFiltre));

});

//Add the FluentValidators in the IOC
builder.Services.AddValidatorsFromAssemblyContaining<AjoutUtilisateurRequeteDTOValidateur>();
builder.Services.AddValidatorsFromAssemblyContaining<ModificationUtilisateurRequeteValidateur>();
builder.Services.AddValidatorsFromAssemblyContaining<AuthentificationDTORequetevalidator>();

builder.Services.AddBLL(options => { });

//ADD the DAL in the IOC
builder.Services.AddDAL(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
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
