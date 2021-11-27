using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    [RequireComponent(typeof(Container))]
    public class Crafter : NetworkBehaviour
    {
        public bool IsCrafting => craftingTimer.Value > 0f;
        public List<Recipe> Recipes => recipes;
        public Container Container => container;

        [SerializeField] private List<Recipe> recipes;
        private Container container;

        private NetworkVariable<float> craftingTimer;
        private Recipe actualCraft;

        private void Awake()
        {
            container = GetComponent<Container>();
        }

        public bool CanCraft(Recipe recipe)
        {
            foreach(var items in recipe.RequiredItems)
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
            foreach(var items in recipe.RequiredItems)
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
            if(CanCraft(recipe))
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
        public void CallCraft(int index)
        {
            if(IsCrafting) return;
            if(recipes.Count <= index) return;
            CraftServerRpc(index);
        }

        public void CallCraft(Recipe recipe)
        {
            int index = recipes.IndexOf(recipe);
            if(index >= 0) CallCraft(index);
        }
        #endregion
    }
}

