using HBAPI.Data;
using HBAPI.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<HbDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))
    )
);

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Serialize enums as strings in JSON responses
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    
    // Add custom converter for DateTime serialization as "yyyy-MM-dd"
    options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());

    // Handle circular references by ignoring them
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    
    // Other serialization options can be added here if needed
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Enable Swagger in development
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Production settings
    app.UseExceptionHandler("/Home/Error"); // Custom error page in production
    app.UseHsts(); // Enforce HTTPS in production
}

// app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();

// Enable CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
