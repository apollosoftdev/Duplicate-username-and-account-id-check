using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework Core with In-Memory Database
builder.Services.AddDbContext<UsernameValidationService.Data.ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("UsernameValidationDb"));

// Add Services
builder.Services.AddScoped<UsernameValidationService.Services.IUsernameValidationService, UsernameValidationService.Services.UsernameValidationService>();

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
