using NUnit.Framework;
using UnityEngine;

namespace ExpressoBits.Inventory.Tests
{
    public class ItemsTests
    {

        [Test]
        public void TestItemsDatabase()
        {
            Items[] items = Resources.FindObjectsOfTypeAll<Items>();
            Assert.IsTrue(items.Length > 0);
        }
    
    }
}
