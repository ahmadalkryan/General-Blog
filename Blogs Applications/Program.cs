using Applicarion.IRepository;
using Applicarion.IService;
using Applicarion.Mapper;
using Application.Serializer;
using Blogs_Applications.TokenBlackListMiddleWare;
using Infrastructure;
using Infrastructure.Repository;
//using Infrastructure.Seeds;
using Infrastructure.Service;
using Infrastructure.Service.HashPassword;
using Infrastructure.Service.JwtService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using SystemTicketing.EXpectionMiddleWare;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.




builder.Services.AddControllers();
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));



//Dependecy Injecton 
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJsonFieldsSerializer, JsonFieldsSerializer>();
//builder.Services.AddScoped<Seed>();
builder.Services.AddScoped<IAIService, OpenAIService>();
builder.Services.AddHttpClient<OpenAIService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHash, PasswordHasher>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITokenBlackList, TokenBlackList>();
builder.Services.AddHttpContextAccessor();


//mapper
builder.Services.AddAutoMapper(typeof(ArticleProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CommentProfile).Assembly);
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);






builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
// إضافة المصادقة JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,

            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };
    });
//builder.Services.AddAuthentication(options =>
//{
//options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme,
//options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme,
//options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme





//    )
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
//        };
//    });

// في Program.cs لتحسين إعدادات الـ Memory Cache
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 100000; // حد أقصى لعدد العناصر
    options.CompactionPercentage = 0.25; // نسبة الضغط عند الوصول للحد
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(5); // تكرار فحص الانتهاء
});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("DevCors", policy =>

//    {
//        policy.WithOrigins(
//            "https://localhost:52091", //react

//              "https://localhost:7148" //api

//            )
//              .AllowAnyMethod().
//              AllowCredentials()
//              .AllowAnyHeader();
//    });
//});
builder.Services.AddCors(options =>
{

    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
            "https://localhost:52091",
            "https://localhost:7148"
            ).AllowAnyOrigin(


            ).AllowAnyHeader().AllowAnyMethod();

    });






});











// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        var context = services.GetRequiredService<BlogDbContext>();
//        var seed = services.GetRequiredService<Seed>();

//        // Ensure database is created
//        context.Database.EnsureCreated();

//        // Seed the data
//        seed.SeedData();

//        Console.WriteLine("Database seeded successfully!");
//    }
//    catch (Exception ex)
//    {
//        var logger = services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while seeding the database.");
//    }
//}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
///app.UseMiddleware<TokenBlackListMiddleWare>();
app.UseCors("AllowAll");
app.UseStaticFiles();
//app.UseStaticFiles(); // هذه السطر ضروري

// إذا كنت تستخدم مجلد مخصص غير wwwroot
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(builder.Environment.ContentRootPath, "Images")),
//    RequestPath = "/Images"
//});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



