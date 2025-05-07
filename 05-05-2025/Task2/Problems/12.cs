// 12) https://www.hackerrank.com/challenges/divisible-sum-pairs/problem?isFullScreen=true

public static int divisibleSumPairs(int n, int k, List<int> ar)
    {
        int count = 0;
        for (int i = 0; i < ar.Count - 1; i++)
        {
            for (int j = i + 1; j < ar.Count; j++)
            {
                if ((ar[i] + ar[j]) % k == 0)
                {
                    count++;
                }
            }
        }
        return count;
    }