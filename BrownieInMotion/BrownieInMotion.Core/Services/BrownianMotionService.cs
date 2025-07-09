using System;

namespace BrownieInMotion.Core.Services;

public class BrownianMotionService
{
    /// <summary>
    /// Gera uma simula��o de movimento browniano geom�trico (Geometric Brownian Motion).
    /// </summary>
    /// <param name="sigma">Volatilidade do ativo (ex: 0.2 para 20%).</param>
    /// <param name="mean">M�dia do retorno di�rio (ex: 0.0002 para 0.02%).</param>
    /// <param name="initialPrice">Pre�o inicial do ativo.</param>
    /// <param name="numDays">N�mero de dias (ou passos) da simula��o.</param>
    /// <returns>Array de pre�os simulados ao longo do tempo.</returns>
    public static double[] GenerateBrownianMotion(
        double sigma,  // Volatilidade di�ria
        double mean,   // Retorno m�dio di�rio
        double initialPrice,
        int numDays)
    {
        Random rand = new();
        double[] prices = new double[numDays];
        prices[0] = initialPrice;

        for (int i = 1; i < numDays; i++)
        {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);

            double retornoDiario = mean + sigma * z;

            prices[i] = prices[i - 1] * Math.Exp(retornoDiario);
        }

        return prices;
    }

    public static double[] GenerateAnnualBrownianMotion(
    double sigma,    // Volatilidade anual
    double mean,     // Retorno m�dio anual
    double initialPrice,
    int years,
    int stepsPerYear = 252)
    {
        Random rand = new();
        int totalSteps = years * stepsPerYear;
        double[] prices = new double[totalSteps];
        prices[0] = initialPrice;

        double dt = 1.0 / stepsPerYear;

        for (int i = 1; i < totalSteps; i++)
        {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);

            double drift = (mean - 0.5 * sigma * sigma) * dt;
            double diffusion = sigma * Math.Sqrt(dt) * z;

            prices[i] = prices[i - 1] * Math.Exp(drift + diffusion);
        }

        return prices;
    }
}