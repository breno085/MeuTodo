using MeuTodo.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurando o DbContext com SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("DataSource=app.db;Cache=Shared"));

// Adicionando os controladores (necessário para API)
builder.Services.AddControllers();

var app = builder.Build();

// Habilitando roteamento para os controladores
app.MapControllers();

app.Run();
