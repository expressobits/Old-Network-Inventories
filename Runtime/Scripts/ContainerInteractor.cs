using System;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    /// <summary>
    /// Responsible for interacting with containers whether the player's own or just loots
    /// </summary>
    [RequireComponent(typeof(Container))]
    public class ContainerInteractor : NetworkBehaviour
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
            OpenStorageServerRpc(storageObject.NetworkObject, false);
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
        /// <param name="networkObject">ItemObject or StorageObject</param>
        public virtual void Interact(NetworkObject networkObject)
        {
            if (networkObject.TryGetComponent(out ItemObject itemObject))
            {
                GetItem(itemObject);
            }
            else if (networkObject.TryGetComponent(out StorageObject storageObject))
            {
                OpenStorage(storageObject, true);
            }
        }

        public void GetItem(ItemObject itemObject)
        {
            if (itemObject.IsInvalid) return;
            itemObject.SetInvalid();
            Add(itemObject.Item);
            itemObject.NetworkObject.Despawn();
        }

        public void Add(Item item)
        {
            container.Add(item, 1);
            ItemTakenClientRpc(item);
        }

        private void Drop(Container container, Item item, ushort amount)
        {
            for (int i = 0; i < amount; i++)
            {
                ItemObject itemObjectPrefab = item.ItemObjectPrefab;
                ItemObject itemObject = Instantiate(itemObjectPrefab, transform.position, transform.rotation);
                itemObject.NetworkObject.Spawn(true);
                container.ItemDropClientRpc(item);
            }
        }

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

            Drop(container, item, (ushort)(amount - valueNoRemoved));

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
            toContainer.Add(item, (ushort)(amount - valueNoRemoved));
        }

        [ServerRpc]
        public void OpenStorageServerRpc(NetworkObjectReference target, bool open)
        {
            if (!target.TryGet(out NetworkObject targetObject)) return;
            if (!targetObject.TryGetComponent(out StorageObject storageObject)) return;
            OpenStorage(storageObject, open);
        }

        public void OpenStorage(StorageObject storageObject, bool open)
        {
            if (open)
            {
                storageObject.Open();
                OpenContainerClientRpc(storageObject.NetworkObject);
            }
            else
            {
                storageObject.Close();
            }
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

