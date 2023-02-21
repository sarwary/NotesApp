using Notes.Shared; // Notes namespace Notes.WebApi.Repositories;

namespace Notes.WebApi.Repositories;
public interface INoteRepository
{
  Task<Note?> CreateAsync(Note n);
  Task<IEnumerable<Note>> RetrieveAllAsync();
  Task<Note?> RetrieveAsync(int id);
  Task<Note?> UpdateAsync(int id, Note n);
  Task<bool?> DeleteAsync(int id);
}