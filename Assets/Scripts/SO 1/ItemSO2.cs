using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;


[System.Serializable]
public class Item2
{
    public string name2; 
    public int attack2;  
    public int defense2;  
    public int cost2;
    public Sprite sprite2; 
    public float percent2; 
}

[CreateAssetMenu(fileName = "ItemSO2", menuName = "Scriptable Object/ItemSO2")]
public class ItemSO2 : ScriptableObject
{
    public Item[] items; 
}