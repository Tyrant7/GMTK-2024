using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutout", menuName = "New Cutout")]
public class Cutout : ScriptableObject
{
    public GameObject[] cardboardEquivalents;
    public GameObject[] realisticEquivalents;
}
