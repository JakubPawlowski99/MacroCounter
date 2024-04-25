using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MacroCounter
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            // Set the current date
            currentDate = DateTime.Today;
            currentDateLabel.Content = currentDate.ToShortDateString();

            // Load data from XML for the current day
            LoadFromXml("eaten_items.xml");
        }

        public void LoadFoodItems(ComboBox comboBox)
        {
            // Load food items from XML file
            List<FoodItem> foodItems = FoodItemLoader.LoadFoodItems("foods.xml");

            // Bind food items to the ComboBox
            comboBox.ItemsSource = foodItems;
            comboBox.DisplayMemberPath = "Name";
        }
    }
}
