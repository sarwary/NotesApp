using Notes.Shared; // Notes namespace Notes.WebApi.Repositories;

namespace Notes.WebApi.Repositories;
public interface INoteRepository
{
  Task<Note?> CreateAsync(Note n); //for POST method
  Task<IEnumerable<Note>> RetrieveAllAsync(); // for GET method: api/notes/
  Task<Note?> RetrieveAsync(int id); //for GET method: api/notes/[id]
  Task<Note?> UpdateAsync(int id, Note n); //for PUT method: api/notes/
  Task<bool?> DeleteAsync(int id); //for DELETE mehtod: api/notes/[id]
}