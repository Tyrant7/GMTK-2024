using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopTable : MonoBehaviour
{
    [SerializeField] LayerMask workshopTableObjectMask;
    [SerializeField] Collider2D workshopTablePlaceArea;

    private List<GameObject> workshopTableObjects;
    private Transform selected;
    private int selectCount = 0;


    public void AddChosenObjects(List<GameObject> chosenObjects)
	{
        workshopTableObjects = new List<GameObject>();

        foreach(GameObject chosenObject in chosenObjects)
		{
            chosenObject.transform.SetParent(transform);
            workshopTableObjects.Add(chosenObject);
		}
	}

    public List<CutoutObject> GetPlacedElements()
    {
        List<CutoutObject> chosenObjects = new List<CutoutObject>();
        foreach (GameObject tableObject in workshopTableObjects)
        {
            if (workshopTablePlaceArea.OverlapPoint(tableObject.transform.position))
            {
                chosenObjects.Add(tableObject.GetComponent<CutoutObject>());
            }
        }
        return chosenObjects;
    }

    private void Update()
    {
        if (!Input.GetKey(KeyCode.Mouse0))
        {
            if(selected)
			{
                Color col = selected.GetComponent<SpriteRenderer>().color;
                col.a = 1;
                selected.GetComponent<SpriteRenderer>().color = col;
            }
         
            selected = null;
            return;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (selected == null)
        {
            Collider2D[] results = Physics2D.OverlapPointAll(mousePos, workshopTableObjectMask);

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
            best.GetComponent<SpriteRenderer>().sortingOrder = workshopTableObjects.Count + selectCount;
            selected = best;

            Color col = selected.GetComponent<SpriteRenderer>().color;
            col.a = 0.75f;
            selected.GetComponent<SpriteRenderer>().color = col;
        }
        selected.position = mousePos;

    }
}
