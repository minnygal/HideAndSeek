namespace HideAndSeek
{
    /// <summary>
    /// Interface for adapter to get names of existing saved game and house files
    /// </summary>
    public interface IGetFileNamesAdapter
    {
        /// <summary>
        /// Get names of existing saved game files (without any extensions)
        /// </summary>
        /// <returns>Names of saved game files without any extensions</returns>
        IEnumerable<string> GetSavedGameFileNames();

        /// <summary>
        /// Get names of existing house files (without any extensions)
        /// </summary>
        /// <returns>Names of house files without any extensions</returns>
        IEnumerable<string> GetHouseFileNames();
    }
}