using ArdentID.Application.DTOs.Verification;
using ArdentID.Application.Interfaces.Shared;
using ArdentID.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ArdentID.Presentation.Controllers
{
    /// <summary>
    /// Handles multi-purpose verification flows, such as email confirmation and password resets.
    /// </summary>
    /// <param name="logger">Logger for logging controller-specific events.</param>
    /// <param name="verificationService">The service for handling verification logic.</param>
    [Route("v1/[controller]")]
    [ApiController]
    public class VerificationController(ILogger<VerificationController> logger, IVerificationService verificationService) : ControllerBase
    {
        private readonly ILogger<VerificationController> _logger = logger;
        private readonly IVerificationService _verificationService = verificationService;

        /// <summary>
        /// Generates and sends a One-Time Password (OTP) to a user's email for a specific purpose.
        /// </summary>
        /// <param name="request">The request containing the user's email and the purpose of the OTP.</param>
        /// <returns>A standard API response indicating the result of the operation.</returns>
        [HttpPost("generate-otp")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GenerateOtpAsync([FromBody] GenerateOtpRequestDto request)
        {
            var transactionId = HttpContext.TraceIdentifier;
            try
            {
                var message = await _verificationService.GenerateAndSendOtpAsync(request);

                _logger.LogInformation("Transaction {Txn}: OTP generated successfully for {Email} with purpose {Purpose}.", transactionId, request.Email, request.Purpose);

                var response = new ApiResponse<string>(
                    status: ApiResponseStatus.Success,
                    statusCode: StatusCodes.Status200OK,
                    responseCode: 2000,
                    successMessage: message,
                    txn: transactionId,
                    returnValue: message
                );

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Transaction {Txn}: Failed to generate OTP for {Email}. Reason: {ErrorMessage}", transactionId, request.Email, ex.Message);

                var response = new ApiResponse<object>(
                    status: ApiResponseStatus.Failure,
                    statusCode: StatusCodes.Status400BadRequest,
                    responseCode: 4004, // Custom code for "Not Found" or invalid state
                    errorMessage: ex.Message,
                    errorCode: "USER_NOT_FOUND_OR_INVALID_STATE",
                    txn: transactionId
                );
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction {Txn}: An unexpected error occurred while generating OTP for {Email}.", transactionId, request.Email);

                var response = new ApiResponse<object>(
                    status: ApiResponseStatus.Failure,
                    statusCode: StatusCodes.Status500InternalServerError,
                    responseCode: 5000,
                    errorMessage: "An unexpected internal server error occurred.",
                    errorCode: "INTERNAL_SERVER_ERROR",
                    txn: transactionId
                );
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        /// <summary>
        /// Verifies a One-Time Password (OTP) provided by a user.
        /// </summary>
        /// <param name="request">The request containing the user's email, the OTP, and its purpose.</param>
        /// <returns>A standard API response indicating if the verification was successful.</returns>
        [HttpPost("verify-otp")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> VerifyOtpAsync([FromBody] VerifyOtpRequestDto request)
        {
            var transactionId = HttpContext.TraceIdentifier;
            try
            {
                var isSuccess = await _verificationService.VerifyOtpAsync(request);

                if (isSuccess)
                {
                    _logger.LogInformation("Transaction {Txn}: OTP verification successful for {Email} with purpose {Purpose}.", transactionId, request.Email, request.Purpose);

                    var response = new ApiResponse<bool>(
                        status: ApiResponseStatus.Success,
                        statusCode: StatusCodes.Status200OK,
                        responseCode: 2000,
                        successMessage: "Verification successful.",
                        txn: transactionId,
                        returnValue: true
                    );
                    return Ok(response);
                }
                else
                {
                    _logger.LogWarning("Transaction {Txn}: OTP verification failed for {Email}. The code may be invalid or expired.", transactionId, request.Email);

                    var response = new ApiResponse<bool>(
                        status: ApiResponseStatus.Failure,
                        statusCode: StatusCodes.Status200OK, // Still a 200 OK because the operation completed, the *result* is just false.
                        responseCode: 4002, // Custom code for invalid token
                        errorMessage: "Verification failed. The code is either invalid or has expired.",
                        errorCode: "INVALID_OTP",
                        txn: transactionId,
                        returnValue: false
                    );
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction {Txn}: An unexpected error occurred during OTP verification for {Email}.", transactionId, request.Email);

                var response = new ApiResponse<object>(
                    status: ApiResponseStatus.Failure,
                    statusCode: StatusCodes.Status500InternalServerError,
                    responseCode: 5000,
                    errorMessage: "An unexpected internal server error occurred.",
                    errorCode: "INTERNAL_SERVER_ERROR",
                    txn: transactionId
                );
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
