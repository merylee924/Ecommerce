using Ecommerce.Services;
using StackExchange.Redis;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Redis as a singleton connection
builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("RedisConnection");
    return ConnectionMultiplexer.Connect(configuration);
});

// Configure the custom RedisService for application logic
builder.Services.AddSingleton<RedisService>();

// Add Ocelot as the API Gateway
builder.Services.AddOcelot();

// Configure CORS for allowing communication with frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddControllers();

var app = builder.Build();
app.UseCors("AllowAll");

// **Middleware Configuration**
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowFlutter");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Use Ocelot middleware to handle the API Gateway logic
//await app.UseOcelot();

app.MapControllers();

app.Run();
