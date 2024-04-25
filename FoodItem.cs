﻿using System.Collections.Generic;
using System.Xml.Serialization;
public class FoodItem
{
    public string Name { get; set; }
    public double Calories { get; set; }
    public double Protein { get; set; }
    public double Fats { get; set; }
    public double Carbs { get; set; }
}

[XmlRoot("foods")]
public class FoodItemList
{
    [XmlElement("food")]
    public List<FoodItem> Foods { get; set; }
}