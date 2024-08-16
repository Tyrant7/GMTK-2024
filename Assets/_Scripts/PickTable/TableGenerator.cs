using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableGenerator : MonoBehaviour
{
    public List<GameObject> GenerateTable(List<EnvironmentObject> environment, List<EnvironmentObject> unlocked, int pollutionAmount)
    {
        // We'll add the environment objects to the list to ensure
        // we can always meet the requirements for this diorama
        List<GameObject> choices = new List<GameObject>();
        foreach (EnvironmentObject envObj in environment)
        {
            GameObject randomEquivalent = envObj.cardboardEquivalents[Random.Range(0, envObj.cardboardEquivalents.Length)];
            choices.Add(randomEquivalent);
        }

        // But we'll also add some extra to pollute the pool
        for (int i = 0; i < pollutionAmount; i++)
        {
            EnvironmentObject randomUnlocked = unlocked[Random.Range(0, unlocked.Count)];
            GameObject randomEquivalent = randomUnlocked.cardboardEquivalents[Random.Range(0, randomUnlocked.cardboardEquivalents.Length)];
            choices.Add(randomEquivalent);
        }
        return choices;
    }
}
