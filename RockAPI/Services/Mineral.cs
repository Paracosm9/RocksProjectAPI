namespace RockAPI.Services;

public class Mineral
{
	public int Id { get; set; }
	public string Name { get; set; }
	public string Formula { get; set; }
	public string Description { get; set; }
	public double Mohs { get; set; }
	// public byte[] ImageBytes { get; set; }

	public Mineral(int id, string name, string formula, string description, double mohs)
	{
		Id = id;
		Name = name;
		Formula = formula;
		Description = description;
		Mohs = mohs;
		// ImageBytes = imageBytes; //implement later, maybe do some type of login? Hm. 
	}
	
	public Mineral() { } //hate to have this, but Dapper wants to ahem... Having this for Mapping. 
}
