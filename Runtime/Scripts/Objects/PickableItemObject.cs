using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventories
{
    /// <summary>
    /// An object that represents the item in the world
    /// </summary>
    [RequireComponent(typeof(ItemObject))]
    public class PickableItemObject : NetworkBehaviour
    {
        public bool IsInvalid => invalidItem;
        public ItemObject ItemObject => itemObject;

        private ItemObject itemObject;
        private bool invalidItem;

        private void Awake()
        {
            itemObject = GetComponent<ItemObject>();
        }

        private void SetInvalid()
        {
            invalidItem = true;
        }

        public void Pick()
        {
            SetInvalid();
            NetworkObject.Despawn();
        }
    }
}
