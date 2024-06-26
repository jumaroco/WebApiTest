﻿using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Security;

public class TokenService : ITokenService
{
    private readonly TestDbContext _context;
    private readonly IConfiguration _configuration;

    public TokenService(TestDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<string> CreateToken(AppUser user)
    {
        var policies = await _context.Database.SqlQuery<string>($@"
            SELECT
                aspr.ClaimValue
            FROM AspNetUsers a
                LEFT Join AspNetUserRoles ar
                    ON a.Id=ar.UserId
                LEFT Join AspNetRoleClaims aspr
                    ON ar.RoleId = aspr.RoleId
            WHERE a.Id = {user.Id}
        ").ToListAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        foreach(var policy in policies)
        {
            if (policy is not null) 
            {
                claims.Add(new(CustomClaims.POLICIES, policy));
            }
        }

        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"]!)),
            SecurityAlgorithms.HmacSha256
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
