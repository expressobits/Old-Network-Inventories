using NUnit.Framework;
using UnityEngine;

namespace ExpressoBits.Inventories.Tests
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
