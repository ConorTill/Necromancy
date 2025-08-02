using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FactoryInventory))]
public class FactoryInventoryEditor : Editor
{
    private Item[] Assets;
    private string[] AssetNames;
    private int SelectedIndex = 0;
    private int SlotIndex = 0;
    private int Quantity = 1;

    private void OnEnable()
    {
        string[] guids = AssetDatabase.FindAssets("t:Item");
        Assets = new Item[guids.Length];
        AssetNames = new string[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            Assets[i] = AssetDatabase.LoadAssetAtPath<Item>(path);
            AssetNames[i] = Assets[i] != null ? Assets[i].name : "Missing";
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var factoryInventory = (FactoryInventory)target;

        if (Assets.Length > 0)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Add Input Item", EditorStyles.boldLabel);

            SelectedIndex = EditorGUILayout.Popup("Item", SelectedIndex, AssetNames);
            SlotIndex = EditorGUILayout.Popup("Slot Index", SlotIndex, Enumerable.Range(0, factoryInventory.InputSlotsCount).Select(x => x.ToString()).ToArray());
            Quantity = EditorGUILayout.IntField("Quantity", Quantity);

            if (GUILayout.Button("Add Item"))
            {
                factoryInventory.TryAddInputItem(Assets[SelectedIndex], SlotIndex, Quantity);
            }

            EditorGUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.HelpBox("No Item assets found in the project.", MessageType.Warning);
        }
    }
}