using Systems.Inventory;
using Systems.Processing;
using UI.Views;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class MachineDetails : ScriptableObject
    {
        public MachineType machineType;
        public float baseProcessingSpeed = 1f;
        public int inputCapacity;
        public int outputCapacity;
        public StorageView inputView;
        public StorageView outputView;
        public Recipe defaultRecipe;
    }
}