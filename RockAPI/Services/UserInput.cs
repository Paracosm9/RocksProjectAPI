namespace RockAPI.Services;

public class UserInput
{
	public int Id { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Type { get; set; } //admin / default
	public byte[] AvatarBytes { get; set; } //implement when I would be good with that convertation between byte[] and image.

	public UserInput()
	{
		
	}

	public UserInput(int id, string username, string password, string email, string firstName, string lastName, string type)
	{
		Id = id;
		Username = username;
		Password = password;
		Email = email;
		FirstName = firstName;
		LastName = lastName;
		Type = type;
	}
}