using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    [RequireComponent(typeof(Container))]
    public class Crafter : NetworkBehaviour
    {
        public bool IsCrafting => craftings.Count > 0f;
        public List<Recipe> Recipes => recipes.AllRecipes;
        public Container Container => container;
        public NetworkList<Crafting> Craftings => craftings;

        public Action OnChanged;
        public Action<Crafting> OnLocalAddCrafting;
        public Action<int> OnLocalRemoveCrafting;

        [SerializeField] private bool limitCrafts = true;
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
            OnChanged?.Invoke();
        }

        public bool CanCraft(Recipe recipe)
        {
            if(limitCrafts && craftings.Count >= craftsLimit) return false;
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

