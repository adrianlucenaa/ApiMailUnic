/*
using APICruzber.Interfaces; // Asegúrate de importar el espacio de nombres donde se encuentra IClienteService

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddControllers();

// Registro de la implementación concreta de IClienteService
builder.Services.AddScoped<ICliente, DatosCliente>(); // Asegúrate de cambiar "DatosCliente" por el nombre correcto de tu clase concreta

var app = builder.Build();

// Configuración de la aplicación
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
*/