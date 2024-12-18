﻿using System.ComponentModel.DataAnnotations;
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
public class UserProfilesController : ControllerBase {
    private readonly IUserProfileRepository _userProfileRepository;

    public UserProfilesController(IUserProfileRepository userProfileRepository) {
        _userProfileRepository = userProfileRepository;
    }
    
    // EndPoints are defined below:
    
    // Create a new UserProfile (Create)
    [HttpPost(Name = "PostUserProfile")]
    public async Task<ActionResult<UserProfileDTO>> CreateUserProfile([FromBody] UserProfileDTO newUserProfile) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert DTO to proper entity:
            UserProfile userProfile = UserProfileConverter.DTOToUserProfile(newUserProfile);
            
            // Attempt to add UserProfile to repository:
            userProfile = await _userProfileRepository.AddAsync(userProfile);
            
            // Return result, by looking up the created UserProfile:
            return CreatedAtAction(nameof(GetUserProfile), new {pId = userProfile.Profile_id, uId = userProfile.User_id}, userProfile); // Hands over some exception throwing/handling to AspNetCore.
            
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read an existing UserProfile (Read)
    [HttpGet(("{pId:int}"), Name = "GetUserProfile")]
    public async Task<ActionResult<UserProfileDTO>> GetUserProfile(int pId, [FromQuery] int uId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Attempt to retrieve UserProfile from repository:
            UserProfile? userProfile = await _userProfileRepository.GetSingleAsync(pId, uId);
            
            // Convert to DTO as response:
            var result = UserProfileConverter.UserProfileToDTO(userProfile);
            
            // return result:
            return Ok(result);
        } catch (KeyNotFoundException) { 
            return NotFound(); // If UserProfile was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Read an existing UserProfile (Read) with a specific username
    /*[HttpGet(Name = "GetUserProfileWithSpecifiedUsername")]
    public async Task<ActionResult<PostDTO>> GetUserProfileWithSpecifiedUsername([FromQuery] string username) {
        try {
            // Validate:
            if (String.IsNullOrEmpty(username) || String.IsNullOrWhiteSpace(username))
                throw new ValidationException();
            
            // Attempt to retrieve UserProfile from repository:
            UserProfile userProfile = await _userProfileRepository.GetSingleAsync(username);
            
            // Convert to DTO as response:
            var result = UserProfileConverter.UserProfileToDTO(userProfile);
            
            // return result:
            return Ok(result);
        } catch (KeyNotFoundException) { 
            return NotFound(); // If UserProfile was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }*/
    
    
    // Read Multiple UserProfiles (Read):
    [HttpGet(Name = "GetUserProfiles")]
    public async Task<ActionResult<List<UserProfileDTO>>> GetUserProfiles([FromQuery] int uId) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Query all matching UserProfile:
            IEnumerable<UserProfile> userProfiles = await _userProfileRepository.GetMany().Where(uP => uP.User_id == uId).ToListAsync();
            
            // If none were found, throw error:
            if(!userProfiles.Any())
                throw new KeyNotFoundException();
            
            // Convert found objects to DTO:
            List<UserProfileDTO> results = new List<UserProfileDTO>();
            foreach (UserProfile userProfile in userProfiles)
                results.Add(UserProfileConverter.UserProfileToDTO(userProfile));
            
            // return result:
            return Ok(results);
            
        } catch (KeyNotFoundException) { 
            return NotFound(); // If UserProfile was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Replace an existing UserProfile (Update)
    [HttpPut(("{uPId:int}"), Name = "PutUserProfile")]
    public async Task<IActionResult> Put(int uPId, [FromQuery] int uId, [FromBody] UserProfileDTO userProfile) {
        try {
            // TODO: Validate parameters/arguments!
            
            // Convert received DTO to repository entity:
            UserProfile userProfileFromClient = UserProfileConverter.DTOToUserProfile(userProfile);
        
            // Check if a UserProfile of this type already exists:
            UserProfile userProfileFromRepository = await _userProfileRepository.GetSingleAsync(uPId, uId);
            
            // Update UserProfile properties:
            userProfileFromRepository.Username = userProfileFromClient.Username;
            userProfileFromRepository.Password = userProfileFromClient.Password;
            
            // Update the entity in repository:
            await _userProfileRepository.UpdateAsync(userProfileFromRepository);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) { 
            return NotFound(); // If UserProfile was not found:
        } catch (Exception) { 
            return ValidationProblem(); // If some other problem occured:
        }
    }
    
    
    // Remove an existing UserProfile (Delete)
    [HttpDelete(("{uPId:int}"), Name = "DeleteUserProfile")]
    public async  Task<IActionResult> DeletePost(int uPId, [FromQuery] int uId) {
        try {
            // TODO: Validate parameters/arguments!
            
            await _userProfileRepository.DeleteAsync(uPId, uId);

            // Return:
            return NoContent();

        } catch (KeyNotFoundException) {
            return NotFound(); // If UserProfile was not found:
        } catch (Exception) {
            return ValidationProblem(); // If some other problem occured:
        }
    }
}