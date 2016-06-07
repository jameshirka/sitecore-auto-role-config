namespace ConfigRenamer
{
    internal static class StringExtensions
    {
        #region Public Methods and Operators

        public static string TrimEnd(this string input, string suffixToRemove)
        {
            if (input != null && suffixToRemove != null && input.EndsWith(suffixToRemove))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }
            else
            {
                return input;
            }
        }

        public static string TrimStart(this string input, string preffixToRemove)
        {
            if (input != null && preffixToRemove != null && input.StartsWith(preffixToRemove))
            {
                return input.Substring(preffixToRemove.Length); 
            }
            else
            {
                return input;
            }
        }

        #endregion
    }
}