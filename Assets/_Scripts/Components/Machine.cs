using System;
using System.Collections.Generic;
using System.Linq;
using Data.Inventory;
using ScriptableObjects;
using Systems.Inventory;
using Systems.Processing;
using UI.Views;
using UnityEngine;
using UnityEngine.UIElements;

namespace Components
{
    public class Machine : MonoBehaviour
    {
        [SerializeField] private MachineDetails machineDetails;

        [SerializeField] private UIDocument document;
        [SerializeField] private StyleSheet styleSheet;
        
        [SerializeField] private List<ItemDetails> inputStartingItems = new();
        [SerializeField] private int inputSize;

        [SerializeField] private List<ItemDetails> outputStartingItems = new();
        [SerializeField] private int outputSize;

        private InventoryModel _inputModel;
        private InventoryModel _outputModel;
        
        private InventoryController _inputController;
        private InventoryController _outputController;

        private MachineView _machineView;

        private Processor _assemblerProcessor;

        private void Awake()
        {
            _inputModel = new InventoryModel(Array.Empty<ItemDetails>(), machineDetails.inputCapacity);
            _outputModel = new InventoryModel(Array.Empty<ItemDetails>(), machineDetails.outputCapacity);

            _inputController = new InventoryController(_inputModel);
            _outputController = new InventoryController(_outputModel);

            _machineView = new MachineView(_inputController, _outputController, document, styleSheet);
            
            _assemblerProcessor = machineDetails.machineType switch
            {
                MachineType.Assembler => new AssemblerProcessor(new InventoryService(_inputModel),
                    new InventoryService(_outputModel), machineDetails),
                _ => throw new ArgumentOutOfRangeException()
            };

            _assemblerProcessor.ProgressChanged += _machineView.UpdateProgress;

            _inputModel.Add(inputStartingItems.Select(i => i.Create(10)));
            _outputModel.Add(outputStartingItems.Select(i => i.Create(5)));
        }

        private void FixedUpdate()
        {
            _assemblerProcessor.Process();
        }
    }
}