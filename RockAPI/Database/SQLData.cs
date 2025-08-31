using System.Data;
using Npgsql;
using RockAPI.Services;
using Dapper;
namespace RockAPI.Database;

public class SQLData : IRocksRepo

{
	private const string DATABASE_NAME = "minerals";
	private const string TABLE_NAME = "minerals";

	private const string CONNECTION_STRING =
		$"Host=localhost;Port=5432;Database={DATABASE_NAME};Username=postgres;Password=Lol12345";

	private NpgsqlConnection _connection;
	private string _connectionString;
	
	public SQLData(IConfiguration configuration) //taking confid from ASP.NET
	{
		_connectionString = CONNECTION_STRING;
		Console.WriteLine(_connectionString);
	}

	public async Task AddMineral(Mineral mineral, MineralInput mineralInput)
	{
		_connection = new NpgsqlConnection(
			_connectionString);
		_connection.Open();
		string commandText =
			$"INSERT INTO minerals (name, formula, description, mohs) " +
			$"VALUES (@name, @formula, @description, @mohs);";
		var queryArgs = new
		{ name = mineral.Name, 
		formula = mineral.Formula,
		description = mineral.Description,
		mohs = mineral.Mohs,
		};
		await _connection.ExecuteAsync(commandText, queryArgs);
		await _connection.CloseAsync();
	}

	public async Task<List<Mineral>> GetAllMinerals()
	{
		_connection = new NpgsqlConnection(_connectionString);
		Console.WriteLine(_connectionString);
		_connection.Open();
		string commandText =
			$"SELECT id   AS id," +
			$"\n   name AS name,\n  formula AS formula," +
			$"\n   description AS description," +
			$"\n   mohs AS mohs" +
			$"\n FROM minerals  " +
			$"ORDER BY id";
		var mineral = _connection.Query<Mineral>(commandText);
		await _connection.CloseAsync();
		return mineral.ToList();
	}

	public async Task<Mineral> GetMineralById(int id)
	{
		_connection = new NpgsqlConnection(_connectionString);
		_connection.Open();
		string commandText = $"SELECT id   AS id," +
		                     $"\n   name AS name,\n  formula AS formula," +
		                     $"\n   description AS description," +
		                     $"\n   mohs AS mohs" +
		                     $"\n FROM minerals WHERE id = @Id; "; 
		var queryArgs = new { Id = id };
		var mineral = _connection.Query<Mineral>(commandText, queryArgs);
		await _connection.CloseAsync();
		return mineral.FirstOrDefault();
	}

	public Task<List<Mineral>> GetFilteredMinerals(Dictionary<string, string> query)
	{
		//to make with query string etc. 
		throw new NotImplementedException();
	}

	public async void UpdateMineral(MineralInput mineralInput, int id)
	{
		try
		{
			_connection = new NpgsqlConnection(_connectionString);
			_connection.Open();
			string commandText = $"UPDATE {TABLE_NAME} SET name = @name, formula = @formula, description = @description, mohs = @mohs WHERE id = @id "; 
			var queryArgs = new
			{ Id = id , 
			  name = mineralInput.Name,
			  formula = mineralInput.Formula,
			  description = mineralInput.Description,
			  mohs = mineralInput.Mohs,
			};
			await _connection.ExecuteAsync(commandText, queryArgs);
			await _connection.CloseAsync();
		}
		catch (Exception e)
		{
			throw; // TODO handle exception
		}
	}

	public async void DeleteMineral(int id)
	{
		try
		{
			_connection = new NpgsqlConnection(
				_connectionString);
			_connection.Open();
			string commandText = $"DELETE FROM {TABLE_NAME} WHERE ID = @id";
			var queryArgs = new
			{ Id = id };
			await _connection.ExecuteAsync(commandText, queryArgs);
			await _connection.CloseAsync();
		}
		catch (Exception e)
		{
			throw; // TODO handle exception
		}
	}
}