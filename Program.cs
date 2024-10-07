using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); // it will add the controllwe

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//added by me for Cors
builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors",(Corsbuilder) => 
            {
                Corsbuilder.WithOrigins("http://localhost:4200","http://localhost:3000","http://localhost:8000") // these are the origin
                          .AllowAnyMethod()
                          .AllowAnyHeader() 
                          .AllowCredentials();

            });
    options.AddPolicy("ProdCors",(Corsbuilder) => 
            {
                Corsbuilder.WithOrigins("https://myProductionSite.com") // these are the origin
                          .AllowAnyMethod()
                          .AllowAnyHeader() 
                          .AllowCredentials();

            });
});


#region  Token Authorization
 string? tokenKeyString = builder.Configuration.GetSection("Appsettings:TokenKey").Value;

//signature
SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(
            tokenKeyString != null ? tokenKeyString : ""
        )
    );

TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
{
    IssuerSigningKey = tokenKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = false,
    ValidateAudience = false
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = tokenValidationParameters;
                });

#endregion


var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // it will map the controller
app.Run();