using Unity.Netcode;

namespace ExpressoBits.Inventory
{
    /// <summary>
    /// Model of any structure that contains items (IItem)
    /// </summary>
    /// <typeparam name="T">whatever type IItem</typeparam>
    public interface IContainer<T> where T : IItem
    {
        /// <summary>
        /// All container slots
        /// </summary>
        /// <value>NetworkList with Slots</value>
        public NetworkList<Slot> Slots { get; }

        /// <summary>
        /// Sum of container weight
        /// </summary>
        /// <value>Float value with sum of weight</value>
        public float Weight { get; }

        /// <summary>
        /// Adds an T item with a quantity and returns values that were not added.
        /// </summary>
        /// <param name="item">T Item to be added</param>
        /// <param name="amount">Amount to be added</param>
        /// <returns>Amount not added in operation</returns>
        public ushort Add(T item, ushort amount);

        /// <summary>
        /// Remove item from index
        /// </summary>
        /// <param name="index">Slot index to be removed</param>
        /// <param name="amount">Amount to be removed</param>
        /// <returns>Amount not removed in operation</returns>
        public ushort RemoveInIndex(int index, ushort amount);

        /// <summary>
        /// Remove T item with a quantity and returns values that were not remove.
        /// </summary>
        /// <param name="item">T Item to be removed</param>
        /// <param name="amount">Amount to be removed</param>
        /// <returns>Amount not removed in operation</returns>
        public ushort Remove(T item, ushort amount);

        /// <summary>
        /// Clear container
        /// </summary>
        public void Clear();

        /// <summary>
        /// Check if container has item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Return true if container contain T item</returns>
        public bool Has(T item);

        /// <summary>
        /// Check if container has item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        /// <returns>Return true if container contain T item with amount</returns>
        public bool Has(T item, ushort amount);
    }
}