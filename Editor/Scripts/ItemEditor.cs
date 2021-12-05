using System;
using UnityEditor;
using UnityEngine;

namespace ExpressoBits.Inventories.Editor
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            Item item = (Item)target;

            Show(item,serializedObject);
        }

        public static void Show(Item item,SerializedObject serializedObject)
        {
            item.name = EditorGUILayout.TextField("Name", item.name);

            SerializedProperty weightProperty = serializedObject.FindProperty("weight");
            SerializedProperty maxStackProperty = serializedObject.FindProperty("maxStack");
            SerializedProperty iconProperty = serializedObject.FindProperty("icon");
            SerializedProperty descriptionProperty = serializedObject.FindProperty("description");
            SerializedProperty itemObjectPrefabProperty = serializedObject.FindProperty("itemObjectPrefab");
            SerializedProperty categoryProperty = serializedObject.FindProperty("category");

            EditorGUILayout.LabelField("ID", item.ID.ToString());
            EditorGUILayout.PropertyField(weightProperty);
            EditorGUILayout.PropertyField(maxStackProperty);
            EditorGUILayout.PropertyField(iconProperty);
            EditorGUILayout.PropertyField(descriptionProperty);
            EditorGUILayout.PropertyField(itemObjectPrefabProperty);
            EditorGUILayout.PropertyField(categoryProperty);

            serializedObject.ApplyModifiedProperties();

        }

    }
}

