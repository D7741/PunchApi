using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UserPunchApi.Data;
using Microsoft.EntityFrameworkCore;
using UserPunchApi.Repositories.Interfaces;
using UserPunchApi.Repositories.Implementations;
using UserPunchApi.Services.Interfaces;
using UserPunchApi.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=userpunch.db")
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Tell Swagger UI about Bearer auth so it shows an Authorize button.
// Without this, Swagger never sends the Authorization header — every
// protected endpoint returns 401 no matter what token you have.
builder.Services.AddSwaggerGen(options =>
{
    // Step 1: define what "Bearer" auth looks like
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",          // lowercase — Swagger uses this to auto-prefix "Bearer "
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste your JWT token here (without the 'Bearer ' prefix)."
    });

    // Step 2: tell Swagger every endpoint requires that definition by default
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// ─── Repositories ────────────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IPunchRecordRepository, PunchRecordRepository>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

// ─── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPunchRecordService, PunchRecordService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

// ─── JWT Token Service ────────────────────────────────────────────────────────
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// ─── Authentication ───────────────────────────────────────────────────────────
// This tells ASP.NET: "When someone sends a request, use the JWT Bearer scheme
// to figure out who they are."
//
// JwtBearerDefaults.AuthenticationScheme = "Bearer"
// which means the client sends:   Authorization: Bearer <token>
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // MapInboundClaims = false keeps claim names exactly as they appear
        // in the token (e.g. "email" stays "email", not remapped to a long URN).
        options.MapInboundClaims = false;

        // TokenValidationParameters is a checklist the middleware runs on EVERY
        // incoming token before your controller code even runs.
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Check that the "iss" field in the token matches our expected issuer
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            // Check that the "aud" field matches our expected audience
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            // Reject expired tokens (checks the "exp" field)
            ValidateLifetime = true,

            // Verify the signature using our secret key.
            // This is the core security check — without it anyone could forge tokens.
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ORDER MATTERS here. Authentication must come before Authorization.
// UseAuthentication: reads the token and populates User (the ClaimsPrincipal)
// UseAuthorization:  checks [Authorize] attributes using the User set above
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
