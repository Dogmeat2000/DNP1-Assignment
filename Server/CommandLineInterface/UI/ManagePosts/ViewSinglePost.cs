using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI.ManagePosts;

public class ViewSinglePost {
    private Post Post { get; set; }

    public async Task<Post> Display(int postId, int parentForumId, IPostRepository postRepo, IUserRepository userRepo, IUserProfileRepository userProfileRepo) {
        Console.Write("| ");
        
        // Attempt to look up post
        if (await LookUpPostAsync(postId, parentForumId, postRepo) != null) {
            // Lookup successful
            
            // Display Post Title:
            if(!DisplayPostTitle())
                Console.WriteLine("ERROR: UNABLE TO FETCH POST TITLE");
            
            // Display Author:
            if(!DisplayPostAuthor(postId, parentForumId))
                Console.WriteLine("ERROR: UNABLE TO FETCH POST AUTHOR");
            
            // Display Post Details:
            if(!DisplayPostText())
                Console.WriteLine("ERROR: UNABLE TO FETCH POST TEXT");
            
            Console.WriteLine("|+ + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + + |");

        } else {
            Console.WriteLine("ERROR, NO POST INFORMATION AVAILABLE!");
        }
        
        return Post;
    }

    private async Task<Post> LookUpPostAsync(int postId, int parentForumId, IPostRepository postRepo) {
        Post = await postRepo.GetSingleAsync(postId, parentForumId);
        return Post;
    }
    
    private bool DisplayPostTitle() {
        string title = Post.Title_txt;
        for (int i = 70; i < title.Length; i++) {
            title = title.Insert(i, "\n| ");
        }
        Console.WriteLine($"[P{Post.Post_id}]: \x1b[1mTitle: {title}\x1b[0m");
        Console.ForegroundColor = ConsoleColor.Cyan;
        return true;
    }

    private bool DisplayPostAuthor(int postId, int parentForumId) {
        Console.Write("| Author: ");
        //TODO Implement
        return false;
    }

    private bool DisplayPostText() {
        string text = Post.Body_txt;
        for (int i = 70; i < text.Length; i++) {
            text = text.Insert(i, "\n| ");
        }
        Console.WriteLine($"|\n| Body: {text}");
        return true;
    }
}