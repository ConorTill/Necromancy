using System;
using System.Collections.Generic;
using Data.Inventory;

namespace Systems.Inventory
{
    public interface IInventoryService
    {
        public event Action<IList<Item>> OnModelChanged;
        public bool Add(Item item);
        public bool Add(IEnumerable<Item> items);
        public bool Remove(Item item);
        public bool Remove(IEnumerable<Item> items);
        public int GetCapacity();
        public bool Has(Item item);
        public bool CanHold(IEnumerable<Item> items);
    }
}