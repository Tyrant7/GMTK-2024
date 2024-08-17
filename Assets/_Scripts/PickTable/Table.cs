using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] float maxRadius;
    [SerializeField] LayerMask tableObjectMask;
    [SerializeField] Collider2D tablePickArea;

    private List<GameObject> tableObjects;

    private Transform selected;
    private int selectCount = 0;

    public void Populate(List<GameObject> contents)
    {
        tableObjects = new List<GameObject>();
        foreach (GameObject prefab in contents)
        {
            Vector2 pos = Random.insideUnitCircle * maxRadius;
            GameObject tableObject = Instantiate(prefab, (Vector2)transform.position + pos, Quaternion.identity, transform);
            tableObjects.Add(tableObject);
        }
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
        }
        selected.position = mousePos;
    }
}
