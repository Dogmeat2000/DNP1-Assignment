using Entities;

namespace ConsoleApp1.UI.SharedLogic;

public class NavigationManager {
    public Post? CurrentPost { get; set; }
    public Forum? CurrentForum { get; set; }
    public Forum? CurrentParentForum { get; set; }
    public Forum? ParentsParentForum { get; set; }
}