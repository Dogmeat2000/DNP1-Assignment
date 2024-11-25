using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories.Repositories;

public class CommentEfcRepository : ICommentRepository {
    
    private readonly LocalAppContext _context;

    public CommentEfcRepository(LocalAppContext context) {
        _context = context;
    }
    
    public async Task<Comment> AddAsync(Comment comment) {
        EntityEntry<Comment> entityEntry = await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(Comment comment) {
        if (!(await _context.Comments.AnyAsync(c => c.Comment_id == comment.Comment_id))) {
            throw new KeyNotFoundException($"Comment with ID '{comment.Comment_id}' in Post '{comment.ParentPost_id}' in Forum '{comment.ParentForum_id}' not found");
        }
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int commentId, int postId, int? forumId) {
        Comment? existing = await _context.Comments.SingleOrDefaultAsync(c => c.Comment_id == commentId && c.ParentPost_id == postId && c.ParentForum_id == forumId);
        if (existing == null) {
            throw new KeyNotFoundException($"Comment with ID '{commentId}' in Post '{postId}' in Forum '{forumId}' not found");
        }
        _context.Comments.Remove(existing);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment> GetSingleAsync(int commentId, int postId, int? forumId) {
        Comment? existing = await _context.Comments.SingleOrDefaultAsync(c => c.Comment_id == commentId && c.ParentPost_id == postId && c.ParentForum_id == forumId);
        if (existing == null) {
            throw new KeyNotFoundException($"Comment with ID '{commentId}' in Post '{postId}' in Forum '{forumId}' not found");
        }
        return existing;
    }

    public IQueryable<Comment> GetMany() {
        return _context.Comments.AsQueryable();
    }
}