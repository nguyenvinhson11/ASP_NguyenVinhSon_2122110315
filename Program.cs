using Microsoft.EntityFrameworkCore;
using NguyenVinhSon_2122110315.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NguyenVinhSon_2122110315.Config;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// ==================== CẤU HÌNH DỊCH VỤ ==================== //


// BIND config section "Jwt" từ appsettings.json vào class JwtSettings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);

// Kết nối DbContext với SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình xác thực JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });



// Thêm Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NguyenVinhSon_2122110315",
        Version = "v1"
    });

    // ✅ Đây là cấu hình đúng để Swagger TỰ THÊM Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http, // ⬅️ Phải là Http, không phải ApiKey
        Scheme = "bearer",               // ⬅️ chữ thường nha
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Dán token vào đây"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Cấu hình CORS cho React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// Thêm Controller
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });


// ==================== BUILD APP ==================== //
var app = builder.Build();

// Gọi CORS TRƯỚC
app.UseCors("AllowReactApp");

// Cho phép truy cập file tĩnh
app.UseStaticFiles(); // wwwroot
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads")),
    RequestPath = "/uploads"
});

// Swagger cho môi trường phát triển
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Gọi authentication & authorization
app.UseAuthentication(); // 🛡️ xác thực
app.UseAuthorization();  // 🔒 phân quyền

// Map Controller
app.MapControllers();

// Chạy App
app.Run();

