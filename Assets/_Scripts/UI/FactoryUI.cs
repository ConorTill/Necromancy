using UnityEngine;
using UnityEngine.UI;


public class FactoryUI : MonoBehaviour
{
    [SerializeField] private Transform CameraTransform;

    [SerializeField] private FactoryController FactoryController;
    [SerializeField] private FactoryInventory FactoryInventory;

    [SerializeField] private Image InputsContainer;
    [SerializeField] private Image OutputsContainer;
    [SerializeField] private GameObject SlotPrefab;
    [SerializeField] private ProgressBar ProgressBar;

    private void Awake()
    {
        CameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (CameraTransform != null)
        {
            transform.rotation = CameraTransform.rotation;
        }
    }

    private void SetSlots()
    {

    }

    private void SetProgressBar(float currentProgress, float maxProgress)
    {
        if (FactoryInventory == null || ProgressBar == null)
        {
            return;
        }

        ProgressBar.SetRatio(currentProgress, maxProgress);
    }

    private void OnEnable()
    {
        FactoryController.ProgressChanged += SetProgressBar;
        FactoryInventory.ItemsChanged += SetSlots;
    }

    private void OnDisable()
    {
        FactoryController.ProgressChanged -= SetProgressBar;
        FactoryInventory.ItemsChanged -= SetSlots;
    }
}