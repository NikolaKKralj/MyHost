using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MyHostAPI.Common.Configurations;
using MyHostAPI.Common.Helpers;
using MyHostAPI.Data.Interfaces;

namespace MyHostAPI.Configuration.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserRepository _userRepository;
        private readonly JwtSection _jwtSection;

        public JwtMiddleware(RequestDelegate next, IUserRepository userRepository, IConfiguration configuration)
        {
            _jwtSection = configuration.GetSection(JwtSection.Name).Get<JwtSection>();
            _next = next;
            _userRepository = userRepository;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = TokenHandler.ValidateJwtToken(token, _jwtSection);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = await _userRepository.GetAsync(userId);
            }

            await _next(context);
        }
    }
}
