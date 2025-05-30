namespace HideAndSeek
{
    /// <summary>
    /// Adapter to get names of existing saved game and house files
    /// </summary>
    public class GetFileNamesAdapter : IGetFileNamesAdapter
    {
        public IEnumerable<string> GetHouseFileNames()
        {
            return House.GetHouseFileNames();
        }

        public IEnumerable<string> GetSavedGameFileNames()
        {
            return SavedGame.GetSavedGameFileNames();
        }
    }
}
