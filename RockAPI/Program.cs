using Microsoft.AspNetCore.Http.HttpResults;
using RockAPI.Database;
using RockAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddScoped<IRocksRepo, SQLData>(); // asking for IRocksrepo - giving SQLData.
builder.Services.AddScoped<RocksService>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

/*
* Read DB
*/

app.MapGet(
    "/getMineral/{id}", async (int id, RocksService rocksService) =>
    {
        return await rocksService.GetMineralById(id);
    }).WithName("GetMineralById").WithOpenApi();


app.MapGet(
    "/getMinerals", async (RocksService rocksService) =>
    {
        return await rocksService.GetAllMinerals();
    }).WithName("GetMineralsByAPI").WithOpenApi();

/*
 * Create in DB
 */

//https://learn.microsoft.com/en-us/aspnet/core/tutorials/min-web-api?view=aspnetcore-9.0&tabs=visual-studio
app.MapPost("/addMineral", async (MineralInput mineralInput, RocksService RocksService) =>
    {
        try
        {
            Mineral mineral = new Mineral(
                0,
                mineralInput.Name, 
                mineralInput.Formula,
                mineralInput.Description,
                mineralInput.Mohs
            );
            RocksService.AddMineral(mineral, mineralInput);
            return Results.Ok(mineral);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}\n{ex.StackTrace}");
            return Results.Problem("Something went wrong with adding mineral. ");
        }
    }
).WithName("AddMineralToDB").WithOpenApi();

/*
 * Update in DB
 */

app.MapPost(
    "/updateMineral/{id}", async (RocksService rocksService, int id, MineralInput mineralInput) =>
    {
        rocksService.UpdateMineral(mineralInput, id);
        return Results.Ok("Mineral was successfully updated.");
    }).WithName("UpdateMineralssss").WithOpenApi();


/*
 * Delete in DB
 */
app.MapGet(
    "/deleteMineral/{id}", async (int id, RocksService rocksService) =>
    {
        rocksService.DeleteMineral(id);
        return Results.Ok("Mineral was successfully deleted");
    }).WithName("DeleteMineralById").WithOpenApi();
app.Run();


