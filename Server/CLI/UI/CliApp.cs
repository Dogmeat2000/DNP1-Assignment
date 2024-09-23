using ConsoleApp1.UI.ManageComments;
using ConsoleApp1.UI.ManagePosts;
using ConsoleApp1.UI.ManageUser;
using ConsoleApp1.UI.ManageUserProfiles;
using ConsoleApp1.UI.SharedLogic;
using Entities;
using RepositoryContracts;

namespace ConsoleApp1.UI;

public class CliApp {
    
    private ICommentRepository CommentRepository { get; set; }
    private IForumRepository ForumRepository { get; set; }
    private IPostRepository PostRepository { get; set; }
    private IUserRepository UserRepository { get; set; }
    private IUserProfileRepository UserProfileRepository { get; set; }
    private User? LocalUser { get; set; }
    private string ErrorMessage { get; }
    private bool InvalidEntry  { get; set; }
    private string LastCmd { get; set; }
    private NavigationManager NavHelper { get; } = new ();
    private CLISettings Settings { get; } = new ();

    
    public CliApp(ICommentRepository commentRepository, IForumRepository forumRepository, IPostRepository postRepository, IUserRepository userRepository, IUserProfileRepository userProfileRepository) {
        CommentRepository = commentRepository;
        ForumRepository = forumRepository;
        PostRepository = postRepository;
        UserRepository = userRepository;
        UserProfileRepository = userProfileRepository;
        LocalUser = null;
        ErrorMessage = ": ERROR: Please enter a valid command...!";
        InvalidEntry = false;
        LastCmd = "";
    }

    
    public async Task StartAsync() {
        
        // Main UI Logic is encapsulated inside this repeating while-loop:
        var runApp = true;
        
        while (runApp) {
            
            // Display Main Menu Text:
            ShowMainMenuText();
            
            // Display relevant forums & posts:
            if (NavHelper.CurrentPost == null) {
                // Display forum and posts, if no post is currently active
                DisplayForums();
                DisplayPosts();
            } else {
                // Display comments, if a Post is active
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
            runApp = EvaluateCommand(await new UserInput().ReadUserInputAsync("\nCmd: ") ?? "INVALID COMMAND").Result;
        }
    }

    
    private void ShowMainMenuText() {
        var userName = "Anonymous";
        Console.ForegroundColor = Settings.UserTextColor;
        if(LocalUser != null) {
            userName = "Anonymous"; //TODO Get the users name here!
            Console.WriteLine($"\n\n\nLogged in as: {userName}");
        } else {
            Console.WriteLine($"\n\n\nViewing as: {userName}");
        }
        Console.ForegroundColor = Settings.MainMenuTextColor;
        Console.WriteLine("\n    \x1b[1mMain Menu:\x1b[0m                                                           ");
        Console.ForegroundColor = Settings.MainMenuTextColor;
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
        
        // Display operations only available for anonymous users
        if (LocalUser != null) {
            // Display operations only available for logged-in users
            Console.WriteLine("| Type 'user' to: Manage user (Create, View, Update or Delete)                                                                   |");
            Console.WriteLine("| Type 'profile' to: Manage your own user profile (change password, name, etc.)                                                  |");
            Console.WriteLine("| Type 'logout' to: Logout                                                                                                       |");
        } else {
            // Display operations only available for anonymous users
            Console.WriteLine("| Type 'login' to : Login to your account                                                                                        |");
            Console.WriteLine("| Type 'create' to: Create a User Account                                                                                        |");
        }
        if(NavHelper.CurrentPost == null && LocalUser != null)
            Console.WriteLine("| Type 'forum' to: Create a new Forum at this location                                                                           |");

        if (NavHelper.CurrentPost == null) {
            Console.WriteLine("| Type 'post'  to: Create a new post inside the currently active Forum                                                           |");
            Console.WriteLine("| Type 'cd ' + 'identifier' next to forum or post names in order to view them. Ex: 'cd F0' to view/enter Forum 1                 |");
        } else
            Console.WriteLine("| Type 'comment '  to: Create a new comment inside the currently active Post                                                     |");
        
        Console.WriteLine("| Type 'exit' to  : Terminate this application                                                                                   |");
        if (NavHelper.CurrentForum != null || NavHelper.CurrentPost != null)
            Console.WriteLine("| Type 'return' to: Return to previous view                                                                                      |"); 
        Console.WriteLine("|--------------------------------------------------------------------------------------------------------------------------------|");
        Console.ResetColor();
    }

    
    private void DisplayForums() {
        Console.ForegroundColor = Settings.ForumTextColor;
        if (NavHelper.CurrentParentForum == null && NavHelper.CurrentForum == null) {
            Console.WriteLine("    \x1b[1mGlobal Forums:\x1b[0m");
            Console.ForegroundColor = Settings.ForumTextColor;
        } else {
            Console.WriteLine($"    \x1b[1mForum:\x1b[0m {ForumRepository
                .GetSingleAsync(NavHelper.CurrentForum?.Forum_id ?? -1, NavHelper.CurrentParentForum?.Forum_id ?? -1)
                .Result
                .Title_txt}");
            Console.ForegroundColor = Settings.ForumTextColor;
        }
        Console.WriteLine("|================================================================================================================================|");
            
        var i = 1;
        foreach (var forum in ForumRepository.GetMany()) {
            if (forum.ParentForum_id == (NavHelper.CurrentForum?.Forum_id ?? -1)) {
                Console.WriteLine($" {i}. [Forum ID: F{forum.Forum_id}] {forum.Title_txt}");
                i++;
            }
        }   
        
        if (i == 1)
            Console.WriteLine("| !!! No forums found !!!");
        Console.WriteLine("|================================================================================================================================|");
        Console.ResetColor();
    }

    
    private void DisplayPosts() {
        Console.ForegroundColor = Settings.PostTextColor;
        if (NavHelper.CurrentForum == null) {
            Console.WriteLine("    \x1b[1mGlobal Posts\x1b[0m");
            Console.ForegroundColor = Settings.PostTextColor;
        } else {
            Console.WriteLine($"    \x1b[1mPosts in Forum '{ForumRepository
                .GetSingleAsync(NavHelper.CurrentForum.Forum_id, NavHelper.CurrentParentForum?.Forum_id ?? -1)
                .Result
                .Title_txt}'\x1b[0m ");
            Console.ForegroundColor = Settings.PostTextColor;
        }

        Console.WriteLine("|................................................................................................................................|");
        var i = 1;
        foreach (var post in PostRepository.GetMany()) {
            if (post.ParentForum_id == (NavHelper.CurrentForum?.Forum_id ?? -1)) {
                Console.WriteLine($" {i}. [Post ID: P{post.Post_id}] {post.Title_txt}");
                i++;
            }
        }

        if (i == 1)
            Console.WriteLine("| !!! No posts found !!!");
        Console.WriteLine("|================================================================================================================================|");
        Console.ResetColor();
    }


    private async void DisplayCommentsAsync() {
        Console.ForegroundColor = Settings.PostTextColor;
        
        // Display Post Details
        if (NavHelper.CurrentPost != null) {
            await new ViewSinglePost().Display(NavHelper.CurrentPost.Post_id, NavHelper.CurrentPost.ParentForum_id, PostRepository, UserRepository, UserProfileRepository);
            
            // Display Corresponding Comments:
            Console.ForegroundColor = Settings.CommentTextColor;
            Console.WriteLine($"    \x1b[1mComments in Post: {PostRepository
                .GetSingleAsync(NavHelper.CurrentPost.Post_id, NavHelper.CurrentForum?.Forum_id ?? -1)
                .Result
                .Title_txt}\x1b[0m");
            Console.ForegroundColor = Settings.CommentTextColor;
            Console.WriteLine("|................................................................................................................................|");
            var i = 1;
            foreach (var comment in CommentRepository.GetMany()) {
                if (comment.ParentPost_id == NavHelper.CurrentPost.Post_id && comment.ParentForum_id == (NavHelper.CurrentForum?.Forum_id ?? -1)) {
                    Console.WriteLine($"| {i}. [Comment ID: C{comment.Comment_id}] {comment.Body_txt}");
                    Console.WriteLine("| ___");
                    i++;
                }
            }
            if(i == 1)
                Console.WriteLine("| !!! No comments found !!!");
        }
    }
    
    
    private async Task<bool> EvaluateCommand(string cmd) {
        LastCmd = cmd.ToLower();
        
        switch (cmd.ToLower()) {
            case "user":
                if (LocalUser != null) {
                    String lastCmdModified = "";
                    await new ManageUsers(UserRepository, UserProfileRepository, LocalUser).Start();
                    LastCmd = lastCmdModified;
                } else {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                }
                break;
            
            case "post":
                // Allow User to create a new post
                if (NavHelper.CurrentForum == null) {
                    Post? newPost = await new CreatePost().NewPostAsync(-1,PostRepository, LocalUser ?? new User());
                } else {
                    Post? newPost = await new CreatePost().NewPostAsync(NavHelper.CurrentForum.Forum_id,PostRepository, LocalUser ?? new User());
                }
                break;
            
            case "comment":
                // Allow User to add a comment
                if (NavHelper.CurrentPost == null) {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                } else if (NavHelper.CurrentForum == null) {
                    Comment? newComment = await new CreateComment().NewCommentAsync(NavHelper.CurrentPost.Post_id, -1,CommentRepository, LocalUser ?? new User());
                } else {
                    Comment? newComment = await new CreateComment().NewCommentAsync(NavHelper.CurrentPost.Post_id, NavHelper.CurrentForum.Forum_id,CommentRepository, LocalUser ?? new User());
                }
                break;
            
            case "forum": 
                // Allow User to Create a new Forum, if User is logged in and not inside a Post!
                if (LocalUser != null && NavHelper.CurrentPost != null) {
                    //TODO Implement
                    throw new NotImplementedException();
                } else {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                }
                break;
            
            case "profile":
                if (LocalUser != null) {
                    //TODO Go to userProfile view!
                    throw new NotImplementedException();
                } else {
                    Console.WriteLine(ErrorMessage);
                    InvalidEntry = true;
                }
                break;
            
            case "logout":
                if (LocalUser != null) {
                    //TODO Go to user logout view!
                    throw new NotImplementedException();
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
                    //TODO Implement
                    throw new NotImplementedException();
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
                if (NavHelper.CurrentPost != null)
                    NavHelper.CurrentPost = null;
                else if (NavHelper.CurrentForum != null && NavHelper.CurrentParentForum != null) {
                    NavHelper.ParentsParentForum = NavHelper.CurrentParentForum;
                    NavHelper.CurrentForum = NavHelper.CurrentParentForum;
                    if(NavHelper.ParentsParentForum != null)
                        NavHelper.CurrentParentForum = ForumRepository.GetSingleAsync(NavHelper.CurrentForum.ParentForum_id, NavHelper.ParentsParentForum.Forum_id).Result;
                    else
                        NavHelper.CurrentParentForum = ForumRepository.GetSingleAsync(NavHelper.CurrentForum.ParentForum_id, -1).Result;
                } else if (NavHelper.CurrentForum != null && NavHelper.CurrentParentForum == null) {
                    NavHelper.ParentsParentForum = null;
                    NavHelper.CurrentForum = null;
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
                        string logMessage = $"New directory is: [Forum_id: {NavHelper.CurrentForum?.Forum_id ?? -1} / Post_id: {NavHelper.CurrentPost?.Post_id ?? -1}]";
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
                    if (NavHelper.CurrentPost == null 
                        && NavHelper.CurrentParentForum == null 
                        && forum.Forum_id == id 
                        && forum.ParentForum_id == -1) 
                    {
                        NavHelper.CurrentParentForum = NavHelper.CurrentForum;
                        NavHelper.CurrentForum = forum;
                        return true;
                    } 
                    
                    // We are NOT the top most level, trying to navigate further into the forum structure!
                    if (NavHelper.CurrentPost == null 
                        && NavHelper.CurrentParentForum != null 
                        && forum.Forum_id == id 
                        && forum.ParentForum_id == NavHelper.CurrentParentForum.Forum_id) 
                    {
                        NavHelper.CurrentParentForum = NavHelper.CurrentForum;
                        NavHelper.CurrentForum = forum;
                        return true;
                    }
                }
                return false;
            
            case "p":
                foreach (var post in PostRepository.GetMany()) {
                    // We are at the top most level, trying to navigate into a post!
                    if (NavHelper.CurrentParentForum == null 
                        && NavHelper.CurrentForum == null 
                        && post.Post_id == id 
                        && post.ParentForum_id == -1) 
                    {
                        NavHelper.CurrentPost = post;
                        return true;
                    }
                    
                    // We are inside the first subforum level, meaning there is no "parentForum":
                    if (NavHelper.CurrentParentForum == null 
                        && NavHelper.CurrentForum != null 
                        && post.Post_id == id 
                        && post.ParentForum_id == NavHelper.CurrentForum.Forum_id 
                        && NavHelper.CurrentForum.ParentForum_id == -1) 
                    {
                        NavHelper.CurrentPost = post;
                        return true;
                    } 
                    
                    // We are inside a subforum trying to navigate into a post!
                    if (NavHelper.CurrentParentForum != null 
                        && post.Post_id == id 
                        && post.ParentForum_id == NavHelper.CurrentForum?.Forum_id 
                        && NavHelper.CurrentForum?.ParentForum_id == NavHelper.CurrentParentForum.Forum_id) 
                    {
                        NavHelper.CurrentPost = post;
                        return true;
                    } 
                }
                return false;
            
            default:
                return false;
            
        }
    }
}