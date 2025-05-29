
namespace HideAndSeek
{
    /// <summary>
    /// Factory for creating game controllers
    /// </summary>
    public class GameControllerFactory : IGameControllerFactory
    {
        public int MaximumNumberOfOpponentsWithDefaultNames
        {
            get
            {
                return GameController.DefaultOpponentNames.Length;
            }
            set 
            { 
                throw new NotImplementedException("Setting the maximum number of opponents with default names is not supported."); 
            }
        }

        public bool IsValidOpponentName(string name)
        {
            return Opponent.IsValidName(name);
        }

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
