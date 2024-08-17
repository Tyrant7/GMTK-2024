using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Scorer : MonoBehaviour
{
    const float MIN_SCORE_DIST = 0.3f;
    const float MAX_SCORE_DIST = 1.5f;

    const int SCORE_MULTIPLIER = 10;

    const int EXTRA_PENALTY = 5;

    public static int ScoreEnvironment(List<EnvironmentObject> actualObjects, List<CutoutObject> placedObjects)
    {
        // For each one, we'll find the closest matching cardboard equivalent
        int totalScore = 0;
        foreach (CutoutObject placed in placedObjects)
        {
            // Give a penalty for each extra object we placed
            if (actualObjects.Count == 0)
            {
                totalScore -= EXTRA_PENALTY;
                continue;
            }

            // Find the closest equivalent to this ID
            EnvironmentObject best = null;
            float bestDist = 10000;

            foreach (EnvironmentObject actual in actualObjects)
            {
                if (placed.equivalentID == actual.equivalentID)
                {
                    float diff = Vector2.Distance(actual.transform.position, placed.transform.position);
                    if (diff < bestDist)
                    {
                        best = actual;
                        bestDist = diff;
                    }
                }
            }

            // Once we've found the closest, we can remove it so as to not count it again
            actualObjects.Remove(best);

            // And score based on distance
            if (bestDist > MAX_SCORE_DIST)
            {
                continue;
            }
            float d = Mathf.Max(bestDist, MIN_SCORE_DIST) - MIN_SCORE_DIST;
            float adjust = MAX_SCORE_DIST / (d + 1);

            int score = Mathf.CeilToInt(adjust * SCORE_MULTIPLIER);
            totalScore += Mathf.Max(score, 0);

            Debug.Log(score + " for " + best + " with dist " + bestDist);
        }
        return totalScore;
    }
}
