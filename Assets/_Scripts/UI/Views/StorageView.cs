using System.Collections.Generic;
using Data.Inventory;
using Systems.Inventory;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Views
{
    public abstract class StorageView
    {
        protected readonly Slot[] Slots;
        protected readonly InventoryController InventoryController;
        
        protected VisualElement Root;
        protected VisualElement Container;

        protected StorageView(InventoryController inventoryController)
        {
            InventoryController = inventoryController;
            Slots = new Slot[inventoryController.GetCapacity()];
            
            InventoryController.OnModelChanged += UpdateView;
        }
        
        protected abstract void Initialize();

        public PutItemResult HandleDrop(int targetSlotIndex, Item item) =>
            InventoryController.HandleDrop(targetSlotIndex, item);

        public void HandleRemove(Slot slot)
        {
            InventoryController.RemoveAt(slot.Index);
        }

        private void UpdateView(IList<Item> items)
        {
            for (var i = 0; i < Slots.Length; i++)
            {
                var item = items[i];
                if (item == null)
                    Slots[i].Remove();
                else
                    Slots[i].Set(item);
            }
        }
    }
}