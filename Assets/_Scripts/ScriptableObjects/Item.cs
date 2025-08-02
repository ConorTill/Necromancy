using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public int MaxStackQuantity = 99;
    public Sprite Icon;
}