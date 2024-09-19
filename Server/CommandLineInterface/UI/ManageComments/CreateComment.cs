using ConsoleApp1.UI.SharedLogic;
using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.ManageComments;

public class CreateComment {
    public async Task<Comment?> NewCommentAsync(int parentPostId, int parentForumId, ICommentRepository commentRepo, User localUser) {
        
        Console.WriteLine(": Creating Comment... Type 'abort' at any step to abort...");

        Console.Write("\nPlease enter comment text: ");
        string body = await new UserInput().ReadUserInputAsync_Alt1() ?? "UNSPECIFIED BODY";
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
}