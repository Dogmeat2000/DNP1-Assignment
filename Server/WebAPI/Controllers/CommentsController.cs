using ApiContracts;
using DTOconverters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

// TODO: Investigate handling pagination and filtering!
// TODO: Investigate how to embed HATEOAS links in responses!

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase {
    private readonly ICommentRepository _commentRepository;

    public CommentsController(ICommentRepository commentRepository) {
        _commentRepository = commentRepository;
    }
    
    // EndPoints are defined below:
    
    // Create a new Comment (Create)
    [HttpPost(Name = "PostComment")]
    public async Task<ActionResult<CommentDTO>> CreateComment([FromQuery] int fId, [FromQuery] int pId, [FromBody] CommentDTO newComment) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert DTO to proper entity:
            Comment comment = CommentConverter.DTOToComment(newComment);
            
            // Attempt to add Comment to repository:
            comment = await _commentRepository.AddAsync(comment);
            
            // Return result, by looking up the created Comment:
            return CreatedAtAction(nameof(GetComment), new {fId, pId, cId = comment.Comment_id}, comment); // Hands over some exception throwing/handling to AspNetCore.
            
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read an existing Comment (Read)
    [HttpGet(("{cId:int}"), Name = "GetComment")]
    public async Task<ActionResult<CommentDTO>> GetComment([FromQuery] int fId, [FromQuery] int pId, int cId) {
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
    
    
    // Read Multiple Comments (Read), with query parameters for filtering by forumId, postId or authorId.
    [HttpGet(Name = "GetComments")]
    public ActionResult<List<CommentDTO>> GetComments([FromQuery] int fId, [FromQuery] int pId, [FromQuery] int? authorId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Query all matching Comments, within this post:
            IQueryable<Comment> comments = _commentRepository.GetMany().Where(c => c.ParentForum_id == fId && c.ParentPost_id == pId);

            Console.WriteLine("FOUND1: " + comments.Count());
            
            // If authorId was provided, remove all non-matching authored comments:
            if (authorId.HasValue)
                comments = comments.Where(a => a.Author_Id == authorId);
            
            Console.WriteLine("FOUND2: " + comments.Count());
            
            // If none were found ,throw error:
            if(!comments.Any())
                throw new KeyNotFoundException();
            
            Console.WriteLine("FOUND3: " + comments.Count());
            
            // Convert found objects to DTO:
            List<CommentDTO> results = new List<CommentDTO>();
            foreach (Comment comment in comments)
                results.Add(CommentConverter.CommentToDTO(comment));
            
            Console.WriteLine("FOUND4: " + results.Count());
            
            // return result:
            return Ok(results);
            
        } catch (KeyNotFoundException) { 
            return NotFound(); // If comment was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Replace an existing comment (Update)
    [HttpPut(("/{cId:int}"), Name = "PutComment")]
    public async Task<IActionResult> Put([FromQuery] int fId, [FromQuery] int pId, int cId, [FromBody] CommentDTO post) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert received DTO to repository entity:
            Comment commentFromClient = CommentConverter.DTOToComment(post);
        
            // Check if a Comment of this type already exists:
            Comment commentFromRepository = await _commentRepository.GetSingleAsync(cId, pId, fId);
            
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
    [HttpDelete(("/{cId:int}"), Name = "DeleteComment")]
    public async  Task<IActionResult> DeleteComment([FromQuery] int fId, [FromQuery] int pId, int cId) {
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