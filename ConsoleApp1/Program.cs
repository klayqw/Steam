using ConsoleApp1.Attribute;
using ConsoleApp1.Controllers;
using System.Net;
using System.Reflection;

HttpListener httpListener = new HttpListener();

const int port = 8080;
httpListener.Prefixes.Add($"http://*:{port}/");

httpListener.Start();

Console.WriteLine($"Server started, port {port}");


while (true)
{
    var context = await httpListener.GetContextAsync();

    var endpointItems = context.Request.Url?.AbsolutePath?.Split("/", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    var controllerType = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.BaseType == typeof(ControllerBase))
        .FirstOrDefault(t => t.Name.ToLower() == $"{endpointItems[0].ToLower()}controller");

    if (controllerType == null)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        context.Response.Close();
        continue;
    }

    string normalizedRequestHttpMethod = context.Request.HttpMethod.ToLower();

    var controllerMethod = controllerType
        .GetMethods()
        .FirstOrDefault(m => {
            return m.GetCustomAttributes()
                .Any(attr => {
                    if (attr is HttpAttributeBase httpAttribute)
                    {
                        bool isHttpMethodCorrect = httpAttribute.MethodType.Method.ToLower() == normalizedRequestHttpMethod;

                        if (isHttpMethodCorrect)
                        {
                            if (endpointItems.Length == 1 && httpAttribute.NormalizedRouting == null)
                                return true;

                            else if (endpointItems.Length > 1)
                            {
                                if (httpAttribute.NormalizedRouting == null)
                                    return false;
                                else
                                {
                                    var expectedEndpoint = string.Join('/', endpointItems[1..]).ToLower();
                                    var actualEndpoint = httpAttribute.NormalizedRouting;

                                    return actualEndpoint == expectedEndpoint;
                                }
                            }
                        }
                    }

                    return false;
                });
        });

    if (controllerMethod == null)
    {
        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        context.Response.Close();
        continue;
    }

    var controller = (Activator.CreateInstance(controllerType) as ControllerBase)!;
    controller.HttpContext = context;
    var methodCall = controllerMethod.Invoke(controller, new object[] { });

    if (methodCall != null && methodCall is Task asyncMethod)
    {
        await asyncMethod.WaitAsync(CancellationToken.None);
    }

    context.Response.Close();
}
