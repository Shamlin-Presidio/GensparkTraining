// 2) https://www.hackerrank.com/challenges/staircase/problem?isFullScreen=true

public static void staircase(int n)
    {
        for (int i = 1; i <= n; i++)
        {
            string spaces = new string(' ', n - i);
            string hashes = new string('#', i);
            Console.WriteLine(spaces + hashes);
        }
    }