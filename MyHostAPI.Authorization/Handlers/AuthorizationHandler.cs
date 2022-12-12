using Microsoft.Extensions.Logging;
using MyHostAPI.Authorization.Interfaces;
using MyHostAPI.Common.Constants;
using MyHostAPI.Common.Exceptions;
using MyHostAPI.Common.Models;

namespace MyHostAPI.Authorization.Handlers
{
    public class AuthorizationHandler<T> : IAuthorizationHandler<T> where T : class
    {
        private readonly ILogger<AuthorizationHandler<T>> _logger;
        private readonly IValidations<T> _validations;

        public AuthorizationHandler(ILogger<AuthorizationHandler<T>> logger, IValidations<T> validations)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validations = validations ?? throw new ArgumentNullException(nameof(validations));
        }

        /// <summary>
        /// Authorize user to perform operation over resource
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="resource"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        public Task Authorize(UserContext userContext, T resource, Operation operation)
        {
            ValidateInputParams(userContext, resource);

            ValidateOperation(userContext, operation);

            ValidateOperationRule(userContext, operation, resource);

            _logger.LogInformation("Authorization successfully done.");

            return Task.CompletedTask;
        }

        /// <summary>
        /// Input parameters validation
        /// </summary>
        /// <param name="userContext">User who asking for resource</param>
        /// <param name="resource">Concrete resource</param>
        /// <returns></returns>
        /// <exception cref="RecordNotFoundException"></exception>
        private Task ValidateInputParams(UserContext userContext, T resource)
        {
            if (userContext == null)
            {
                _logger.LogError($"User context is null!");
                throw new RecordNotFoundException("User context is null!");
            }

            if (resource == null)
            {
                _logger.LogError($"Resource is null!");
                throw new RecordNotFoundException("Resource is null!");
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Method that check is current user allowed to start operation based on Role
        /// </summary>
        /// <param name="userContext">User who asking for resource</param>
        /// <param name="operation">Concrete operation</param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        private Task ValidateOperation(UserContext userContext, Operation operation)
        {
            if (!_validations.Rules[userContext.Role].Keys.ToList().Any(x => x == operation))
            {
                _logger.LogError($"Operation {operation} is not allowed for this user!");
                throw new UnauthorizedException($"Operation {operation} is not allowed for this user!");
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Validate all specific rules based on user role and and wanted operation
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="operation"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedException"></exception>
        private Task ValidateOperationRule(UserContext userContext, Operation operation, T resource)
        {
            if (!_validations.Rules[userContext.Role][operation].All(x => x.Invoke(userContext, resource)))
            {
                _logger.LogError($"Operation: {operation} rule is not satisfied for this user!");
                throw new UnauthorizedException($"Operation: {operation} rule is not satisfied for this user!");
            }

            return Task.CompletedTask;
        }
    }
}
