using UnityEngine;
using Unity.Netcode;
using ExpressoBits.Inventory.UI;
using System;

namespace ExpressoBits.Inventory
{
    [RequireComponent(typeof(Container))]
    public class StorageObject : NetworkBehaviour
    {
        private Container container;
        [SerializeField] private Item[] lootItems;

        public Action OnLocalOpen;
        public Action OnLocalClose;

        private void Awake()
        {
            container = GetComponent<Container>();
        }
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                foreach (Item item in lootItems)
                {
                    container.Add(item,1);
                }
            }
        }

        [ClientRpc]
        public void OpenContainerClientRPC(ClientRpcParams clientRpcParams = default)
        {
            InventorySystemUI.Instance.OpenLootContainer(container);
        }

        [ClientRpc]
        private void OpenClientRpc()
        {
            OnLocalOpen?.Invoke();
            
        }

        [ClientRpc]
        private void CloseClientRpc()
        {
            OnLocalClose?.Invoke();
        }

        public void Open()
        {
            OpenClientRpc();
        }

        public void Close()
        {
            CloseClientRpc();
        }

        public string PreviewMessage()
        {
            return "open storage";
        }
    }
}

