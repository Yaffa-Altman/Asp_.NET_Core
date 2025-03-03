using Microsoft.AspNetCore.Mvc;
using _2025_02_18.Models;
using _2025_02_25.interfaces;

namespace _2025_02_18.Controllers;

[ApiController]
[Route("[controller]")]
public class MyShoesController : ControllerBase
{

    private IShoesService shoesService;

    public MyShoesController(IShoesService shoesService)
    {
        this.shoesService = shoesService;
    }

    [HttpGet("{id}")]
    public ActionResult<MyShoes> Get(int id)
    {
        MyShoes shoes = shoesService.Get(id);
        if (shoes == null)
            return NotFound();
        return shoes;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<MyShoes>> Get()
    {
        return shoesService.Get();
    }

    [HttpPost()]
    public ActionResult Post(MyShoes shoes)
    {
        int result = shoesService.Add(shoes);
        if(result == -1)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new { Id= result});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, MyShoes shoes)
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
