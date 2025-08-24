#if UNITY_EDITOR
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
public class RequireInterfaceDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (RequireInterfaceAttribute)attribute;
        EditorGUI.BeginProperty(position, label, property);

        var oldObj = property.objectReferenceValue;
        var newObj = EditorGUI.ObjectField(position, label, oldObj, typeof(ItemDetails), false);

        if (newObj != null && !attr.RequiredType.IsAssignableFrom(newObj.GetType()))
        {
            Debug.LogWarning($"Assigned object must implement {attr.RequiredType.Name}");
            newObj = oldObj;
        }

        property.objectReferenceValue = newObj;
        EditorGUI.EndProperty();
    }
}
#endif