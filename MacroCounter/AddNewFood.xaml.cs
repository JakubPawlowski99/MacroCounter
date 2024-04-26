using System;
using System.IO;
using System.Windows;
using System.Xml;

namespace MacroCounter
{
    public partial class AddNewFood : Window
    {
        public AddNewFood()
        {
            InitializeComponent();
        }
        private string GetFilePath()
        {
            string fileName = "foods.xml";
            string directoryPath = Environment.CurrentDirectory;
            return Path.Combine(directoryPath, fileName);
        }

        private void CreateNewFile()
        {
            string filePath = GetFilePath();
            XmlDocument doc = new XmlDocument();
            XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(declaration);
            XmlElement root = doc.CreateElement("foods");
            doc.AppendChild(root);
            doc.Save(filePath);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Get values from textboxes
            string name = NameTextBox.Text;
            double calories, protein, fats, carbs;

            if (double.TryParse(CaloriesTextBox.Text, out calories) &&
                double.TryParse(ProteinTextBox.Text, out protein) &&
                double.TryParse(FatsTextBox.Text, out fats) &&
                double.TryParse(CarbsTextBox.Text, out carbs))
            {
                string filePath = GetFilePath();

                try
                {
                    // Check if the file exists, if not, create it
                    if (!File.Exists(filePath))
                    {
                        CreateNewFile();
                    }

                    // Load existing or newly created XML file
                    XmlDocument doc = new XmlDocument();
                    doc.Load(filePath);

                    // Create a new food element
                    XmlElement foodElem = doc.CreateElement("food");
                    XmlElement nameElem = doc.CreateElement("name");
                    nameElem.InnerText = name;
                    foodElem.AppendChild(nameElem);

                    XmlElement caloriesElem = doc.CreateElement("calories");
                    caloriesElem.InnerText = calories.ToString();
                    foodElem.AppendChild(caloriesElem);

                    XmlElement proteinElem = doc.CreateElement("protein");
                    proteinElem.InnerText = protein.ToString();
                    foodElem.AppendChild(proteinElem);

                    XmlElement fatsElem = doc.CreateElement("fats");
                    fatsElem.InnerText = fats.ToString();
                    foodElem.AppendChild(fatsElem);

                    XmlElement carbsElem = doc.CreateElement("carbs");
                    carbsElem.InnerText = carbs.ToString();
                    foodElem.AppendChild(carbsElem);

                    // Append the new food element to the root
                    doc.DocumentElement?.AppendChild(foodElem);

                    // Save the modified XML back to the file
                    doc.Save(filePath);

                    MessageBox.Show("Data saved successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving data: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please enter valid numeric values for calories, protein, fats, and carbs.");
            }
        }
    }
}
