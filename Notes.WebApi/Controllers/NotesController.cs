using Microsoft.AspNetCore.Mvc;
using Notes.Shared;
using Notes.WebApi.Repositories;

namespace Notes.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteRepository repo;

    public NotesController(INoteRepository repo)
    {
        this.repo = repo;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Note>))]
    public async Task<IEnumerable<Note>> GetNotes(int? id)
    {
        if (id is null)
        {
            return await repo.RetrieveAllAsync();

        }
        else
        {
            return (await repo.RetrieveAllAsync()).Where(note => note.NoteId == id);
        }
    }

    //GET: api/notes/id
    [HttpGet("{id}", Name = nameof(GetNote))]
    [ProducesResponseType(200, Type = typeof(Note))]
    [ProducesResponseType(404)]

    public async Task<IActionResult> GetNote(int id)
    {
        Note? n = await repo.RetrieveAsync(id);
        if (n == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(n);
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
            return BadRequest("not is null");
        }

        Note? addedNote = await repo.CreateAsync(n);
        if (addedNote == null)
        {
            return BadRequest("Repository failed to create repository");
        }
        else
        {
            return CreatedAtRoute(
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

            WriteLine("alles gut");
            return BadRequest();
        }
        Note? existing = await repo.RetrieveAsync(id);
        if (existing == null)
        {
            return NotFound();
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