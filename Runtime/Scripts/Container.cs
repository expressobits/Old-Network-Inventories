using System;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventories
{
    public class Container : NetworkBehaviour, IContainer<Item>
    {
        public Items Items => items;
        public NetworkList<Slot> Slots => slots;
        public float Weight
        {
            get
            {
                float total = 0;
                foreach (Slot slot in slots) total += slot.Weight;
                return total;
            }
        }

        public Action<Item, ushort> OnItemRemove;
        public Action<Item, ushort> OnItemAdd;

        [SerializeField] protected Items items;
        private NetworkList<Slot> slots;
        [SerializeField] private bool haveSlotAmountLimit;
        [SerializeField] private int slotAmountLimit = 8;

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
        public ushort Add(Item item, ushort amount)
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
            if(!haveSlotAmountLimit || Slots.Count < slotAmountLimit)
            {
                slots.Add(new Slot() { itemId = item.ID, amount = amount, id = UnityEngine.Random.Range(0, int.MaxValue) });
                ItemAddClientRpc(item, amount);
                amount = 0;
            }
            return amount;
        }

        public ushort RemoveInIndex(int index, ushort valueToRemove)
        {
            ushort valueNoRemoved = valueToRemove;
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
                ItemRemoveClientRpc(item, (ushort)(valueToRemove - valueNoRemoved));
            }
            return valueNoRemoved;
        }

        public ushort Remove(Item item, ushort valueToRemove)
        {
            ushort valueNoRemoved = valueToRemove;
            for (int i = 0; i < slots.Count; i++)
            {
                Slot slot = slots[i];
                if (slot.itemId == item.ID)
                {
                    valueNoRemoved = slot.Remove(valueNoRemoved);
                    slots[i] = slot;
                    if (slot.IsEmpty)
                    {
                        slots.RemoveAt(i);
                    }
                    if (valueNoRemoved == 0) return 0;
                }
            }
            ItemRemoveClientRpc(item, (ushort)(valueToRemove - valueNoRemoved));
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

        public bool Has(Item item, ushort amount)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                Slot slot = slots[i];
                if (slot.itemId == item.ID)
                {
                    amount = (ushort)Mathf.Max(amount - slot.amount,0);
                }
                if (amount <= 0) return true;
            }
            return false;
        }

        public void Clear()
        {
            slots.Clear();
        }
        #endregion

        #region Client Callbacks
        [ClientRpc]
        private void ItemAddClientRpc(ushort itemId, ushort amount)
        {
            OnItemAdd?.Invoke(itemId, amount);
        }

        [ClientRpc]
        private void ItemRemoveClientRpc(ushort itemId, ushort amount)
        {
            OnItemRemove?.Invoke(itemId, amount);
        }
        #endregion

    }
}


