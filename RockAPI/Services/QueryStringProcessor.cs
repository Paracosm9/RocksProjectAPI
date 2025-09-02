namespace RockAPI.Services;

public class QueryStringProcessor

{
	private HttpRequest Request { get; set; }
	private Dictionary<string, string> ParsedQuery { get; set; }

	public QueryStringProcessor(HttpRequest request)
	{
		Request = request;
		GetParsedQuery();
	}

	public Dictionary<string, string> GetParsedQuery()
	{
		ParsedQuery = new Dictionary<string, string>();
		foreach (var key in Request.Query.Keys)
		{
			ParsedQuery.Add(key, Request.Query[key]);
		}

		return ParsedQuery;
	}

	public string GetCommandString(string beginning)
	{
		int iterator = 0;
		foreach (var (key, value) in ParsedQuery)
		{
			Console.WriteLine(key + " " + value);
			if (key.ToLower() == "orderby") continue;
			beginning += iterator == 0
				? $"WHERE {key}={GetFixedValue(key, value)} "
				: $" AND \"{key}\"={GetFixedValue(key, value)} ";
			iterator++;
		}

		if (ParsedQuery.TryGetValue("orderBy", out var orderBy))
		{
			beginning = beginning + "ORDER BY " + orderBy;
		}

		return beginning;
	}

	private string GetFixedValue(string key, string value)
	{
		return (key is "mohs" or "id") ? value : $"'{value}'";
	}

}