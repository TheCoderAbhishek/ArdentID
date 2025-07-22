using ArdentID.Domain.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArdentID.Presentation.Filters
{
    /// <summary>
    /// An action filter that validates incoming DTOs using FluentValidation.
    /// If validation fails, it short-circuits the pipeline with a bad request.
    /// </summary>
    public class ValidationFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Called asynchronously before the action executes. Performs validation on the first non-null argument using the registered FluentValidator.
        /// </summary>
        /// <param name="context">Contextual information about the action being executed.</param>
        /// <param name="next">Delegate to invoke the next action filter or the action itself.</param>
        /// <returns>A task representing asynchronous operation.</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Find the first DTO argument in the action method.
            var argument = context.ActionArguments.Values.FirstOrDefault(v => v is not null);
            if (argument is null)
            {
                await next(); // No DTO to validate, proceed.
                return;
            }

            // Use dependency injection to find the validator for this DTO type.
            var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());

            if (context.HttpContext.RequestServices.GetService(validatorType) is not IValidator validator)
            {
                await next();
                return;
            }

            // Perform the validation.
            var validationResult = await validator.ValidateAsync(new ValidationContext<object>(argument));

            if (!validationResult.IsValid)
            {
                // If validation fails, create your standard error response and stop the request.
                var errors = validationResult.Errors.ConvertAll(e => e.ErrorMessage);
                var errorMessage = string.Join(" ", errors);

                var response = new ApiResponse<object>(
                    status: ApiResponseStatus.Failure,
                    statusCode: StatusCodes.Status400BadRequest,
                    responseCode: 4000,
                    errorMessage: errorMessage,
                    errorCode: "VALIDATION_ERROR",
                    txn: context.HttpContext.TraceIdentifier
                );

                context.Result = new BadRequestObjectResult(response);
                return;
            }

            await next();
        }
    }
}
