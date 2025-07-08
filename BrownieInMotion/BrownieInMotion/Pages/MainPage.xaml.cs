using BrownieInMotion.Core.ViewModels;
using BrownieInMotion.Pages; // para LineStyle

namespace BrownieInMotion.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(SimulationViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(viewModel.Simulations)
                || e.PropertyName == nameof(viewModel.SelectedLineStyleIndex)
                || e.PropertyName == nameof(viewModel.LineThickness)
                || e.PropertyName == nameof(viewModel.ShowExtremes)
                || e.PropertyName == nameof(viewModel.ShowGrid))
            {
                var lineStyle = (LineStyle)viewModel.SelectedLineStyleIndex;
                BrownianChart.Drawable = viewModel.Simulations is not null
                    ? new BrownianChartDrawable(
                        viewModel.Simulations,
                        null,
                        lineStyle,
                        viewModel.LineThickness,
                        viewModel.ShowExtremes,
                        viewModel.ShowGrid)
                    : null;
                BrownianChart.Invalidate();
            }
        };
    }

    private void OnChartTapped(object sender, TappedEventArgs e)
    {
        if (BindingContext is not SimulationViewModel vm || vm.Prices == null || vm.Prices.Length < 2)
            return;

        var point = e.GetPosition(BrownianChart);
        if (point == null)
            return;

        float width = (float)BrownianChart.Width;
        float height = (float)BrownianChart.Height;
        float margin = 60f; // Use o mesmo valor do seu drawable
        float plotWidth = width - 2 * margin;

        int idx = (int)Math.Round((point.Value.X - margin) / (plotWidth / (vm.Prices.Length - 1)));
        idx = Math.Clamp(idx, 0, vm.Prices.Length - 1);

        if (point.Value.X < margin || point.Value.X > width - margin)
        {
            TooltipLabel.IsVisible = false;
            return;
        }

        TooltipLabel.Text = $"Dia {idx + 1}: {vm.Prices[idx]:F2}";
        TooltipLabel.IsVisible = true;
        double labelX = Math.Clamp(point.Value.X, 0, width - 80);
        double labelY = Math.Clamp(point.Value.Y, 0, height - 30);
        TooltipLabel.TranslationX = labelX;
        TooltipLabel.TranslationY = labelY;
    }
}