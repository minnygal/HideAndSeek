namespace HideAndSeek
{
    public interface IGameController
    {
        House House { get; }
        Location CurrentLocation { get; }
        int MoveNumber { get; }
        Dictionary<Opponent, LocationWithHidingPlace> OpponentsAndHidingLocations { get; }
        List<Opponent> FoundOpponents { get; }
        bool GameOver { get; }
        string Status { get; }
        string Prompt { get; }

        // Methods
        GameController RestartGame();
        string Move(Direction direction);
        string CheckCurrentLocation();
        string Teleport();
        string SaveGame(string fileNameWithoutEnding);
        string LoadGame(string fileNameWithoutEnding);
        string DeleteGame(string fileNameWithoutEnding);
    }
}