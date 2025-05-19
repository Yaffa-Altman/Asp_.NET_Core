
using CoreProject.interfaces;
using CoreProject.Models;
using CoreProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
namespace CoreProject.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IGenericService<User> userService;
    private IGenericService<Shoes> shoesService;

    private User activeUser;
    private readonly ILogger<UserController> _logger;

    public UserController(IGenericService<User> userService, IGenericService<Shoes> shoesService, ActiveUser au, ILogger<UserController> logger)
    {
        this.userService = userService;
        this.shoesService = shoesService;
        this.activeUser = au.GetActiveUser();
        _logger = logger;

    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        //פלסתר!!!!
        id = activeUser.Id;
        User user = userService.Get(id);
        if (user == null)
            return NotFound();
        return user;
    }

    [HttpGet()]
    [Authorize(Policy = "ADMIN")]
    public ActionResult<IEnumerable<User>> Get()
    {
        return userService.Get();
    }

    [HttpPost()]
    [Authorize(Policy = "ADMIN")]
    public ActionResult Post(User user)
    {
        int result = userService.Add(user);
        if (result == -1)
        {
            return BadRequest();
        }
        return CreatedAtAction(nameof(Post), new { Id = result });
    }

    // [HttpPut("{id}")]
    // public ActionResult Put(int id, User user)
    // {
    //     bool result = userService.Update(id,user);
    //     if(result){
    //         return NoContent();
    //     }
    //     return BadRequest();
    // }

    [HttpDelete("{id}")]
    [Authorize(Policy = "ADMIN")]
    public ActionResult Delete(int id)
    {
        _logger.LogInformation("id");
        // var item = Get(id);
        // if (item == null)
        // {
        //     return NotFound();
        // }
        // var user = item.Value as User;
        // string name = user.Name;
        var shoesResult = shoesService.Get();
        List<Shoes> shoes = shoesResult as List<Shoes>;

        shoes.RemoveAll(shoe => shoe.UserId == id && shoesService.Delete(shoe.Id));
        bool result = userService.Delete(id);
        if (result)
        {
            return Ok();
        }
        return NotFound();
    }


}

