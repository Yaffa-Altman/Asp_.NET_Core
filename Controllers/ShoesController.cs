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

    private User activeUser;
    private IGenericService<Shoes> shoesService;

    public ShoesController(IGenericService<Shoes> shoesService, ActiveUser au)
    {
        System.Console.WriteLine("ctor controller");

        Log.Information("in shoes controller constructor");
        this.shoesService = shoesService;
        this.activeUser = au.GetActiveUser();
    }

    [HttpGet("{id}")]
    public ActionResult<Shoes> Get(int id)
    {
        System.Console.WriteLine("get id controller");
        System.Console.WriteLine(User.Claims);
        System.Console.WriteLine(User.FindFirst("type")?.Value);
        System.Console.WriteLine(User.Claims.Count());
        Shoes shoes = shoesService.Get(id);
        if (shoes == null || shoes.UserId != activeUser.Id)
            return NotFound();
        return shoes;
    }

    [HttpGet]
    
    public ActionResult<IEnumerable<Shoes>> Get()
    {
        System.Console.WriteLine("----get controller----");
        System.Console.WriteLine(User.Claims);
        System.Console.WriteLine(User.FindFirst("type")?.Value);
        System.Console.WriteLine(User.Claims.Count());
        var filteredShoes = shoesService.Get().Where(s => s.UserId == activeUser.Id);
        return Ok(filteredShoes);
    }

    [HttpPost()]
    public ActionResult Post(Shoes shoes)
    {
        shoes.UserId = activeUser.Id;
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
        shoes.UserId = activeUser.Id;
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
