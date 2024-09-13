using Microsoft.AspNetCore.StaticFiles;
using WebApplicationAbad.Repository.RepositoryInterface;
using WebApplicationAbad.Repository;
using todoApiAbadnet.Data;
using Microsoft.EntityFrameworkCore;
using todoApiAbadnet.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Register IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyHeader()
				   .AllowAnyMethod();
		});
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DataConnection") ?? throw new InvalidOperationException("Connection string 'DBCon' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(connectionString));



builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
//// تسجيل TabbyController كخدمة
//builder.Services.AddTransient<TabbyController>();

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".zip"] = "application/zip";
provider.Mappings[".rar"] = "application/x-rar-compressed";
builder.Services.AddSingleton(provider);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure the HTTP request pipeline.
builder.Services.AddHttpClient();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
app.UseSwaggerUI();
}




app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
