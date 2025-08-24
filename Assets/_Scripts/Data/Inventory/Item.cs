using System;
using ScriptableObjects;

namespace Data.Inventory
{
    [Serializable]
    public class Item
    {
        public Guid Id;
        public ItemDetails details;
        public int quantity;

        public Item(ItemDetails details, int quantity = 1)
        {
            Id = Guid.NewGuid();
            this.details = details;
            this.quantity = quantity;
        }
        
        private Item(Guid id, ItemDetails details, int quantity = 1)
        {
            Id = id;
            this.details = details;
            this.quantity = quantity;
        }

        public Item WithQuantity(int newQuantity)
        {
            quantity = newQuantity;
            return this;
        }

        public Item Copy() => new(Id, details, quantity);
    }
}