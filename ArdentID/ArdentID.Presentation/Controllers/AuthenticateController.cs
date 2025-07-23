using ArdentID.Application.DTOs.Authentication;
using ArdentID.Application.Interfaces.Authentication;
using ArdentID.Domain.Entities.UserManagement.UserAggregate;
using ArdentID.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
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
        /// Asynchronously verifies if a user's provided password is correct.
        /// </summary>
        /// <param name="loginRequestDto">The model email address and password of the user whose password is to be verified.</param>
        /// <returns>An <see cref="ApiResponse{T}"/> of type <see cref="bool"/>, indicating if the password was successfully verified.</returns>
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [HttpPut]
        [Route("AuthenticationAsync")]
        public async Task<IActionResult> AuthenticationAsync([FromBody] LoginRequestDto loginRequestDto)
        {
            var transactionId = HttpContext.TraceIdentifier;
            try
            {
                // 1. Call the service layer to perform the registration logic.
                var res = await _authenticationService.AuthenticationAsync(loginRequestDto);

                if (res.Result)
                {
                    // 2. Log the successful registration.
                    _logger.LogInformation("Transaction {Txn}: Password verified successfully.", transactionId);

                    // 3. Create a successful API response using your custom structure.
                    var response = new ApiResponse<LoginResponseDto>(
                        status: ApiResponseStatus.Success,
                        statusCode: StatusCodes.Status200OK,
                        responseCode: 2000,
                        successMessage: "User authenticate successfully.",
                        txn: transactionId,
                        returnValue: res
                    );

                    // 4. Return an HTTP 200 OK response with the payload.
                    return Ok(response);
                }
                else
                {
                    var response = new ApiResponse<LoginResponseDto>(
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
                _logger.LogError(ex, "Transaction {Txn}: An unexpected error occurred during user authentication.", transactionId);

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
        /// Retrieves all registered users.
        /// </summary>
        /// <returns>HTTP 200 with user list or 500 on error.</returns>
        [ProducesResponseType(typeof(ApiResponse<List<User>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("GetAllUsersAsync")]
        [Authorize]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var transactionId = HttpContext.TraceIdentifier;
            try
            {
                // 1. Call the service layer to perform the registration logic.
                var data = await _authenticationService.GetAllUsersAsync();

                // 2. Log the successful registration.
                _logger.LogInformation("Transaction {Txn}: User fetched successfully.", transactionId);

                // 3. Create a successful API response using your custom structure.
                var response = new ApiResponse<List<User>>(
                    status: ApiResponseStatus.Success,
                    statusCode: StatusCodes.Status200OK,
                    responseCode: 2000,
                    successMessage: "User fetched successfully.",
                    txn: transactionId,
                    returnValue: data
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
                    errorMessage: $"We encountered an unexpected issue while fetching users. Please try again later. {ex.Message}",
                    errorCode: ErrorCode._internalServerError,
                    txn: transactionId
                );
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
