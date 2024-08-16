using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    public List<Vector2> GenerateEnvironment(GameObject environmentPrefab, int objectCount)
    {
        List<Vector2> objectPoints = new List<Vector2>();

        GameObject visualObject = Instantiate(environmentPrefab, Vector2.zero, Quaternion.identity);
        Environment environment = visualObject.GetComponent<Environment>();
        for (int i = 0; i < objectCount; i++)
        {
            Vector2 newPos = environment.AddRandomObject();
            if (newPos != Vector2.positiveInfinity)
            {
                objectPoints.Add(newPos);
            }
        }
        return objectPoints;
    }
}
