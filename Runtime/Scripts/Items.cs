using System.Collections.Generic;
using UnityEngine;

namespace ExpressoBits.Inventories
{
    /// <summary>
    /// Database of all items, when creating items here it automatically generates an item id
    /// </summary>
    [CreateAssetMenu(fileName = "Items", menuName = "Expresso Bits/Inventory/Items")]
    public class Items : ScriptableObject
    {

        [SerializeField] private List<Item> items = new List<Item>();

        /// <summary>
        /// Generate new id for an item
        /// </summary>
        /// <returns></returns>
        public ushort GetNewItemId()
        {
            ushort id = 1;
            while(HasItem(id))
            {
                id++;
            }
            return id;
        }

        /// <summary>
        /// Add a new item to the database
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        internal void Add(Item item,ushort id)
        {
            item.Setup(id);
            items.Insert(id,item);
        }

        /// <summary>
        /// Checks if there is an item with id
        /// </summary>
        /// <param name="id">Id of Item</param>
        /// <returns>True if item exists or false if item does not exist in database</returns>
        public bool HasItem(ushort id)
        {
            foreach(Item item in items)
            {
                if(item.ID == id) return true;
            }
            return false;
        }

        /// <summary>
        /// Get an item with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Item GetItem(ushort id)
        {
            foreach(Item item in items)
            {
                if(item.ID == id) return item;
            }
            return null;
        }
    }
}

