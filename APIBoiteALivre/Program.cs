using BLL;
using DAL;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddBLL();

//Ajout de la couche DAL avec les services
builder.Services.AddDAL(new DALOptions()
{
    ConnectionString = "" //<= La chaine de connexion � la base de donn�es "BOOK_STORE"
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
