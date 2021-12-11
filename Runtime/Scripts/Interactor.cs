using System;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventories
{
    /// <summary>
    /// Responsible for interacting with containers whether the player's own or just loots
    /// Client can ask to drop an item, exchange an item from a storage or interact with a storage
    /// </summary>
    [RequireComponent(typeof(Container))]
    public class Interactor : NetworkBehaviour
    {
        /// <summary>
        /// Container relacionado ao interator, normalmente o inventário do jogador
        /// </summary>
        public Container Container => container;

        /// <summary>
        /// Events fired only on the client that has the interactor's owner
        /// </summary>
        public Action<Item> OnOwnerItemTaken;
        public Action<Container> OnOwnerOpenContainer;

        /// <summary>
        /// Events fired on all clients
        /// </summary>
        public Action<Item> OnItemTaken;
        public Action<Container> OnOpenContainer;
        public Action<Container> OnCloseContainer;

        private Container container;

        private void Awake()
        {
            container = GetComponent<Container>();
        }

        #region Local Calls
        /// <summary>
        /// Makes a request to the server to close a stock, the parameter must be a StorageObject with NetworkObject
        /// </summary>
        /// <param name="storageObject">StorageObject with NetworkObject</param>
        public void RequestCloseStorage(StorageObject storageObject)
        {
            CloseStorageServerRpc(storageObject.NetworkObject);
        }

        /// <summary>
        /// Makes a request for a switch between containers
        /// </summary>
        /// <param name="index">Slot index to be trade</param>
        /// <param name="amount">Amount to be trade</param>
        /// <param name="from">Container where the slot comes from</param>
        /// <param name="to">Container where the slot goes</param>
        public void RequestTrade(int index, ushort amount, Container from, Container to)
        {
            TradeServerRpc(index, amount, from.NetworkObject, to.NetworkObject);
        }

        /// <summary>
        /// Make a request to drop an item from a container
        /// </summary>
        /// <param name="index">Slot index to be drop</param>
        /// <param name="amount">Amount to be drop</param>
        /// <param name="from">Container where the slot we want to drop is</param>
        public void RequestDrop(int index, ushort amount, Container from)
        {
            DropServerRpc(index, amount, from.NetworkObject);
        }
        #endregion

        #region Server Tasks
        /// <summary>
        /// Calls an interaction with network object, if this object is an item it will be added to the container,
        /// if it is a storageObject it will open the storage
        /// </summary>
        /// REVIEW Change responsibility for the interactables
        /// TODO Change to dynamic options on interact
        /// Hold interaction button open an options wheel
        /// <param name="networkObject">ItemObject or StorageObject</param>
        public virtual void Interact(NetworkObject networkObject)
        {
            if (networkObject.TryGetComponent(out StorageObject storageObject))
            {
                OpenStorage(storageObject);
            }
            else if (networkObject.TryGetComponent(out PickableItemObject pickableItemObject))
            {
                PickItem(pickableItemObject);
            }
        }

        public void PickItem(PickableItemObject pickableItemObject)
        {
            if (pickableItemObject.IsInvalid) return;
            if (container.Add(pickableItemObject.ItemObject.Item, 1) == 0)
            {
                ItemTakenClientRpc(pickableItemObject.ItemObject.Item);
                pickableItemObject.Pick();
            }
        }

        public void OpenStorage(StorageObject storageObject)
        {
            storageObject.Open();
            OpenContainerClientRpc(storageObject.NetworkObject);
        }

        public void CloseStorage(StorageObject storageObject)
        {
            storageObject.Close();
        }

        /// <summary>
        /// If possible add the item in the player's container, if not possible drop the item
        /// </summary>
        /// <param name="item">Item to be added to player inventory</param>
        /// <returns></returns>
        public bool AddOrDropItem(Item item)
        {
            if (container.Add(item, 1) == 0)
            {
                ItemTakenClientRpc(item);
                return true;
            }
            Drop(item, 1);
            return false;
        }

        private void Drop(Item item, ushort amount)
        {
            for (int i = 0; i < amount; i++)
            {
                ItemObject itemObjectPrefab = item.ItemObjectPrefab;
                ItemObject itemObject = Instantiate(itemObjectPrefab, transform.position, transform.rotation);
                itemObject.NetworkObject.Spawn(true);
            }
        }
        #endregion


        #region Server Rpcs
        [ServerRpc]
        private void DropServerRpc(int index, ushort amount, NetworkObjectReference from)
        {
            if (!from.TryGet(out NetworkObject containerNetworkObject)) return;
            Container container = containerNetworkObject.GetComponentInChildren<Container>();
            if (!container) return;

            Slot slot = container.Slots[index];
            ushort itemId = slot.ItemID;
            Item item = container.Items.GetItem(itemId);
            ushort valueNoRemoved = container.RemoveInIndex(index, amount);

            Drop(item, (ushort)(amount - valueNoRemoved));
        }

        [ServerRpc]
        private void TradeServerRpc(int index, ushort amount, NetworkObjectReference from, NetworkObjectReference to)
        {
            if (!from.TryGet(out NetworkObject fromNetworkObject)) return;
            Container fromContainer = fromNetworkObject.GetComponentInChildren<Container>();
            if (!fromContainer) return;

            if (!to.TryGet(out NetworkObject toNetworkObject)) return;
            Container toContainer = toNetworkObject.GetComponentInChildren<Container>();
            if (!toContainer) return;

            Slot slot = fromContainer.Slots[index];
            Item item = fromContainer.Items.GetItem(slot.itemId);

            ushort valueNoRemoved = fromContainer.RemoveInIndex(index, amount);
            ushort valueNoAdded = toContainer.Add(item, (ushort)(amount - valueNoRemoved));
            if (valueNoAdded > 0) fromContainer.Add(item, valueNoAdded);
        }

        [ServerRpc]
        public void CloseStorageServerRpc(NetworkObjectReference target)
        {
            if (!target.TryGet(out NetworkObject targetObject)) return;
            if (!targetObject.TryGetComponent(out StorageObject storageObject)) return;
            CloseStorage(storageObject);
        }
        #endregion

        #region Client Responses
        [ClientRpc]
        private void ItemTakenClientRpc(ushort itemId)
        {
            Item item = container.Items.GetItem(itemId);
            if (IsOwner && item != null) OnOwnerItemTaken?.Invoke(item);
            OnItemTaken?.Invoke(item);
        }

        [ClientRpc]
        private void OpenContainerClientRpc(NetworkObjectReference reference)
        {
            if (!reference.TryGet(out NetworkObject targetObject)) return;
            if (!targetObject.TryGetComponent(out Container container)) return;
            if (IsOwner) OnOwnerOpenContainer?.Invoke(container);
            OnOpenContainer?.Invoke(container);
        }
        #endregion
    }
}

