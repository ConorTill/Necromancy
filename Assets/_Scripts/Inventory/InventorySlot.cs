using UnityEngine;

public class InventorySlot
{
    public Item Item;
    public int Quantity;

    public bool IsEmpty => Item == null || Quantity <= 0;
    public bool IsFull => Quantity >= Item.MaxStackQuantity;
    public int AvailableSpace => Item.MaxStackQuantity - Quantity;

    public InventorySlot()
    {
        Item = null;
        Quantity = 0;
    }

    public InventorySlot(Item item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }

    public bool HasItem(Item item)
    {
        if (Item == null)
            return false;

        return Item.Name == item.Name;
    }

    public bool HasItem(Item item, int quantity)
    {
        if (Item == null)
            return false;

        return Item.Name == item.Name && quantity <= Quantity;
    }

    public bool CanHoldAll(Item item, int quantity)
    {
        if (IsEmpty) return true;
        if (Item.Name != item.Name) return false;
        if (Quantity + quantity > Item.MaxStackQuantity) return false;

        return true;
    }

    public bool TryAddItem(Item item, int quantity, out int remainder)
    {
        if (IsEmpty)
        {
            Item = item;
            Quantity = quantity;
            remainder = 0;
            return true;
        }

        if (Item.Name == item.Name)
        {
            var newQuantity = Quantity + quantity;
            if (newQuantity > Item.MaxStackQuantity)
            {
                remainder = Item.MaxStackQuantity - newQuantity;
                Quantity = Item.MaxStackQuantity;
            }
            else
            {
                remainder = 0;
                Quantity = newQuantity;
            }

            return true;
        }

        remainder = 0;
        return false;
    }

    public bool TryRemoveItem(Item item, int quantity)
    {
        if (!HasItem(item, quantity))
        {
            return false;
        }

        Quantity -= quantity;
        
        if (Quantity == 0)
            Clear();

        return true;
    }

    public void Clear()
    {
        Item = null;
        Quantity = 0;
    }
}