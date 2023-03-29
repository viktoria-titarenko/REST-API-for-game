using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1;
using WebApplication1.Interfaces;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "My API - V1",
            Version = "v1"
        }
     );
    var filePath = Path.Combine(System.AppContext.BaseDirectory, "swagger/api.xml");
    c.IncludeXmlComments(filePath);
    });

builder.Services.AddTransient<ICellService, CellService>();

builder.Services.AddTransient<IConnectService, ConnectService>();


builder.Services.AddDbContext<MyDbContext>(x=>x.UseSqlServer("Data Source=localhost;Initial Catalog=MyDatabase;Integrated Security=True;TrustServerCertificate=true"));



var app = builder.Build();

app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse 
            {
                ErrorCode = "InternalServerError",
                ErrorMessage = exception.Error.Message
            };

            var errorJson = System.Text.Json.JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(errorJson);
        }
    });
});


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


public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse 
            {
                ErrorCode = "InternalServerError",
                ErrorMessage = ex.Message
            };

            var errorJson = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(errorJson);
        }
    }
}

public class ErrorResponse
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}
