using System;
using Data.Inventory;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class ItemDetails : ScriptableObject
    {
        public Guid Id = Guid.NewGuid();
        public string description;
        public int maxStackQuantity = 99;
        public Sprite icon;

        public Item Create(int quantity)
        {
            return new Item(this, quantity);
        }
    }
}