using System.Collections.Generic;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Expresso Bits/Inventory/Recipe")]
    public class Recipe : ScriptableObject
    {
        public List<RequiredItem> RequiredItems => requiredItems;
        public float TimeForCraft => timeForCraft;
        public Item Product => product;

        [SerializeField] private List<RequiredItem> requiredItems;
        [SerializeField] private Item product;
        [SerializeField] private float timeForCraft = 4f;
    }

    
}

