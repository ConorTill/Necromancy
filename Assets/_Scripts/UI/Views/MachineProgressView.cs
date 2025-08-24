using Extensions;
using UnityEngine.UIElements;

namespace UI.Views
{
    public class MachineProgressView
    {
        private readonly UIDocument _document;
        private readonly StyleSheet _styleSheet;
        private readonly VisualElement _root;
        private VisualElement _container;

        private VisualElement _progressBar;
        private VisualElement _progressFill;
        
        public MachineProgressView(VisualElement root)
        {
            _root = root;
            
            Initialize();
        }

        private void Initialize()
        {
            _container = _root.GetChildOrCreate("progressBar_container", "progressBarContainer");
            _progressBar = _container.CreateChild("progressBar");
            _progressFill = _progressBar.CreateChild("progressFill");
        }

        public void UpdateProgress(float progress, float max)
        {
            _progressFill.style.width = _container.layout.width;
            _progressBar.style.width = new Length(progress / max * 100, LengthUnit.Percent);
        }
    }
}