using System.Globalization;
using Microsoft.Maui.Graphics;

namespace BrownieInMotion.Pages;

public enum LineStyle
{
    Solid,
    Dashed,
    Dotted
}

public class BrownianChartDrawable : IDrawable
{
    private readonly List<double[]> _simulations;
    private readonly IFont _fontInstance;
    private readonly LineStyle _lineStyle;
    private readonly double _lineThickness;
    private readonly bool _showExtremes;
    private readonly bool _showGrid;

    public BrownianChartDrawable(
        List<double[]> simulations,
        IFont? fontInstance = null,
        LineStyle lineStyle = LineStyle.Solid,
        double lineThickness = 2.5,
        bool showExtremes = true,
        bool showGrid = true)
    {
        _simulations = simulations;
        _fontInstance = fontInstance ?? Microsoft.Maui.Graphics.Font.Default;
        _lineStyle = lineStyle;
        _lineThickness = lineThickness;
        _showExtremes = showExtremes;
        _showGrid = showGrid;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (_simulations == null || _simulations.Count == 0 || _simulations[0].Length < 2)
            return;

        canvas.Antialias = true;

        double min = _simulations.Min(arr => arr.Min());
        double max = _simulations.Max(arr => arr.Max());

        float width = dirtyRect.Width;
        float height = dirtyRect.Height;

        string maxLabel = Math.Max(Math.Abs(min), Math.Abs(max)).ToString("F2", CultureInfo.InvariantCulture);
        float labelWidth = canvas.GetStringSize(maxLabel, _fontInstance, 12).Width;
        float margin = Math.Max(60f, labelWidth + 24f);

        float plotWidth = width - 2 * margin;
        float plotHeight = height - 2 * margin;

        double yScale = (max - min) == 0 ? 1 : (max - min);

        // Gridlines horizontais e verticais
        int yTicks = 5;
        int xTicks = Math.Min(10, _simulations[0].Length - 1);

        if (_showGrid)
        {
            canvas.StrokeColor = Colors.LightGray;
            canvas.StrokeSize = 1;

            for (int i = 0; i <= yTicks; i++)
            {
                float y = margin + plotHeight - (i * plotHeight / yTicks);
                canvas.DrawLine(margin, y, margin + plotWidth, y);
            }
            for (int i = 0; i <= xTicks; i++)
            {
                float x = margin + (i * plotWidth / xTicks);
                canvas.DrawLine(x, margin, x, margin + plotHeight);
            }
        }

        // Eixos
        canvas.StrokeColor = Colors.Gray;
        canvas.StrokeSize = 1.5f;
        canvas.DrawLine(margin, margin, margin, margin + plotHeight); // Y
        canvas.DrawLine(margin, margin + plotHeight, margin + plotWidth, margin + plotHeight); // X

        // Ticks e labels do eixo Y
        for (int i = 0; i <= yTicks; i++)
        {
            float y = margin + plotHeight - (i * plotHeight / yTicks);
            double value = min + (max - min) * i / yTicks;

            // Tick
            canvas.StrokeColor = Colors.Gray;
            canvas.DrawLine(margin - 5, y, margin, y);

            // Label
            canvas.FontColor = Colors.Black;
            canvas.FontSize = 12;
            canvas.DrawString(
                value.ToString("F2", CultureInfo.InvariantCulture),
                0,
                y - 8,
                margin - 8,
                16,
                HorizontalAlignment.Right,
                VerticalAlignment.Center);
        }

        // Ticks e labels do eixo X
        for (int i = 0; i <= xTicks; i++)
        {
            float x = margin + (i * plotWidth / xTicks);
            int idx = (int)Math.Round(i * (_simulations[0].Length - 1) / (double)xTicks);

            // Tick
            canvas.StrokeColor = Colors.Gray;
            canvas.DrawLine(x, margin + plotHeight, x, margin + plotHeight + 5);

            // Label
            canvas.FontColor = Colors.Black;
            canvas.FontSize = 12;
            canvas.DrawString(
                (idx + 1).ToString(),
                x - 12,
                margin + plotHeight + 6,
                24,
                16,
                HorizontalAlignment.Center,
                VerticalAlignment.Top);
        }

        // Desenhar todas as linhas
        var colors = new[] { Colors.MediumPurple, Colors.Orange, Colors.Green, Colors.Red, Colors.Blue, Colors.Teal, Colors.Brown, Colors.Pink, Colors.Gray, Colors.Black };
        for (int s = 0; s < _simulations.Count; s++)
        {
            var prices = _simulations[s];
            var color = colors[s % colors.Length];
            canvas.StrokeColor = color;
            canvas.StrokeSize = (float)_lineThickness;

            // Estilo de linha
            switch (_lineStyle)
            {
                case LineStyle.Dashed:
                    canvas.StrokeDashPattern = new float[] { 8, 6 };
                    break;
                case LineStyle.Dotted:
                    canvas.StrokeDashPattern = new float[] { 2, 6 };
                    break;
                default:
                    canvas.StrokeDashPattern = null;
                    break;
            }

            float xStep = plotWidth / (prices.Length - 1);

            for (int i = 1; i < prices.Length; i++)
            {
                float x1 = margin + (i - 1) * xStep;
                float y1 = margin + plotHeight - (float)((prices[i - 1] - min) / yScale * plotHeight);
                float x2 = margin + i * xStep;
                float y2 = margin + plotHeight - (float)((prices[i] - min) / yScale * plotHeight);
                canvas.DrawLine(x1, y1, x2, y2);
            }

            // Destaca máximo e mínimo desta simulação
            if (_showExtremes)
            {
                int maxIdx = Array.IndexOf(prices, prices.Max());
                int minIdx = Array.IndexOf(prices, prices.Min());
                float xMax = margin + maxIdx * xStep;
                float yMax = margin + plotHeight - (float)((prices[maxIdx] - min) / yScale * plotHeight);
                float xMin = margin + minIdx * xStep;
                float yMin = margin + plotHeight - (float)((prices[minIdx] - min) / yScale * plotHeight);

                canvas.FillColor = Colors.Red.WithAlpha(0.7f);
                canvas.FillCircle(xMax, yMax, 5);
                canvas.FillColor = Colors.Blue.WithAlpha(0.7f);
                canvas.FillCircle(xMin, yMin, 5);
            }
        }
        // Reset dash pattern after drawing
        canvas.StrokeDashPattern = null;
    }
}