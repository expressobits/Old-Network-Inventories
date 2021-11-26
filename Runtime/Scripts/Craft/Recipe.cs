using System.Collections.Generic;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Expresso Bits/Inventory/Recipe")]
    public class Recipe : ScriptableObject
    {
        [System.Serializable]
        public struct RecipeSlot
        {
            public Item item;
            public byte amount;
        }

        public List<RecipeSlot> RecipeSlots => recipeSlots;
        public float TimeForCraft => timeForCraft;
        public Item Product => product;

        [SerializeField] private List<RecipeSlot> recipeSlots;
        [SerializeField] private Item product;
        [SerializeField] private float timeForCraft = 4f;
    }

    
}

