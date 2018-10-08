﻿using NUnit.Framework;
using System.Collections.Generic;

namespace csharp
{
    [TestFixture]
    public class GildedRoseTest
    {
        [Test]
        public void foo()
        {
            IList<Item> Items = new List<Item> { new Item { Name = "foo", SellIn = 0, Quality = 0 } };
            // GildedRose app = new GildedRose(Items);
            // app.UpdateQuality();
            VegaCollardKata.GildedRose.UpdateQuality(Items);
            Assert.AreEqual("fixme", Items[0].Name);
        }
    }
}
