using System;

namespace ExpressoBits.Inventory
{
    public struct Crafting : IEquatable<Crafting>
    {
        public int index;
        public float time;

        public bool Equals(Crafting other)
        {
            return index == other.index && time == other.time;
        }
    }
}