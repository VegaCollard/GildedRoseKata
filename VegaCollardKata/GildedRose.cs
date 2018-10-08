using System;
using System.Reflection;
using System.Collections.Generic;

namespace VegaCollardKata
{
    public class GildedRose
    {
        #region Public Quality calculation methods

        /// <summary>
        /// new implementation of the existing "UpdateQuality" method, containing all the necessary calculations 
        /// </summary>
        /// <param name="itemList">The list of Item's to process</param>
        public static void UpdateQuality(IEnumerable<object> itemList)
        {
            if (Enum.IsDefined(typeof(ItemTypesAccepted), itemList.GetType().GenericTypeArguments[0].Name))
            {
                foreach (object itemToTreat in itemList)
                {
                    Type item = itemToTreat.GetType();
                    PropertyInfo name = item.GetProperty("Name");
                    PropertyInfo sellIn = item.GetProperty("SellIn");                    
                    string itemName = name.GetValue(itemToTreat).ToString();

                    if (!IsLegendary(itemName))
                    {
                        sellIn.SetValue(itemToTreat, (Convert.ToInt32(sellIn.GetValue(itemToTreat)) - 1));
                        int sellInValue = Convert.ToInt32(sellIn.GetValue(itemToTreat));
                        if (IsItemQualityHaveToRaise(itemName))
                        {
                            CalculateItemsWithRaiseQuality(itemToTreat);
                            if (Convert.ToInt32(sellIn.GetValue(itemToTreat)) < 0)
                            {
                                CalculateItemsWithRaiseQuality(itemToTreat);
                            }
                        }
                        else if (IsBackstagePass(itemName))
                        {
                            CalculateBackStageItemsQuality(itemToTreat);
                            if (Convert.ToInt32(sellIn.GetValue(itemToTreat)) < 0)
                            {
                                CalculateBackStageItemsQuality(itemToTreat);
                            }
                        }
                        else
                        {
                            CalculateNormalItemsQuality(itemToTreat);
                            if (Convert.ToInt32(sellIn.GetValue(itemToTreat)) < 0)
                            {
                                CalculateNormalItemsQuality(itemToTreat);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Bool methods

        /// <summary>
        /// Method that will test if Item name belongs to legendary items (if so name belongs to LegendaryItems enum)
        /// </summary>
        /// <param name="name">Item name</param>
        /// <returns>true if name is a legendary item; false if not</returns>
        private static bool IsLegendary(string name)
        {
            foreach (var item in Enum.GetValues(typeof(LegendaryItems)))
            {
                if (name.ToLower().Contains(item.ToString().ToLower())) return true;
            }
            return false;
        }

        /// <summary>
        /// Method that will test if Item name is an item that quality raise (if so name belongs to enum ItemWithQualityRaise)
        /// </summary>
        /// <param name="name">Item name</param>
        /// <returns>true if the name is a listed item that quality raise; false if not</returns>
        private static bool IsItemQualityHaveToRaise(string name)
        {
            foreach (var item in Enum.GetValues(typeof(ItemsWithQualityRaise)))
            {
                if (name.Replace(" ", string.Empty).ToLower().Contains(item.ToString().ToLower())) return true;
            }
            return false;
        }

        /// <summary>
        /// Method that will test if Item name is a backstage pass or not. 
        /// </summary>
        /// <param name="name">Item name</param>
        /// <returns>true/false</returns>
        private static bool IsBackstagePass(string name)
        {
            return name.Replace(" ", string.Empty).ToLower().Trim().Contains("backstagepass");
        }

        /// <summary>
        /// Method that will test if Item name is a conjured one or not. 
        /// </summary>
        /// <param name="name">Item name</param>
        /// <returns>true/false</returns>
        private static bool IsNormalConjuredItem(string name)
        {
            return name.ToLower().Trim().Contains(NormalSpecialItems.Conjured.ToString().ToLower());
        }

        #endregion

        #region Private Item quality calculation methods

        /// <summary>
        /// this method will treat all the calculation needed for the "Aged Brie" item.
        /// As it's a generic method, it can treat aventually other object having the same properties (Name, SellIn, Quality)
        /// </summary>
        /// <param name="itemToTreat">the current object to be calculated</param>
        private static void CalculateItemsWithRaiseQuality(object itemToTreat)
        {
            Type item = itemToTreat.GetType();
            PropertyInfo quality = item.GetProperty("Quality");

            int initialValue = Convert.ToInt32(quality.GetValue(itemToTreat));
            if (initialValue <= 50)
            {
                if ((initialValue + Math.Abs(Convert.ToInt32(QualityVariationsByDay.NormalVariation))) <= 50)
                {
                    quality.SetValue(itemToTreat, initialValue + Math.Abs(Convert.ToInt32(QualityVariationsByDay.NormalVariation)));
                }
                else
                {
                    quality.SetValue(itemToTreat, 50);
                }
            }
        }

        /// <summary>
        /// this method will treat all the calculation needed for the "Back Stage(s)" item.
        /// As it's a generic method, it can treat aventually other object having the same properties (Name, SellIn, Quality)
        /// </summary>
        /// <param name="itemToTreat">the current object to be calculated</param>
        private static void CalculateBackStageItemsQuality(object itemToTreat)
        {
            Type item = itemToTreat.GetType();
            PropertyInfo sellIn = item.GetProperty("SellIn");
            PropertyInfo quality = item.GetProperty("Quality");

            int initialValue = Convert.ToInt32(quality.GetValue(itemToTreat));
            if (initialValue <= 50)
            {
                int sellInValue = Convert.ToInt32(sellIn.GetValue(itemToTreat));
                if (sellInValue < 0)
                {
                    quality.SetValue(itemToTreat, 0);
                }
                else if (sellInValue > 10)
                {
                    if ((initialValue + Math.Abs(Convert.ToInt32(QualityVariationsByDay.NormalVariation))) <= 50)
                    {
                        quality.SetValue(itemToTreat, initialValue + Math.Abs(Convert.ToInt32(QualityVariationsByDay.NormalVariation)));
                    }
                    else
                    {
                        quality.SetValue(itemToTreat, 50);
                    }
                }
                else if (sellInValue < 11 && sellInValue > 5)
                {
                    if (initialValue + Math.Abs(Convert.ToInt32(QualityVariationsByDay.SellInIsMinusThanEleven)) <= 50)
                    {
                        quality.SetValue(itemToTreat, initialValue + Math.Abs(Convert.ToInt32(QualityVariationsByDay.SellInIsMinusThanEleven)));
                    }
                    else
                    {
                        quality.SetValue(itemToTreat, 50);
                    }
                }
                else if (sellInValue < 6)
                {
                    if (initialValue + Math.Abs(Convert.ToInt32(QualityVariationsByDay.SellInIsMinusThanSix)) <= 50)
                    {
                        quality.SetValue(itemToTreat, initialValue + Math.Abs(Convert.ToInt32(QualityVariationsByDay.SellInIsMinusThanSix)));
                    }
                    else
                    {
                        quality.SetValue(itemToTreat, 50);
                    }
                }
            }
        }
        
        /// <summary>
        /// this method will treat all the calculation needed for all the other items.
        /// As it's a generic method, it can treat aventually other object having the same properties (Name, SellIn, Quality)
        /// </summary>
        /// <param name="itemToTreat">the current object to be calculated</param>

        private static void CalculateNormalItemsQuality(object itemToTreat)
        {
            Type item = itemToTreat.GetType();
            PropertyInfo quality = item.GetProperty("Quality");

            int initialValue = Convert.ToInt32(quality.GetValue(itemToTreat));
            int decreaseValue = Math.Abs(Convert.ToInt32(QualityVariationsByDay.NormalVariation));

            if (Convert.ToString(item.GetProperty("Name").GetValue(itemToTreat)).ToLower().StartsWith(
                NormalSpecialItems.Conjured.ToString().ToLower()))
            {
                decreaseValue *= 2;
            }
            if (initialValue > 0)
            {
                quality.SetValue(itemToTreat, initialValue - decreaseValue);
            }
        }

        #endregion

        #region private enums

        private enum LegendaryItems
        {
            Sulfuras
        }

        private enum ItemsWithQualityRaise
        {
            AgedBrie
        }

        private enum NormalSpecialItems
        {
            Conjured
        }

        private enum QualityVariationsByDay
        {
            Legendary = 0,
            NormalVariation = 1,
            SellInIsMinusThanEleven = 2,
            SellInIsMinusThanSix = 3
        }

        private enum ItemTypesAccepted
        {
            Item
        }

        #endregion
    }
}
