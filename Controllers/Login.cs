using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreProject.Models;
using CoreProject.Services;

namespace CoreProject.Controllers;

public class LoginController
{
    public LoginController() { }

    [HttpPost]
    [Route("[action]")]
    public ActionResult<String> Login([FromBody] User user)
    {
        var claims;
        if (user.Username != "Miriam Levi"
        || user.Password != "MiriamPassword2023")
        {
            claims = new List<Claim>
            {
                new Claim("type", "User"),
            };
        }
        else
        {
            claims = new List<Claim>
                {
                    new Claim("type", "Admin"),
                };
        }
        var token = TokenService.GetToken(claims);
        return new OkObjectResult(TokenService.WriteToken(token));
    }

    // [HttpPost]
    // [Route("[action]")]
    // [Authorize(Policy = "Admin")]
    // public IActionResult GenerateBadge([FromBody] Agent Agent)
    // {
    //     var claims = new List<Claim>
    //         {
    //             new Claim("type", "Agent")
    //         };

    //     var token = TokenService.GetToken(claims);

    //     return new OkObjectResult(TokenService.WriteToken(token));
    // }
}