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

        public Container Container => container;

        public static Action<Item> OnOwnerItemGet;
        public Action<Container> OnOwnerOpenContainer;

        public Action<Item> OnItemGet;
        public Action<Container> OnOpenContainer;
        public Action<Container> OnCloseContainer;

        [SerializeField] private Container container;

        private void Awake()
        {
            container = GetComponent<Container>();
        }

        #region Local Calls
        public void RequestCloseChest(StorageObject storageObject)
        {
            OpenStorageServerRpc(storageObject.NetworkObject, false);
        }

        internal void RequestTrade(int index, ushort amount, Container from, Container to)
        {
            TradeServerRpc(index, amount, from.NetworkObject, to.NetworkObject);
        }

        internal void RequestDrop(int index, ushort amount, Container from)
        {
            DropServerRpc(index, amount, from.NetworkObject);
        }
        #endregion

        #region Server Tasks
        public void Interact(NetworkObject networkObject)
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

        private void GetItem(ItemObject itemObject)
        {
            if (itemObject.IsInvalid) return;
            itemObject.SetInvalid();
            Add(itemObject.Item);
            itemObject.NetworkObject.Despawn();
        }

        public void Add(Item item)
        {
            container.Add(item, 1);
            ItemGettedClientRpc(item);
        }

        public void Drop(Container container, Item item, ushort amount)
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

        private void OpenStorage(StorageObject storageObject, bool open)
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
        private void ItemGettedClientRpc(ushort itemId)
        {
            Item item = container.Items.GetItem(itemId);
            if (IsOwner && item != null) OnOwnerItemGet?.Invoke(item);
            OnItemGet?.Invoke(item);
        }

        [ClientRpc]
        private void OpenContainerClientRpc(NetworkObjectReference reference)
        {
            if (reference.TryGet(out NetworkObject networkObject))
            {
                if (networkObject.TryGetComponent(out Container container))
                {
                    if (IsOwner) OnOwnerOpenContainer?.Invoke(container);
                    OnOpenContainer?.Invoke(container);
                }
            }
        }
        #endregion
    }
}

