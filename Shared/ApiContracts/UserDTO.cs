namespace ApiContracts;

public class UserDTO {
    public int User_id { get; set; }
    public string? Username { get; set; } //Note; this is not part of the raw server User entity.
}