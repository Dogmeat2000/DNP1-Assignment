using ApiContracts;
using DTOconverters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

// TODO: Investigate handling pagination and filtering!
// TODO: Investigate how to embed HATEOAS links in responses!

[ApiController]
[Route("")]
public class ForumsController : ControllerBase {
    private readonly IForumRepository _forumRepository;

    public ForumsController(IForumRepository forumRepository) {
        _forumRepository = forumRepository;
    }
    
    // EndPoints are defined below:
    
    // Create a new Forum (Create)
    [HttpPost(("/"), Name = "PostForum")]
    public async Task<ActionResult<ForumDTO>> CreateForum([FromBody] ForumDTO newForum) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert DTO to proper entity:
            Forum forum = ForumConverter.DTOToForum(newForum);
            
            // Attempt to add Forum to repository:
            forum = await _forumRepository.AddAsync(forum);
            
            // Return result, by looking up the created Forum:
            return CreatedAtAction(nameof(GetForum), new {forum.Forum_id, forum.ParentForum_id}); // Hands over some exception throwing/handling to AspNetCore.
            
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read an existing Forum (Read)
    [HttpGet(("{fId:int}"), Name = "Get")]
    public async Task<ActionResult<ForumDTO>> GetForum(int fId, int parentForumId) {
        try {
            // TODO: Validate parameters/arguments!
            
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
    [HttpGet(("/[controller]"), Name = "Get")]
    public ActionResult<List<ForumDTO>> GetForums(int fId, int parentForumdId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Query all matching Forums:
            IQueryable<Forum> forums = _forumRepository.GetMany().Where(f => f.ParentForum_id == fId && f.ParentForum_id == parentForumdId);
            
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
    [HttpPut(("/"), Name = "Put")]
    public async Task<IActionResult> Put(int fId, [FromBody] ForumDTO forum) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert received DTO to repository entity:
            Forum forumFromClient = ForumConverter.DTOToForum(forum);
        
            // Check if a Forum of this type already exists:
            Forum forumFromRepository = await _forumRepository.GetSingleAsync(forumFromClient.Forum_id, forumFromClient.ParentForum_id);
            
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
    [HttpDelete(("/{cId:int}"), Name = "Delete")]
    public async  Task<IActionResult> DeleteComment(int fId, int parentForumdId) {
        try {
            // TODO: Validate parameters/arguments!
            
            await _forumRepository.DeleteAsync(fId, parentForumdId);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) {
            return NotFound(); // If forum was not found:
        } catch (Exception) {
            return ValidationProblem(); // If some other problem occured:
        }
    }
}