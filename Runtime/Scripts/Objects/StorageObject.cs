using UnityEngine;
using Unity.Netcode;
using ExpressoBits.Interactions;
using ExpressoBits.Inventory.UI;
using System;

namespace ExpressoBits.Inventory
{
    [RequireComponent(typeof(Container))]
    public class StorageObject : NetworkBehaviour, IInteractable, IPreviewInteract
    {
        private Container container;
        [SerializeField] private Item[] lootItems;

        public Transform Transform => transform;

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

        public void Interact(Interactor interactor)
        {
            if(!IsServer) return;
            // NOTE! In case you know a list of ClientId's ahead of time, that does not need change,
            // Then please consider caching this (as a member variable), to avoid Allocating Memory every time you run this function
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[]{interactor.NetworkObject.OwnerClientId}
                }
            };
            OpenContainerClientRPC(clientRpcParams);
        }

        [ClientRpc]
        private void OpenContainerClientRPC(ClientRpcParams clientRpcParams = default)
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

