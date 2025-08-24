using System;
using System.Collections.Generic;
using Data.Inventory;

namespace Systems.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryModel _model;
        
        public event Action<IList<Item>> OnModelChanged = delegate { };
        public InventoryService(InventoryModel model)
        {
            _model = model;
            _model.OnModelChanged += HandleModelUpdated;
        }

        public bool Add(Item item) => _model.Add(item);
        public bool Add(IEnumerable<Item> items) => _model.Add(items);

        public bool Remove(Item item) => _model.Remove(item);
        public bool Remove(IEnumerable<Item> items) => _model.Remove(items);

        public bool Has(Item item) => _model.Has(item);
        public void HandleModelUpdated(IList<Item> items) => OnModelChanged.Invoke(items);

        public int GetCapacity() => _model.Capacity;
        
        public bool CanHold(IEnumerable<Item> items) => _model.CanHold(items);
    }
}