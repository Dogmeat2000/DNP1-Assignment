using ConsoleApp1.UI.SharedLogic;
using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.ManagePosts;

public class CreatePost {
    
    public async Task<Post?> NewPostAsync(int parentForumId, IPostRepository postRepo, User localUser, CLISettings settings) {
        
        Console.ForegroundColor = settings.AppPromptTextColor;
        Console.WriteLine(": Creating Post... Type 'abort' at any step to abort...");
        Console.Write("\nPlease enter a title: ");
        Console.ResetColor();
        
        string title =  await new LocalUserManager().ReadUserInputAsync("") ?? "UNSPECIFIED TITLE";
        if(CheckForAbort(title))
            return null;
        
        Console.ForegroundColor = settings.AppPromptTextColor;
        Console.Write("\nPlease enter body text: ");
        Console.ResetColor();
        
        string body = await new LocalUserManager().ReadUserInputAsync("") ?? "UNSPECIFIED TITLE";
        if(CheckForAbort(title))
            return null;
        
        // Create the post
        Post newPost = new Post {Title_txt = title, Body_txt = body};
        newPost.ParentForum_id = parentForumId;
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
}