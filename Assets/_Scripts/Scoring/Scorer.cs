using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scorer : MonoBehaviour
{
    [SerializeField] GameObject scoreTextPrefab;

    const float MIN_SCORE_DIST = 0.3f;
    const float MAX_SCORE_DIST = 1.5f;

    const int SCORE_MULTIPLIER = 10;

    const int EXTRA_PENALTY = 5;

    const float SCORE_INTERVALS = 0.25f;

    public bool Animating { get; private set; }

    public ScoreDisplayInfo ScoreEnvironment(List<EnvironmentObject> actualObjects, List<CutoutObject> placedObjects)
    {
        ScoreDisplayInfo displayInfo = new ScoreDisplayInfo();

        // For each one, we'll find the closest matching cardboard equivalent
        int totalScore = 0;
        foreach (CutoutObject placed in placedObjects)
        {
            // Give a penalty for each extra object we placed
            if (actualObjects.Count == 0)
            {
                totalScore -= EXTRA_PENALTY;
                displayInfo.AddScore(placed.transform.position, EXTRA_PENALTY);
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

            int score = Mathf.Max(Mathf.CeilToInt(adjust * SCORE_MULTIPLIER), 0);
            totalScore += score;
            displayInfo.AddScore(placed.transform.position, score);
        }
        displayInfo.totalScore = totalScore;
        return displayInfo;
    }

    public IEnumerator AnimateScores(ScoreDisplayInfo displayInfo)
    {
        Animating = true;
        foreach (var info in displayInfo.scores)
        {
            GameObject g = Instantiate(scoreTextPrefab, info.Item1, Quaternion.identity);
            ScoreText t = g.GetComponent<ScoreText>();
            t.Initialize(info.Item2.ToString());
            yield return new WaitForSeconds(SCORE_INTERVALS);
        }
        Animating = false;
    }
}

public class ScoreDisplayInfo
{
    public List<Tuple<Vector2, int>> scores = new List<Tuple<Vector2, int>>();
    public int totalScore = 0;

    public void AddScore(Vector2 pos, int score)
    {
        scores.Add(new Tuple<Vector2, int>(pos, score));
    }
}