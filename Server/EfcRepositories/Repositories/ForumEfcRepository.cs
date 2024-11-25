using Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories.Repositories;

public class ForumEfcRepository : IForumRepository {
    
    private readonly LocalAppContext _context;

    public ForumEfcRepository(LocalAppContext context) {
        _context = context;
    }
    
    public async Task<Forum> AddAsync(Forum forum) {
        try {
            EntityEntry<Forum> entityEntry = await _context.Forums.AddAsync(forum);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        } catch (DbUpdateException dbEx)
        {
            if (dbEx.InnerException is SqliteException sqliteEx)
            {
                if (sqliteEx.Message.Contains("FOREIGN KEY constraint failed"))
                {
                    Console.WriteLine($"Foreign Key Constraint Failed: {sqliteEx.Message}");
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.StackTrace);
            return null;
        }
    }

    public async Task UpdateAsync(Forum forum) {
        if (!(await _context.Forums.AnyAsync(f => f.Forum_id == forum.Forum_id))) {
            throw new KeyNotFoundException($"Forum with ID '{forum.Forum_id}' not found");
        }
        _context.Forums.Update(forum);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int forumId, int? parentForumId) {
        Forum? existing = await _context.Forums.SingleOrDefaultAsync(f => f.Forum_id == forumId && f.ParentForum_id == parentForumId);
        if (existing == null) {
            throw new KeyNotFoundException($"Forum with ID '{forumId}' not found");
        }
        _context.Forums.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<Forum> GetSingleAsync(int forumId, int? parentForumId) {
        Forum? existing = await _context.Forums.SingleOrDefaultAsync(f => f.Forum_id == forumId && f.ParentForum_id == parentForumId);
        if (existing == null) {
            throw new KeyNotFoundException($"Forum with ID '{forumId}' not found");
        }
        return existing;
    }

    public IQueryable<Forum> GetMany() {
        return _context.Forums.AsQueryable();
    }
}