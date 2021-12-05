using UnityEngine;

namespace ExpressoBits.Inventories
{
    /// <summary>
    /// Item required by a craft, also stores amount information
    /// </summary>
    [System.Serializable]
    public struct RequiredItem
    {
        public Item Item => item;
        public ushort Amount => amount;

        [SerializeField] private Item item;
        [SerializeField] private ushort amount;
    }
}

