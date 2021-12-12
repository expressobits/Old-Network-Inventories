using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventories
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
        private InventoryActionInteractor containerInteractor;
        private NetworkList<Crafting> craftings;
        [SerializeField] private NetworkVariableReadPermission craftingsReadPermission = NetworkVariableReadPermission.OwnerOnly;

        #region Unity Events
        private void Awake()
        {
            container = GetComponent<Container>();
            containerInteractor = GetComponent<InventoryActionInteractor>();
            craftings = new NetworkList<Crafting>(craftingsReadPermission, new Crafting[0]{});
        }

        private void OnEnable()
        {
            craftings.OnListChanged += CraftingsChanged;
        }

        private void OnDisable()
        {
            craftings.OnListChanged -= CraftingsChanged;
        }

        private void Update()
        {
            if(IsCrafting && IsServer)
            {
                for (int i = 0; i < craftings.Count; i++)
                {
                    Crafting crafting = craftings[i];
                    crafting.AddTimeElapsed(Time.deltaTime);
                    if(crafting.IsFinished)
                    {
                        containerInteractor.AddOrDropItem(recipes.AllRecipes[crafting.Index].Product);
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
        #endregion

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

        /// <summary>
        /// Check if it is possible to create this recipe
        /// It is checked if the crafts limit has been exceeded and then it is checked if the recipe items contain in the container
        /// </summary>
        /// <param name="recipe">Recipe to be checked</param>
        /// <returns>True if the recipe to be crafted is available</returns>
        public bool CanCraft(Recipe recipe)
        {
            if(isLimitCrafts && craftings.Count >= craftsLimit) return false;
            foreach(var items in recipe.RequiredItems)
            {
                if(!container.Has(items.Item,items.Amount))
                {
                    return false;
                }
            }
            return true;
        }

        private bool UseItems(Recipe recipe)
        {
            foreach(var items in recipe.RequiredItems)
            {
                if(container.Remove(items.Item,items.Amount) > 0)
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
                    craftings.Add(new Crafting(recipes.AllRecipes.IndexOf(recipe),recipe.TimeForCraft));
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
        /// <summary>
        /// Check if there are recipes available, if you have one, call an rpc server to request a new craft
        /// </summary>
        /// <param name="index">Craft index</param>
        public void CallCraft(int index)
        {
            if(recipes.AllRecipes.Count <= index) return;
            CraftServerRpc(index);
        }

        /// <summary>
        /// Check if there are recipes available, if you have one, call an rpc server to request a new craft
        /// </summary>
        /// <param name="index">Craft Recipe</param>
        public void CallCraft(Recipe recipe)
        {
            int index = recipes.AllRecipes.IndexOf(recipe);
            if(index >= 0) CallCraft(index);
        }
        #endregion
    }
}

