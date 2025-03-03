using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using _2025_02_11.Models;


namespace _2025_02_11.Controllers;

[ApiController]
[Route("[controller]")]
public class MyPizzaController : ControllerBase
{
    private static List<MyPizza> myList;

    static MyPizzaController()
    {
        myList = new List<MyPizza> {
            new MyPizza{Id = 1, Name = "classic"},
            new MyPizza{Id = 2, Name = "family pizza"},
            new MyPizza{Id = 3, Name = "Personal pizza"}
        };
    }

    [HttpGet("{id}")]
    public ActionResult<MyPizza> Get(int id)
    {
        MyPizza pizza;
        if (get(id, out pizza))
        {
            return pizza;
        }
        return NotFound();
    }

    [HttpGet()]
    public ActionResult<IEnumerable<MyPizza>> Get()
    {
        return myList;
    }

    [HttpPost()]
    public ActionResult Post(MyPizza pizza)
    {
        int maxId = myList.Max(p => p.Id);
        pizza.Id = maxId;
        myList.Add(pizza);
        return CreatedAtAction("Post", new { id = maxId }, pizza);
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id, MyPizza newPizza)
    {
        if (id != newPizza.Id 
            || !get(id , out MyPizza pizza))
        {
            return BadRequest();
        }
        myList[myList.IndexOf(pizza)] = newPizza;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        if (!get(id , out MyPizza pizza))
        {
            return BadRequest();
        }
        myList.RemoveAt(myList.IndexOf(pizza));
        return NoContent();
    } 

    private bool get(int id, out MyPizza pizza)
    {
        pizza = myList.FirstOrDefault(p => p.Id == id);
        if (pizza == null)
                return false;
        return true;
    }
}
