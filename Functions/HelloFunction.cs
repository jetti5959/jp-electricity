// Namespace for HTTP status codes
using System.Net;

// Azure Function attributes
using Microsoft.Azure.Functions.Worker;

// HTTP Request and Response classes
using Microsoft.Azure.Functions.Worker.Http;

namespace JPAzureFunctions.Functions;

public class HelloFunction
{
	// Function Name visible to Azure Runtime
	[Function("HelloFunction")]

	// HTTP Trigger Function
	public HttpResponseData Run(

		// Allow GET request without authentication
		[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "hello")]

        // Incoming HTTP Request
        HttpRequestData request)

	{
		// Create HTTP Response
		var response = request.CreateResponse(HttpStatusCode.OK);

		// Return JSON Response
		response.WriteAsJsonAsync(new
		{
			message = "Hello Hitesh!",
			project = "JP Azure Functions",
			status = "Working"
		});

		// Return response to browser/Postman
		return response;
	}
}