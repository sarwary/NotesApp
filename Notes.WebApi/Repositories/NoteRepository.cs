using Microsoft.EntityFrameworkCore.ChangeTracking; // EntityEntry<T>
using Notes.Shared; //Note
using System.Collections.Concurrent; // ConcurrentDictionary

namespace Notes.WebApi.Repositories;

public class NoteRepository : INoteRepository
{
     // Use a static thread-safe dictionary field to cache the Notes.
    public static ConcurrentDictionary<int, Note>? NotesCache;

// Use an instance data context field because it should not be
// cached due to the data context having internal caching.
    private NotesContext db;

    public NoteRepository(NotesContext injectedContext)
    {
        db = injectedContext;

        // Pre-load customers from database as a normal
        // Dictionary with CustomerId as the key,
        // then convert to a thread-safe ConcurrentDictionary.

        if (NotesCache is null)
        {
            NotesCache = new ConcurrentDictionary<int, Note>(
                db.Notes.ToDictionary(n => n.NoteId)
            );
        }
    }

    public async Task<Note?> CreateAsync(Note n){

        // Add to database using EF Core.
        EntityEntry<Note> added = await db.Notes.AddAsync(n);
        int affected = await db.SaveChangesAsync();
       if (affected==1)
       {
         if(NotesCache is null) return n;
         
            // If the customer is new, add it to cache, else
            // call UpdateCache method.
        return NotesCache.AddOrUpdate(n.NoteId,n,UpdateCache);
       }else
       {
        return null;
       }
    }


    public Task<IEnumerable<Note>> RetrieveAllAsync(){
        return Task.FromResult(NotesCache is null? Enumerable.Empty<Note>():NotesCache.Values);
    }

    public Task<Note?> RetrieveAsync(int id){
       
        if(NotesCache is null) return null;
        NotesCache.TryGetValue(id, out Note n);

        return Task.FromResult(n);
    }

    private Note UpdateCache(int id, Note n){
        Note? old;
        if(NotesCache is null){
            if (NotesCache.TryGetValue(id, out old))
            {
                if (NotesCache.TryUpdate(id,n,old))
                {
                    return n;
                }
            }
        }
        return null;
    }

    public async Task<Note?> UpdateAsync(int id, Note n){
        
        db.Notes.Update(n);
        int affected =  await db.SaveChangesAsync();
        if (affected == 1)
        {
            return UpdateCache(id, n);
        }
        return null;

    }

    public async Task<bool?> DeleteAsync(int id){
        
        Note? n = db.Notes.Find(id);
        if (n is null)
        {
            return null;
        }

        db.Notes.Remove(n);
        int affected = await db.SaveChangesAsync();
        if(affected==1){
                if(NotesCache is null) return null;
                return NotesCache.TryRemove(id, out n);
            
        }
        else{
            return null;
        }
    }
    
}