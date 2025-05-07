// 3) https://www.hackerrank.com/challenges/mini-max-sum/problem?isFullScreen=true

public static void miniMaxSum(List<int> arr)
    {
        List<long> longArr = arr.Select(x => (long)x).ToList();
        long totalSum = longArr.Sum();

        long min = totalSum - longArr.Max(); 
        long max = totalSum - longArr.Min(); 

        Console.WriteLine($"{min} {max}");
    }