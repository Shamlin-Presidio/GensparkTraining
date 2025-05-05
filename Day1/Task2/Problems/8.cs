// 8) https://www.hackerrank.com/challenges/kangaroo/problem?isFullScreen=true

public static string kangaroo(int x1, int v1, int x2, int v2)
    {
        if (v1 == v2)
        {
            return x1 == x2 ? "YES" : "NO";
        }

        if ((x2 - x1) % (v1 - v2) == 0 && (v1 > v2))
        {
            return "YES";
        }

        return "NO";
    }