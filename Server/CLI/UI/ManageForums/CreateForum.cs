using ConsoleApp1.UI.SharedLogic;
using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.ManageForums;

public class CreateForum {
    public async Task<Forum?> NewForumAsync(int parentForumId, IForumRepository forumRepo, User localUser, CLISettings settings) {
        
        Console.ForegroundColor = settings.AppPromptTextColor;
        Console.WriteLine(": Creating new Forum... Type 'abort' at any step to abort...");
        Console.Write("\nPlease enter a title: ");
        Console.ResetColor();
        
        string title =  await new LocalUserManager().ReadUserInputAsync("") ?? "abort";
        
        // Check if User 'aborted'
        if(CheckForAbort(title))
            return null;
        
        //Validate Forum Title:
        string titlevalidation = ValidateTitle(title, parentForumId, forumRepo);
        if (!titlevalidation.Equals("valid")) {
            Console.ForegroundColor = settings.ErrorTextColor;
            Console.WriteLine($"-> ERROR: Invalid title [code = {titlevalidation}]");
            Console.ResetColor();
            return null;
        }
        
        // Create the Forum
        Forum newForum = new Forum {
            ParentForum_id = parentForumId,
            Title_txt = title,
            Author_id = localUser.User_id
        };

        // Attempt to add post
        Forum forumAdded = await forumRepo.AddAsync(newForum);
        
        return forumAdded;
    }
    
    
    /** True: Abort was entered.
     * False: Do not abort.
     */
    private bool CheckForAbort(string input) {
        if (input.Equals("abort"))
            return true;
        return false;
    }


    /** Returns string containing validation code title validated:
     * valid        = Title is valid
     * errEmpty     = Title cannot be empty/blank
     * errDuplicate = There already exists a Forum with this title inside this forum area.
     * errIllegal   = There are illegal characters in the title.
     */
    private string ValidateTitle(string title, int parentForumId, IForumRepository forumRepo) {
        List<char> invalidChars = []; // Currently not used.
        
        // Check that title does not contain any illegal characters:
        foreach (var character in invalidChars)
            if (title.Contains(character))
                return "errIllegal";
        
        // Check that title is not either empty/blank:
        if (title.Length == 0 || string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
            return "errEmpty";

        try {
            // Check that title is not a duplicate:
            if (forumRepo.GetMany().SingleOrDefault(f => f.ParentForum_id == parentForumId && f.Title_txt.ToLower().Equals(title.ToLower())) != null)
                return "errDuplicate";
        } catch (InvalidOperationException e) {
            return "errDuplicate";
        }

        return "valid";
    }
}