namespace RpgRoguelikeRun.UI;

public static class ConsoleReader
{
    public static int ReadChoice(int max)
    {
        int number = 0;
        bool hasDigit = false;

        while (true)
        {
            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Escape)
            {
                Console.WriteLine();
                return 0;
            }

            if (key.Key == ConsoleKey.Enter && hasDigit)
                return number;

            if (key.Key == ConsoleKey.Backspace && hasDigit)
            {
                number /= 10;
                hasDigit = number > 0;
                Console.Write("\b \b");
                continue;
            }

            if ((key.Key >= ConsoleKey.D0 && key.Key <= ConsoleKey.D9) ||
                (key.Key >= ConsoleKey.NumPad0 && key.Key <= ConsoleKey.NumPad9))
            {
                int digit = key.Key >= ConsoleKey.NumPad0
                    ? key.Key - ConsoleKey.NumPad0
                    : key.Key - ConsoleKey.D0;

                int candidate = number * 10 + digit;
                if (candidate > max) continue;

                number = candidate;
                hasDigit = true;
                Console.Write(digit);
            }
        }
    }
}