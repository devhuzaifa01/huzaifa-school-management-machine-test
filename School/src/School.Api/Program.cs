using School.Api.Configuration;
using School.Api.DependencyInjection;
using School.Api.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilogConfiguration();

builder.Services.AddJwtConfiguration(builder.Configuration);
builder.Services.AddFileUploadConfiguration(builder.Configuration);
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddProjectDependencies(builder.Configuration);
builder.Services.AddFluentValidations();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
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
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
