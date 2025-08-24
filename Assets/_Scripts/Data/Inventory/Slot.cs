using System;
using Extensions;
using Systems.Inventory;
using UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace Data.Inventory
{
    public class Slot : VisualElement
    {
        public readonly Image Icon;
        public readonly Label StackLabel;
        public int Index => parent.IndexOf(this);
        public Guid ItemId => Item?.Id ?? Guid.Empty;
        public Sprite BaseSprite => Item?.details.icon;
        public Item Item;

        public StorageView Owner { get; internal set; }

        public Slot()
        {
            var frame = this.CreateChild("slotFrame");
            Icon = frame.CreateChild<Image>("slotIcon");
            StackLabel = Icon.CreateChild<Label>("stackCount");
            RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button != 0 || ItemId == Guid.Empty)
            {
                return;
            }
            
            ItemDragHandler.Instance.StartDrag(evt.position, this);
            evt.StopPropagation();
        }

        public void Set(Item item)
        {
            Item = item;
            Icon.image = BaseSprite ? BaseSprite.texture : null;

            StackLabel.text = item.quantity > 1 ? item.quantity.ToString() : string.Empty;
            StackLabel.visible = item.quantity > 1;
        }

        public void Remove()
        {
            Icon.image = null;
            StackLabel.visible = false;
            StackLabel.text = null;
            Item = null;
        }
    }
}