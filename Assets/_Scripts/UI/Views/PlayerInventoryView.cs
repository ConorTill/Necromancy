using Data.Inventory;
using Extensions;
using Systems.Inventory;
using UnityEngine.UIElements;

namespace UI.Views
{
    public sealed class PlayerInventoryView : StorageView
    {
        private readonly UIDocument _document;
        private readonly StyleSheet _styleSheet;

        public PlayerInventoryView(InventoryController inventoryController, UIDocument document,
            StyleSheet styleSheet) : base(
            inventoryController)
        {
            _document = document;
            _styleSheet = styleSheet;

            Initialize();
        }

        protected override void Initialize()
        {
            Root = _document.rootVisualElement;
            Root.Clear();
            Root.styleSheets.Add(_styleSheet);
            Root.style.justifyContent = Justify.FlexEnd;
            Container = Root.CreateChild("container");
            Container.style.alignSelf = Align.Center;
            Container.style.top = 0;

            var inventory = Container.CreateChild("inventory");
            var header = inventory.CreateChild("inventoryHeader");
            header.Add(new Label("Inventory"));

            var slotsContainer = inventory.CreateChild("slotsContainer");

            for (var i = 0; i < Slots.Length; i++)
            {
                var slot = slotsContainer.CreateChild<Slot>("slot");
                slot.Owner = this;
                Slots[i] = slot;
            }
        }
    }
}