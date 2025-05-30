namespace HideAndSeek
{
    /// <summary>
    /// Interface for factory for creating game controllers
    /// </summary>
    public interface IGameControllerFactory
    {
        /// <summary>
        /// Maximum number of opponents with default names (setter should only be called in test mocking)
        /// </summary>
        int MaximumNumberOfOpponentsWithDefaultNames { get; set; }

        /// <summary>
        /// Method to return whether name is a valid name for an opponent
        /// </summary>
        /// <param name="name">Name for opponent</param>
        /// <returns>True if name is valid name for opponent</returns>
        bool IsValidOpponentName(string name);

        /// <summary>
        /// Create game controller with default settings
        /// </summary>
        /// <returns>Game controller with default settings</returns>
        IGameController GetDefaultGameController();

        /// <summary>
        /// Create game controller with opponents with specific names
        /// </summary>
        /// <param name="namesOfOpponents">Names of opponents</param>
        /// <returns>Game controller with opponents with specific names</returns>
        IGameController GetGameControllerWithSpecificNamesOfOpponents(string[] namesOfOpponents);

        /// <summary>
        /// Create game controller with specific number of opponents
        /// </summary>
        /// <param name="numberOfOpponents">Number of opponents</param>
        /// <returns>Game controller with specific number of opponents</returns>
        IGameController GetGameControllerWithSpecificNumberOfOpponents(int numberOfOpponents);
    }
}