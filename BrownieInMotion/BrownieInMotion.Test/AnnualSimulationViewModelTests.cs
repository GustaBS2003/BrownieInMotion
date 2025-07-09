using BrownieInMotion.Core.ViewModels;
using Xunit;
using System.Linq;

namespace BrownieInMotion.Test;

public class AnnualSimulationViewModelTests
{
    [Fact]
    public void SimulateCommand_GeneratesSimulationsAndPrices()
    {
        var vm = new AnnualSimulationViewModel
        {
            InitialPrice = 100,
            Volatility = 0.2,
            Mean = 0.01,
            Years = 2,
            StepsPerYear = 5,
            NumSimulations = 2
        };

        vm.SimulateCommand.Execute(null);

        Assert.NotNull(vm.Simulations);
        Assert.Equal(2, vm.Simulations!.Count);
        Assert.All(vm.Simulations, arr => Assert.Equal(10, arr.Length)); // 2*5=10
        Assert.NotNull(vm.Prices);
        Assert.Equal(10, vm.Prices!.Length);
    }

    [Fact]
    public void PropertyChanged_IsRaised_OnSet()
    {
        var vm = new AnnualSimulationViewModel();
        string? lastProp = null;
        vm.PropertyChanged += (s, e) => lastProp = e.PropertyName;

        vm.InitialPrice = 321;
        Assert.Equal(nameof(vm.InitialPrice), lastProp);

        vm.Volatility = 0.3;
        Assert.Equal(nameof(vm.Volatility), lastProp);

        vm.Mean = 0.02;
        Assert.Equal(nameof(vm.Mean), lastProp);

        vm.Years = 3;
        Assert.Equal(nameof(vm.Years), lastProp);

        vm.StepsPerYear = 100;
        Assert.Equal(nameof(vm.StepsPerYear), lastProp);

        vm.NumSimulations = 4;
        Assert.Equal(nameof(vm.NumSimulations), lastProp);

        vm.SelectedLineStyleIndex = 2;
        Assert.Equal(nameof(vm.SelectedLineStyleIndex), lastProp);

        vm.LineThickness = 4.5;
        Assert.Equal(nameof(vm.LineThickness), lastProp);

        vm.ShowExtremes = false;
        Assert.Equal(nameof(vm.ShowExtremes), lastProp);

        vm.ShowGrid = false;
        Assert.Equal(nameof(vm.ShowGrid), lastProp);
    }
}