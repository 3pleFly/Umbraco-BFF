using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient("UmbracoClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:8080/umbraco/delivery/api/v2/content");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<UmbracoService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
