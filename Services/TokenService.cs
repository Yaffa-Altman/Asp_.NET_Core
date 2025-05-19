using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
// using Microsoft.Extensions.Hosting;

namespace CoreProject.Services;
public static class TokenService
{
    private static SymmetricSecurityKey key
        = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                "SXkSqsKyNUyvGbnHs7ke2NCq8zQzNLW7mPmHbnZZ"));
    private static string issuer = "http://localhost:5187";
    public static SecurityToken GetToken(List<Claim> claims) =>
        new JwtSecurityToken(
            issuer,
            issuer,
            claims,
            expires: DateTime.Now.AddDays(31.0),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

    public static string WriteToken(SecurityToken token) =>
        new JwtSecurityTokenHandler().WriteToken(token);


     public static TokenValidationParameters
     GetTokenValidationParameters() => 
         new TokenValidationParameters
        {
            // ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            // ValidAudience = audience,
            ClockSkew = TimeSpan.Zero 
        };


    // internal static TokenValidationParameters ValidateToken()
    // {
    //     throw new NotImplementedException();
    // }
}
