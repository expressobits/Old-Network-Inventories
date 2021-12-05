using UnityEngine;

namespace ExpressoBits.Inventories
{
    [CreateAssetMenu(fileName = "Item", menuName = "Expresso Bits/Inventory/Item")]
    public class Item : ScriptableObject, IItem
    {
        public ushort ID => id;
        public string Name => name;
        public string Description => description;
        public Sprite Icon => icon;
        public Category Category => category;
        public ushort MaxStack => maxStack;
        public ItemObject ItemObjectPrefab => itemObjectPrefab;
        public float Weight => weight;

        [SerializeField] private ushort id = 0;
        [SerializeField, TextArea] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField, Min(0.01f)] private float weight = 0.1f;
        [SerializeField, Min(1)] private ushort maxStack = 64;
        [SerializeField] private ItemObject itemObjectPrefab;
        [SerializeField] private Category category;

        internal void Setup(ushort id)
        {
            this.id = id;
        }

        public static implicit operator ushort(Item item)
        {
            return item.ID;
        }

        public static implicit operator Item(ushort id)
        {
            Items[] items = Resources.FindObjectsOfTypeAll<Items>();
            if(items.Length > 0)
            {
                return items[0].GetItem(id);
            }
            Debug.Log("There are no scriptable items in the project!");
            return null;
        }

    }
}

