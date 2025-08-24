using System.Collections.Generic;
using System.Linq;
using Data.Inventory;
using ScriptableObjects;
using Systems.Inventory;
using UnityEngine;

namespace Systems.Processing
{
    public sealed class AssemblerProcessor : Processor
    {
        public AssemblerProcessor(IInventoryService inputInventoryService, IInventoryService outputInventoryService,
            MachineDetails machineDetails) : base(inputInventoryService, outputInventoryService, machineDetails)
        {
            SetRecipe(machineDetails.defaultRecipe);
            
            inputInventoryService.OnModelChanged += HandleInputsChanged;
            outputInventoryService.OnModelChanged += HandleOutputsChanged;
        }

        public override void Process()
        {
            if (!Running || State != ProcessorState.Processing)
                return;

            if (!HasValidRecipe())
            {
                Stop();
                return;
            }

            CurrentProgress += Time.fixedDeltaTime * ProcessingSpeed;
            ProgressChanged.Invoke(CurrentProgress, CurrentRecipe.timeToProcess);

            if (CurrentProgress >= CurrentRecipe.timeToProcess)
            {
                if (CompleteProcessing())
                    BeginProcessing();
            }
        }

        public override bool SetRecipe(Recipe recipe)
        {
            if (!IsValidRecipe(recipe))
                return false;

            if (CurrentRecipe.name == recipe.name)
                return true;

            Stop();
            CurrentRecipe = recipe;
            return true;
        }

        private void BeginProcessing()
        {
            if (!HasValidRecipe() || !HasRequiredInputs() || State == ProcessorState.Processing)
                return;

            ResetProgress();
            State = ProcessorState.Processing;

            InputInventoryService.Remove(CurrentRecipe.inputItems);
        }

        private bool CompleteProcessing()
        {
            if (!OutputInventoryService.CanHold(CurrentRecipe.outputItems))
                return false;
            
            Stop();
            return OutputInventoryService.Add(CurrentRecipe.outputItems);
        }

        private bool HasValidRecipe() => CurrentRecipe && IsValidRecipe(CurrentRecipe);

        private bool IsValidRecipe(Recipe recipe) =>
            recipe.RequiredInputSlotsCount <= InputInventoryService.GetCapacity() &&
            recipe.RequiredOutputSlotsCount <= OutputInventoryService.GetCapacity();

        private void Stop()
        {
            ResetProgress();
            State = ProcessorState.Idle;
        }

        private void ResetProgress()
        {
            CurrentProgress = 0f;
            ProgressChanged.Invoke(CurrentProgress, CurrentRecipe.timeToProcess);
        }

        private bool HasRequiredInputs() => CurrentRecipe.inputItems.All(input => InputInventoryService.Has(input));

        private void HandleInputsChanged(IList<Item> _) => BeginProcessing();
        private void HandleOutputsChanged(IList<Item> _)
        {
            if (CurrentProgress < CurrentRecipe.timeToProcess)
                return;

            if (CompleteProcessing())
                BeginProcessing();
        }
    }
}