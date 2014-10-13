using System;
using System.Collections.Generic;
using NUnit.Framework;

// I assume that "Conjured" is a distinct item category. That is, there are no mixed categories such as "Conjured Aged Brie". 

namespace GildedRoseKata
{
    class Program
    {
        private const string Passes = "Backstage passes to a TAFKAL80ETC concert";
        private const int MaxMinusOne = 49;
        private const int Min = 0;

        class Item
        {
            public string Name { get; set; }
            public int SellIn { get; set; }
            public int Quality { get; set; }
        }

        static void Main() { }

        static void UpdateQuality()
        {
            foreach (var item in Items)
            {
                if (item.Quality < Min) throw new Exception("Quality out of range");
                UpdateAgedBrieQuality(item);
                UpdateBackStageTicketsQuality(item);
                UpdateQualityForNormalItem(item);
                UpdateConjuredItemQuality(item);
                UpdateSellIn(item);
            }
        }
      
        private static void UpdateAgedBrieQuality(Item item)
        {
            if (item.Name != "Aged Brie") return;
            if (item.Quality == MaxMinusOne) item.Quality++;
            else if (item.Quality < MaxMinusOne) StandardChangeBy(1, 0, item);
        }

        private static void UpdateBackStageTicketsQuality(Item item)
        {
            if (item.Name != Passes) return;
            item.Quality++;
            if (item.SellIn <= 10 && item.Quality < MaxMinusOne) StandardChangeBy(1, 5, item);
            if (item.SellIn <= Min) item.Quality = 0;
        }

        private static void UpdateQualityForNormalItem(Item item)
        {
            if (NotNormalItem(item)) return;
            if (item.Quality == Min + 1) item.Quality = 0;
            else if (item.Quality > Min + 1) StandardChangeBy(-1, 0, item);
        }

        private static void UpdateConjuredItemQuality(Item item)
        {
            if (!item.Name.Contains("Conjured")) return;
            if (item.Quality == Min + 1 || item.Quality == Min + 2) item.Quality = 0;
            else if (item.Quality > Min + 2) StandardChangeBy(-2, 0, item);
        }

        private static void UpdateSellIn(Item item)
        {
            if (item.Name != "Sulfuras, Hand of Ragnaros") item.SellIn--;
        }

        private static bool NotNormalItem(Item item)
        {
            return item.Name == "Aged Brie" || item.Name == Passes
                   || item.Name == "Sulfuras, Hand of Ragnaros" || item.Name.Contains("Conjured");
        }

        private static void StandardChangeBy(int changeAmount, int border, Item item)
        {
            item.Quality += item.SellIn <= border ? 2 * changeAmount : changeAmount;
        }

        static readonly IList<Item> Items = new List<Item>
            {
                new Item { Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20 },
                new Item { Name = "Aged Brie", SellIn = 2, Quality = 0 },
                new Item { Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7 },
                new Item { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 },
                new Item { Name = Passes, SellIn = 15, Quality = 20 },
                new Item { Name = "Conjured Mana Cake", SellIn = 3, Quality = 6 },
            };

        [TestFixture]
        public class UpdateTest
        {
            [Test]
            public void UpdateQualityOnce()
            {
                ResetItems();
                UpdateForDays(1);
                ExpectedQualitiesOfListItems(19, 1 ,6, 80, 21, 4);
            }

            [Test]
            public void UpdateQualityTwice()
            {
                ResetItems();
                UpdateForDays(2);
                ExpectedQualitiesOfListItems(18, 2, 5, 80, 22, 2);
            }
 
            [Test]
            public void UpdateQualityThree()
            {
                ResetItems();
                UpdateForDays(3);
                ExpectedQualitiesOfListItems(17, 4, 4, 80, 23,0);
            }

            [Test]
            public void UpdateQualityFive()
            {
                ResetItems();
                UpdateForDays(5);
                ExpectedQualitiesOfListItems(15, 8, 2, 80, 25, 0);
            }

            [Test]
            public void UpdateQualitySix()
            {
                ResetItems();
                UpdateForDays(6);
                ExpectedQualitiesOfListItems(14, 10, 0, 80, 27, 0);
            }

            [Test]
            public void UpdateQualityTen()
            {
                ResetItems();
                UpdateForDays(10);
                ExpectedQualitiesOfListItems(10, 18, 0, 80, 35, 0);
            }

            [Test]
            public void UpdateQualityEleven()
            {
                ResetItems();
                UpdateForDays(11);
                ExpectedQualitiesOfListItems(8, 20, 0, 80, 38, 0);
            }

            [Test]
            public void UpdateQualityFifteen()
            {
                ResetItems();
                UpdateForDays(15);
                ExpectedQualitiesOfListItems(0, 28, 0, 80, 50, 0);
            }

            [Test]
            public void UpdateQualityTwenty()
            {
                ResetItems();
                UpdateForDays(20);
                ExpectedQualitiesOfListItems(0, 38, 0, 80, 0, 0);
            }

            [Test]
            public void UpdateQualityThirtyOne()
            {
                ResetItems();
                UpdateForDays(31);
                ExpectedQualitiesOfListItems(0, 50, 0, 80, 0, 0);
            }

            [Test]
            public void UpdateQualityThirtyFive()
            {
                ResetItems();
                UpdateForDays(35);
                ExpectedQualitiesOfListItems(0, 50, 0, 80, 0, 0);
            }

            private static void ResetItems()
            {
                Items[0].SellIn = 10;
                Items[0].Quality = 20;
                Items[1].SellIn = 2;
                Items[1].Quality = 0;
                Items[2].SellIn = 5;
                Items[2].Quality = 7;
                Items[3].SellIn = 0;
                Items[3].Quality = 80;
                Items[4].SellIn = 15;
                Items[4].Quality = 20;
                Items[5].SellIn = 3;
                Items[5].Quality = 6;
            }

            public void ExpectedQualitiesOfListItems(int a, int b, int c, int d, int e, int f)
            {
                var lst = new List<int> { a, b, c, d, e, f };
                for (var i = 0; i <= 5; i++)
                {
                    Assert.That(Items[i].Quality, Is.EqualTo(lst[i]));
                }
            }

            private static void UpdateForDays(int days)
            {
                for (var i = 0; i < days; i++)
                {
                    UpdateQuality();
                }
            }

        }

    }
}