using APIBoiteALivre.Filtre;
using BLL;
using DAL;
using Domain.DTO.Requetes;
using FluentValidation;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionsFiltre));
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



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
