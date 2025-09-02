namespace RockAPI.Services;

public class RocksService(IRocksRepo repository)
{
	private IRocksRepo _repository = repository;

	public Task AddMineral(Mineral mineral, MineralInput mineralInput)
	{
		return _repository.AddMineral( mineral, mineralInput);
	}
	public Task<List<Mineral>> GetAllMinerals()
	{
		return _repository.GetAllMinerals();
	}
	public Task<Mineral> GetMineralById(int id)
	{
		return _repository.GetMineralById(id);
	}

	public Task<List<Mineral>> GetFilteredMinerals(HttpRequest request)
	{
		return _repository.GetFilteredMinerals(new QueryStringProcessor(request));
	}
	
	public void DeleteMineral(int id)
	{
		_repository.DeleteMineral(id);
	}

	public void UpdateMineral(MineralInput mineralInput , int id)
	{
		_repository.UpdateMineral(mineralInput, id);
	}

}