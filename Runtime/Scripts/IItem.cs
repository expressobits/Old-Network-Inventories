using UnityEngine;

namespace ExpressoBits.Inventory
{
    public interface IItem
    {
        public string Name { get; }
        public string Description { get; }
        public Sprite Icon { get; }
        public byte MaxStack { get; }
    }
}

