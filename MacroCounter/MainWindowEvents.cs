using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MacroCounter
{
    public partial class MainWindow : Window
    {

        private List<EatenFoodItem> eatenFoodItems = new List<EatenFoodItem>();

        private void AddAnotherFoodBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create Grid dynamically
            Grid foodInputGrid = new Grid();
            foodInputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
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
            // Create Add button dynamically
            Button addButton = new Button {  Height = 30, Width = 60, Content = "Add" };
            addButton.Click += AddButton_Click;

            // Add ComboBox, TextBox, and Remove button to the Grid
            foodInputGrid.Children.Add(comboBox);
            foodInputGrid.Children.Add(textBox);
            foodInputGrid.Children.Add(removeButton);
            foodInputGrid.Children.Add(addButton);

            // Set Grid.Row property for each control
            Grid.SetColumn(comboBox, 0);
            Grid.SetColumn(textBox, 1);
            Grid.SetColumn(removeButton, 2);
            Grid.SetColumn(addButton, 3);

            // Set Grid.Row property for each control to the next available row
            int rowIndex = AddAnotherFoodPanel.Children.Count; // Get the index of the next row
            Grid.SetRow(comboBox, rowIndex); // Set ComboBox to next row
            Grid.SetRow(textBox, rowIndex); // Set TextBox to next row
            Grid.SetRow(removeButton, rowIndex); // Set Remove button to next row
            Grid.SetRow(addButton, rowIndex);

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


        private void CalculateTotalNutrition(List<EatenFoodItem> eatenFoodItems)
        {
            double totalCalories = 0;
            double totalProtein = 0;
            double totalFat = 0;
            double totalCarbs = 0;

            foreach (var item in eatenFoodItems)
            {
                totalCalories += item.Calories * item.Amount;
                totalProtein += item.Protein * item.Amount;
                totalFat += item.Fat * item.Amount;
                totalCarbs += item.Carbs * item.Amount;
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
            // Clear the ListView before adding new items
            EatenItemsListView.Items.Clear();

            // Clear the list before adding new items
            eatenFoodItems.Clear();


            foreach (Grid foodInputGrid in AddAnotherFoodPanel.Children.OfType<Grid>())
            {
                ComboBox comboBox = foodInputGrid.Children.OfType<ComboBox>().FirstOrDefault();
                TextBox textBox = foodInputGrid.Children.OfType<TextBox>().FirstOrDefault();

                if (comboBox != null && textBox != null && comboBox.SelectedItem is FoodItem selectedFoodItem && double.TryParse(textBox.Text, out double amount))
                {
                    EatenFoodItem eatenFoodItem = new EatenFoodItem
                    {
                        Name = selectedFoodItem.Name,
                        Calories = selectedFoodItem.Calories,
                        Protein = selectedFoodItem.Protein,
                        Fat = selectedFoodItem.Fats,
                        Carbs = selectedFoodItem.Carbs,
                        Amount = amount
                    };

                    eatenFoodItems.Add(eatenFoodItem);
                }
            }

            // Add the eaten food items to the ListView
            foreach (var eatenFoodItem in eatenFoodItems)
            {
                EatenItemsListView.Items.Add(eatenFoodItem);
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

    }
}
