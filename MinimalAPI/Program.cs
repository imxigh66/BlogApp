
using MinimalAPI.Exstensions;

var builder = WebApplication.CreateBuilder(args);


builder.RegisterServices();
builder.Services.AddAuthorizationPolicies();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();


app.UseCors("AllowFrontend");


app.UseDeveloperExceptionPage(); 

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.RegisterEndpointDefinitions();


app.Run();
