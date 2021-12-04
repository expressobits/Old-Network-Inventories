using System;
using System.ComponentModel;
using ExpressoBits.Interactions;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventory
{

    public class ContainerInteractor : NetworkBehaviour
    {
        [SerializeField] private Interactor interactor;
        [SerializeField] private Container container;

        public Action<Item> OnItemGet;

        public static Action<Item> OnLocalItemGet;

        public Container Container => container;

        private void Awake()
        {
            container = GetComponent<Container>();
            if (IsServer)
            {
                interactor.OnInteract += Interact;
            }
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
        private void Interact(NetworkObject networkObject)
        {
            if (networkObject.TryGetComponent(out ItemObject itemObject))
            {
                GetItem(itemObject);
            }
            else if (networkObject.TryGetComponent(out StorageObject storageObject))
            {
                storageObject.Open();
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

            Drop(container,item, (ushort)(amount-valueNoRemoved));
            
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
            if (open)
            {
                storageObject.Open();
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
            if (IsOwner && item != null) OnLocalItemGet?.Invoke(item);
            OnItemGet?.Invoke(item);
        }
        #endregion
    }
}

