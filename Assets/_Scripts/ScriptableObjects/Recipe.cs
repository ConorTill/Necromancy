using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public List<RecipeComponent> InputItems;
    public List<RecipeComponent> OutputItems;
    public float BaseSpeed;

    public int RequiredInputSlotsCount => InputItems.Count;
    public int RequiredOutputSlotsCount => OutputItems.Count;
}