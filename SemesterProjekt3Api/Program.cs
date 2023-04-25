using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Configure the JWT Authentication Service
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "JwtBearer";
        options.DefaultChallengeScheme = "JwtBearer";
    })
    .AddJwtBearer("JwtBearer", jwtOptions =>
    {
    jwtOptions.TokenValidationParameters = new TokenValidationParameters()
    { // The SigningKey is defined in the TokenController
         class ValidateIssuerSigningKey = true, 
    // IssuerSigningKey = new SecurityHelper(configuration).GetSecurityKey(),
    IssuerSigningKey = new SecurityHelper(builder.Configuration).GetSecurityKey(), 
    ValidateIssuer = true, 
    ValidateAudience = true, 
    ValidIssuer = "https://localhost:7150", 
    ValidAudience = "https://localhost:7150", 
    ValidateLifetime = true }; 
});
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseAuthentication();
// Notice: Added app.
UseAuthorization();
app.MapControllers();
app.Run();