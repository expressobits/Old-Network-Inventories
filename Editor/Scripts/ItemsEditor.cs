using UnityEditor;
using UnityEngine;

namespace ExpressoBits.Inventory.Editor
{
    [CustomEditor(typeof(Items))]
    public class ItemsEditor : UnityEditor.Editor
    {

        //private SerializedProperty itemsProperty;

        // private void Awake()
        // {
        //     itemsProperty = serializedObject.FindProperty("items");
        // }

        // public override void OnInspectorGUI()
        // {
        //     itemsProperty = serializedObject.FindProperty("items");
        //     NewItemButton();
        //     Show(itemsProperty);


        //     serializedObject.ApplyModifiedProperties();
        // }

        // private string newName = "Item";
        // private GameObject newPrefab;
        // private byte itemId = 0;

        // private void NewItemButton()
        // {
        //     var origFontStyle = EditorStyles.label.fontStyle;
        //     var items = (Items)target;
        //     //EditorGUILayout.BeginVertical("box");
        //     EditorStyles.label.fontStyle = FontStyle.Bold;
        //     EditorGUILayout.LabelField("New Item");
        //     EditorStyles.label.fontStyle = origFontStyle;
        //     byte id = (byte)EditorGUILayout.IntField("Item ID", itemId);
        //     id = items.GetNewItemId();
        //     itemId = id;
        //     newName = EditorGUILayout.TextField("Name", newName);
        //     //newPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", newPrefab, typeof(GameObject), false);
        //     //newIncreaseValue = EditorGUILayout.("Increase Value", newIncreaseValue);
        //     EditorGUI.BeginDisabledGroup(newName.Length == 0);
        //     if (GUILayout.Button("Add New Item"))
        //     {
        //         MakeNewItem(items, itemId, newName);
        //     }
        //     EditorGUI.EndDisabledGroup();
        //     //EditorGUILayout.EndVertical();
        // }

        // public void Show(SerializedProperty list)
        // {
        //     for (int i = 0; i < list.arraySize; i++)
        //     {

        //         SerializedProperty property = list.GetArrayElementAtIndex(i);

        //         EditorGUILayout.BeginVertical();
        //         DrawItem(i, ref property);
        //         EditorGUILayout.EndVertical();
        //     }
        //     EditorUtils.DrawSplitter(false);
        // }

        // private void DrawItem(int index, ref SerializedProperty itemProperty)
        // {
        //     SerializedObject nestedObject = new SerializedObject(itemProperty.objectReferenceValue);

        //     Item item = (Item)itemProperty.objectReferenceValue;

        //     EditorUtils.DrawSplitter(false);
        //     bool displayContent = EditorUtils.DrawHeaderToggle(item.name, itemProperty, pos => OnContextClick(pos, index));

        //     if (displayContent)
        //     {
        //         ItemEditor.Show(item, nestedObject);
        //         EditorGUILayout.Space(16);
        //     }
        // }

        // private void OnContextClick(Vector2 position, int id)
        // {
        //     var menu = new GenericMenu();

        //     menu.AddSeparator(string.Empty);
        //     menu.AddItem(EditorGUIUtility.TrTextContent("Remove"), false, () => RemoveComponent(id));

        //     menu.DropDown(new Rect(position, Vector2.zero));
        // }

        // private void RemoveComponent(int id)
        // {
        //     SerializedProperty property = itemsProperty.GetArrayElementAtIndex(id);
        //     Object component = property.objectReferenceValue;
        //     property.objectReferenceValue = null;

        //     Undo.SetCurrentGroupName(component == null ? "Remove Pool" : $"Remove {component.name}");

        //     // remove the array index itself from the list
        //     itemsProperty.DeleteArrayElementAtIndex(id);
        //     //UpdateEditorList();
        //     serializedObject.ApplyModifiedProperties();

        //     // Destroy the setting object after ApplyModifiedProperties(). If we do it before, redo
        //     // actions will be in the wrong order and the reference to the setting object in the
        //     // list will be lost.
        //     if (component != null)
        //     {
        //         Undo.DestroyObjectImmediate(component);
        //     }

        //     // Force save / refresh
        //     ForceSave();
        // }

        // private void ForceSave()
        // {
        //     EditorUtility.SetDirty(target);
        //     AssetDatabase.SaveAssets();
        // }

        // public static void MakeNewItem(Items items, byte id, string name)
        // {
        //     Item item = ScriptableObject.CreateInstance<Item>();
        //     item.name = name;
        //     items.Add(item,id);

        //     AssetDatabase.AddObjectToAsset(item, items);
        //     AssetDatabase.SaveAssets();

        //     EditorUtility.SetDirty(items);
        //     EditorUtility.SetDirty(item);
        // }


    }
}

