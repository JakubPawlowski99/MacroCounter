using System.ComponentModel;

public class EatenFoodItem : INotifyPropertyChanged
{
    private string _name;
    private double _calories;
    private double _protein;
    private double _fat;
    private double _carbs;
    private double _amount;

    public string Name
    {
        get { return _name; }
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public double Calories
    {
        get { return _calories; }
        set
        {
            if (_calories != value)
            {
                _calories = value;
                OnPropertyChanged(nameof(Calories));
                CalculateNutrients();
            }
        }
    }

    public double Protein
    {
        get { return _protein; }
        set
        {
            if (_protein != value)
            {
                _protein = value;
                OnPropertyChanged(nameof(Protein));
                CalculateNutrients();
            }
        }
    }

    public double Fat
    {
        get { return _fat; }
        set
        {
            if (_fat != value)
            {
                _fat = value;
                OnPropertyChanged(nameof(Fat));
                CalculateNutrients();
            }
        }
    }

    public double Carbs
    {
        get { return _carbs; }
        set
        {
            if (_carbs != value)
            {
                _carbs = value;
                OnPropertyChanged(nameof(Carbs));
                CalculateNutrients();
            }
        }
    }

    public double Amount
    {
        get { return _amount; }
        set
        {
            if (_amount != value)
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
                CalculateNutrients();
            }
        }
    }

    // Calculated properties based on amount
    public double CalculatedCalories { get; private set; }
    public double CalculatedProtein { get; private set; }
    public double CalculatedFat { get; private set; }
    public double CalculatedCarbs { get; private set; }

    private void CalculateNutrients()
    {
        // Calculate nutrients based on the amount
        CalculatedCalories = Calories * Amount;
        CalculatedProtein = Protein * Amount;
        CalculatedFat = Fat * Amount;
        CalculatedCarbs = Carbs * Amount;

        OnPropertyChanged(nameof(CalculatedCalories));
        OnPropertyChanged(nameof(CalculatedProtein));
        OnPropertyChanged(nameof(CalculatedFat));
        OnPropertyChanged(nameof(CalculatedCarbs));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
