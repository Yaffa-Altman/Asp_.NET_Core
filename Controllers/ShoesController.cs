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
        // System.Console.WriteLine("ctor controller");

        Log.Information("start shoesController constructor");
        this.shoesService = shoesService;
        this.activeUser = au;
        Log.Information("end shoesController constructor");
    }

    [HttpGet("{id}")]
    public ActionResult<Shoes> Get(int id)
    {
        Log.Information("start shoesController Get{"+id+"}");
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            Log.Information("in shoesController Get{"+id+"} - Unauthorized");
            return Unauthorized();
        }

        var tokenValue = token.ToString().Replace("Bearer ", "");

        var id2 = activeUser.GetActiveUser(tokenValue).Id;

        Shoes shoes = shoesService.Get(id2);
        if (shoes == null || shoes.UserId != id2)
        {
            Log.Information("in shoesController Get{"+id+"} - NotFound shoes");
            return NotFound();
        }
        Log.Information("end shoesController Get{"+id+"}");
        return shoes;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Shoes>> Get()
    {
        Log.Information("start shoesController Get");
        // if (!Request.Headers.TryGetValue("Authorization", out var token))
        // {
        //     Log.Information("in shoesController Get - Unauthorized");
        //     return Unauthorized();
        // }

        // var tokenValue = token.ToString().Replace("Bearer ", "");

        // var id = activeUser.GetActiveUser(tokenValue).Id;

        var filteredShoes = shoesService.Get();//.Where(s => s.UserId == id);
        Log.Information("end shoesController Get");
        return Ok(filteredShoes);
    }

    [HttpPost()]
    public ActionResult Post(Shoes shoes)
    {
        Log.Information("start shoesController Post");
        //new shoes for the active user
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            Log.Information("in shoesController Post - Unauthorized");
            return Unauthorized();
        }

        var tokenValue = token.ToString().Replace("Bearer ", "");
        var id = activeUser.GetActiveUser(tokenValue).Id;

        shoes.UserId = id;
        int result = shoesService.Add(shoes);
        if(result == -1)
        {
            Log.Information("in shoesController Post - absent the shoes name");
            return BadRequest();
        }
        Log.Information("end shoesController Post");
        return CreatedAtAction(nameof(Post), new { Id= result});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, Shoes shoes)
    {
        Log.Information("start shoesController Put");
        if (!Request.Headers.TryGetValue("Authorization", out var token))
        {
            Log.Information("in shoesController Put - Unauthorized");
            return Unauthorized();
        }

        var tokenValue = token.ToString().Replace("Bearer ", "");
        id = activeUser.GetActiveUser(tokenValue).Id;

        shoes.UserId = id;
        bool result = shoesService.Update(id,shoes);
        if(result){
            Log.Information("end shoesController Put");
            return NoContent();
        }
        Log.Information("in shoesController Put - Absent details of shoes or shoes not found");
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        Log.Information("start shoesController Delete");
        bool result = shoesService.Delete(id);
        if(result){
            Log.Information("end shoesController Delete");
            return Ok();
        }
        Log.Information("in shoesController Delete - NotFound");
        return NotFound();
    } 

}
