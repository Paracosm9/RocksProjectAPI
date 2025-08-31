using RockAPI.Services;

namespace RockAPI.TestLikeStuff;

public class TestsForDB : IRocksRepo
{
	public Task AddMineral(Mineral mineral, MineralInput mineralInput)
	{
		throw new NotImplementedException();
	}

	public Task<List<Mineral>> GetAllMinerals()
	{
		throw new NotImplementedException();
	}

	public Task<Mineral> GetMineralById(int id)
	{
		throw new NotImplementedException();
	}

	public Task<List<Mineral>> GetFilteredMinerals(Dictionary<string, string> query)
	{
		throw new NotImplementedException();
	}

	public void UpdateMineral(MineralInput mineralInput, int id)
	{
		throw new NotImplementedException();
	}

	public void DeleteMineral(int id)
	{
		throw new NotImplementedException();
	}
}