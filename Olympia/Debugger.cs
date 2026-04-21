namespace Olympia
{
    internal static class Debugger
    {
        public static void Print<T>(T message)
        {
            Console.WriteLine(message);
        }

        public static void PrintWarning<T>(T message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Print(message);
            Console.ResetColor();
        }

        public static void PrintError<T>(T message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Print(message);
            Console.ResetColor();
        }
    }
}
