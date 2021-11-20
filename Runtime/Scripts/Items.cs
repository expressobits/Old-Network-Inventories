using System.Collections.Generic;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    [CreateAssetMenu(fileName = "Items", menuName = "Expresso Bits/Inventory/Items")]
    public class Items : ScriptableObject
    {
        [SerializeField] private List<Item> items = new List<Item>();

        public byte GetNewItemId()
        {
            byte id = 1;
            while(HasItem(id))
            {
                id++;
            }
            return id;
        }

        public void Add(Item item,byte id)
        {
            item.Setup(id);
            items.Insert(id,item);
        }

        public bool HasItem(int id)
        {
            foreach(Item item in items)
            {
                if(item.ID == id) return true;
            }
            return false;
        }

        public Item GetItem(int id)
        {
            foreach(Item item in items)
            {
                if(item.ID == id) return item;
            }
            return null;
        }
    }
}

