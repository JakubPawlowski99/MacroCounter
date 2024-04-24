using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MacroCounter
{
    public partial class MainWindow : Window
    {

        private void AddAnotherFoodBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create Grid dynamically
            Grid foodInputGrid = new Grid();
            foodInputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            foodInputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            foodInputGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

            // Create ComboBox dynamically
            ComboBox comboBox = new ComboBox { Height = 30,  Width = 150, Margin = new Thickness(0, 5, 0, 0) };
            // Create TextBox dynamically
            TextBox textBox = new TextBox { Height=30, Width = 100, Margin = new Thickness(0, 5, 0, 0) };
            // Create Remove button dynamically
            Button removeButton = new Button { Height = 30,  Width = 60, Content = "Remove" };
            removeButton.Click += RemoveFoodBtn_Click;

            // Add ComboBox, TextBox, and Remove button to the Grid
            Grid.SetColumn(comboBox, 0);
            Grid.SetColumn(textBox, 1);
            Grid.SetColumn(removeButton, 2);

            foodInputGrid.Children.Add(comboBox);
            foodInputGrid.Children.Add(textBox);
            foodInputGrid.Children.Add(removeButton);

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

            // Update the UI to display the total nutritional values
            TotalCaloriesTextBlock.Text = $"Total Calories: {totalCalories}";
            TotalProteinTextBlock.Text = $"Total Protein: {totalProtein}";
            TotalFatTextBlock.Text = $"Total Fat: {totalFat}";
            TotalCarbsTextBlock.Text = $"Total Carbs: {totalCarbs}";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            List<EatenFoodItem> eatenFoodItems = new List<EatenFoodItem>();

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

            CalculateTotalNutrition(eatenFoodItems);
        }

    }
}
