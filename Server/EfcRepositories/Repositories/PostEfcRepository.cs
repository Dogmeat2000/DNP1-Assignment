using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories.Repositories;

public class PostEfcRepository : IPostRepository {
    
    private readonly AppContext _context;

    public PostEfcRepository(AppContext context) {
        _context = context;
    }
    
    public async Task<Post> AddAsync(Post post) {
        EntityEntry<Post> entityEntry = await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(Post post) {
        if (!(await _context.Posts.AnyAsync(p => p.Post_id == post.Post_id))) {
            throw new KeyNotFoundException($"Post with ID '{post.Post_id}' in Forum '{post.ParentForum_id}' not found");
        }
        _context.Posts.Update(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int postId, int parentForumId) {
        Post? existing = await _context.Posts.SingleOrDefaultAsync(p => p.Post_id == postId && p.ParentForum_id == parentForumId);
        if (existing == null) {
            throw new KeyNotFoundException($"Post with ID '{postId}' in Forum '{parentForumId}' not found");
        }
        _context.Posts.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<Post> GetSingleAsync(int postId, int parentForumId) {
        Post? existing = await _context.Posts.SingleOrDefaultAsync(p => p.Post_id == postId && p.ParentForum_id == parentForumId);
        if (existing == null) {
            throw new KeyNotFoundException($"Post with ID '{postId}' Forum '{parentForumId}' not found");
        }
        return existing;
    }

    public IQueryable<Post> GetMany() {
        return _context.Posts.AsQueryable();
    }
}