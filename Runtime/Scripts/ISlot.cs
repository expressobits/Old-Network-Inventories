namespace ExpressoBits.Inventories
{
    public interface ISlot<T> where T : IItem
    {
        /// <summary>
        /// Slot Item Type
        /// </summary>
        /// <value></value>
        public T Item { get; }

        /// <summary>
        /// Total weight of slot items
        /// </summary>
        /// <value>Total weight</value>
        public float Weight { get; }

        /// <summary>
        /// Return item id
        /// </summary>
        /// <value></value>
        public ushort ItemID { get; }

        /// <summary>
        /// Return value of itens in slot
        /// </summary>
        /// <value></value>
        public ushort Amount  { get; }

        /// <summary>
        /// Returns the maximum stack value
        /// </summary>
        /// <value></value>
        public ushort MaxStack { get; }

        /// <summary>
        /// Return true if amount is less than stack
        /// </summary>
        /// <value></value>
        public bool IsSpace { get; }

        /// <summary>
        /// Return true if Amount is zero
        /// </summary>
        /// <value></value>
        public bool IsEmpty { get; }

        /// <summary>
        /// Remaining amount to fill the stack
        /// </summary>
        /// <value>Remaining value</value>
        public ushort Remaining { get; }

        /// <summary>
        /// Adds item to stack and returns quantity that was not successfully added.
        /// </summary>
        /// <param name="count">Amount to be added</param>
        /// <returns>Unadded quantity</returns>
        public ushort Add(ushort count);

        /// <summary>
        /// Remove item from stack and return quantity that was not successfully removed.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public ushort Remove(ushort count);
    }
}