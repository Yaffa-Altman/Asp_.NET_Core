
using CoreProject.interfaces;
using CoreProject.Models;
using Microsoft.AspNetCore.Mvc;
namespace CoreProject.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IGenericService<User> userService;

    public UserController(IGenericService<User> userService)
    {
        this.userService = userService;
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        User user = userService.Get(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<User>> Get()
    {
        return userService.Get();
    }

    [HttpPost()]
    public ActionResult Post(User user)
    {
        int result = userService.Add(user);
        if(result == -1)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new { Id= result});
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, User user)
    {
        bool result = userService.Update(id,user);
        if(result){
            return NoContent();
        }
        return BadRequest();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        bool result = userService.Delete(id);
        if(result){
            return Ok();
        }
        return NotFound();
    } 


}

