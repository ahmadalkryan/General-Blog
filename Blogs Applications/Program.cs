using Applicaion.IRepository;
using Applicaion.IService;

using Application.IService;
using Application.Mapper;
using Application.Serializer;
using Application.Service;
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
builder.Services.AddSignalR();
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));



//Dependecy Injecton 
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJsonFieldsSerializer, JsonFieldsSerializer>();
builder.Services.AddHttpClient<IAIService, TinyLlamaAIService>((provider, client) =>
{
  
    client.BaseAddress = new Uri("http://localhost:11434");
    client.Timeout = TimeSpan.FromMinutes(2);
    client.DefaultRequestHeaders.Add("User-Agent", "MyApp/1.0");
});
builder.Services.AddScoped<IAIService, TinyLlamaAIService>();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPasswordHash, PasswordHasher>();
builder.Services.AddScoped<IArticleService, ArticleService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ITokenBlackList, TokenBlackList>();
builder.Services.AddScoped<ISummaryService, SummaryService>();
builder.Services.AddScoped<IAskService, AskService>();
builder.Services.AddScoped<ILikeService,LikeService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IGlobalChatService, GlobalChatService>();
builder.Services.AddScoped<INotiService, NotiService>();
builder.Services.AddScoped<IApprovalService, ApprovalService>();


builder.Services.AddHttpContextAccessor();


//mapper
builder.Services.AddAutoMapper(typeof(ArticleProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);
builder.Services.AddAutoMapper(typeof(CommentProfile).Assembly);
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);
builder.Services.AddAutoMapper(typeof(SummaryProfile).Assembly);
builder.Services.AddAutoMapper(typeof(LikeProfile).Assembly);
builder.Services.AddAutoMapper(typeof(PersonaProfile).Assembly);
builder.Services.AddAutoMapper(typeof(NotificationProfile).Assembly);
builder.Services.AddAutoMapper(typeof(MessageProfile).Assembly);
builder.Services.AddAutoMapper(typeof(NotiProfile).Assembly);
builder.Services.AddAutoMapper(typeof(AprovalProfile).Assembly);




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
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/notificationHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });





// في Program.cs لتحسين إعدادات الـ Memory Cache
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 100000; // حد أقصى لعدد العناصر
    options.CompactionPercentage = 0.25; // نسبة الضغط عند الوصول للحد
    options.ExpirationScanFrequency = TimeSpan.FromMinutes(5); // تكرار فحص الانتهاء
});
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});


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


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();
///app.UseMiddleware<TokenBlackListMiddleWare>();
app.MapHub<NotificationHub>("/notificationHub");
app.UseCors("AllowAll");
app.UseStaticFiles();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



public partial class Program { }