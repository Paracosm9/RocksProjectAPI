namespace RockAPI.Services;

public interface IRocksRepo
{
	public Task AddMineral(Mineral mineral, MineralInput mineralInput);
	public Task<List<Mineral>> GetAllMinerals();
	public Task<Mineral> GetMineralById(int id);
	public Task<List<Mineral>> GetFilteredMinerals(Dictionary<string, string> query);
	public void UpdateMineral(MineralInput mineralInput, int id);
	public void DeleteMineral(int id);
}