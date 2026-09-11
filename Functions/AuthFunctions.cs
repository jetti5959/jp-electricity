using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using JPAzureFunctions.Models;
using JPAzureFunctions.Models.DTOs;
using JPAzureFunctions.Services.Interfaces;

namespace JPAzureFunctions.Functions;

public class AuthFunctions
{
    private readonly IAuthService _authService;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AuthFunctions(IAuthService authService)
    {
        _authService = authService;
    }

    // ---------------- REGISTER API ----------------
    [Function("RegisterUser")]
    public async Task<HttpResponseData> Register(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "register")]
        HttpRequestData request)
    {
        try
        {
            var requestBody = await request.ReadAsStringAsync();

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

            var user = JsonSerializer.Deserialize<User>(requestBody, JsonOptions);

            if (user is null)
            {
                var response = request.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Invalid request body."
                });

                return response;
            }

            var result = await _authService.RegisterAsync(user);

            var statusCode = result.Success
                ? HttpStatusCode.Created
                : HttpStatusCode.BadRequest;

            var registerResponse = request.CreateResponse(statusCode);

            await registerResponse.WriteAsJsonAsync(new
            {
                success = result.Success,
                message = result.Message
            });

            return registerResponse;
        }
        catch (Exception ex)
        {
            var response = request.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteAsJsonAsync(new
            {
                success = false,
                message = ex.Message
            });

            return response;
        }
    }

    // ---------------- LOGIN API ----------------
    [Function("LoginUser")]
    public async Task<HttpResponseData> Login(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "login")]
        HttpRequestData request)
    {
        try
        {
            var requestBody = await request.ReadAsStringAsync();

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

            var loginRequest = JsonSerializer.Deserialize<LoginRequest>(requestBody, JsonOptions);

            if (loginRequest is null)
            {
                var response = request.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Invalid login request."
                });

                return response;
            }

            var result = await _authService.LoginAsync(loginRequest);

            if (result is null)
            {
                var response = request.CreateResponse(HttpStatusCode.Unauthorized);
                await response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Invalid email or password."
                });

                return response;
            }

            var successResponse = request.CreateResponse(HttpStatusCode.OK);
            await successResponse.WriteAsJsonAsync(result);

            return successResponse;
        }
        catch (Exception ex)
        {
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