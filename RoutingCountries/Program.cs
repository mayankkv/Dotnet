using System.Net;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseRouting();

Dictionary<int, string> countriesDict = new()
{
    { 1, "United States" },
    { 2, "Canada" },
    { 3, "United Kingdom" },
    { 4, "India" },
    { 5, "Japan" }
};

#pragma warning disable ASP0014 // Suggest using top level route registrations
app.UseEndpoints(endpoints => {

    endpoints.MapGet("countries", async context => {
        foreach(var item in countriesDict)
            {
                await context.Response.WriteAsync($"{item.Key}. {item.Value}\n");
            }
    });

    endpoints.MapGet("countries/{countryID:int:range(1,100)}", async context => {
        if(countriesDict.ContainsKey(Convert.ToInt32(context.Request.RouteValues["countryID"])))
        {
            await context.Response.WriteAsync(countriesDict[Convert.ToInt32(context.Request.RouteValues["countryID"])]);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync("[No Country]");
        }
    });

    endpoints.MapGet("countries/{countryID:int:min(101)}", async context => {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("The CountryID should be between 1 and 100");
    });

});
#pragma warning restore ASP0014 // Suggest using top level route registrations


app.Run(async context => {
    await context.Response.WriteAsync("No response");
});

app.Run();
