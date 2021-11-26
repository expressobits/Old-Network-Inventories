using System.Collections.Generic;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    [CreateAssetMenu(fileName = "Recipes", menuName = "Expresso Bits/Inventory/Recipes")]
    public class Recipes : ScriptableObject
    {
        [SerializeField] private List<Recipe> recipes;
    }
}

