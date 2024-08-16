using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] float maxRadius;

    public void Populate(List<GameObject> contents)
    {
        Debug.Log(contents.Count);

        foreach (GameObject prefab in contents)
        {
            Vector2 pos = Random.insideUnitCircle * maxRadius;
            Instantiate(prefab, (Vector2)transform.position + pos, Quaternion.identity, transform);
        }
    }
}
