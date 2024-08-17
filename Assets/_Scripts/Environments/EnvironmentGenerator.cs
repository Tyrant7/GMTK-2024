using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    public List<EnvironmentObject> GenerateEnvironment(GameObject environmentPrefab, int objectCount, bool debug)
    {
        List<EnvironmentObject> objectPoints = new List<EnvironmentObject>();

        GameObject visualObject = Instantiate(environmentPrefab, Vector2.zero, Quaternion.identity);
        Environment environment = visualObject.GetComponent<Environment>();
        DontDestroyOnLoad(visualObject);
        for (int i = 0; i < objectCount; i++)
        {
            EnvironmentObject newObject = environment.AddRandomObject();
            if (newObject != null)
            {
                if (debug) newObject.gameObject.SetActive(false);
                objectPoints.Add(newObject);
            }
        }
        if (debug) visualObject.SetActive(false);
        return objectPoints;
    }
}
