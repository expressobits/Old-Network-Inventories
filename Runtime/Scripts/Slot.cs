using System;
using Unity.Netcode;
using UnityEngine;

namespace ExpressoBits.Inventory
{
    public struct Slot : IEquatable<Slot>, ISlot<Item>
    {
        public Item Item => ItemID;
        public byte ItemID => itemId;
        public byte Amount => amount;
        public byte MaxStack => Item.MaxStack;
        public byte Remaining => (byte)(MaxStack - Amount);
        public bool IsEmpty => Amount <= 0;
        public bool IsSpace => Amount < MaxStack;
        public float Weight => Item.Weight * amount;

        public byte itemId;
        public byte amount;
        public int id;

        public bool Equals(Slot other)
        {
            if (other.itemId == itemId && other.amount == amount && other.id == id) return true;
            return false;
        }

        // public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        // {
        //     serializer.SerializeValue(ref itemId);
        //     serializer.SerializeValue(ref amount);
        //     id = UnityEngine.Random.Range(0,Int32.MaxValue);
        // }

        public byte Add(byte value)
        {
            byte valueToAdd = (byte)Mathf.Min(value, Remaining);
            amount += valueToAdd;
            return (byte)(value - valueToAdd);
        }

        public byte Remove(byte value)
        {
            byte valueToRemove = (byte)Mathf.Min(value, Amount);
            amount -= valueToRemove;
            return (byte)(value - valueToRemove);
        }

        public static implicit operator short(Slot slot)
        {
            short s = (short)(slot.itemId | (slot.amount << 8));
            return s;
        }

        public static implicit operator Slot(short s)
        {
            byte[] b = BitConverter.GetBytes(s);
            return new Slot() { itemId = b[0], amount = b[1] };
        }
    }
}