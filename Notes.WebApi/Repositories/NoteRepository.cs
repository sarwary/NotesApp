using Microsoft.EntityFrameworkCore.ChangeTracking;
using Notes.Shared;
using System.Collections.Concurrent;

namespace Notes.WebApi.Repositories;

public class NoteRepository : INoteRepository
{
    public static ConcurrentDictionary<int, Note>? NotesCache;

    private NotesContext db;

    public NoteRepository(NotesContext injectedContext)
    {
        db = injectedContext;

        if (NotesCache is null)
        {
            NotesCache = new ConcurrentDictionary<int, Note>(
                db.Notes.ToDictionary(c => c.NoteId)
            );
        }
    }

    public async Task<Note?> CreateAsync(Note n){
        
        EntityEntry<Note> added = await db.Notes.AddAsync(n);
        int affected = await db.SaveChangesAsync();
       if (affected==1)
       {
         if(NotesCache is null) return n;

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