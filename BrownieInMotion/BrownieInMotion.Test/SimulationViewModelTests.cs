using BrownieInMotion.Core.ViewModels;
using Xunit;
using System.Linq;

namespace BrownieInMotion.Test;

public class SimulationViewModelTests
{
    [Fact]
    public void SimulateCommand_GeneratesSimulationsAndPrices()
    {
        var vm = new SimulationViewModel
        {
            InitialPrice = 50,
            Volatility = 0.1,
            Mean = 0.001,
            NumDays = 10,
            NumSimulations = 3
        };

        vm.SimulateCommand.Execute(null);

        Assert.NotNull(vm.Simulations);
        Assert.Equal(3, vm.Simulations!.Count);
        Assert.All(vm.Simulations, arr => Assert.Equal(10, arr.Length));
        Assert.NotNull(vm.Prices);
        Assert.Equal(10, vm.Prices!.Length);
    }

    [Fact]
    public void PropertyChanged_IsRaised_OnSet()
    {
        var vm = new SimulationViewModel();
        string? lastProp = null;
        vm.PropertyChanged += (s, e) => lastProp = e.PropertyName;

        vm.InitialPrice = 123;
        Assert.Equal(nameof(vm.InitialPrice), lastProp);

        vm.Volatility = 0.5;
        Assert.Equal(nameof(vm.Volatility), lastProp);

        vm.Mean = 0.01;
        Assert.Equal(nameof(vm.Mean), lastProp);

        vm.NumDays = 42;
        Assert.Equal(nameof(vm.NumDays), lastProp);

        vm.NumSimulations = 2;
        Assert.Equal(nameof(vm.NumSimulations), lastProp);

        vm.SelectedLineStyleIndex = 1;
        Assert.Equal(nameof(vm.SelectedLineStyleIndex), lastProp);

        vm.LineThickness = 3.2;
        Assert.Equal(nameof(vm.LineThickness), lastProp);

        vm.ShowExtremes = false;
        Assert.Equal(nameof(vm.ShowExtremes), lastProp);

        vm.ShowGrid = false;
        Assert.Equal(nameof(vm.ShowGrid), lastProp);
    }
}