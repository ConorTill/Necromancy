using UnityEngine;

[CreateAssetMenu]
public class Factory : Item, IPlaceable
{
    public float SpeedMultiplier = 1f;
    public GameObject Prefab;
    public int InputSlotCount = 2;
    public int OutputSlotCount = 1;
}