using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CoreProject.Models;
using CoreProject.Services;
using CoreProject.interfaces;


namespace CoreProject.Controllers;

public class LoginController: ControllerBase
{
    private IGenericService<User> userService;
    private readonly JsonService<User> jsonService;

    public LoginController(IGenericService<User> userService, JsonService<User> jsonService) { 
        this.userService = userService;
        this.jsonService = jsonService;
    }

    [HttpPost]
    [Route("[action]")]
    public ActionResult<String> Login([FromBody] User user)
    {
        var users = jsonService.GetItems();
        var currentUser = users.FirstOrDefault(u => u.Name == user.Name && u.Password == user.Password);
        
        if(currentUser == null)
            return Unauthorized();
        var claims = new List<Claim>();
        if ((user.Name == "Yaffi Altman" 
        && user.Password == "YP")
        ||(user.Name == "Tzipi Klarberg" 
        && user.Password == "TzipiPassword"))
        {
            claims.Add(new Claim("type", "ADMIN"));
            claims.Add(new Claim("id", currentUser.Id.ToString()));
            claims.Add(new Claim("name", currentUser.Name));
        }
        else
        {
            
            claims.Add(new Claim("type", "USER"));
            claims.Add(new Claim("id", currentUser.Id.ToString()));
            claims.Add(new Claim("name", currentUser.Name));
        }
        var token = TokenService.GetToken(claims);
        // Response.Cookies.Append("token", TokenService.WriteToken(token));
        string aa = TokenService.WriteToken(token);
        System.Console.WriteLine("login");
        System.Console.WriteLine(aa);
        System.Console.WriteLine(claims.Count());
        System.Console.WriteLine("-----");
        return Ok(TokenService.WriteToken(token));
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