using Microsoft.AspNetCore.Mvc;
using _2025_02_18.Models;
using _2025_02_18.Services;

namespace _2025_02_18.Controllers;

[ApiController]
[Route("[controller]")]
public class MyShoesController : ControllerBase
{

    [HttpGet("{id}")]
    public ActionResult<MyShoes> Get(int id)
    {
        MyShoes shoes = MyShoesService.Get(id);
        if (shoes == null)
            return NotFound();
        return shoes;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<MyShoes>> Get()
    {
        return MyShoesService.Get();
    }

    [HttpPost()]
    public ActionResult Post(MyShoes shoes)
    {
        int result = MyShoesService.Add(shoes);
        if(result == -1)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new { Id= result});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, MyShoes shoes)
    {
        bool result = MyShoesService.Update(id,shoes);
        if(result){
            return NoContent();
        }
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        bool result = MyShoesService.Delete(id);
        if(result){
            return Ok();
        }
        return NotFound();
    } 

}
