using System;
using ScriptableObjects;
using Systems.Inventory;

namespace Systems.Processing
{
    public abstract class Processor
    {
        protected readonly IInventoryService InputInventoryService;
        protected readonly IInventoryService OutputInventoryService;
        protected MachineType MachineType;
        protected Recipe CurrentRecipe;
        protected float ProcessingSpeed;
        protected ProcessorState State = ProcessorState.Idle;
        protected float CurrentProgress;
        protected bool Running = true;

        public Action<float, float> ProgressChanged = delegate { };

        protected Processor(IInventoryService inputInventoryService, IInventoryService outputInventoryService, MachineDetails machineDetails)
        {
            InputInventoryService = inputInventoryService;
            OutputInventoryService = outputInventoryService;
            MachineType = machineDetails.machineType;
            CurrentRecipe = machineDetails.defaultRecipe;
            ProcessingSpeed = machineDetails.baseProcessingSpeed;
        }
        
        public void Pause() => Running = false;
        public void Resume() => Running = true;

        public abstract void Process();
        public abstract bool SetRecipe(Recipe recipe);
    }
}