using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.ManageComments;

public class CreateComment {
    public async Task<Comment?> NewCommentAsync(int parentPostId, int parentForumId, ICommentRepository commentRepo, User localUser) {
        
        Console.WriteLine(": Creating Comment... Type 'abort' at any step to abort...");

        Console.Write("\nPlease enter comment text: ");
        string body = ReadUserInput() ?? "UNSPECIFIED BODY";
        if(CheckForAbort(body))
            return null;
        
        // Create the Comment:
        Comment newComment = new Comment();
        newComment.ParentPost_id = parentPostId;
        newComment.ParentForum_id = parentForumId;
        newComment.Body_txt = body;
        newComment.Author_Id = localUser.User_id;
        
        // Attempt to add Comment
        Comment commentAdded = await commentRepo.AddAsync(newComment);
        
        return commentAdded;
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