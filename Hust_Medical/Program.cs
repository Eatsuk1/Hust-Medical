using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR().AddAzureSignalR(options =>
{
    options.ConnectionString = builder.Configuration["AzureSignalRConnectionString"];
});

// Add services to the container.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddServerSideBlazor();
builder.Services.AddRazorPages();
builder.Services.AddMudServices();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMvcCore().AddApiExplorer();

// Add Repositories
builder.AddRepositories();
// Add Services
builder.AddServices();
// Add Authentication
builder.AddAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next(context);
});

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

