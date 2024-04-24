using System.Collections.Generic;
using System.Xml.Linq;

public static class FoodItemLoader
{
    public static List<FoodItem> LoadFoodItems(string filePath)
    {
        List<FoodItem> foodItems = new List<FoodItem>();

        XDocument doc = XDocument.Load(filePath);

        foreach (XElement foodElement in doc.Root.Elements("food"))
        {
            FoodItem foodItem = new FoodItem
            {
                Name = foodElement.Element("name").Value,
                Calories = double.Parse(foodElement.Element("calories").Value),
                Protein = double.Parse(foodElement.Element("protein").Value),
                Fats = double.Parse(foodElement.Element("fats").Value),
                Carbs = double.Parse(foodElement.Element("carbs").Value)
            };

            foodItems.Add(foodItem);
        }

        return foodItems;
    }
}
