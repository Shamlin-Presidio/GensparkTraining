// 7) https://www.hackerrank.com/challenges/apple-and-orange/problem?isFullScreen=true

    public static void countApplesAndOranges(int s, int t, int a, int b, List<int> apples, List<int> oranges)
    {
        
        // landing positions for apples
        List<int> applePositions = new List<int>();
        foreach (int apple in apples)
        {
            int position = a + apple;
            applePositions.Add(position);
        }

        // Count apples 
        int applesOnHouse = 0;
        foreach (int pos in applePositions)
        {
            if (pos >= s && pos <= t)
            {
                applesOnHouse++;
            }
        }

        // landing positions for oranges
        List<int> orangePositions = new List<int>();
        foreach (int orange in oranges)
        {
            int position = b + orange;
            orangePositions.Add(position);
        }

        // Count oranges 
        int orangesOnHouse = 0;
        foreach (int pos in orangePositions)
        {
            if (pos >= s && pos <= t)
            {
                orangesOnHouse++;
            }
        }

        Console.WriteLine(applesOnHouse);
        Console.WriteLine(orangesOnHouse);

        // used GPT to check for optimisation:
        // int applesOnHouse = apples.Select(apple => a + apple).Count(pos => pos >= s && pos <= t);
        // int orangesOnHouse = oranges.Select(orange => b + orange).Count(pos => pos >= s && pos <= t);
        // here, LINQ is udes to transform and filter the data and to reduce the lines of code, save memory
    }