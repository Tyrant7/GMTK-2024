using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] EnvironmentObject[] environmentObjects;
    [SerializeField] Collider2D[] objectAreas;

    const int MAX_ATTEMPTS = 5000;

    public EnvironmentObject AddRandomObject()
    {
        List<EnvironmentObject> weightedList = new List<EnvironmentObject>();
        foreach (EnvironmentObject env in environmentObjects)
        {
            for (int i = 0; i < env.probability; i++)
            {
                weightedList.Add(env);
            }
        }
        EnvironmentObject randomObject = weightedList[Random.Range(0, weightedList.Count)];

        // Now that we have our random object, we'll pick a point inside its allowed area
        Collider2D allowedArea = objectAreas[randomObject.allowedAreaIndex];
        Bounds areaBounds = allowedArea.bounds;

        // Max attempts is in case there isn't a valid area at all
        for (int i = 0; i < MAX_ATTEMPTS; i++)
        {
            Vector2 randomPoint = new Vector2(Random.Range(areaBounds.min.x, areaBounds.max.x), Random.Range(areaBounds.min.y, areaBounds.max.y));
            Collider2D[] results = Physics2D.OverlapPointAll(randomPoint);
            foreach (Collider2D result in results)
            {
                if (result == allowedArea)
                {
                    return Instantiate(randomObject, randomPoint, Quaternion.identity, transform);
                }
            }
        }
        Debug.LogWarning("Failed to successfully place object " + randomObject.name);
        return null;
    }
}