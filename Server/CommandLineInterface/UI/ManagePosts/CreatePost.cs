using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.ManagePosts;

public class CreatePost {
    
    public async Task<Post?> NewPostAsync(int parentForumId, IPostRepository postRepo, User localUser) {
        
        Console.WriteLine(": Creating Post... Type 'abort' at any step to abort...");
        
        Console.Write("\nPlease enter a title: ");
        string title = ReadUserInput() ?? "UNSPECIFIED TITLE";
        if(CheckForAbort(title))
            return null;
        
        Console.Write("\nPlease enter body text: ");
        string body = ReadUserInput() ?? "UNSPECIFIED BODY";
        if(CheckForAbort(title))
            return null;
        
        // Create the post
        Post newPost = new Post();
        newPost.ParentForum_id = parentForumId;
        newPost.Title_txt = title;
        newPost.Body_txt = body;
        newPost.Author_id = localUser.User_id;
        
        // Attempt to add post
        Post postAdded = await postRepo.AddAsync(newPost);
        
        return postAdded;
    }
    
    /** True: Abort was entered.
     * False: Do not abort.
     */
    private bool CheckForAbort(string input) {
        if (input.Equals("abort"))
            return true;
        return false;
    }
    
    private string? ReadUserInput() {
        var inputReceived = false;
        string? userInput = null;
        while (!inputReceived) {
            userInput = Console.ReadLine();

            if (userInput != null)
                inputReceived = true;
        }
        return userInput;
    }
}