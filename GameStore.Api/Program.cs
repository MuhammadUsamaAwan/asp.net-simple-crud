using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games = [
    new(1, "Witcher: III", "RPG", 59.99m, new DateOnly(2015, 5, 18))
];

app.MapGet("/games", () => games);

app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id)).WithName(GetGameEndpointName);

app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );
    games.Add(game);
    return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
});

app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    int index = games.FindIndex(game => game.Id == id);
    games[index] = new(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
    );
    return Results.NoContent();
});

app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});

app.Run();
