var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) => {
    if(context.Request.Method == "GET" && context.Request.Path == "/")
    {
        if(!int.TryParse(context.Request.Query["firstNumber"].FirstOrDefault(), out var firstNumber))
        {
            HandleInvalidInput(context, "firstNumber");
            return;
        }

        if(!int.TryParse(context.Request.Query["secondNumber"].FirstOrDefault(), out var secondNumber))
        {
            HandleInvalidInput(context, "secondNumber");
            return;
        }

        string? operation = context.Request.Query["operation"].FirstOrDefault();

        if(operation == null)
        {
            HandleInvalidInput(context, "operation");
            return;
        }

        long? result = null;

        switch (operation)
        {
            case "add" : result = firstNumber + secondNumber; break;
            case "subtract" : result = firstNumber - secondNumber; break;
            case "multiply" : result = firstNumber * secondNumber; break;
            case "divide" : result = (secondNumber != 0) ? firstNumber / secondNumber : 0; break;
            case "mod" : result = (secondNumber != 0) ? firstNumber % secondNumber : 0; break;
            default:
                HandleInvalidInput(context, "operation");
                return;
        }

        await context.Response.WriteAsync(result.ToString() ?? "N/A");
    }
    else
    {
        if(context.Request.Method != "GET")
            await context.Response.WriteAsync($"{context.Request.Method} is not valid!");
        else
            await context.Response.WriteAsync($"{context.Request.Path} is not valid");
    }
});
app.Run();

async void HandleInvalidInput(HttpContext context, string parameter)
{
    if(context.Response.StatusCode == 200)
    {
        context.Response.StatusCode = 400;
    }
    await context.Response.WriteAsync($"Invalid input for '{parameter}'");
}
