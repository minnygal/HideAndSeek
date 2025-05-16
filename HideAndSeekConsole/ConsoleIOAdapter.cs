
namespace HideAndSeek
{
    /// <summary>
    /// Adapter for console input and output 
    /// to allow the static Console class to be used in a testable way
    /// </summary>
    public class ConsoleIOAdapter : IConsoleIO
    {
        public ConsoleKeyInfo ReadKey() => Console.ReadKey();

        public string ReadLine() => Console.ReadLine();

        public void Write(string message) => Console.Write(message);

        public void WriteLine() => Console.WriteLine();

        public void WriteLine(string message) => Console.WriteLine(message);
    }
}