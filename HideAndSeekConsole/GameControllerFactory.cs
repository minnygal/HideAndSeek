
namespace HideAndSeek
{
    /// <summary>
    /// Factory for creating game controllers
    /// </summary>
    public class GameControllerFactory : IGameControllerFactory
    {
        public IGameController GetDefaultGameController()
        {
            return new GameController();
        }

        public IGameController GetGameControllerWithSpecificNamesOfOpponents(string[] namesOfOpponents)
        {
            return new GameController(namesOfOpponents);
        }

        public IGameController GetGameControllerWithSpecificNumberOfOpponents(int numberOfOpponents)
        {
            return new GameController(numberOfOpponents);
        }
    }
}
