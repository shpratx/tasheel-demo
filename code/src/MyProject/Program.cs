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
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Register data stores as singletons (in-memory)
builder.Services.AddSingleton<IApplicationDataStore, InMemoryApplicationDataStore>();
builder.Services.AddSingleton<ICustomerDataStore, InMemoryCustomerDataStore>();
builder.Services.AddSingleton<IProductDataStore, InMemoryProductDataStore>();
builder.Services.AddSingleton<ILandingPageContentDataStore, InMemoryLandingPageContentDataStore>();
builder.Services.AddSingleton<ICommunicationTemplateDataStore, InMemoryCommunicationTemplateDataStore>();

// Register services
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IEligibilityService, EligibilityService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<ILandingPageService, LandingPageService>();
builder.Services.AddScoped<ICommunicationService, CommunicationService>();
builder.Services.AddScoped<IProductService, ProductService>();

// Add logging
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

// Use global exception handler
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Set base path
app.UsePathBase("/api");

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

// Seed initial data
SeedData(app.Services);

app.Run();

void SeedData(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var landingPageStore = scope.ServiceProvider.GetRequiredService<ILandingPageContentDataStore>();
    var productStore = scope.ServiceProvider.GetRequiredService<IProductDataStore>();
    var templateStore = scope.ServiceProvider.GetRequiredService<ICommunicationTemplateDataStore>();

    // Seed landing page content
    landingPageStore.CreateAsync(new MyProject.Models.Entities.LandingPageContent
    {
        Id = Guid.NewGuid(),
        PageTitle = "BridgeNow Finance - Your Bridge to Financial Freedom",
        HeroText = "Get approved in minutes with our streamlined application process",
        CtaText = "Apply Now",
        BrandingAssetUrl = "https://example.com/assets/bridgenow-logo.png",
        ProductHighlights = "Fast approval, Competitive rates, Flexible terms",
        EligibilityCriteria = "Credit score 700+, Annual income $50,000+",
        IsActive = true,
        Version = 1,
        CreatedDate = DateTime.UtcNow,
        UpdatedDate = DateTime.UtcNow
    }).Wait();

    // Seed product
    productStore.CreateAsync(new MyProject.Models.Entities.Product
    {
        Id = Guid.NewGuid(),
        ProductCode = "BRIDGE-001",
        ProductName = "BridgeNow Standard Loan",
        Description = "Our flagship bridge loan product",
        MinLoanAmount = 10000,
        MaxLoanAmount = 500000,
        InterestRate = 5.99m,
        ProcessingFee = 500,
        IsActive = true,
        CreatedDate = DateTime.UtcNow,
        UpdatedDate = DateTime.UtcNow
    }).Wait();

    // Seed communication templates
    templateStore.CreateAsync(new MyProject.Models.Entities.CommunicationTemplate
    {
        Id = Guid.NewGuid(),
        TemplateCode = "APP_CONFIRMATION",
        TemplateName = "Application Confirmation",
        Channel = MyProject.Models.Entities.CommunicationChannel.Email,
        Subject = "Your BridgeNow Finance Application",
        Body = "Dear {FirstName}, your application has been received.",
        IsActive = true,
        ComplianceApproved = true,
        ApprovedDate = DateTime.UtcNow,
        CreatedDate = DateTime.UtcNow,
        UpdatedDate = DateTime.UtcNow
    }).Wait();
}