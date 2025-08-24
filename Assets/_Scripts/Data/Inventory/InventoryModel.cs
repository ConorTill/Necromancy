using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;

namespace Data.Inventory
{
    public class InventoryModel
    {
        private IObservableArray<Item> Items { get; }

        public event Action<Item[]> OnModelChanged
        {
            add => Items.AnyValueChanged += value;
            remove => Items.AnyValueChanged -= value;
        }

        public InventoryModel(IEnumerable<ItemDetails> itemDetails, int capacity)
        {
            Items = new ObservableArray<Item>(capacity);
            foreach (var itemDetail in itemDetails)
            {
                Items.TryAdd(itemDetail.Create(1));
            }
        }

        public int Capacity => Items.Size;

        public Item Get(int index) => Items.Get(index);

        #region Add

        public bool CanHold(IEnumerable<Item> items)
        {
            var snapshot = Items
                .GetAll()
                .Select(itm => itm == null
                    ? null
                    : new Item(itm.details, itm.quantity) { Id = itm.Id })
                .ToArray();

            return items.All(itm => TryPlaceInSnapshot(itm, snapshot));
        }

        public bool Add(Item item) => Add(new[] { item });

        public bool Add(IEnumerable<Item> incomingItems)
        {
            var itemsSnapshot =
                Items
                    .GetAll()
                    .Select(itm => itm == null ? null : new Item(itm.details, itm.quantity) { Id = itm.Id })
                    .ToArray();

            if (incomingItems.Any(itm => !TryPlaceInSnapshot(itm, itemsSnapshot)))
                return false;

            Items.ReplaceAll(itemsSnapshot);
            return true;
        }
        
        private static bool TryPlaceInSnapshot(Item incomingItem, Item[] snapshot)
        {
            var remainder = incomingItem.quantity;

            foreach (var i in Enumerable.Range(0, snapshot.Length).Where(idx =>
                     {
                         var snapshotItem = snapshot[idx];
                         return snapshotItem?.details.Id == incomingItem.details.Id &&
                                snapshotItem.quantity < snapshotItem.details.maxStackQuantity;
                     }).OrderByDescending(idx => snapshot[idx].quantity))
            {
                var snapshotItem = snapshot[i];
                var remainingCapacity = incomingItem.details.maxStackQuantity - snapshotItem.quantity;
                if (remainingCapacity == 0)
                    continue;

                var quantityToPut = Math.Min(remainingCapacity, remainder);
                snapshotItem.quantity += quantityToPut;
                remainder -= quantityToPut;
                if (remainder <= 0)
                    break;
            }

            for (var i = 0; i < snapshot.Length && remainder > 0; i++)
            {
                if (snapshot[i] != null)
                    continue;

                var quantityToPut = Math.Min(incomingItem.details.maxStackQuantity, remainder);
                snapshot[i] = new Item(incomingItem.details, quantityToPut);
                remainder -= quantityToPut;
            }

            return remainder == 0;
        }

        #endregion

        #region Remove

        public bool Has(Item item) => Items.GetAll()
            .Any(i => i != null && i.details.Id == item.details.Id && i.quantity >= item.quantity);
        
        public bool Remove(Item item) => Remove(new[] { item });

        public bool Remove(IEnumerable<Item> itemsToRemove)
        {
            var snapshot = Items
                .GetAll()
                .Select(itm => itm == null
                    ? null
                    : new Item(itm.details, itm.quantity) { Id = itm.Id })
                .ToArray();

            if (itemsToRemove.Any(itm => !TryRemoveFromSnapshot(itm, snapshot)))
                return false;

            Items.ReplaceAll(snapshot);
            return true;
        }

        private static bool TryRemoveFromSnapshot(Item itemToRemove, Item[] snapshot)
        {
            var remainder = itemToRemove.quantity;

            foreach (var i in Enumerable.Range(0, snapshot.Length)
                         .Where(idx => snapshot[idx]?.details.Id == itemToRemove.details.Id)
                         .OrderBy(idx => snapshot[idx].quantity))
            {
                var snapshotItem = snapshot[i];

                if (snapshotItem.quantity >= remainder)
                {
                    snapshotItem.quantity -= remainder;
                    if (snapshotItem.quantity == 0)
                        snapshot[i] = null;

                    return true;
                }

                remainder -= snapshotItem.quantity;
                snapshot[i] = null;
            }

            return false;
        }

        #endregion

        public PutItemResult Put(int index, Item item)
        {
            var slot = Items.Get(index);
            if (slot == null)
            {
                Items.ReplaceAt(index, item);
                return PutItemResult.FullDeposit();
            }

            if (slot.details.Id == item.details.Id)
            {
                var remainder = slot.quantity + item.quantity - item.details.maxStackQuantity;
                if (remainder <= 0)
                {
                    Items.ReplaceAt(index, slot.WithQuantity(slot.quantity + item.quantity));
                    return PutItemResult.FullDeposit();
                }

                Items.ReplaceAt(index, slot.WithQuantity(item.details.maxStackQuantity));
                return PutItemResult.PartialDeposit(item.WithQuantity(remainder));
            }

            var result = PutItemResult.Clobber(slot);
            Items.ReplaceAt(index, item);
            return result;
        }

        public void RemoveAt(int index) => Items.ReplaceAt(index, null);
    }
}