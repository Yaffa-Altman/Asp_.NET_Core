using Microsoft.AspNetCore.Mvc;
using CoreProject.Models;
using CoreProject.interfaces;
using CoreProject.Services;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Serilog;


namespace CoreProject.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ShoesController : ControllerBase
{

    private ActiveUser activeUser;
    private IGenericService<Shoes> shoesService;

    public ShoesController(IGenericService<Shoes> shoesService, ActiveUser au)
    {
        System.Console.WriteLine("ctor controller");

        Log.Information("in shoes controller constructor");
        this.shoesService = shoesService;
        this.activeUser = au;
    }

    [HttpGet("{id}")]
    public ActionResult<Shoes> Get(int id)
    {
        // System.Console.WriteLine("get id controller");
        // System.Console.WriteLine(User.Claims);
        // System.Console.WriteLine(User.FindFirst("type")?.Value);
        // System.Console.WriteLine(User.Claims.Count());

        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            return Unauthorized();
        }

        var tokenValue = token.ToString().Replace("Bearer ", "");

        var id2 = activeUser.GetActiveUser(tokenValue).Id;

        Shoes shoes = shoesService.Get(id2);
        if (shoes == null || shoes.UserId != id2)
            return NotFound();
        return shoes;
    }

    [HttpGet]
    
    public ActionResult<IEnumerable<Shoes>> Get()
    {
        // System.Console.WriteLine("----get controller----");
        // System.Console.WriteLine(User.Claims);
        // System.Console.WriteLine(User.FindFirst("type")?.Value);
        // System.Console.WriteLine(User.Claims.Count());

        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            return Unauthorized();
        }

        var tokenValue = token.ToString().Replace("Bearer ", "");

        var id = activeUser.GetActiveUser(tokenValue).Id;

        var filteredShoes = shoesService.Get().Where(s => s.UserId == id);
        return Ok(filteredShoes);
    }

    [HttpPost()]
    public ActionResult Post(Shoes shoes)
    {
        //new shoes for the active user
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            return Unauthorized();
        }

        var tokenValue = token.ToString().Replace("Bearer ", "");
        var id = activeUser.GetActiveUser(tokenValue).Id;

        shoes.UserId = id;
        int result = shoesService.Add(shoes);
        if(result == -1)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new { Id= result});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Shoes shoes)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            return Unauthorized();
        }

        var tokenValue = token.ToString().Replace("Bearer ", "");
        id = activeUser.GetActiveUser(tokenValue).Id;

        shoes.UserId = id;
        bool result = shoesService.Update(id,shoes);
        if(result){
            return NoContent();
        }
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        bool result = shoesService.Delete(id);
        if(result){
            return Ok();
        }
        return NotFound();
    } 

}
