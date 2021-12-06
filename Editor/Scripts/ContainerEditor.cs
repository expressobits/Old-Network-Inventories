using UnityEditor;
using UnityEngine;

namespace ExpressoBits.Inventories.Editor
{
    [CustomEditor(typeof(Container))]
    public class ContainerEditor : UnityEditor.Editor
    {

        SerializedProperty itemsSerializedProperty;
        SerializedProperty slotsReadPermissionSerializedProperty;
        SerializedProperty haveSlotAmountLimitSerializedProperty;
        SerializedProperty slotAmountLimitSerializedProperty;

        public override void OnInspectorGUI()
        {
            Container container = (Container)target;
            itemsSerializedProperty = serializedObject.FindProperty("items");
            slotsReadPermissionSerializedProperty = serializedObject.FindProperty("slotsReadPermission");
            haveSlotAmountLimitSerializedProperty = serializedObject.FindProperty("haveSlotAmountLimit");
            slotAmountLimitSerializedProperty = serializedObject.FindProperty("slotAmountLimit");

            EditorGUILayout.PropertyField(itemsSerializedProperty);
            EditorGUILayout.PropertyField(slotsReadPermissionSerializedProperty);
            EditorGUILayout.PropertyField(haveSlotAmountLimitSerializedProperty);
            if(haveSlotAmountLimitSerializedProperty.boolValue)
            {
                EditorGUILayout.PropertyField(slotAmountLimitSerializedProperty);
            }
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Actual Slots");
            EditorGUI.indentLevel++;
            if (container.Slots != null && container.Slots.Count > 0)
            {
                foreach (var slot in container.Slots)
                {
                    EditorGUILayout.LabelField("Slot = " + slot.Item.name + " X " + slot.Amount);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No slots...");
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
        }
    }
}

