using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventories.UI
{
    public class CraftsPanel : MonoBehaviour
    {

        private Crafter crafter;
        [SerializeField] private CraftingInfo craftingInfoPrefab;
        private List<CraftingInfo> craftingInfos = new List<CraftingInfo>();

        public void SetCrafter(Crafter crafter)
        {
            Clear();
            this.crafter = crafter;
            crafter.OnLocalAddCrafting += AddCrafting;
            crafter.OnLocalRemoveCrafting += RemoveCrafting;
        }

        private void RemoveCrafting(int index)
        {
            Destroy(craftingInfos[index].gameObject);
            craftingInfos.RemoveAt(index);
        }

        private void Update()
        {
            if(!crafter && crafter == null) return;
            NetworkList<Crafting> craftings = crafter.Craftings;
            if(craftings == null) return;
            int secureIndex = Mathf.Min(craftingInfos.Count, craftings.Count);
            for (int i = 0; i < secureIndex; i++)
            {
                craftingInfos[i].UpdateCrafting(craftings[i]);
            }
        }

        private void AddCrafting(Crafting crafting)
        {
            CraftingInfo craftingInfo = Instantiate(craftingInfoPrefab,transform);
            craftingInfo.SetCrafting(crafter,crafting);
            craftingInfos.Add(craftingInfo);
        }

        public void Clear()
        {
            foreach(CraftingInfo craftingInfo in craftingInfos)
            {
                Destroy(craftingInfo.gameObject);
            }
            craftingInfos.Clear();
        }
    }
}

