// 13) https://www.hackerrank.com/challenges/migratory-birds/problem?isFullScreen=true

public static int migratoryBirds(List<int> arr)
    {
        // Frequency dictionary
        Dictionary<int, int> freq = new Dictionary<int, int>();

        foreach (int bird in arr)
        {
            if (!freq.ContainsKey(bird))
                freq[bird] = 0;
                
            freq[bird]++;
        }

        // Find max frequency
        int maxFreq = freq.Values.Max();
        // Filter by max frequency and get the minimum bird type ID
        return freq
            .Where(entry => entry.Value == maxFreq)
            .Select(entry => entry.Key)
            .Min();
    }
