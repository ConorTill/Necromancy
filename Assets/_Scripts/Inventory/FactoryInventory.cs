using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class FactoryInventory : MonoBehaviour
{
    [SerializeField]
    private int InitialInputSlotCount;
    [SerializeField]
    private int InitialOutputSlotCount;
    private InventorySlot[] InputSlots;
    private InventorySlot[] OutputSlots;

    public Action ItemsChanged;

    public int InputSlotsCount => InputSlots == null ? 0 : InputSlots.Length;
    public int OutputSlotsCount => OutputSlots == null ? 0 : OutputSlots.Length;

    private void Awake()
    {
        InputSlots = new InventorySlot[InitialInputSlotCount];
        OutputSlots = new InventorySlot[InitialOutputSlotCount];

        for (int i = 0; i < InputSlots.Length; i++)
        {
            InputSlots[i] = new InventorySlot();
        }

        for (int i = 0; i < OutputSlots.Length; i++)
        {
            OutputSlots[i] = new InventorySlot();
        }
    }

    public bool TryCompleteRecipe(Recipe recipe)
    {
        Debug.Log("Trying to complete recipe");
        var tempInputSlots = InputSlots.ToArray();
        var tempOutputSlots = OutputSlots.ToArray();

        foreach (var output in recipe.OutputItems)
        {
            Debug.Log($"Looking for output slot for {output.Quantity} of {output.Item.Name}");
            var outputSlot = tempOutputSlots.FirstOrDefault(slot => slot.HasItem(output.Item));
            if (outputSlot == null || !outputSlot.CanHoldAll(output.Item, output.Quantity))
            {
                outputSlot = tempOutputSlots.FirstOrDefault(slot => slot.IsEmpty);
            }

            if (outputSlot == null)
            {
                Debug.Log("No available output slot");
                return false;
            }

            var outputSlotIndex = Array.IndexOf(tempOutputSlots, outputSlot);
            if (!tempOutputSlots[outputSlotIndex].TryAddItem(output.Item, output.Quantity, out var remainder))
            {
                Debug.Log($"Failed to add to output slot index {outputSlotIndex}");
                return false;
            }
        }

        foreach (var input in recipe.InputItems)
        {
            Debug.Log($"Looking for input slot containing {input.Quantity} of {input.Item.Name}");
            var inputSlot = tempInputSlots.FirstOrDefault(slot => slot.HasItem(input.Item, input.Quantity));
            if (inputSlot == null)
            {
                Debug.Log("No available input");
                return false;
            }

            var inputSlotIndex = Array.IndexOf(tempInputSlots, inputSlot);

            if (!tempInputSlots[inputSlotIndex].TryRemoveItem(input.Item, input.Quantity))
            {
                Debug.Log($"Failed to take from input slot index {inputSlotIndex}");
                return false;
            }
        }

        InputSlots = tempInputSlots;
        OutputSlots = tempOutputSlots;
        ItemsChanged?.Invoke();
        Debug.Log("Completed recipe");
        return true;
    }

    public bool HasRequiredInputs(Recipe recipe)
    {
        foreach (var input in recipe.InputItems)
        {
            if (!InputSlots.Any(x => x.HasItem(input.Item, input.Quantity)))
            {
                Debug.Log($"No input could be found for {input.Quantity} of {input.Item}");
                return false;
            }
        }

        return true;
    }

    public bool CanHoldOutputs(Recipe recipe)
    {
        if (recipe.OutputItems.Count > OutputSlots.Length) return false;

        List<int> usedOutputSlotIndices = new();
        foreach (var output in recipe.OutputItems)
        {
            var availableSlot = OutputSlots.FirstOrDefault(x => x.CanHoldAll(output.Item, output.Quantity) && !usedOutputSlotIndices.Contains(Array.IndexOf(OutputSlots, x)));
            if (availableSlot == null)
            {
                Debug.Log($"No output could be found for {output.Quantity} of {output.Item}");
                return false;
            }

            usedOutputSlotIndices.Add(Array.IndexOf(OutputSlots, availableSlot));
        }

        return true;
    }

    public bool TryAddInputItem(Item item, int slotIndex, int quantity)
    {
        if (InputSlots[slotIndex].TryAddItem(item, quantity, out var remainder))
        {
            ItemsChanged?.Invoke();
            return true;
        }

        Debug.Log($"Failed to add {quantity} of {item.Name} to input index {slotIndex}");
        return false;
    }

    public bool TryAddOutputItem(Item item, int slotIndex, int quantity)
    {
        if (OutputSlots[slotIndex].TryAddItem(item, quantity, out var remainder))
        {
            ItemsChanged?.Invoke();
            return true;
        }

        Debug.Log($"Failed to add {quantity} of {item.Name} to output index {slotIndex}");
        return false;
    }
}