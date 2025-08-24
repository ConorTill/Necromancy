using Extensions;
using Systems.Inventory;
using UnityEngine.UIElements;

namespace UI.Views
{
    public class MachineView
    {
        private readonly UIDocument _document;
        private readonly StyleSheet _styleSheet;
        private VisualElement _root;
        private VisualElement _container;

        public MachineInventoryView InputView;
        public MachineInventoryView OutputView;
        public MachineProgressView ProgressView;
        
        public MachineView(InventoryController inputController, InventoryController outputController, UIDocument document, StyleSheet styleSheet)
        {
            _document = document;
            _styleSheet = styleSheet;

            Initialize(inputController, outputController);
        }

        private void Initialize(InventoryController inputController, InventoryController outputController)
        {
            _root = _document.rootVisualElement;

            _root.Clear();
            _root.styleSheets.Add(_styleSheet);

            _container = _root.CreateChild("container").WithManipulator(new DragManipulator());
            var headerLabel = new Label("Machine");
            _container.Add(headerLabel);

            InputView = new MachineInventoryView(inputController, _container, "Inputs");
            ProgressView = new MachineProgressView(_container);
            OutputView = new MachineInventoryView(outputController, _container, "Outputs");
        }
        
        public void UpdateProgress(float progress, float max) => ProgressView.UpdateProgress(progress, max);
    }
}