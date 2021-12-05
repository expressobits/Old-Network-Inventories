using NUnit.Framework;
using UnityEngine;

namespace ExpressoBits.Inventories.Tests
{
    // public class ContainerTests
    // {

    //     [Test]
    //     public void ContainerTestAddItem()
    //     {
    //         Item item = ScriptableObject.CreateInstance<Item>();
    //         item.name = "Dummy item";

    //         GameObject inventory = new GameObject();
    //         Container c = inventory.AddComponent<Container>();
    //         Assert.AreEqual(c.Add(item),0);

    //         Assert.AreEqual(c.Slots.Count,1);

    //         Assert.AreEqual(c.Slots[0].Item,item);
    //         Assert.AreEqual(c.Slots[0].Amount,1);

    //         Assert.IsTrue(c.Slots[0].IsSpace);
    //     }

    //     [Test]
    //     public void ContainerTestReAddItem()
    //     {
    //         Item item = ScriptableObject.CreateInstance<Item>();
    //         item.name = "Dummy item";

    //         GameObject inventory = new GameObject();
    //         Container c = inventory.AddComponent<Container>();
    //         c.Add(item);

    //         Assert.AreEqual(c.Add(item),0);

    //         Assert.AreEqual(c.Slots.Count,1);
    //         Assert.IsTrue(c.Slots[0].IsSpace);
    //         Assert.AreEqual(c.Slots[0].Amount,2);
    //     }



    //     // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    //     // `yield return null;` to skip a frame.
    //     // [UnityTest]
    //     // public IEnumerator SlotTestsWithEnumeratorPasses()
    //     // {
    //     //     // Use the Assert class to test conditions.
    //     //     // Use yield to skip a frame.
    //     //     yield return null;
    //     // }
    // }
}
