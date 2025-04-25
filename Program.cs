using nettruyen.Data;
using Microsoft.EntityFrameworkCore;
using nettruyen.Mapping;
using nettruyen.Services;
using nettruyen.Validators.Admin;
using FluentValidation;
using nettruyen.Dto.Admin;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
// Dăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Đăng ký serive 
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IComicService, ComicService>();

builder.Services.AddTransient<IValidator<CategoryDTO>, CategoryDTOValidator>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated(); // 👈 Dòng này rất quan trọng!
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseStaticFiles();

app.Run();
