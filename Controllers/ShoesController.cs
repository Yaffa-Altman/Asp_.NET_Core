using Microsoft.AspNetCore.Mvc;
using CoreProject.Models;
using CoreProject.interfaces;

namespace CoreProject.Controllers;

[ApiController]
[Route("[controller]")]
public class ShoesController : ControllerBase
{

    private IGenericService<Shoes> shoesService;

    public ShoesController(IGenericService<Shoes> shoesService)
    {
        this.shoesService = shoesService;

    }

    [HttpGet("{id}")]
    public ActionResult<Shoes> Get(int id)
    {
        Shoes shoes = shoesService.Get(id);
        if (shoes == null)
            return NotFound();
        return shoes;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<Shoes>> Get()
    {
        return shoesService.Get();
    }

    [HttpPost()]
    public ActionResult Post(Shoes shoes)
    {
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
