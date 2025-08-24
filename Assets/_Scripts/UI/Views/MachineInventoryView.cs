using Data.Inventory;
using Extensions;
using Systems.Inventory;
using UnityEngine.UIElements;

namespace UI.Views
{
    public sealed class MachineInventoryView : StorageView
    {
        private readonly string _panelName;

        public MachineInventoryView(InventoryController inventoryController, VisualElement root, string panelName) : base(inventoryController)
        {
            Root = root;
            _panelName = panelName;
            
            Initialize();
        }

        protected override void Initialize()
        {
            Container = Root.GetChildOrCreate($"{_panelName}_container", "machineInventoryContainer");
            Container.Add(new Label(_panelName));
            
            var slotsContainer = Container.CreateChild("slotsContainer");
            for (var i = 0; i < Slots.Length; i++)
            {
                var slot = slotsContainer.CreateChild<Slot>("slot");
                slot.Owner = this;
                Slots[i] = slot;
            }
        }
    }
}