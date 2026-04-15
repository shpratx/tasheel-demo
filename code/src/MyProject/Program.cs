using MyProject.DataStores.Implementations;\using MyProject.DataStores.Interfaces;
using MyProject.Middleware;
using MyProject.Services.Implementations;
using MyProject.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BridgeNow Finance API",
        Version = "v1",
        Description = "API for BridgeNow Finance Application Journey & Communication"
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("BridgeNowCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register DataStores as Singletons (in-memory)
builder.Services.AddSingleton<IApplicationDataStore, InMemoryApplicationDataStore>();
builder.Services.AddSingleton<ICustomerDataStore, InMemoryCustomerDataStore>();
builder.Services.AddSingleton<IProductDataStore, InMemoryProductDataStore>();
builder.Services.AddSingleton<ILandingPageContentDataStore, InMemoryLandingPageContentDataStore>();
builder.Services.AddSingleton<ICommunicationTemplateDataStore, InMemoryCommunicationTemplateDataStore>();

// Register Services
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IEligibilityService, EligibilityService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<ILandingPageService, LandingPageService>();
builder.Services.AddScoped<ICommunicationService, CommunicationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IManualReviewService, ManualReviewService>();

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BridgeNow Finance API v1");
    });
}

// Use global exception handler middleware
app.UseGlobalExceptionHandler();

app.UsePathBase("/api");
app.UseHttpsRedirection();
app.UseCors("BridgeNowCorsPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run("http://0.0.0.0:8080");