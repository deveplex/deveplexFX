namespace Deveplex.TeamFoundation.Build.Activities
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Text;

    public static class StringExtension
    {
        public static string Append(this string input, string append)
        {
            if (string.IsNullOrEmpty(append))
            {
                return (input ?? string.Empty);
            }
            StringBuilder builder = new StringBuilder(input ?? string.Empty);
            builder.Append(append);
            return builder.ToString();
        }
    }
}

