using System.Collections.Generic;
using Data.Inventory;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class Recipe : ScriptableObject
    {
        public List<Item> inputItems;
        public List<Item> outputItems;
        public float timeToProcess;

        public int RequiredInputSlotsCount => inputItems.Count;
        public int RequiredOutputSlotsCount => outputItems.Count;
    }
}