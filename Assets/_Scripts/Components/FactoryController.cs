using System;
using UnityEngine;

public class FactoryController : MonoBehaviour
{
    private FactoryInventory Inventory;
    [SerializeField] private Recipe CurrentRecipe;
    [SerializeField] private float SpeedMultiplier = 1f;
    [SerializeField] private FactoryState State = FactoryState.Stopped;
    [SerializeField] private float CurrentProgress = 0f;
    [SerializeField] private bool Running = true;

    public Action<float, float> ProgressChanged;

    private void Awake()
    {
        Inventory = GetComponent<FactoryInventory>();
    }

    private void FixedUpdate()
    {
        if (Running)
        {
            ProcessRecipe();
        }
    }

    private void ProcessRecipe()
    {
        if (!CanProcess())
            return;

        if (State != FactoryState.Processing)
        {
            BeginProcessing();
        }

        CurrentProgress += Time.fixedDeltaTime * SpeedMultiplier;
        ProgressChanged?.Invoke(CurrentProgress, CurrentRecipe.BaseSpeed);

        if (CurrentProgress >= CurrentRecipe.BaseSpeed)
        {
            CompleteProcessing();
        }
    }

    private bool CanProcess()
    {
        Debug.Log("Checking if factory can process");
        if (!HasValidRecipe() || !Inventory.HasRequiredInputs(CurrentRecipe))
        {
            Debug.Log("Unable to process");
            if (State != FactoryState.Stopped)
            {
                Debug.Log("Stopping processing");
                State = FactoryState.Stopped;
            }
            if (CurrentProgress != 0f)
            {
                ResetProgress();
            }

            return false;
        }

        if (!Inventory.CanHoldOutputs(CurrentRecipe))
        {
            Debug.Log("Unable to process");
            if (State == FactoryState.Processing)
            {
                Debug.Log("Pausing processing");
                State = FactoryState.Paused;
            }

            return false;
        }

        Debug.Log("Can process");
        return true;
    }

    private void BeginProcessing()
    {
        Debug.Log("Beginning processing");
        if (State == FactoryState.Processing)
        {
            Debug.Log("Already processing");
            return;
        }

        if (State == FactoryState.Stopped || State == FactoryState.Completed)
        {
            ResetProgress();
            Debug.Log("Restarting processing");
            State = FactoryState.Processing;
        }
        else if (State == FactoryState.Paused)
        {
            Debug.Log("Resuming processing");
            State = FactoryState.Processing;
        }
    }

    private void CompleteProcessing()
    {
        Debug.Log($"Completing processing for {CurrentRecipe.name}");
        if (Inventory.TryCompleteRecipe(CurrentRecipe))
        {
            ResetProgress();
            Debug.Log($"Completed processing");
            return;
        }
        Debug.Log($"Failed to complete processing");
    }

    private bool HasValidRecipe()
    {
        if (CurrentRecipe == null)
        {
            Debug.Log("Recipe is null");
            return false;
        }

        if (CurrentRecipe.RequiredInputSlotsCount > Inventory.InputSlotsCount)
        {
            Debug.Log("Not enough input slots for the current recipe");
            return false;
        }

        if (CurrentRecipe.RequiredOutputSlotsCount > Inventory.OutputSlotsCount)
        {
            Debug.Log("Not enough output slots for the current recipe");
            return false;
        }

        return true;
    }

    private void ResetProgress()
    {
        CurrentProgress = 0f;
        ProgressChanged?.Invoke(CurrentProgress, CurrentRecipe.BaseSpeed);
        Debug.Log("Reset progress");
    }

    private enum FactoryState
    {
        Stopped,
        Processing,
        Paused,
        Completed
    }
}