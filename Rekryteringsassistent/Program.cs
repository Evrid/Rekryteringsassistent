using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rekryteringsassistent.Data;
using Rekryteringsassistent.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddRazorPages();

builder.Services.AddHttpClient();

builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container
builder.Services.AddScoped<AIAnalysisService>();

builder.Services.AddScoped<GPT3ResponseProcessor>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "RekryteringsassistentAPI", Version = "v1" });
});


// Add configuration sources
builder.Configuration.AddUserSecrets<Program>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rekryteringsassistent v1");
});

app.MapControllers();
app.MapRazorPages();

app.Run();
