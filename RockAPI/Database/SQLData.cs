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
		  mohs = mineral.Mohs, };
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
		var queryArgs = new
		{ Id = id };
		var mineral = _connection.Query<Mineral>(commandText, queryArgs);
		await _connection.CloseAsync();
		return mineral.FirstOrDefault();
	}

	public async Task<List<Mineral>> GetFilteredMinerals(QueryStringProcessor query)
	{
		Console.WriteLine("Well....");
		try
		{
			bool result = await IsFieldsExist(query);
			Console.WriteLine("OLOLOLO");
			if (result)
			{
				string commandString = $"SELECT id   AS id," +
				                       $"\n   name AS name,\n  formula AS formula," +
				                       $"\n   description AS description," +
				                       $"\n   mohs AS mohs" +
				                       $"\n FROM minerals \n";
				commandString = query.GetCommandString(commandString);
				Console.WriteLine(commandString);
				var mineral = _connection.Query<Mineral>(commandString);
				await _connection.CloseAsync();
				return mineral.ToList();
				
			}
		}
		catch (NpgsqlException e)
		{
			_connection.Close();
			
		}
		return null;
	}

	public async void UpdateMineral(MineralInput mineralInput, int id)
	{
		try
		{
			_connection = new NpgsqlConnection(_connectionString);
			_connection.Open();
			string commandText =
				$"UPDATE {TABLE_NAME} SET name = @name, formula = @formula, description = @description, mohs = @mohs WHERE id = @id ";
			var queryArgs = new
			{ Id = id,
			  name = mineralInput.Name,
			  formula = mineralInput.Formula,
			  description = mineralInput.Description,
			  mohs = mineralInput.Mohs, };
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

	private async Task<bool> IsFieldsExist(QueryStringProcessor query)
	{
		_connection = new NpgsqlConnection(_connectionString);
		Console.WriteLine("HERE");
		foreach (var key in query.GetParsedQuery())
		{
			Console.WriteLine(key.Key);
			await _connection.OpenAsync();
			var queryArgs = new
			{ column_name = key.Key };
			string commandText =
				$"SELECT EXISTS (SELECT 1 \nFROM information_schema.columns \nWHERE table_schema='public' AND table_name='minerals' AND column_name=@column_name)";
			bool IsExists = await _connection.ExecuteScalarAsync<bool>(commandText, queryArgs);
			await _connection.CloseAsync();
			if (!IsExists)
			{
				return false;
			}

			Console.WriteLine(IsExists.ToString());
			Console.WriteLine(Boolean.Parse(IsExists.ToString()));
		}

		return true;
	}
}