using System.Globalization;

namespace BrownieInMotion.Pages;

public class BrownianChartDrawable : IDrawable
{
    private readonly double[] _prices;
    private readonly IFont _fontInstance; 

    public BrownianChartDrawable(double[] prices, IFont? fontInstance = null) 
    {
        _prices = prices;
        _fontInstance = fontInstance ?? Microsoft.Maui.Graphics.Font.Default; 
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (_prices == null || _prices.Length < 2)
            return;

        canvas.Antialias = true;

        double min = _prices.Min();
        double max = _prices.Max();

        float width = dirtyRect.Width;
        float height = dirtyRect.Height;

        // Calcular a maior largura de label Y dinamicamente
        string maxLabel = Math.Max(Math.Abs(min), Math.Abs(max)).ToString("F2", CultureInfo.InvariantCulture);
        float labelWidth = canvas.GetStringSize(maxLabel, _fontInstance, 12).Width;   
        float margin = Math.Max(60f, labelWidth + 24f);

        float plotWidth = width - 2 * margin;
        float plotHeight = height - 2 * margin;

        float xStep = plotWidth / (_prices.Length - 1);
        double yScale = (max - min) == 0 ? 1 : (max - min);

        // Gridlines horizontais e verticais
        int yTicks = 5;
        int xTicks = Math.Min(10, _prices.Length - 1);

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
            int idx = (int)Math.Round(i * (_prices.Length - 1) / (double)xTicks);

            // Tick
            canvas.StrokeColor = Colors.Gray;
            canvas.DrawLine(x, margin + plotHeight, x, margin + plotHeight + 5);

            // Label
            canvas.FontColor = Colors.Black;
            canvas.FontSize = 12;
            canvas.DrawString(
                idx.ToString(),
                x - 12,
                margin + plotHeight + 6,
                24,
                16,
                HorizontalAlignment.Center,
                VerticalAlignment.Top);
        }

        // Linha do gráfico
        canvas.StrokeColor = Colors.MediumPurple;
        canvas.StrokeSize = 2.5f;
        for (int i = 1; i < _prices.Length; i++)
        {
            float x1 = margin + (i - 1) * xStep;
            float y1 = margin + plotHeight - (float)((_prices[i - 1] - min) / yScale * plotHeight);
            float x2 = margin + i * xStep;
            float y2 = margin + plotHeight - (float)((_prices[i] - min) / yScale * plotHeight);

            canvas.DrawLine(x1, y1, x2, y2);
        }

        // Destaque para máximo e mínimo
        int maxIdx = Array.IndexOf(_prices, max);
        int minIdx = Array.IndexOf(_prices, min);
        float xMax = margin + maxIdx * xStep;
        float yMax = margin + plotHeight - (float)((max - min) / yScale * plotHeight);
        float xMin = margin + minIdx * xStep;
        float yMin = margin + plotHeight - (float)((min - min) / yScale * plotHeight);

        canvas.FillColor = Colors.Red;
        canvas.FillCircle(xMax, yMax, 5);
        canvas.FillColor = Colors.Blue;
        canvas.FillCircle(xMin, yMin, 5);
    }
}