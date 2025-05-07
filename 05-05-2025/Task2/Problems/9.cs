// 9) https://www.hackerrank.com/challenges/between-two-sets/problem?isFullScreen=true

public static int getTotalX(List<int> a, List<int> b)
    {
        // LCM of array a
        int lcmA = a[0];
        for (int i = 1; i < a.Count; i++)
        {
            lcmA = Lcm(lcmA, a[i]);
        }

        // GCD of array b
        int gcdB = b[0];
        for (int i = 1; i < b.Count; i++)
        {
            gcdB = Gcd(gcdB, b[i]);
        }

        // Count numbers between LCM and GCD
        int count = 0;
        for (int i = lcmA; i <= gcdB; i += lcmA)
        {
            if (gcdB % i == 0)
            {
                count++;
            }
        }
        return count;
    }
    // GCD 
    private static int Gcd(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }
    // LCM
    private static int Lcm(int a, int b)
    {
        return (a * b) / Gcd(a, b);
    }