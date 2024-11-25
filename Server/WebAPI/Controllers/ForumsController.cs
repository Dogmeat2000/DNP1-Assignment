using ApiContracts;
using DTOconverters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;

namespace WebAPI.Controllers;

// TODO: Investigate handling pagination and filtering!
// TODO: Investigate how to embed HATEOAS links in responses!

[ApiController]
[Route("[controller]")]
public class ForumsController : ControllerBase {
    private readonly IForumRepository _forumRepository;

    public ForumsController(IForumRepository forumRepository) {
        _forumRepository = forumRepository;
    }
    
    // EndPoints are defined below:
    
    // Create a new Forum (Create)
    [HttpPost(Name = "PostForum")]
    public async Task<ActionResult<ForumDTO>> CreateForum([FromBody] ForumDTO newForum) {
        try {
            // Validate parameters/arguments!
            if (newForum == null)
                throw new BadHttpRequestException("Forum object is null");

            if (newForum.ParentForum_id == 0)
                newForum.ParentForum_id = null;
            
            if (newForum.LastComment_id == 0)
                newForum.LastComment_id = null;
            
            if(newForum.LastPost_id == 0)
                newForum.LastPost_id = null;
            
            if(newForum.LastCommentPost_id == 0)
                newForum.LastCommentPost_id = null;
            
            if(newForum.Timestamp_modified != null && newForum.Timestamp_modified == DateTime.MinValue)
                newForum.Timestamp_modified = null;
            
            if(newForum.Timestamp_deleted != null && newForum.Timestamp_deleted == DateTime.MinValue)
                newForum.Timestamp_deleted = null;
            
            // Convert DTO to proper entity:
            Forum forum = ForumConverter.DTOToForum(newForum);
            
            // Attempt to add Forum to repository:
            forum = await _forumRepository.AddAsync(forum);
            
            // Return result, by looking up the created Forum:
            return CreatedAtAction(nameof(GetForum), new {fId = forum.Forum_id, parentForumId = forum.ParentForum_id}, forum); // Hands over some exception throwing/handling to AspNetCore.
            
        } catch (Exception e) {
            Console.WriteLine(e.StackTrace);
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read an existing Forum (Read)
    [HttpGet(("{fId:int}"), Name = "GetForum")]
    public async Task<ActionResult<ForumDTO>> GetForum(int fId, [FromQuery] int? parentForumId) {
        try {
            // Validate parameters/arguments!
            if(parentForumId != null && parentForumId == 0)
                parentForumId = null;
            
            // Attempt to retrieve Forum from repository:
            Forum? forum = await _forumRepository.GetSingleAsync(fId, parentForumId);
            
            // Convert to DTO as response:
            var result = ForumConverter.ForumToDTO(forum);
            
            // return result:
            return Ok(result);
        } catch (KeyNotFoundException) { 
            return NotFound(); // If forum was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read Multiple Forums (Read), filtered by forum_id.
    [HttpGet(Name = "GetForums")]
    public async Task<ActionResult<List<ForumDTO>>> GetForums([FromQuery] int? parentForumId) {
        try {
            // Validate parameters/arguments!
            if(parentForumId != null && parentForumId == 0)
                parentForumId = null;
            
            // Query all matching Forums:
            IEnumerable<Forum> forums = await _forumRepository.GetMany().Where(f => f.ParentForum_id == parentForumId).ToListAsync();
            
            // If none were found, throw error:
            if(!forums.Any())
                throw new KeyNotFoundException();
            
            // Convert found objects to DTO:
            List<ForumDTO> results = new List<ForumDTO>();
            foreach (Forum forum in forums)
                results.Add(ForumConverter.ForumToDTO(forum));
            
            // return result:
            return Ok(results);
            
        } catch (KeyNotFoundException) { 
            return NotFound(); // If forum was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Replace an existing forum (Update)
    [HttpPut(("{fId:int}"), Name = "PutForum")]
    public async Task<IActionResult> Put([FromQuery] int fId, [FromBody] ForumDTO forum) {
        try {
            // Validate parameters/arguments!
            if (forum == null)
                throw new BadHttpRequestException("Forum object is null");

            if (forum.ParentForum_id == 0)
                forum.ParentForum_id = null;
            
            if (forum.LastComment_id == 0)
                forum.LastComment_id = null;
            
            if(forum.LastPost_id == 0)
                forum.LastPost_id = null;
            
            if(forum.LastCommentPost_id == 0)
                forum.LastCommentPost_id = null;
            
            if(forum.Timestamp_modified != null && forum.Timestamp_modified == DateTime.MinValue)
                forum.Timestamp_modified = null;
            
            if(forum.Timestamp_deleted != null && forum.Timestamp_deleted == DateTime.MinValue)
                forum.Timestamp_deleted = null;
            
            // Convert received DTO to repository entity:
            Forum forumFromClient = ForumConverter.DTOToForum(forum);
        
            // Check if a Forum of this type already exists:
            Forum forumFromRepository = await _forumRepository.GetSingleAsync(fId, forumFromClient.ParentForum_id ?? 0);
            
            // Update Forum properties:
            forumFromRepository.Title_txt = forumFromClient.Title_txt;
            forumFromRepository.Author_id = forumFromClient.Author_id;
            
            // Update the entity in repository:
            await _forumRepository.UpdateAsync(forumFromRepository);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) { 
            return NotFound(); // If forum was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Remove an existing forum (Delete)
    [HttpDelete(("{fId:int}"), Name = "DeleteForum")]
    public async  Task<IActionResult> DeleteComment(int fId, [FromQuery] int? parentForumId) {
        try {
            // Validate parameters/arguments!
            if(parentForumId != null && parentForumId == 0)
                parentForumId = null;
            
            await _forumRepository.DeleteAsync(fId, parentForumId);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) {
            return NotFound(); // If forum was not found:
        } catch (Exception) {
            return ValidationProblem(); // If some other problem occured:
        }
    }
}