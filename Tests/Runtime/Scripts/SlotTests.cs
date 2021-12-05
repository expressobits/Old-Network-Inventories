using NUnit.Framework;
using UnityEngine;

namespace ExpressoBits.Inventories.Tests
{
    public class SlotTests
    {

        [Test]
        public void TestAmountAndInitializationOfSlot()
        {
            Slot slot = new Slot(){ itemId = 1, amount = 0 };
            slot.Add(2);
            Assert.AreEqual(slot.Amount,2);
            //Assert.AreEqual(slot.Item,item);
        }

        [Test]
        public void TestAddOnSlot()
        {
            Slot slot = new Slot(){ itemId = 1 };
            Assert.AreEqual(slot.Add(1),0);
            Assert.AreEqual(slot.Amount,1);
            Assert.AreEqual(slot.Add(slot.MaxStack),1);
            Assert.AreEqual(slot.Amount,slot.MaxStack);
        }

        [Test]
        public void TestRemoveValues()
        {
            Slot slot = new Slot(){ itemId = 1 };

            slot.Add(12);

            Assert.AreEqual(slot.Remove(5),0);
            Assert.AreEqual(slot.Amount,7);
            Assert.AreEqual(slot.Remove(12),5);
            Assert.AreEqual(slot.Amount,0);
        }

        [Test]
        public void TestSpace()
        {
            Slot slot = new Slot(){ itemId = 1 };

            slot.Add(12);

            Assert.IsTrue(slot.IsSpace);

            slot.Add(slot.MaxStack);

            Assert.IsFalse(slot.IsSpace);
        }

    
    }
}
