using System.Collections.Generic;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    /// <summary>
    /// Craft recipe stores required item information, craft time and craft product
    /// </summary>
    [CreateAssetMenu(fileName = "Recipe", menuName = "Expresso Bits/Inventory/Recipe")]
    public class Recipe : ScriptableObject
    {
        /// <summary>
        /// Item structure list required by craft, these items have required amount information
        /// </summary>
        public List<RequiredItem> RequiredItems => requiredItems;
        /// <summary>
        /// Time for this recipe to be crafted
        /// </summary>
        public float TimeForCraft => timeForCraft;
        /// <summary>
        /// Craft result item
        /// </summary>
        public Item Product => product;

        [SerializeField] private List<RequiredItem> requiredItems;
        [SerializeField] private Item product;
        [SerializeField] private float timeForCraft = 4f;
    }

    
}

