using Newtonsoft.Json.Linq;

namespace MiddlewareLogin.CustomMiddleware;

public class LoginMiddleware
{
    private readonly RequestDelegate _next;
    public LoginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path == "/" && context.Request.Method == "POST")
        {
            string requestBody;

            using (StreamReader reader = new(context.Request.Body))
            {
                requestBody = await reader.ReadToEndAsync();
            }

            JObject requestBodyObject = JObject.Parse(requestBody);
            string? email = ExtractParameter(requestBodyObject, "email");
            string? password = ExtractParameter(requestBodyObject, "password");

            bool credentialsMatch = email != null && email == "admin@example.com" && password != null && password == "admin1234";
            
            if(credentialsMatch)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                await context.Response.WriteAsync("Successful login!");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid login!");
            }
        }
        else
            await _next(context);
    }

    private static string? ExtractParameter(JObject requestBodyObject, string param)
    {
        // List<string> values = new();
        string? value = null;
        if(requestBodyObject.ContainsKey(param))
        {
            JToken paramValue = requestBodyObject.GetValue(param) ?? JValue.CreateNull();
            if(paramValue != null){
                if(paramValue.First != null && paramValue.Type == JTokenType.Array && paramValue.HasValues)
                    value = paramValue.First.ToString();
                    // values.Add(paramValue.First.ToString());
                else
                    value = paramValue.ToString();
                    // values.Add(paramValue.ToString());
            }
        }
        return value;
    }
}

