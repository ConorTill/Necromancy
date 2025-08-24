using System;
using Data.Inventory;
using UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace Systems.Inventory
{
    public class ItemDragHandler : MonoBehaviour
    {
        public static ItemDragHandler Instance { get; private set; }

        [SerializeField] private DragIconView view;

        private IPanel _panel;
        private bool _isDragging;
        private Slot _origin;
        private Item _item;

        private void Awake()
        {
            Instance = this;
            view?.Initialize();
        }

        private void Update()
        {
            if (!_isDragging || _panel == null) return;

            var panelPos = RuntimePanelUtils.ScreenToPanel(_panel, Input.mousePosition);
            panelPos.y = _panel.visualTree.resolvedStyle.height - panelPos.y;

            SetGhostIconPosition(panelPos);

            if (Input.GetMouseButtonUp(0))
            {
                ResolveDrop(panelPos);
            }
        }

        public void StartDrag(Vector2 position, Slot origin)
        {
            if (origin == null || origin.ItemId == Guid.Empty) return;

            _panel = origin.panel;
            SetGhostIconPosition(position);
            StartDragging(origin);

            origin.Owner.HandleRemove(origin);
        }

        private void ResolveDrop(Vector2 screenPos)
        {
            var target = FindSlotUnderPointer(screenPos);

            var result = target != null
                ? target.Owner.HandleDrop(target.Index, _item)
                : _origin.Owner.HandleDrop(_origin.Index, _item);

            if (result.Item != null)
            {
                SetGhostItem(result.Item);
                return;
            }

            EndDrag();
        }

        private Slot FindSlotUnderPointer(Vector2 panelPos) =>
            _panel?.visualTree.Query<Slot>()
                .Where(slot => slot.worldBound.Contains(panelPos))
                .Last();

        private void StartDragging(Slot slot)
        {
            _isDragging = true;

            SetGhostItem(slot.Item);
            _origin = slot;
        }
        
        private void SetGhostItem(Item item)
        {
            if (!view || item is null || _item == item)
                return;

            _item = item.Copy();
            SetGhostIcon(_item);
        }

        private void SetGhostIcon(Item item)
        {
            view.GhostIcon.style.backgroundImage = item.details.icon.texture;
            view.GhostIcon.style.visibility = Visibility.Visible;

            view.StackLabel.text = item.quantity.ToString();
            view.StackLabel.style.visibility = item.quantity > 1 ? Visibility.Visible : Visibility.Hidden;
        }

        private void EndDrag()
        {
            _isDragging = false;
            _panel = null;
            _origin = null;
            _item = null;

            ClearGhostIcon();
        }
        
        private void ClearGhostIcon()
        {
            if (!view)
                return;

            view.GhostIcon.style.backgroundImage = null;
            view.GhostIcon.style.visibility = Visibility.Hidden;

            view.StackLabel.text = _item?.quantity.ToString();
            view.StackLabel.style.visibility = _item == null ? Visibility.Hidden :
                _item.quantity > 1 ? Visibility.Visible : Visibility.Hidden;
        }

        private void SetGhostIconPosition(Vector2 position)
        {
            if (!view) return;

            view.GhostIcon.style.top = position.y - view.GhostIcon.layout.height / 2;
            view.GhostIcon.style.left = position.x - view.GhostIcon.layout.width / 2;
        }
    }
}