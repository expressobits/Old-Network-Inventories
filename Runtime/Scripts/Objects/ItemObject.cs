using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventories
{
    /// <summary>
    /// An object that represents the item in the world
    /// </summary>
    public class ItemObject : NetworkBehaviour, IItemObject
    {
        /// <summary>
        /// The item that this object represents
        /// </summary>
        public Item Item => item;

        [SerializeField] private Item item;

    }   
}
