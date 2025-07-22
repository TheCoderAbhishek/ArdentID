using ArdentID.Application.DTOs.Authentication;
using ArdentID.Application.Interfaces;
using ArdentID.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ArdentID.Presentation.Controllers
{
    /// <summary>
    /// Handles user authentication, registration, and token management.
    /// </summary>
    /// <param name="logger">Logger for logging controller-specific events.</param>
    /// <param name="authenticationService">The service for handling authentication logic.</param>
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthenticateController(ILogger<AuthenticateController> logger, IAuthenticationService authenticationService) : ControllerBase
    {
        private readonly ILogger<AuthenticateController> _logger = logger;
        private readonly IAuthenticationService _authenticationService = authenticationService;

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="userRegistrationDto">The data required to register a new user.</param>
        /// <returns>The ID of the newly created user and a success message.</returns>
        [ProducesResponseType(typeof(ApiResponse<(Guid, string)>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("RegisterUserAsync")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserRegistrationDto userRegistrationDto)
        {
            var transactionId = HttpContext.TraceIdentifier;
            try
            {
                // 1. Call the service layer to perform the registration logic.
                var (userId, message) = await _authenticationService.RegisterUserAsync(userRegistrationDto);

                // 2. Log the successful registration.
                _logger.LogInformation("Transaction {Txn}: User with ID {UserId} registered successfully.", transactionId, userId);

                // 3. Create a successful API response using your custom structure.
                var response = new ApiResponse<(Guid, string)>(
                    status: ApiResponseStatus.Success,
                    statusCode: StatusCodes.Status200OK,
                    responseCode: 2000,
                    successMessage: message,
                    txn: transactionId,
                    returnValue: (userId, message)
                );

                // 4. Return an HTTP 200 OK response with the payload.
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction {Txn}: An unexpected error occurred during user registration.", transactionId);

                var response = new ApiResponse<object>(
                    status: ApiResponseStatus.Failure,
                    statusCode: StatusCodes.Status500InternalServerError,
                    responseCode: 5000,
                    errorMessage: $"An unexpected internal server error occurred. Please try again later. {ex.Message}",
                    errorCode: ErrorCode._internalServerError,
                    txn: transactionId
                );
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Asynchronously verifies if a user's provided password is correct.
        /// </summary>
        /// <param name="email">The email address of the user whose password is to be verified.</param>
        /// <param name="plainPassword">The plaintext password to check.</param>
        /// <returns>An <see cref="ApiResponse{T}"/> of type <see cref="bool"/>, indicating if the password was successfully verified.</returns>
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("PasswordVerifyAsync")]
        public async Task<IActionResult> PasswordVerifyAsync(string email, string plainPassword)
        {
            var transactionId = HttpContext.TraceIdentifier;
            try
            {
                // 1. Call the service layer to perform the registration logic.
                var res = await _authenticationService.PasswordVerifyAsync(email, plainPassword);

                if (res)
                {
                    // 2. Log the successful registration.
                    _logger.LogInformation("Transaction {Txn}: Password verified successfully.", transactionId);

                    // 3. Create a successful API response using your custom structure.
                    var response = new ApiResponse<bool>(
                        status: ApiResponseStatus.Success,
                        statusCode: StatusCodes.Status200OK,
                        responseCode: 2000,
                        successMessage: "Password verified successfully.",
                        txn: transactionId,
                        returnValue: res
                    );

                    // 4. Return an HTTP 200 OK response with the payload.
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<bool>(
                        status: ApiResponseStatus.Failure,
                        statusCode: StatusCodes.Status400BadRequest,
                        responseCode: 4000,
                        errorMessage: "Invalid password provided.",
                        txn: transactionId,
                        returnValue: res
                    );

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction {Txn}: An unexpected error occurred during password verification.", transactionId);

                var response = new ApiResponse<object>(
                    status: ApiResponseStatus.Failure,
                    statusCode: StatusCodes.Status500InternalServerError,
                    responseCode: 5000,
                    errorMessage: "An unexpected internal server error occurred. Please try again later.",
                    errorCode: ErrorCode._internalServerError,
                    txn: transactionId
                );
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
