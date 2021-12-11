using System;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventories
{
    /// <summary>
    /// An object that has a container
    /// </summary>
    [RequireComponent(typeof(Container))]
    public class StorageObject : NetworkBehaviour
    {
        public Container Container => container;

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
                    container.Add(item, 1);
                }
            }
        }

        [ClientRpc]
        private void OpenClientRpc()
        {
            OnLocalOpen?.Invoke();
        }

        // [ClientRpc]
        // private void CallActionInOwnerClientRpc(NetworkObjectReference reference, ClientRpcParams rpcParams)
        // {
        //     if(reference.TryGet(out NetworkObject networkObject))
        //     {
        //         if(networkObject.TryGetComponent(out ContainerInteractor containerInteractor))
        //         {
        //             containerInteractor.OnOpenContainer?.Invoke(this);
        //         }
        //     }
        // }

        [ClientRpc]
        private void CloseClientRpc()
        {
            OnLocalClose?.Invoke();
        }

        public void Open()
        {
            // TODO logic locked storage
            // if(locked) return locked actions
            // ClientRpcParams clientRpcParams = new ClientRpcParams
            // {
            //     Send = new ClientRpcSendParams
            //     {
            //         TargetClientIds = new ulong[] { clientId }
            //     }
            // };
            //CallActionInOwnerClientRpc(containerInteractor.NetworkObject, clientRpcParams);
            OpenClientRpc();
        }
        
        public void Close()
        {
            CloseClientRpc();
        }

        public void Action()
        {
            Open();
        }
    }
}

