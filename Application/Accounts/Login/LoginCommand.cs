using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Models;
using System.Reflection.Metadata.Ecma335;

namespace Application.Accounts.Login;

public class LoginCommand
{
    public record LoginCommandRequest(LoginRequest loginRequest) : IRequest<Result<Profile>>;

    internal class LoginCommandHandler : IRequestHandler<LoginCommandRequest, Result<Profile>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }
        public async Task<Result<Profile>> Handle(LoginCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.loginRequest.Email);
            if (user == null) return Result<Profile>.Failure("No se encontró el usuario.");

            var resultado = await _userManager.CheckPasswordAsync(user, request.loginRequest.Password!);

            if(!resultado) return Result<Profile>.Failure("Credenciales incorrectas.");

            var profile = new Profile
            {
                Email = request.loginRequest.Email,
                NombreCompleto = user.NombreCompleto,
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user)
            };

           return Result<Profile>.Success(profile);

        }


    }

    public class LoginCommandRequestValidator : AbstractValidator<LoginCommandRequest>
    {
        public LoginCommandRequestValidator()
        {
            RuleFor(x => x.loginRequest).SetValidator(new LoginValidator());
        }
    }
}
