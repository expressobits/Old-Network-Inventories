using System;

namespace ExpressoBits.Inventories
{
    /// <summary>
    /// Structure of a crafting , here is stored the index of the recipe to be created and the current time of the craft
    /// </summary>
    public struct Crafting : IEquatable<Crafting>
    {
        /// <summary>
        /// Recipe index of crafting
        /// </summary>
        public int Index => index;
        /// <summary>
        /// Time left to finish the craft 
        /// </summary>
        public float Time => time;
        /// <summary>
        /// Is Crafting finished?
        /// </summary>
        public bool IsFinished => Time <= 0f;
        
        private int index;
        private float time;

        public Crafting(int index, float time)
        {
            this.index = index;
            this.time = time;
        }

        /// <summary>
        /// Add time elapsed to actual time
        /// </summary>
        /// <param name="timeElapsed">Time elapsed, normally Time.deltaTime</param>
        internal void AddTimeElapsed(float timeElapsed)
        {
            time -= timeElapsed;
        }

        public bool Equals(Crafting other)
        {
            return index == other.index && time == other.time;
        }
    }
}