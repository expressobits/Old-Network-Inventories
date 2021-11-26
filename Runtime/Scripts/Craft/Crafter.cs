using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    [RequireComponent(typeof(Container))]
    public class Crafter : NetworkBehaviour
    {
        public bool IsCrafting => craftingTimer.Value > 0f;

        [SerializeField] private List<Recipe> recipes;
        private Container container;

        private NetworkVariable<float> craftingTimer;
        private Recipe actualCraft;

        private void Awake()
        {
            container = GetComponent<Container>();
        }

        public bool HasCraft(Recipe recipe)
        {
            foreach(var items in recipe.RecipeSlots)
            {
                if(!container.Has(items.item,items.amount))
                {
                    return false;
                }
            }
            return true;
        }


        public bool UseItems(Recipe recipe)
        {
            foreach(var items in recipe.RecipeSlots)
            {
                if(container.Remove(items.item,items.amount) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void Craft(Recipe recipe)
        {
            if(HasCraft(recipe))
            {
                if(UseItems(recipe))
                {
                    actualCraft = recipe;
                    craftingTimer.Value = recipe.TimeForCraft;
                }
            }
        }

        private void Update()
        {
            if(IsOwner)
            {
                if(Input.GetKeyDown(KeyCode.L))
                {
                    CallCraft(0);
                }
            }
            if(IsCrafting && IsServer)
            {
                craftingTimer.Value -= Time.deltaTime;
                if(!IsCrafting)
                {
                    container.Add(actualCraft.Product,1);
                    actualCraft = null;
                }
            }
        }

        [ServerRpc]
        private void CraftServerRpc(int index)
        {
            if(IsCrafting) return;
            if(recipes.Count <= index) return;
            Recipe recipe = recipes[index];
            Craft(recipe);
        }

        #region Client Calls
        private void CallCraft(int index)
        {
            if(IsCrafting) return;
            if(recipes.Count <= index) return;
            CraftServerRpc(index);
        }
        #endregion
    }
}

