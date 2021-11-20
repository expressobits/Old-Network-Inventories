using System;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    public class Container : NetworkBehaviour, IContainer<Item>
    {
        public Items Items => items;
        public NetworkList<Slot> Slots => slots;
        private NetworkList<Slot> slots;
        public float Weight
        {
            get
            {
                float total = 0;
                foreach (Slot slot in slots) total += slot.Weight;
                return total;
            }
        }

        public Action<Item> OnItemDrop;
        public Action<Item, byte> OnLocalItemRemove;
        public Action<Item, byte> OnLocalItemAdd;
        public static Action<Item> OnLocalItemDrop;

        [SerializeField] protected Items items;

        /// <summary>
        /// Basic client received update event
        /// </summary>
        public Action OnChanged;

        #region Unity Events
        private void Awake()
        {
            slots = new NetworkList<Slot>();
        }

        private void OnEnable()
        {
            slots.OnListChanged += ListChanged;
        }

        private void OnDisable()
        {
            slots.OnListChanged -= ListChanged;
        }
        #endregion

        private void ListChanged(NetworkListEvent<Slot> changeEvent)
        {
            OnChanged?.Invoke();
        }

        #region IContainer Functions
        public byte Add(Item item, byte amount)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                Slot slot = slots[i];
                if (slot.itemId == item.ID)
                {
                    amount = slot.Add(amount);
                    slots[i] = slot;
                    if (amount == 0)
                    {
                        ItemAddClientRpc(item, amount);
                        return 0;
                    }
                }
            }
            slots.Add(new Slot() { itemId = item.ID, amount = amount, id = UnityEngine.Random.Range(0, Int32.MaxValue) });
            ItemAddClientRpc(item, amount);
            return 0;
        }

        public byte RemoveInIndex(int index, byte valueToRemove)
        {
            byte valueNoRemoved = valueToRemove;
            if (slots.Count > index)
            {
                Slot slot = slots[index];
                Item item = items.GetItem(slot.itemId);
                if (item != null)
                {
                    valueNoRemoved = slot.Remove(valueNoRemoved);
                    slots[index] = slot;
                    if (slot.IsEmpty)
                    {
                        slots.RemoveAt(index);
                    }
                }
                ItemRemoveClientRpc(item, (byte)(valueToRemove - valueNoRemoved));
            }
            return valueNoRemoved;
        }

        public byte Remove(Item item, byte valueToRemove)
        {
            byte valueNoRemoved = valueToRemove;
            for (int i = 0; i < slots.Count; i++)
            {
                Slot slot = slots[i];
                if (slot.itemId == item.ID)
                {
                    valueNoRemoved = slot.Remove(valueNoRemoved);
                    slots[i] = slot;
                    if (slot.IsEmpty)
                    {
                        slots.Remove(slot);
                    }
                    if (valueNoRemoved == 0) return 0;
                }
            }
            ItemRemoveClientRpc(item, (byte)(valueToRemove - valueNoRemoved));
            return valueNoRemoved;
        }

        public bool Has(Item item)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                Slot slot = slots[i];
                if (slot.itemId == item.ID && !slot.IsEmpty)
                {
                    return true;
                }
            }
            return false;
        }

        public void Clear()
        {
            slots.Clear();
        }
        #endregion

        [ServerRpc]
        private void DropItemFromIndexServerRpc(int index, byte amount, Vector3 position, Quaternion rotation)
        {
            if (slots.Count > index)
            {
                Slot slot = slots[index];
                Item item = items.GetItem(slot.itemId);
                if (item != null)
                {
                    byte valueNoRemoved = RemoveInIndex(index, amount);
                    for (int i = valueNoRemoved; i < amount; i++)
                    {
                        ItemObject itemObjectPrefab = item.ItemObjectPrefab;
                        ItemObject itemObject = Instantiate(itemObjectPrefab, GetPhysicPosition(position), rotation);
                        itemObject.NetworkObject.Spawn(true);
                        ItemDropClientRpc(slot.itemId);
                    }
                }
            }

        }

        #region Client Callbacks
        [ClientRpc]
        private void ItemAddClientRpc(byte itemId, byte amount)
        {
            OnLocalItemAdd?.Invoke(itemId, amount);
        }

        [ClientRpc]
        private void ItemRemoveClientRpc(byte itemId, byte amount)
        {
            OnLocalItemRemove?.Invoke(itemId, amount);
        }

        [ClientRpc]
        internal void ItemDropClientRpc(byte itemId)
        {
            Item item = items.GetItem(itemId);
            if (IsOwner && item != null) OnLocalItemDrop?.Invoke(item);
            OnItemDrop?.Invoke(item);
        }
        #endregion
        
        

        private static Vector3 GetPhysicPosition(Vector3 position)
        {
            Ray ray = new Ray(position, Vector3.down);
            if (Physics.Raycast(ray, out var hit, 15))
            {
                return hit.point;
            }
            return position;
        }

    }
}


