using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<JWTContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("JWTContext") ?? throw new InvalidOperationException("Connection String 'JWTContext' not Found.")));
builder.Services.AddScoped<UserViewModel>();
//builder.Services.AddHttpClient<UserViewModel>();
// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("x-my-custom-header"));
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
