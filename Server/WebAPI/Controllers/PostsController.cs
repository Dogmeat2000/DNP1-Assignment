using ApiContracts;
using DTOconverters;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

// TODO: Investigate handling pagination and filtering!
// TODO: Investigate how to embed HATEOAS links in responses!

[ApiController]
[Route("{fId:int}")]
public class PostsController : ControllerBase {
    private readonly IPostRepository _postRepository;

    public PostsController(IPostRepository postRepository) {
        _postRepository = postRepository;
    }
    
    // EndPoints are defined below:
    
    // Create a new Post (Create)
    [HttpPost(("/"), Name = "PostPost")]
    public async Task<ActionResult<PostDTO>> CreatePost([FromBody] PostDTO newPost) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert DTO to proper entity:
            Post post = PostConverter.DTOToPost(newPost);
            
            // Attempt to add Post to repository:
            post = await _postRepository.AddAsync(post);
            
            // Return result, by looking up the created Post:
            return CreatedAtAction(nameof(GetPost), new {post.Post_id, post.ParentForum_id}); // Hands over some exception throwing/handling to AspNetCore.
            
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read an existing Post (Read)
    [HttpGet(("/{pId:int}"), Name = "Get")]
    public async Task<ActionResult<PostDTO>> GetPost(int fId, int pId) {
        try {
            // TODO: Validate parameters/arguments!
            
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
    
    
    // Read Multiple Posts (Read), filtered by forum_id.
    [HttpGet(("/[controller]"), Name = "Get")]
    public ActionResult<List<PostDTO>> GetPosts(int fId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Query all matching Forums:
            IQueryable<Post> posts = _postRepository.GetMany().Where(p => p.ParentForum_id == fId);
            
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
    
    
    // Read Multiple Posts (Read), filtered by forum_id, containing a certain String value.
    [HttpGet(("/[controller]"), Name = "Get")]
    public ActionResult<List<PostDTO>> GetPostsWithTitleContaining(int fId, string searchString) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Query all matching Forums:
            IQueryable<Post> posts = _postRepository.GetMany().Where(p => p.ParentForum_id == fId && p.Title_txt.Contains(searchString));
            
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
    
    
    // Read Multiple Posts (Read), filtered by forum_id, written by a specific user.
    [HttpGet(("/[controller]"), Name = "Get")]
    public ActionResult<List<PostDTO>> GetPostsAuthoredBy(int fId, int authorId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Query all matching Forums:
            IQueryable<Post> posts = _postRepository.GetMany().Where(p => p.ParentForum_id == fId && p.Author_id == authorId);
            
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
    [HttpPut(("/{pId:int}"), Name = "Put")]
    public async Task<IActionResult> Put(int fId, int pId, [FromBody] PostDTO post) {
        try {
            // TODO: Validate parameters/arguments!
            
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
    [HttpDelete(("/{pId:int}"), Name = "Delete")]
    public async  Task<IActionResult> DeletePost(int fId, int pId) {
        try {
            // TODO: Validate parameters/arguments!
            
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