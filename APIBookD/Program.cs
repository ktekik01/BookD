using APIBookD.Models.Entities.Chatting;
using APIBookD.Data;
using APIBookD.JwtFeatures;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using APIBookD.Controllers.ChattingControllers;
using GroqSharp;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

var apiKey = builder.Configuration["ApiKey"];
var apiModel = "llama-3.1-70b-versatile";


// Add services to the container.


// Register the GroqClient as a singleton using a factory method
builder.Services.AddSingleton<IGroqClient>(sp =>
    new GroqClient(apiKey, apiModel)
        .SetTemperature(0.5)
        .SetMaxTokens(512)
        .SetTopP(1)
        .SetStop("NONE")
        .SetStructuredRetryPolicy(5));

builder.Services.AddControllers();


builder.Services.AddHttpClient();


builder.Services.AddSignalR();

builder.Services.AddScoped<ChattingController>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BookDDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("BookD")));


/*
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => { options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = jwtSettings["validIssuer"],
    ValidAudience = jwtSettings["validAudience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.GetSection("securityKey").Value))
}; });

builder.Services.AddSingleton<JwtHandler>();

builder.Services.AddScoped<IEmailService, EmailService>();   */


// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()); // Allow cookies and authorization headers
});

builder.Services.Configure<FormOptions>
    (o => {
        o.ValueLengthLimit = int.MaxValue;
        o.MultipartBodyLengthLimit = int.MaxValue;
        o.MemoryBufferThreshold = int.MaxValue;
    });


var app = builder.Build();


app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});


app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chathub");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();




app.MapControllers();

app.Run();
