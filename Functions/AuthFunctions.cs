using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using JPAzureFunctions.Models;
using JPAzureFunctions.Models.DTOs;
using JPAzureFunctions.Services.Interfaces;

namespace JPAzureFunctions.Functions
{
    /// <summary>
    /// Azure Function APIs for user authentication.
    /// Contains endpoints for user registration and login.
    /// </summary>
    public class AuthFunctions
    {
        // Service used to handle authentication business logic.
        private readonly IAuthService _authService;

        // JSON serializer configuration (accepts property names in any case).
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Constructor that injects the authentication service.
        /// </summary>
        public AuthFunctions(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registers a new user.
        /// Endpoint: POST /api/register
        /// </summary>
        [Function("RegisterUser")]
        public async Task<HttpResponseData> Register(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "register")]
            HttpRequestData request)
        {
            try
            {
                // Read the incoming request body as a JSON string.
                string requestBody = await request.ReadAsStringAsync();

                // Return 400 if the request body is empty.
                if (string.IsNullOrWhiteSpace(requestBody))
                {
                    var response = request.CreateResponse(HttpStatusCode.BadRequest);

                    await response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Request body is empty."
                    });

                    return response;
                }

                // Convert the JSON request into a User object.
                User? user = JsonSerializer.Deserialize<User>(requestBody, JsonOptions);

                // Return 400 if the JSON data is invalid.
                if (user == null)
                {
                    var response = request.CreateResponse(HttpStatusCode.BadRequest);

                    await response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Invalid user data."
                    });

                    return response;
                }

                // Call the service layer to register the user.
                var result = await _authService.RegisterAsync(user);

                // Return 201 for successful registration, otherwise 400.
                var registerResponse = request.CreateResponse(
                    result.Success ? HttpStatusCode.Created : HttpStatusCode.BadRequest);

                await registerResponse.WriteAsJsonAsync(new
                {
                    success = result.Success,
                    message = result.Message
                });

                return registerResponse;
            }
            catch (Exception ex)
            {
                // Return 500 if an unexpected error occurs.
                var response = request.CreateResponse(HttpStatusCode.InternalServerError);

                await response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = ex.Message
                });

                return response;
            }
        }

        /// <summary>
        /// Authenticates an existing user.
        /// Endpoint: POST /api/login
        /// </summary>
        [Function("LoginUser")]
        public async Task<HttpResponseData> Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")]
            HttpRequestData request)
        {
            try
            {
                // Read the incoming request body.
                string requestBody = await request.ReadAsStringAsync();

                // Return 400 if the request body is empty.
                if (string.IsNullOrWhiteSpace(requestBody))
                {
                    var response = request.CreateResponse(HttpStatusCode.BadRequest);

                    await response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Request body is empty."
                    });

                    return response;
                }

                // Convert the JSON request into a LoginRequest object.
                LoginRequest? loginRequest =
                    JsonSerializer.Deserialize<LoginRequest>(requestBody, JsonOptions);

                // Return 400 if the login request is invalid.
                if (loginRequest == null)
                {
                    var response = request.CreateResponse(HttpStatusCode.BadRequest);

                    await response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Invalid login request."
                    });

                    return response;
                }

                // Validate the user's email and password.
                LoginResponse? result = await _authService.LoginAsync(loginRequest);

                // Return 401 if authentication fails.
                if (result == null)
                {
                    var response = request.CreateResponse(HttpStatusCode.Unauthorized);

                    await response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = "Invalid email or password."
                    });

                    return response;
                }

                // Return 200 with user details if login is successful.
                var successResponse = request.CreateResponse(HttpStatusCode.OK);

                await successResponse.WriteAsJsonAsync(result);

                return successResponse;
            }
            catch (Exception ex)
            {
                // Return 500 if an unexpected error occurs.
                var response = request.CreateResponse(HttpStatusCode.InternalServerError);

                await response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = ex.Message
                });

                return response;
            }
        }
    }
}