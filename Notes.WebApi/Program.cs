// IOutputFormatter, OutputFormatter
using Microsoft.AspNetCore.Mvc.Formatters;
using Notes.Shared; // AddNotesContext extension method
using Notes.WebApi.Repositories; // ICustomerRepository, CustomerRepository
using Swashbuckle.AspNetCore.SwaggerUI; // SubmitMethod


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<INoteRepository, NoteRepository>();// ICustomerRepository, CustomerRepository

builder.Services.AddNotesContext();
// Add services to the container.

builder.Services.AddControllers()
.AddXmlDataContractSerializerFormatters()
.AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
             c.SwaggerEndpoint("/swagger/v1/swagger.json",
                 "Northwind Service API Version 1");
            c.SupportedSubmitMethods(new[] {
               SubmitMethod.Get, SubmitMethod.Post,
               SubmitMethod.Put, SubmitMethod.Delete });
            }
           );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
