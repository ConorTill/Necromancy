using Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Views
{
    public class DragIconView : MonoBehaviour
    {
        [SerializeField] private UIDocument document;
        [SerializeField] private StyleSheet styleSheet;
        public VisualElement GhostIcon;
        public Label StackLabel;
        
        public void Initialize()
        {
            var root =  document.rootVisualElement;
            
            root.Clear();
            root.styleSheets.Add(styleSheet);

            root.BringToFront();
            root.pickingMode = PickingMode.Ignore;
            
            var container = root.CreateChild("container");
            container.pickingMode = PickingMode.Ignore;
            GhostIcon = container.CreateChild("dragIcon");
            GhostIcon.pickingMode = PickingMode.Ignore;
            GhostIcon.BringToFront();
            StackLabel = GhostIcon.CreateChild<Label>("stackCount");
            StackLabel.pickingMode = PickingMode.Ignore;
        }
    }
}