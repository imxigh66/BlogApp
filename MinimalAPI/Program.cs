

using MinimalAPI.Exstensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterServices();


var app = builder.Build();

app.Use(async (ctx, next) =>
{
	try
	{
		await next();
	}
	catch (Exception)
	{

		ctx.Response.StatusCode = 500;
		await ctx.Response.WriteAsync("An error occured");
	}

});

app.UseDeveloperExceptionPage(); // Показывает ошибки в ответах

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.RegisterEndpointDefinitions();



app.Run();
