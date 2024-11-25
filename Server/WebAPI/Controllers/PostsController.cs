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
public class PostsController : ControllerBase {
    private readonly IPostRepository _postRepository;

    public PostsController(IPostRepository postRepository) {
        _postRepository = postRepository;
    }
    
    // EndPoints are defined below:
    
    // Create a new Post (Create)
    [HttpPost(Name = "PostPost")]
    public async Task<ActionResult<PostDTO>> CreatePost([FromBody] PostDTO newPost) {
        try {
            // Validate parameters/arguments!
            if (newPost == null)
                throw new BadHttpRequestException("Forum object is null");

            if (newPost.ParentForum_id == 0)
                newPost.ParentForum_id = null;
            
            if(newPost.Timestamp_modified != null && newPost.Timestamp_modified == DateTime.MinValue)
                newPost.Timestamp_modified = null;
            
            if(newPost.Timestamp_deleted != null && newPost.Timestamp_deleted == DateTime.MinValue)
                newPost.Timestamp_deleted = null;
            
            // Convert DTO to proper entity:
            Post post = PostConverter.DTOToPost(newPost);
            
            // Attempt to add Post to repository:
            post = await _postRepository.AddAsync(post);
            
            // Return result, by looking up the created Post:
            return CreatedAtAction(nameof(GetPost), new {fId = post.ParentForum_id, pId = post.Post_id}, post); // Hands over some exception throwing/handling to AspNetCore.
            
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read an existing Post (Read)
    [HttpGet(("{pId:int}"), Name = "GetPost")]
    public async Task<ActionResult<PostDTO>> GetPost(int pId, [FromQuery] int? fId) {
        try {
            // Validate parameters/arguments!
            if (fId != null && fId == 0)
                fId = null;
            
            // Attempt to retrieve Post from repository:
            Post? post = await _postRepository.GetSingleAsync(pId, fId);
            
            // Convert to DTO as response:
            var result = PostConverter.PostToDTO(post);
            
            // return result:
            return Ok(result);
        } catch (KeyNotFoundException) { 
            return NotFound(); // If post was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read Multiple Posts (Read), filtered by forum_id, with optional parameters for filtering by title contents or authorId
    [HttpGet(Name = "GetPosts")]
    public async Task<ActionResult<List<PostDTO>>> GetPosts([FromQuery] int? fId, [FromQuery] string? searchString, [FromQuery] int? authorId) {
        try {
            // Validate parameters/arguments!
            if (fId != null && fId == 0)
                fId = null;
            
            if(authorId != null && authorId == 0)
                authorId = null;
            
            // Query all matching Posts:
            IEnumerable<Post> posts = await _postRepository.GetMany().Where(p => p.ParentForum_id == fId).ToListAsync();
            
            if(authorId.HasValue)
                posts = posts.Where(p => p.Author_id == authorId);
  
            if(searchString != null)
                posts = posts.Where(p => p.Title_txt.Contains(searchString));
            
            // If none were found, throw error:
            if(!posts.Any())
                throw new KeyNotFoundException();
            
            // Convert found objects to DTO:
            List<PostDTO> results = new List<PostDTO>();
            foreach (Post post in posts)
                results.Add(PostConverter.PostToDTO(post));
            
            // return result:
            return Ok(results);
            
        } catch (KeyNotFoundException) { 
            return NotFound(); // If post was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Replace an existing post (Update)
    [HttpPut(("{pId:int}"), Name = "PutPost")]
    public async Task<IActionResult> Put([FromQuery] int? fId, int pId, [FromBody] PostDTO post) {
        try {
            // Validate parameters/arguments!
            if (fId != null && fId == 0)
                fId = null;
            
            if (post == null)
                throw new BadHttpRequestException("Forum object is null");

            if (post.ParentForum_id == 0)
                post.ParentForum_id = null;
            
            if(post.Timestamp_modified != null && post.Timestamp_modified == DateTime.MinValue)
                post.Timestamp_modified = null;
            
            if(post.Timestamp_deleted != null && post.Timestamp_deleted == DateTime.MinValue)
                post.Timestamp_deleted = null;
            
            // Convert received DTO to repository entity:
            Post postFromClient = PostConverter.DTOToPost(post);
        
            // Check if a Post of this type already exists:
            Post postFromRepository = await _postRepository.GetSingleAsync(pId, fId);
            
            // Update Post properties:
            postFromRepository.Title_txt = postFromClient.Title_txt;
            postFromRepository.Body_txt = postFromClient.Body_txt;
            postFromRepository.Author_id = postFromClient.Author_id;
            
            // Update the entity in repository:
            await _postRepository.UpdateAsync(postFromRepository);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) { 
            return NotFound(); // If post was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Remove an existing post (Delete)
    [HttpDelete(("{pId:int}"), Name = "DeletePost")]
    public async  Task<IActionResult> DeletePost([FromQuery] int? fId, int pId) {
        try {
            // Validate parameters/arguments!
            if (fId != null && fId == 0)
                fId = null;
            
            await _postRepository.DeleteAsync(pId, fId);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) {
            return NotFound(); // If post was not found:
        } catch (Exception) {
            return ValidationProblem(); // If some other problem occured:
        }
    }
}