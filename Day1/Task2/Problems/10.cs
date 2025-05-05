// 10) https://www.hackerrank.com/challenges/breaking-best-and-worst-records/problem?isFullScreen=true

public static List<int> breakingRecords(List<int> scores)
    {
        int maxCount = 0; 
        int minCount = 0; 
        int maxScore = scores[0];
        int minScore = scores[0];

        // Loop amd get count
        for (int i = 1; i < scores.Count; i++)
        {
            if (scores[i] > maxScore) 
            {
                maxScore = scores[i];
                maxCount++; 
            }
            else if (scores[i] < minScore) 
            {
                minScore = scores[i];
                minCount++; 
            }
        }
        return new List<int> { maxCount, minCount };
    }