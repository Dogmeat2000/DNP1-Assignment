using ApiContracts;
using DTOconverters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

// TODO: Investigate handling pagination and filtering!
// TODO: Investigate how to embed HATEOAS links in responses!

[ApiController]
[Route("{fId:int}/{pId:int}")]
public class CommentsController : ControllerBase {
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository) {
        _commentRepository = commentRepository;
    }
    
    // EndPoints are defined below:
    
    // Create a new Comment (Create)
    [HttpPost(("/"), Name = "PostComment")]
    public async Task<ActionResult<CommentDTO>> CreateComment(int fId, int pId, [FromBody] CommentDTO newPost) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert DTO to proper entity:
            Comment comment = CommentConverter.DTOToComment(newPost);
            
            // Attempt to add Comment to repository:
            comment = await _commentRepository.AddAsync(comment);
            
            // Return result, by looking up the created Comment:
            return CreatedAtAction(nameof(GetComment), new {fId, pId, comment.Comment_id}); // Hands over some exception throwing/handling to AspNetCore.
            
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read an existing Comment (Read)
    [HttpGet(("{cId:int}"), Name = "Get")]
    public async Task<ActionResult<CommentDTO>> GetComment(int fId, int pId, int cId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Attempt to retrieve Comment from repository:
            Comment? comment = await _commentRepository.GetSingleAsync(cId, pId, fId);
            
            // Convert to DTO as response:
            var result = CommentConverter.CommentToDTO(comment);
            
            // return result:
            return Ok(result);
        } catch (KeyNotFoundException) { 
            return NotFound(); // If comment was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read Multiple Comments (Read), filtered by forum and post id.
    [HttpGet(("/[controller]"), Name = "Get")]
    public ActionResult<List<CommentDTO>> GetComments(int fId, int pId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Query all matching Comments:
            IQueryable<Comment> comments = _commentRepository.GetMany().Where(c => c.ParentForum_id == fId && c.ParentPost_id == pId);
            
            // If none were found ,throw error:
            if(!comments.Any())
                throw new KeyNotFoundException();
            
            // Convert found objects to DTO:
            List<CommentDTO> results = new List<CommentDTO>();
            foreach (Comment comment in comments)
                results.Add(CommentConverter.CommentToDTO(comment));
            
            // return result:
            return Ok(results);
            
        } catch (KeyNotFoundException) { 
            return NotFound(); // If comment was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read Multiple Comments (Read), filtered by forum, post and user_id
    [HttpGet(("/[controller]"), Name = "Get")]
    public ActionResult<List<CommentDTO>> GetCommentsByAuthor(int fId, int pId, int authorId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Query all matching Comments:
            IQueryable<Comment> comments = _commentRepository.GetMany().Where(
                c => c.ParentForum_id == fId && c.ParentPost_id == pId && c.Author_Id == authorId);
            
            // If none were found ,throw error:
            if(!comments.Any())
                throw new KeyNotFoundException();
            
            // Convert found objects to DTO:
            List<CommentDTO> results = new List<CommentDTO>();
            foreach (Comment comment in comments)
                results.Add(CommentConverter.CommentToDTO(comment));
            
            // return result:
            return Ok(results);
            
        } catch (KeyNotFoundException) { 
            return NotFound(); // If comment was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Replace an existing comment (Update)
    [HttpPut(("/[controller]"), Name = "Put")]
    public async Task<IActionResult> Put(int fId, int pId, [FromBody] CommentDTO post) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert received DTO to repository entity:
            Comment commentFromClient = CommentConverter.DTOToComment(post);
        
            // Check if a Comment of this type already exists:
            Comment commentFromRepository = await _commentRepository.GetSingleAsync(commentFromClient.Comment_id, pId, fId);
            
            // Update Comment properties:
            commentFromRepository.Body_txt = commentFromClient.Body_txt;
            commentFromRepository.Author_Id = commentFromClient.Author_Id;
            
            // Update the entity in repository:
            await _commentRepository.UpdateAsync(commentFromRepository);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) { 
            return NotFound(); // If comment was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Remove an existing comment (Delete)
    [HttpDelete(("/{cId:int}"), Name = "Delete")]
    public async  Task<IActionResult> DeleteComment(int fId, int pId, int cId) {
        try {
            // TODO: Validate parameters/arguments!
            
            await _commentRepository.DeleteAsync(fId, pId, cId);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) {
            return NotFound(); // If comment was not found:
        } catch (Exception) {
            return ValidationProblem(); // If some other problem occured:
        }
    }
}