using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Ordering.DataAccess;
using Ordering.DataAccess.Ef;
using Ordering.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

#region Configuration
builder.Services.AddOrderingDataAccessEf(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("OrderingDatabase")!;
builder.Services.AddHealthChecks().AddNpgSql(connectionString, name: "postgresql");

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.AddOrderingModules();
});
#endregion Configuration

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var applyMigrations = builder.Configuration.GetSection("DatabaseOptions").GetValue<bool>("ApplyMigrationsOnStartup");

if (applyMigrations)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
