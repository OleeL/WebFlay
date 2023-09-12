using Microsoft.EntityFrameworkCore;
using WebFlay.Lib.Data;
using WebFlay.Lib.Data.Repositories;
using WebFlay.Lib.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<WebFlayDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("webFlayDb") ??
                           throw new InvalidOperationException("Connection string for \"webFlayDb\" not found.");
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IScrapeRepository, ScrapeRepository>();
builder.Services.AddScoped<IScrapeService, ScrapeService>();


builder.Services.AddCors(p => p.AddPolicy("corsPolicy", corsPolicyBuilder =>
{
    // TO-DO restrict origins on go live (allowing any origin for demo) 
    corsPolicyBuilder.AllowAnyOrigin();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("corsPolicy");
app.UseAuthorization();
app.MapControllers();
app.Run();