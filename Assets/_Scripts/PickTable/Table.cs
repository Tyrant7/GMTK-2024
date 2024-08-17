using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] float maxRadius;
    [SerializeField] LayerMask tableObjectMask;
    [SerializeField] Collider2D tablePickArea;

    private List<GameObject> tableObjects;
    private GameObject playerObject;

    private Transform selected;
    private int selectCount = 0;

    public void Populate(List<GameObject> contents, GameObject playerPrefab)
    {
        tableObjects = new List<GameObject>();
        foreach (GameObject prefab in contents)
        {
            Vector2 pos = Random.insideUnitCircle * maxRadius;
            GameObject tableObject = Instantiate(prefab, (Vector2)transform.position + pos, Quaternion.identity, transform);
            tableObjects.Add(tableObject);
        }

        // Max attempts is in case there isn't a valid area at all
        for (int i = 0; i < 100; i++)
        {
            float randX = Random.Range(tablePickArea.bounds.min.x, tablePickArea.bounds.max.x);
            float randY = Random.Range(tablePickArea.bounds.min.y, tablePickArea.bounds.max.y);
            Vector2 randomPoint = new Vector2(randX, randY);
            Collider2D[] results = Physics2D.OverlapPointAll(randomPoint);
            foreach (Collider2D result in results)
            {
                if (result == tablePickArea)
                {
                    playerObject = Instantiate(playerPrefab, randomPoint, Quaternion.identity, transform);
                    tableObjects.Add(playerObject);
                    return;
                }
            }
        }
        // If failed we just spawn in the centre
        playerObject = Instantiate(playerPrefab, tablePickArea.bounds.center, Quaternion.identity, transform);
        tableObjects.Add(playerObject);
    }

    public List<GameObject> GetChosenElements()
    {
        List<GameObject> chosenObjects = new List<GameObject>();
        foreach (GameObject tableObject in tableObjects)
        {
            if (tablePickArea.OverlapPoint(tableObject.transform.position))
            {
                tableObject.transform.parent = null;
                DontDestroyOnLoad(tableObject);
                chosenObjects.Add(tableObject);
            }
        }
        return chosenObjects;
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.Mouse0))
        {
            if(selected)
            selected.localScale = new Vector3(selected.localScale.x / 1.2f, selected.localScale.y / 1.2f, selected.localScale.z);

            selected = null;
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (selected == null)
        {
            Collider2D[] results = Physics2D.OverlapPointAll(mousePos, tableObjectMask);

            // Get the ones furthest on top
            Transform best = null;
            int bestOrder = -1;
            foreach (Collider2D result in results)
            {
                // Cannot remove player object from selection area
                if (result.gameObject == playerObject) continue;

                int priority = result.transform.GetComponent<SpriteRenderer>().sortingOrder;
                if (priority > bestOrder)
                {
                    bestOrder = priority;
                    best = result.transform;
                }
            }
            if (best == null) return;

            selectCount++;
            best.GetComponent<SpriteRenderer>().sortingOrder = tableObjects.Count + selectCount;
            selected = best;

            selected.localScale = new Vector3(selected.localScale.x * 1.2f, selected.localScale.y * 1.2f, selected.localScale.z);
        }
        selected.position = mousePos;
      
    }
}
