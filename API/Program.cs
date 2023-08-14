using API.Mappers;
using AutoMapper;
using Infrastructure.Database.Mappers;
using IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWebApiServices(builder.Configuration);

builder.Services.AddAutoMapper(x =>
    x.AddProfiles(new List<Profile>
    {
        new ApiMappingProfile(), 
        new DatabaseMappingProfile()
    }));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
