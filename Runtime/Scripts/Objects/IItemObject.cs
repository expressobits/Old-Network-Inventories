namespace ExpressoBits.Inventories
{
    /// <summary>
    /// Component added to gameobject that represents the Item,
    /// Usually used in prefabs that will instantiate the object in the scene
    /// </summary>
    public interface IItemObject
    {
        /// <summary>
        /// Item that this Item Object represents
        /// </summary>
        /// <value></value>
        public Item Item { get; }
    }
}