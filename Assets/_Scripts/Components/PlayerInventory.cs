using System;
using System.Collections.Generic;
using System.Linq;
using Data.Inventory;
using ScriptableObjects;
using Systems.Inventory;
using UI.Views;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Components
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private UIDocument document;
        [SerializeField] private StyleSheet styleSheet;
        
        [SerializeField] private int capacity = 20;
        [SerializeField] private List<ItemDetails> startingItems = new();

        private InventoryModel _model;
        private InventoryController _controller;
        private PlayerInventoryView _view;

        private void Awake()
        {
            _model = new InventoryModel(Array.Empty<ItemDetails>(), capacity);
            _controller = new InventoryController(_model);
            _view = new PlayerInventoryView(_controller, document, styleSheet);
            _model.Add(startingItems.Select(i => i.Create(Random.Range(1, 3))));
        }
    }
}