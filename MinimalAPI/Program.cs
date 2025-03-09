
using MinimalAPI.Exstensions;

var builder = WebApplication.CreateBuilder(args);


builder.RegisterServices();
builder.Services.AddAuthorizationPolicies();

var app = builder.Build();





app.UseDeveloperExceptionPage(); 

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.RegisterEndpointDefinitions();


app.Run();
