using WebApplication1.DatabaseContext;
using WebApplication2;
using WebApplication2.Interfaces;
using WebApplication2.Repository;
using WebApplication2.Security;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the IConfiguration instance which MyDbContextFactory depends on
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Register MyDbContextFactory
builder.Services.AddSingleton<MyDbContextFactory>();

// Register ConnectionStringProvider
builder.Services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();

// Register DecryptionService
var encryptionKey = "In the eye of the beholder doth lie beauty's true essence, for each gaze doth fashion its own fair visage";
var salt = new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f };
builder.Services.AddSingleton(new DecryptionService(encryptionKey, salt));

// Register the repository and service with a factory for DbContext
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICountSheetService, CountSheetService>();
builder.Services.AddScoped<MyDbContext>(provider =>
{
    var connectionStringProvider = provider.GetRequiredService<IConnectionStringProvider>();
    var dbContextFactory = provider.GetRequiredService<MyDbContextFactory>();
    return dbContextFactory.CreateDbContext(connectionStringProvider.GetConnectionString());
});

// Configure CORS to allow any origin, method, and header
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
