namespace ClientApp.Entities;

public class User {
    public int User_id { get; set; }

    public User(int user_id) {
        User_id = user_id;
    }
    
    public User() {
        User_id = -1;
    }
}