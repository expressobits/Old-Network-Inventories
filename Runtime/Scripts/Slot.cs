using System;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    public struct Slot : IEquatable<Slot>, ISlot<Item>
    {
        public Item Item => ItemID;
        public ushort ItemID => itemId;
        public ushort Amount => amount;
        public ushort MaxStack => Item.MaxStack;
        public ushort Remaining => (ushort)(MaxStack - Amount);
        public bool IsEmpty => Amount <= 0;
        public bool IsSpace => Amount < MaxStack;
        public float Weight => Item.Weight * amount;

        public ushort itemId;
        public ushort amount;
        public int id;

        public bool Equals(Slot other)
        {
            if (other.itemId == itemId && other.amount == amount && other.id == id) return true;
            return false;
        }

        public ushort Add(ushort value)
        {
            ushort valueToAdd = (ushort)Mathf.Min(value, Remaining);
            amount += valueToAdd;
            return (ushort)(value - valueToAdd);
        }

        public ushort Remove(ushort value)
        {
            ushort valueToRemove = (ushort)Mathf.Min(value, Amount);
            amount -= valueToRemove;
            return (ushort)(value - valueToRemove);
        }

        public static implicit operator int(Slot slot)
        {
            byte[] b1 = BitConverter.GetBytes(slot.itemId);
            byte[] b2 = BitConverter.GetBytes(slot.amount);
            int s = b1[0] | (b1[1] << 8) | (b2[0] << 16) | (b2[1] << 24);
            return s;
        }

        public static implicit operator Slot(int s)
        {
            byte[] b = BitConverter.GetBytes(s);
            ushort itemId = (ushort)(b[0] | b[1] << 8);
            ushort amount = (ushort)(b[2] << 16 | b[3] << 24);
            return new Slot() { itemId = itemId, amount = amount};
        }
    }
}