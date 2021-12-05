using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    [RequireComponent(typeof(Container))]
    public class Crafter : NetworkBehaviour
    {
        /// <summary>
        /// Is something currently being crafted?
        /// </summary>
        public bool IsCrafting => craftings.Count > 0f;
        /// <summary>
        /// List of possible recipes to craft
        /// </summary>
        public List<Recipe> Recipes => recipes.AllRecipes;
        /// <summary>
        /// Crafter related container, Crafts will use this container to assess whether it is possible to craft
        /// </summary>
        public Container Container => container;
        /// <summary>
        /// List of current crafts being made
        /// </summary>
        public NetworkList<Crafting> Craftings => craftings;

        /// <summary>
        /// Event triggered when list of craftings being made is modified
        /// </summary>
        public Action OnCraftingsChanged;
        /// <summary>
        /// Event triggered when a new craft is being created
        /// </summary>
        public Action<Crafting> OnLocalAddCrafting;
        /// <summary>
        /// Event triggered when one's craftings is removed from the crafting list
        /// </summary>
        public Action<int> OnLocalRemoveCrafting;

        /// <summary>
        /// Is the crafting list limited?
        /// </summary>
        [SerializeField] private bool isLimitCrafts = true;
        /// <summary>
        /// Maximum number of craftings if limit is used
        /// </summary>
        [SerializeField] private uint craftsLimit = 8;
        [SerializeField] private Recipes recipes;
        private Container container;
        private NetworkList<Crafting> craftings;

        private void Awake()
        {
            container = GetComponent<Container>();
            craftings = new NetworkList<Crafting>();
        }

        private void OnEnable()
        {
            if(IsOwner)
            {
                craftings.OnListChanged += CraftingsChanged;
            }
        }

        private void OnDisable()
        {
            if(IsOwner)
            {
                craftings.OnListChanged -= CraftingsChanged;
            }
        }

        private void CraftingsChanged(NetworkListEvent<Crafting> changeEvent)
        {
            switch(changeEvent.Type)
            {
                case NetworkListEvent<Crafting>.EventType.Add:
                    OnLocalAddCrafting?.Invoke(changeEvent.Value);
                    break;
                case NetworkListEvent<Crafting>.EventType.RemoveAt:
                    OnLocalRemoveCrafting?.Invoke(changeEvent.Index);
                    break;
            }
            OnCraftingsChanged?.Invoke();
        }

        public bool CanCraft(Recipe recipe)
        {
            if(isLimitCrafts && craftings.Count >= craftsLimit) return false;
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
                    craftings.Add(new Crafting(){ index = recipes.AllRecipes.IndexOf(recipe), time = recipe.TimeForCraft});
                }
            }
        }

        private void Update()
        {
            if(IsCrafting && IsServer)
            {
                for (int i = 0; i < craftings.Count; i++)
                {
                    Crafting crafting = craftings[i];
                    crafting.time -= Time.deltaTime;
                    if(crafting.time < 0)
                    {
                        container.Add(recipes.AllRecipes[craftings[i].index].Product,1);
                        craftings.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        craftings[i] = crafting;
                    }
                }
                
            }
        }

        [ServerRpc]
        private void CraftServerRpc(int index)
        {
            if(recipes.AllRecipes.Count <= index) return;
            Recipe recipe = recipes.AllRecipes[index];
            Craft(recipe);
        }

        #region Client Calls
        public void CallCraft(int index)
        {
            if(recipes.AllRecipes.Count <= index) return;
            CraftServerRpc(index);
        }

        public void CallCraft(Recipe recipe)
        {
            int index = recipes.AllRecipes.IndexOf(recipe);
            if(index >= 0) CallCraft(index);
        }
        #endregion
    }
}

