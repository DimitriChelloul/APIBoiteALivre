using APIBoiteALivre.Filtre;
using BLL;
using DAL;
using Domain.DTO.Utilisateur.Requetes;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(options =>
{
#if !DEBUG
    options.Filters.Add(typeof(ApiExceptionsFiltre));
#endif
});

//Add the FluentValidators in the IOC
builder.Services.AddValidatorsFromAssemblyContaining<AjoutUtilisateurRequeteDTOValidateur>();
builder.Services.AddValidatorsFromAssemblyContaining<ModificationUtilisateurRequeteValidateur>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
