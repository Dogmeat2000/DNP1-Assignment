using ConsoleApp1.UI.ManageComments;
using ConsoleApp1.UI.ManagePosts;
using ConsoleApp1.UI.ManageUser;
using ConsoleApp1.UI.ManageUserProfiles;
using ConsoleApp1.UI.SharedLogic;
using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI;

public class CliApp {
    
    public ICommentRepository CommentRepository { get; set; }
    public IForumRepository ForumRepository { get; set; }
    public IPostRepository PostRepository { get; set; }
    public IUserRepository UserRepository { get; set; }
    public IUserProfileRepository UserProfileRepository { get; set; }
    private User? LocalUser { get; set; }
    private string ErrorMessage { get; set; }
    private Boolean InvalidEntry  { get; set; }
    private string LastCmd { get; set; }
    private Post? CurrentPost { get; set; }
    private Forum? CurrentForum { get; set; }
    private Forum? CurrentParentForum { get; set; }
    private Forum? ParentsParentForum { get; set; }

    
    public CliApp(ICommentRepository commentRepository, IForumRepository forumRepository, IPostRepository postRepository, IUserRepository userRepository, IUserProfileRepository userProfileRepository) {
        this.CommentRepository = commentRepository;
        this.ForumRepository = forumRepository;
        this.PostRepository = postRepository;
        this.UserRepository = userRepository;
        this.UserProfileRepository = userProfileRepository;
        this.LocalUser = null;
        ErrorMessage = ": ERROR: Please enter a valid command...!";
        InvalidEntry = false;
        CurrentPost = null;
        CurrentForum = null;
        CurrentParentForum = null;
        ParentsParentForum = null;
        LastCmd = "";
    }

    
    public async Task StartAsync() {
        
        // Main UI Logic is encapsulated inside this repeating while-loop:
        var runApp = true;
        
        while (runApp) {
            
            // Display Main Menu Text:
            ShowMainMenuText();
            
            // Display relevant forums & posts:
            if (CurrentPost == null) {
                // Display forum and posts, if no post is currently active
                DisplayForums();
                DisplayPosts();
            } else {
                // Display comments, if a post was previously selected
                DisplayCommentsAsync();
            }
            
            // Display last user action
            if(LastCmd.Length > 0)
                Console.WriteLine(": " + LastCmd);
            
            // Display last error, if user previously entered invalid command:
            if (InvalidEntry) {
                Console.WriteLine(ErrorMessage);
                InvalidEntry = false;
            }
                
            // Read userInput, and pick a corresponding command
            runApp = EvaluateCommand(await new UserInput().ReadUserInputAsync_Main() ?? "INVALID COMMAND").Result;
        }
    }

    
    private void ShowMainMenuText() {
        var userName = "Anonymous";
        if(LocalUser != null) {
            userName = "Anonymous"; //TODO Get the users name here!
            Console.WriteLine($"\n\n\nLogged in as: {userName}");
        } else {
            Console.WriteLine($"\n\n\nViewing as: {userName}");
        }
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
        Console.WriteLine("|                                                            \x1b[1mMain Menu:\x1b[0m                                                          |");
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
        
        // Display operations only available for anonymous users
        if (LocalUser != null) {
            // Display operations only available for logged-in users
            Console.WriteLine("| Type 'user' to: Manage user (Create, View, Update or Delete)                                                                   |");
            Console.WriteLine("| Type 'profile' to: Manage your own user profile (change password, name, etc.)                                                  |");
            Console.WriteLine("| Type 'logout' to: Logout                                                                                                       |");
        } else {
            // Display operations only available for anonymous users
            Console.WriteLine("| Type 'login' to : Login to your account                                                                                         |");
            Console.WriteLine("| Type 'create' to: Create a User Account                                                                                        |");
        }
        if(CurrentPost == null) 
            Console.WriteLine("| Type 'post '  to: Create a new post inside the currently active Forum                                                          |");
        else
            Console.WriteLine("| Type 'comment '  to: Create a new comment inside the currently active Post                                                     |");
        if (CurrentPost == null)
            Console.WriteLine("| Type 'cd ' + 'identifier' next to forum or post names in order to view them. Ex: 'cd F0' to view/enter Forum 1                 |");
        Console.WriteLine("| Type 'exit' to  : Terminate this application                                                                                     |");
        if (CurrentForum != null || CurrentPost != null)
            Console.WriteLine("| Type 'return' to: Return to previous view                                                                                      |"); 
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
    }

    
    private void DisplayForums() {
        if (CurrentParentForum == null && CurrentForum == null) {
            Console.WriteLine("|                                                            \x1b[1mForums:\x1b[0m");
            Console.WriteLine("|================================================================================================================================|");
            
            var i = 1;
            foreach (var forum in ForumRepository.GetMany()) {
                if (forum.ParentForum_id == -1) {
                    Console.WriteLine($"| {i}. [Forum ID: F{forum.Forum_id}] {forum.Title_txt}");
                    i++;
                }
            }   
        } else if (CurrentForum != null && CurrentParentForum == null) {
            Console.WriteLine($"|                                                            \x1b[1mForum:\x1b[0m {ForumRepository.GetSingleAsync(CurrentForum.Forum_id, -1).Result.Title_txt}");
            Console.WriteLine("|================================================================================================================================|");
            
            var i = 1;
            foreach (var forum in ForumRepository.GetMany()) {
                if (forum.ParentForum_id == CurrentForum.Forum_id) {
                    Console.WriteLine($"| {i}. [Forum ID: F{forum.Forum_id}] {forum.Title_txt}");
                    i++;
                }
            }   
        } else {
            Console.WriteLine($"|                                                            \x1b[1mForum:\x1b[0m {ForumRepository.GetSingleAsync(CurrentForum.Forum_id, CurrentParentForum.ParentForum_id).Result.Title_txt}");
            Console.WriteLine("|================================================================================================================================|");
            
            var i = 1;
            foreach (var forum in ForumRepository.GetMany()) {
                if (forum.ParentForum_id == CurrentParentForum.ParentForum_id) {
                    Console.WriteLine($"| {i}. [Forum ID: F{forum.Forum_id}] {forum.Title_txt}");
                    i++;
                }
            }   
        }
        Console.WriteLine("|================================================================================================================================|");
    }

    
    private void DisplayPosts() {
        bool postExists = false;
        
        if(CurrentForum == null) {
            Console.WriteLine("|                                                \x1b[1mPosts in Main Forum\x1b[0m");
            Console.WriteLine("|................................................................................................................................|");
            var i = 1;
            foreach (var post in PostRepository.GetMany()) {
                if (post.ParentForum_id == -1) {
                    Console.WriteLine($"| {i}. [Post ID: P{post.Post_id}] {post.Title_txt}");
                    postExists = true;
                    i++;
                }
            }
        } else if (CurrentParentForum == null) {
            Console.WriteLine($"|                                                  \x1b[1mPosts in Forum '{ForumRepository.GetSingleAsync(CurrentForum.Forum_id, -1).Result.Title_txt}'\x1b[0m ");
            Console.WriteLine("|................................................................................................................................|");
            var i = 1;
            foreach (var post in PostRepository.GetMany()) {
                if (post.ParentForum_id == CurrentForum?.Forum_id) {
                    Console.WriteLine($"| {i}. [Post ID: P{post.Post_id}] {post.Title_txt}");
                    postExists = true;
                    i++;
                }
            }
        } else {
            Console.WriteLine($"|                                                  \x1b[1mPosts in Forum '{ForumRepository.GetSingleAsync(CurrentForum.Forum_id, CurrentParentForum.ParentForum_id).Result.Title_txt}'\x1b[0m ");
            Console.WriteLine("|................................................................................................................................|");
            var i = 1;
            foreach (var post in PostRepository.GetMany()) {
                if (post.ParentForum_id == CurrentForum?.Forum_id) {
                    Console.WriteLine($"| {i}. [Post ID: P{post.Post_id}] {post.Title_txt}");
                    postExists = true;
                    i++;
                }
            }
        }

        if (!postExists)
            Console.WriteLine("| !!! No posts found !!!");
        Console.WriteLine("|================================================================================================================================|");
    }


    private async void DisplayCommentsAsync() {
        bool commentsExists = false;
        
        // Display Post Details
        Post detailsDisplayed = await new ViewSinglePost().Display(CurrentPost.Post_id, CurrentPost.ParentForum_id, PostRepository, UserRepository, UserProfileRepository);
        
        // Display Corresponding Comments:
        if (CurrentForum == null) {
            Console.WriteLine($"|                       \x1b[1mComments in Post: {PostRepository.GetSingleAsync(CurrentPost.Post_id, -1).Result.Title_txt}\x1b[0m");
            Console.WriteLine("|................................................................................................................................|");
            var i = 1;
            foreach (var comment in CommentRepository.GetMany()) {
                if (comment.ParentPost_id == CurrentPost.Post_id && comment.ParentForum_id == -1) {
                    Console.WriteLine($"| {i}. [Comment ID: C{comment.Comment_id}] {comment.Body_txt}");
                    Console.WriteLine($"| ___");
                    commentsExists = true;
                    i++;
                }
            }
        } else {
            Console.WriteLine($"|                       \x1b[1mComments in Post: {PostRepository.GetSingleAsync(CurrentPost.Post_id, CurrentForum.Forum_id).Result.Title_txt}\x1b[0m");
            Console.WriteLine("|................................................................................................................................|");
            var i = 1;
            foreach (var comment in CommentRepository.GetMany()) {
                if (comment.ParentPost_id == CurrentPost.Post_id && comment.ParentForum_id == CurrentForum.Forum_id) {
                    Console.WriteLine($"| {i}. [Comment ID: C{comment.Comment_id}] {comment.Body_txt}");
                    Console.WriteLine($"| ___");
                    commentsExists = true;
                    i++;
                }
            }
        }
        if(!commentsExists)
            Console.WriteLine("| !!! No comments found !!!");
    }
    
    
    private async Task<bool> EvaluateCommand(string cmd) {
        LastCmd = cmd.ToLower();
        
        switch (cmd.ToLower()) {
            case "user":
                if (LocalUser != null) {
                    String lastCmdModified = "";
                    new ManageUsers(UserRepository, UserProfileRepository, LocalUser).Start();
                    LastCmd = lastCmdModified;
                } else {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                }
                break;
            
            case "post":
                // Allow User to create a new post
                if (CurrentForum == null) {
                    Post newPost = await new CreatePost().NewPostAsync(-1,PostRepository, LocalUser ?? new User(-1));
                } else {
                    Post newPost = await new CreatePost().NewPostAsync(CurrentForum.Forum_id,PostRepository, LocalUser ?? new User(-1));
                }
                break;
            
            case "comment":
                // Allow User to add a comment
                if (CurrentPost == null) {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                } else if (CurrentForum == null) {
                    Comment newComment = await new CreateComment().NewCommentAsync(CurrentPost.Post_id, -1,CommentRepository, LocalUser ?? new User(-1));
                } else {
                    Comment newComment = await new CreateComment().NewCommentAsync(CurrentPost.Post_id, CurrentForum.Forum_id,CommentRepository, LocalUser ?? new User(-1));
                }
                break;
            
            case "profile":
                if (LocalUser != null) {
                    //TODO Go to userProfile view!
                } else {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                }
                break;
            
            case "logout":
                if (LocalUser != null) {
                    //TODO Go to user logout view!
                } else {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                }
                break;
            
            case "login":
                if (LocalUser != null) {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                } else {
                    //TODO Go to user login view!
                }
                break;
            
            case "create":
                if (LocalUser != null) {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                } else {
                    UserProfile? newUser = await new CreateUser().NewUserAsync(UserRepository, UserProfileRepository);
                    if (newUser == null) {
                        LastCmd += $"\n: ERROR, FAILED TO CREATE NEW USER!!";
                        InvalidEntry = true;
                        break;
                    }
                    LastCmd += $"\n-> New user created [name = {newUser?.Username ?? "ERROR N/A"}, password = {newUser?.Password ?? "ERROR N/A"}]!";
                }
                break;
            
            case "return":
                if (CurrentPost != null)
                    CurrentPost = null;
                else if (CurrentForum != null && CurrentParentForum != null) {
                    ParentsParentForum = CurrentParentForum;
                    CurrentForum = CurrentParentForum;
                    if(ParentsParentForum != null)
                        CurrentParentForum = ForumRepository.GetSingleAsync(CurrentForum.ParentForum_id, ParentsParentForum.Forum_id).Result;
                    else
                        CurrentParentForum = ForumRepository.GetSingleAsync(CurrentForum.ParentForum_id, -1).Result;
                } else if (CurrentForum != null && CurrentParentForum == null) {
                    ParentsParentForum = null;
                    CurrentForum = null;
                } else {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                }
                break;
            
            case "exit":
                Console.WriteLine("Terminating application...!");
                return false;
            
            default:
                if (cmd.ToLower().Substring(0, 3).Equals("cd ")) {
                    //Check if it is a valid change directory command;
                    if (!ChangeDirectory(cmd.ToLower())) {
                        //Invalid command:
                        Console.WriteLine(ErrorMessage);
                        InvalidEntry = true;
                    } else {
                        string logMessage = $"New directory is: [Forum_id: {CurrentForum?.Forum_id ?? -1} / Post_id: {CurrentPost?.Post_id ?? -1}]";
                        Console.WriteLine(logMessage);
                        LastCmd = cmd.ToLower() + "\n" + logMessage;
                    }
                }
                break;
        }
        
        return true;
    }

    
    /** Returns true if directory change occured. False if it failed, for whatever reason */
    private bool ChangeDirectory(string cmd) {
        // Extract directory information from cmd
        if (cmd.Length < 5)
            return false;
        
        var directoryType = cmd.Substring(3, 1);
        int? id = Int32.Parse(cmd.Substring(4));
        
        if (id == null)
            return false;
        
        // Check if this directory and id exists, and change it, if it does:
        switch (directoryType) {
            case "f":
                foreach (var forum in ForumRepository.GetMany()) { 
                    // We are at the top most level, trying to navigate into the forum structure!
                    if (CurrentPost == null && CurrentParentForum == null && forum.Forum_id == id && forum.ParentForum_id == -1) {
                        CurrentParentForum = CurrentForum;
                        CurrentForum = forum;
                        return true;
                    } 
                    
                    // We are NOT the top most level, trying to navigate further into the forum structure!
                    if (CurrentPost == null && CurrentParentForum != null && forum.Forum_id == id && forum.ParentForum_id == CurrentParentForum.Forum_id) {
                        CurrentParentForum = CurrentForum;
                        CurrentForum = forum;
                        return true;
                    }
                }
                return false;
            
            case "p":
                foreach (var post in PostRepository.GetMany()) {
                    // We are at the top most level, trying to navigate into a post!
                    if (CurrentParentForum == null && CurrentForum == null && post.Post_id == id && post.ParentForum_id == -1) {
                        CurrentPost = post;
                        return true;
                    }
                    
                    // We are inside the first subforum level, meaning there is no "parentForum":
                    if (CurrentParentForum == null && CurrentForum != null && post.Post_id == id && post.ParentForum_id == CurrentForum.Forum_id && CurrentForum.ParentForum_id == -1) {
                        CurrentPost = post;
                        return true;
                    } 
                    
                    // We are inside a subforum trying to navigate into a post!
                    if (CurrentParentForum != null && post.Post_id == id && post.ParentForum_id == CurrentForum?.Forum_id && CurrentForum?.ParentForum_id == CurrentParentForum.Forum_id) {
                        CurrentPost = post;
                        return true;
                    } 
                }
                return false;
            
            default:
                return false;
            
        }
    }
}