using UnityEditor;
using UnityEngine;

namespace ExpressoBits.Inventories.Editor
{
    [CustomEditor(typeof(Crafter))]
    public class CrafterEditor : UnityEditor.Editor
    {

        SerializedProperty recipesSerializedProperty;
        SerializedProperty isLimitCraftsSerializedProperty;
        SerializedProperty craftsLimitSerializedProperty;

        public override void OnInspectorGUI()
        {
            Crafter crafter = (Crafter)target;
            recipesSerializedProperty = serializedObject.FindProperty("recipes");
            isLimitCraftsSerializedProperty = serializedObject.FindProperty("isLimitCrafts");
            craftsLimitSerializedProperty = serializedObject.FindProperty("craftsLimit");

            EditorGUILayout.PropertyField(recipesSerializedProperty);
            EditorGUILayout.PropertyField(isLimitCraftsSerializedProperty);
            if (isLimitCraftsSerializedProperty.boolValue)
            {
                EditorGUILayout.PropertyField(craftsLimitSerializedProperty);
            }
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Actual Craftings");
            EditorGUI.indentLevel++;
            if (crafter.Craftings != null && crafter.Craftings.Count > 0)
            {
                foreach (var crafting in crafter.Craftings)
                {
                    EditorGUILayout.LabelField("Recipe Index = " + crafting.Index + " Time = " + crafting.Time);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No craftings...");
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
}

