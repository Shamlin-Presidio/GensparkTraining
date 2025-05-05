// 1) https://www.hackerrank.com/challenges/plus-minus/problem?isFullScreen=true

public static void plusMinus(List<int> arr)
    {
        int n = arr.Count;
        int pos = 0, neg = 0, zero = 0;

        foreach (int num in arr)
        {
            if (num > 0)
                pos++;
            else if (num < 0)
                neg++;
            else
                zero++;
        }

        Console.WriteLine($"{(double)pos / n:F6}");
        Console.WriteLine($"{(double)neg / n:F6}");
        Console.WriteLine($"{(double)zero / n:F6}");
    }