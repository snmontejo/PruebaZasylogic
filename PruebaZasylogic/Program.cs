var builder = WebApplication.CreateBuilder(args);

// Esto ya debería estar:
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

// ⬇️ Agrega esto si usarás wwwroot
builder.Services.AddDirectoryBrowser();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita CORS
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

// ⬇️ Esto es lo importante para servir HTML/JS/CSS
app.UseDefaultFiles(); // Carga index.html por defecto
app.UseStaticFiles();  // Permite acceder a wwwroot

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();