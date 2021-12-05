using UnityEngine;
using UnityEngine.UI;

namespace ExpressoBits.Inventory.UI
{
    public class CraftingInfo : MonoBehaviour
    {
        [SerializeField] private Image bar;
        [SerializeField] private Image icon;
        private Recipe recipe;

        public void SetCrafting(Crafter crafter, Crafting crafting)
        {
            recipe = crafter.Recipes[crafting.Index];
            icon.sprite = recipe.Product.Icon;
            UpdateCrafting(crafting);
        }

        public void UpdateCrafting(Crafting crafting)
        {
            bar.fillAmount = 1f - (crafting.Time / recipe.TimeForCraft);
        }
    }
}

