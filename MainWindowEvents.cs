using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace MacroCounter
{
    public partial class MainWindow : Window
    {
        private DateTime currentDate = DateTime.Today;
        private List<EatenFoodItem> eatenFoodItems = new List<EatenFoodItem>();



        private void AddAnotherFoodBtn_Click(object sender, RoutedEventArgs e)
        {

            // Create Grid dynamically
            Grid foodInputGrid = new Grid();
            foodInputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            foodInputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            foodInputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            // Create ComboBox dynamically
            ComboBox comboBox = new ComboBox { Height = 30, Width = 150, Margin = new Thickness(0, 5, 0, 0) };
            // Create TextBox dynamically
            TextBox textBox = new TextBox { Height = 30, Width = 100, Margin = new Thickness(0, 5, 0, 0) };
            // Create Remove button dynamically
            Button removeButton = new Button { Height = 30, Width = 60, Content = "Remove" };
            removeButton.Click += RemoveFoodBtn_Click;

            // Add ComboBox, TextBox, and Remove button to the Grid
            foodInputGrid.Children.Add(comboBox);
            foodInputGrid.Children.Add(textBox);
            foodInputGrid.Children.Add(removeButton);

            // Set Grid.Row property for each control
            Grid.SetColumn(comboBox, 0);
            Grid.SetColumn(textBox, 1);
            Grid.SetColumn(removeButton, 2);

            // Set Grid.Row property for each control to the next available row
            int rowIndex = AddAnotherFoodPanel.Children.Count; // Get the index of the next row
            Grid.SetRow(comboBox, rowIndex); // Set ComboBox to next row
            Grid.SetRow(textBox, rowIndex); // Set TextBox to next row
            Grid.SetRow(removeButton, rowIndex); // Set Remove button to next row

            // Add Grid to the AddAnotherFoodPanel
            AddAnotherFoodPanel.Children.Add(foodInputGrid); // Add at the end
 
            // Call LoadFoodItems to load food items into the ComboBox
            LoadFoodItems(comboBox);


        }
        private void RemoveFoodBtn_Click(object sender, RoutedEventArgs e)
        {
            Button removeButton = (Button)sender;
            Grid foodInputGrid = (Grid)removeButton.Parent;

            // Remove the food input grid when the Remove button is clicked
            if (AddAnotherFoodPanel.Children.Contains(foodInputGrid))
            {
                AddAnotherFoodPanel.Children.Remove(foodInputGrid);
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            T parent = parentObject as T;
            return parent ?? FindParent<T>(parentObject);
        }


        // Modify the CalculateTotalNutrition method to calculate nutritional values based on foods.xml
        private void CalculateTotalNutrition(List<EatenFoodItem> eatenFoodItems)
        {
            double totalCalories = 0;
            double totalProtein = 0;
            double totalFat = 0;
            double totalCarbs = 0;

            foreach (var item in eatenFoodItems)
            {
                // Find the corresponding food item from your food XML based on the name
                FoodItem foodItem = FindFoodItem(item.Name);

                // If the corresponding food item is found, calculate the macros
                if (foodItem != null)
                {
                    totalCalories += foodItem.Calories * item.Amount;
                    totalProtein += foodItem.Protein * item.Amount;
                    totalFat += foodItem.Fats * item.Amount;
                    totalCarbs += foodItem.Carbs * item.Amount;
                }
            }

            // Round up the total nutrition values to one decimal point
            totalCalories = Math.Round(totalCalories, 1);
            totalProtein = Math.Round(totalProtein, 1);
            totalFat = Math.Round(totalFat, 1);
            totalCarbs = Math.Round(totalCarbs, 1);

            // Update the UI to display the rounded total nutritional values
            TotalCaloriesTextBlock.Text = $"Total Calories: {totalCalories}";
            TotalProteinTextBlock.Text = $"Total Protein: {totalProtein}";
            TotalFatTextBlock.Text = $"Total Fat: {totalFat}";
            TotalCarbsTextBlock.Text = $"Total Carbs: {totalCarbs}";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Grid foodInputGrid in AddAnotherFoodPanel.Children.OfType<Grid>())
            {
                ComboBox comboBox = foodInputGrid.Children.OfType<ComboBox>().FirstOrDefault();
                TextBox textBox = foodInputGrid.Children.OfType<TextBox>().FirstOrDefault();

                if (comboBox != null && textBox != null && comboBox.SelectedItem is FoodItem selectedFoodItem && double.TryParse(textBox.Text, out double amount))
                {
                    // Calculate the calories and macros for the new food item
                    double calories = selectedFoodItem.Calories * amount;
                    double protein = selectedFoodItem.Protein * amount;
                    double fat = selectedFoodItem.Fats * amount;
                    double carbs = selectedFoodItem.Carbs * amount;

                    // Create the EatenFoodItem object with calculated values
                    EatenFoodItem eatenFoodItem = new EatenFoodItem
                    {
                        Name = selectedFoodItem.Name,
                        Amount = amount,
                        Calories = calories,
                        Protein = protein,
                        Fat = fat,
                        Carbs = carbs
                    };

                    // Add the eaten food item to the private list
                    eatenFoodItems.Add(eatenFoodItem);

                    // Add the eaten food item to the ListView
                    EatenItemsListView.Items.Add(eatenFoodItem);
                }
            }

            // Calculate total nutrition after adding items
            CalculateTotalNutrition(eatenFoodItems);

        }


        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that was clicked
            Button removeButton = (Button)sender;

            // Get the data item bound to the button
            EatenFoodItem itemToRemove = (EatenFoodItem)removeButton.Tag;

            // Remove the item from the ListView
            EatenItemsListView.Items.Remove(itemToRemove);

            // Remove the item from the private list
            eatenFoodItems.Remove(itemToRemove);

            // Recalculate total nutrition after removing item
            CalculateTotalNutrition(eatenFoodItems);
        }

        private List<EatenFoodItem> GetEatenFoodItems()
        {
            List<EatenFoodItem> eatenFoodItems = new List<EatenFoodItem>();

            foreach (EatenFoodItem item in EatenItemsListView.Items)
            {
                eatenFoodItems.Add(item);
            }

            return eatenFoodItems;
        }




        // Wrapper class to hold the date and list of eaten food items
        [Serializable]
        public class DayEatenFoodItems
        {
            public DateTime Date { get; set; }
            public List<EatenFoodItem> EatenFoodItems { get; set; }
        }

        [Serializable]
        public class EatenFoodItem
        {
            public string Name { get; set; }
            public double Amount { get; set; }
            public double Calories { get; set; }
            public double Protein { get; set; }
            public double Fat { get; set; }
            public double Carbs { get; set; }
        }

        // Modify the SaveToXml method to correctly serialize only the Name and Amount
        private void SaveToXml(string fileName)
        {
            try
            {
                // Create a list of DayEatenFoodItems objects for serialization
                List<DayEatenFoodItems> dayEatenFoodItemsList = new List<DayEatenFoodItems>();

                // Create a DayEatenFoodItems object with the current date and list of eaten items
                DayEatenFoodItems dayEatenFoodItems = new DayEatenFoodItems
                {
                    Date = currentDate,
                    EatenFoodItems = eatenFoodItems
                };

                // Add the DayEatenFoodItems object to the list
                dayEatenFoodItemsList.Add(dayEatenFoodItems);

                // Create the EatenItems object to hold the list of DayEatenFoodItems
                EatenItems eatenItems = new EatenItems
                {
                    DayEatenFoodItems = dayEatenFoodItemsList
                };

                // Serialize the EatenItems object to XML
                XmlSerializer serializer = new XmlSerializer(typeof(EatenItems));
                using (TextWriter writer = new StreamWriter(fileName))
                {
                    serializer.Serialize(writer, eatenItems);
                }

                MessageBox.Show("Data saved to XML successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to XML: {ex.Message}");
            }
        }

        private List<FoodItem> LoadFoodItemsFromXml(string fileName)
        {
            List<FoodItem> foodItems = new List<FoodItem>();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FoodItemList));
                using (TextReader reader = new StreamReader(fileName))
                {
                    FoodItemList foodItemList = (FoodItemList)serializer.Deserialize(reader);
                    foodItems = foodItemList.Foods;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading food items from XML: {ex.Message}");
            }

            return foodItems;
        }

        // Modify the FindFoodItem method to return FoodItem based on Name from foods.xml
        private FoodItem FindFoodItem(string itemName)
        {
            // Load the food XML data into memory
            List<FoodItem> foodItems = LoadFoodItemsFromXml("foods.xml");

            // Search for the food item by name
            return foodItems.FirstOrDefault(item => item.Name == itemName);
        }


        [Serializable]
        public class EatenItems
        {
            public List<DayEatenFoodItems> DayEatenFoodItems { get; set; }
        }

        // Modify the LoadFromXml method to correctly deserialize the XML data
        private void LoadFromXml(string fileName)
{
    try
    {
        XmlSerializer serializer = new XmlSerializer(typeof(EatenItems));
        using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
        {
            EatenItems eatenItems = (EatenItems)serializer.Deserialize(fileStream);

            // Find the DayEatenFoodItems object for the current date
            DayEatenFoodItems dayEatenFoodItems = eatenItems.DayEatenFoodItems.FirstOrDefault(item => item.Date == currentDate);

            if (dayEatenFoodItems != null)
            {
                // Update the eaten food items with the data from the deserialized object
                eatenFoodItems = dayEatenFoodItems.EatenFoodItems;

                // Refresh the ListView
                RefreshEatenItemsListView();

                // Recalculate total nutrition based on loaded data
                CalculateTotalNutrition(eatenFoodItems);
            }
            else
            {
                MessageBox.Show("No data available for the current date.");
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Error loading data from XML: {ex.Message}");
    }
}


        // Method to update the ListView with the current list of eaten food items
        private void RefreshEatenItemsListView()
        {
            EatenItemsListView.Items.Clear();
            foreach (var eatenFoodItem in eatenFoodItems)
            {
                EatenItemsListView.Items.Add(eatenFoodItem);
            }
            CalculateTotalNutrition(eatenFoodItems);
        }

        // Event handler for "Save to XML" button click event
        private void SaveToXmlButton_Click(object sender, RoutedEventArgs e)
        {
            SaveToXml("eaten_items.xml");
        }


        // Method to update the current date for the previous day
        private void UpdateCurrentDatePrevious()
        {
            currentDate = currentDate.AddDays(-1);
            currentDateLabel.Content = currentDate.ToShortDateString();
            LoadFromXml("eaten_items.xml"); // Load data for the new date
        }

        // Method to update the current date for the next day
        private void UpdateCurrentDateNext()
        {
            currentDate = currentDate.AddDays(1);
            currentDateLabel.Content = currentDate.ToShortDateString();
            LoadFromXml("eaten_items.xml"); // Load data for the new date
        }

        // Event handler for "Previous Day" button click event
        private void PreviousDayButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentDatePrevious();
        }

        // Event handler for "Next Day" button click event
        private void NextDayButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCurrentDateNext();
        }

    }
}
