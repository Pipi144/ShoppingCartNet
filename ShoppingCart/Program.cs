using DotNetEnv;
using Amazon;
using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data;
using ShoppingCart.Repository;
using ShoppingCart.Services;


var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ShoppingCartContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ShoppingCartContext") ?? throw new InvalidOperationException("Connection string 'ShoppingCartContext' not found.")));





// Configure AWS S3 using environment variables
var awsOptions = new AmazonS3Config
{
    RegionEndpoint = RegionEndpoint.APSoutheast1
};

var s3Client = new AmazonS3Client(
    Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
    Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"),
    awsOptions
);

// Register the S3 client as a singleton service
builder.Services.AddSingleton<IAmazonS3>(s3Client);
// Register repositories and services
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductImageRepository>();
builder.Services.AddScoped<ProductImageService>();
builder.Services.AddScoped<StorageService>();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();