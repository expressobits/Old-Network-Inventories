using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    public class ItemObject : NetworkBehaviour, IItemObject
    {
        public Item Item => item;

        [SerializeField] private Item item;

        private bool invalidItem;

        public bool IsInvalid => invalidItem;

        public void SetInvalid()
        {
            invalidItem = true;
        }
    }   
}
