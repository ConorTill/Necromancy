using System;
using System.Collections.Generic;
using Data.Inventory;

namespace Systems.Inventory
{
    public class InventoryController
    {
        private readonly InventoryModel _model;

        public event Action<IList<Item>> OnModelChanged = delegate { };

        public InventoryController(InventoryModel model)
        {
            _model = model;
            
            _model.OnModelChanged += HandleModelChanged;
        }

        public PutItemResult HandleDrop(int targetSlotIndex, Item item) => _model.Put(targetSlotIndex, item);

        public void RemoveAt(int index) => _model.RemoveAt(index);
        
        public int GetCapacity() => _model.Capacity;

        private void HandleModelChanged(IList<Item> items) => OnModelChanged.Invoke(items);
    }
}