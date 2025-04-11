using Azure.Storage.Blobs;
using CodeFirstEFAPI.Data;
using CodeFirstEFAPI.Models;
using CodeFirstEFAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configure DBContext
var connectionString = builder.Configuration.GetConnectionString("dbcs");
builder.Services.AddDbContext<StudentDBContext>(options => options.UseSqlServer(connectionString));

builder.Services.Configure<AzureBlobSettings>(builder.Configuration.GetSection("AzureBlobSettings"));

builder.Services.AddSingleton(config => new BlobServiceClient(builder.Configuration.GetValue<string>("AzStorageConnString")));
builder.Services.AddSingleton<IBlobService, BlobService>();
builder.Services.AddScoped<IContainers, Containers>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Commented for certification issue
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
