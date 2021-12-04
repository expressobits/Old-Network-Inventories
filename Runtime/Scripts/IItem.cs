using UnityEngine;

namespace ExpressoBits.Inventory
{
    /// <summary>
    /// Template of any item
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Unique identification of the item within an <ref>Items</ref> database
        /// </summary>
        /// <value></value>
        public ushort ID { get; }

        /// <summary>
        /// Name of Item
        /// </summary>
        /// <value></value>
        public string Name { get; }

        /// <summary>
        /// Description of Item
        /// </summary>
        /// <value></value>
        public string Description { get; }

        /// <summary>
        /// Icon of Item, Commonly used in UI
        /// </summary>
        /// <value>Sprite Icon</value>
        public Sprite Icon { get; }

        /// <summary>
        /// Maximum amount of items of this type a slot can have
        /// </summary>
        /// <value></value>
        public ushort MaxStack { get; }

        /// <summary>
        /// Item category, commonly used to highlight item in user interfaces or differentiate craft list
        /// </summary>
        /// <value></value>
        public Category Category { get; }

        /// <summary>
        /// Prefab that represents the item in the scene, used when dropping an item on the ground for example
        /// </summary>
        public ItemObject ItemObjectPrefab { get; }
    }
}

