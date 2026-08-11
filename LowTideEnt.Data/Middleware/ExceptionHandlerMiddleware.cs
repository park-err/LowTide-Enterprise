using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LowTideEnt.Domain;

namespace LowTideEnt.Infrastructure.Middleware
{
    public sealed class ExceptionHandlerMiddleware : IExceptionHandler
    {
        // private readonly ILogger<GlobalExceptionHandler> _logger;

        public ExceptionHandlerMiddleware()
        {
            // _logger = logger;
        }
        public class InvalidRequestException : Exception { }
        public class ExpectedEntityNotFoundException : Exception { }
        public class RequestedAccessException : Exception { }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            /// TODO: Add logging
            ///_logger.LogError(
            ///    exception, "Exception occurred: {Message}", exception.Message);

            var problemDetails = new ProblemDetails() { Detail = exception.Message };
            switch (exception)
            {
                case UnauthorizedAccessException:
                    problemDetails.Status = StatusCodes.Status401Unauthorized;
                    problemDetails.Title = "You cannot access this resource. Please contact administration for details.";
                    break;
                case RequestedAccessException:
                    problemDetails.Status = StatusCodes.Status403Forbidden;
                    problemDetails.Title = "Your access to this resource has been requested. An administrator will review shortly.";
                    break;
                case InvalidRequestException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "The request is missing required or expected fields.";
                    break;
                case ExpectedEntityNotFoundException:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Expected a value to return but returned null.";
                    break;
                case SqlException:
                case DbUpdateException:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    if (exception.InnerException is SqlException sqlEx)
                    {
                        // constraint violations (fk, pk, null)
                        if (sqlEx.Number == 547 || sqlEx.Number == 2627 || sqlEx.Number == 2601)
                        {
                            problemDetails.Title = "Unable to process request. Please refresh the page and try again.";
                        }
                        // invalid column name
                        else if (sqlEx.Number == 207)
                        {
                            problemDetails.Title = "This operation is not ready for use or is under construction.";
                        }
                        // query timeout
                        else if (sqlEx.Number == -2)
                        {
                            problemDetails.Title = "Connection to the database took too long to establish.";
                        }
                        problemDetails.Detail = sqlEx.Message;
                    }
                    else
                    {
                        problemDetails.Title = "Something unexpected happened.";
                    }
                    break;
                default:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;
                    problemDetails.Title = "Unexpected server error";
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }

}
