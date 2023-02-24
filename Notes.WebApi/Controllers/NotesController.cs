using Microsoft.AspNetCore.Mvc;  // [Route], [ApiController], ControllerBase
using Notes.Shared; // Note
using Notes.WebApi.Repositories; // ICustomerRepository

namespace Notes.WebApi.Controllers;

// base address: api/notes
[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteRepository repo;

    // constructor injects repository registered in Startup
    public NotesController(INoteRepository repo)
    {
        this.repo = repo;
    }

    // GET: api/notes
    // GET: api/notes/?notid=[id]
    // this will always return a list of notes (but it might be empty)
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Note>))]
    public async Task<IEnumerable<Note>> GetNotes()
    {
            return await repo.RetrieveAllAsync();

    }

    //GET: api/notes/id
    [HttpGet("{id}", Name = nameof(GetNote))] // named route
    [ProducesResponseType(200, Type = typeof(Note))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetNote(int id)
    {
        Note? n = await repo.RetrieveAsync(id);
        if (n == null)
        {
            return NotFound(); // 404 Resource not found
        }
        else
        {
            return Ok(n); // 200 OK with Note in body
        }
    }

    //POST: api/notes
    //BODY: customer (JSON, XML)

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(Note))]
    [ProducesResponseType(400)]

    public async Task<IActionResult> Create([FromBody] Note n)
    {
        
        if (n == null)
        {
            return BadRequest(); // 400 Bad request
        }

        Note? addedNote = await repo.CreateAsync(n);
        if (addedNote == null)
        {
            return BadRequest("Repository failed to create repository");
        }
        else
        {
            return CreatedAtRoute(  // 201 Created
                routeName: nameof(GetNote),
                routeValues: new { id = addedNote.NoteId },
                value: addedNote
            );
        }
    }

    //PUT: api/notes/[id]
    //BODY: Customer(JSON, XML)

    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Update(int id, [FromBody] Note n)
    {
        if (n == null || n.NoteId != id)
        {
            return BadRequest();
        }
        Note? existing = await repo.RetrieveAsync(id);
        if (existing == null)
        {
            return NotFound(); // 400 Bad request
        }
        await repo.UpdateAsync(id, n);

        return new NoContentResult();
    }


    // DELETE: api/notes/[id]
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(int id)
    {
        Note? existing = await repo.RetrieveAsync(id);
        if (existing == null)
        {
            return NotFound(); // 404 Resource not found 
        }
        bool? deleted = await repo.DeleteAsync(id);
        if (deleted.HasValue && deleted.Value) // short circuit AND 
        {
            return new NoContentResult(); // 204 No content 
        }
        else
        {
            return BadRequest( // 400 Bad request
                    $"Note {id} was found but failed to delete.");
        }
    }


}