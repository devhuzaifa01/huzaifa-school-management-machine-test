using School.Api.Configuration;
using School.Api.DependencyInjection;
using School.Api.Filters;
using School.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogConfiguration();

builder.Services.AddMemoryCache();
builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddFileUploadConfiguration(builder.Configuration);
builder.Services.AddCacheConfiguration(builder.Configuration);
builder.Services.AddEmailConfiguration(builder.Configuration);
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddProjectDependencies(builder.Configuration);
builder.Services.AddFluentValidations();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
    options.Filters.Add<ApiExceptionFilter>();
});
builder.Services.AddSwaggerConfiguration();
builder.Services.AddAuthPolicies();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
