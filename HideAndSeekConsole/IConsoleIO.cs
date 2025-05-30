namespace HideAndSeek
{
    /// <summary>
    /// Interface for console input and output
    /// </summary>
    public interface IConsoleIO
    {
        void WriteLine();
        void WriteLine(string message);
        void Write(string message);
        string ReadLine();
        ConsoleKeyInfo ReadKey();
    }
}