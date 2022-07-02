namespace ResourcePackerGUI.Application.Common.Utilities
{
    internal static class FastMath
    {
        private static readonly double[] RoundLookup = CreateRoundLookup();

        internal static double Round(double value)
        {
            return Math.Floor(value + 0.5);
        }

        internal static double Round(double value, int decimalPlaces)
        {
            var adjustment = RoundLookup[decimalPlaces];
            return Math.Floor((value * adjustment) + 0.5) / adjustment;
        }

        private static double[] CreateRoundLookup()
        {
            var result = new double[15];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = Math.Pow(10, i);
            }

            return result;
        }
    }
}