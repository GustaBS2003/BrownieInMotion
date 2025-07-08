using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BrownieInMotion.Core.Services;

namespace BrownieInMotion.Core.ViewModels;

public class AnnualSimulationViewModel : INotifyPropertyChanged
{
    private double _initialPrice = 100.0;
    private double _volatility = 0.2;
    private double _mean = 0.01;
    private int _years = 1;
    private int _stepsPerYear = 252;
    private double[]? _prices;
    private int _numSimulations = 1;
    private List<double[]>? _simulations;

    // Personalização do gráfico
    private int _selectedLineStyleIndex = 0;
    private double _lineThickness = 2.5;
    private bool _showExtremes = true;
    private bool _showGrid = true;

    public double InitialPrice
    {
        get => _initialPrice;
        set { _initialPrice = value; OnPropertyChanged(); }
    }

    public double Volatility
    {
        get => _volatility;
        set { _volatility = value; OnPropertyChanged(); }
    }

    public double Mean
    {
        get => _mean;
        set { _mean = value; OnPropertyChanged(); }
    }

    public int Years
    {
        get => _years;
        set { _years = value; OnPropertyChanged(); }
    }

    public int StepsPerYear
    {
        get => _stepsPerYear;
        set { _stepsPerYear = value; OnPropertyChanged(); }
    }

    public double[]? Prices
    {
        get => _prices;
        private set { _prices = value; OnPropertyChanged(); }
    }

    public int NumSimulations
    {
        get => _numSimulations;
        set { _numSimulations = value; OnPropertyChanged(); }
    }

    public List<double[]>? Simulations
    {
        get => _simulations;
        private set { _simulations = value; OnPropertyChanged(); }
    }

    // Propriedades para personalização visual
    public int SelectedLineStyleIndex
    {
        get => _selectedLineStyleIndex;
        set { _selectedLineStyleIndex = value; OnPropertyChanged(); }
    }

    public double LineThickness
    {
        get => _lineThickness;
        set { _lineThickness = value; OnPropertyChanged(); }
    }

    public bool ShowExtremes
    {
        get => _showExtremes;
        set { _showExtremes = value; OnPropertyChanged(); }
    }

    public bool ShowGrid
    {
        get => _showGrid;
        set { _showGrid = value; OnPropertyChanged(); }
    }

    public ICommand SimulateCommand { get; }

    public AnnualSimulationViewModel()
    {
        SimulateCommand = new RelayCommand(ExecuteSimulation);
    }

    private void ExecuteSimulation()
    {
        var sims = new List<double[]>();
        for (int i = 0; i < NumSimulations; i++)
        {
            sims.Add(BrownianMotionService.GenerateAnnualBrownianMotion(
                Volatility, Mean, InitialPrice, Years, StepsPerYear));
        }
        Simulations = sims;
        Prices = sims.FirstOrDefault();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}