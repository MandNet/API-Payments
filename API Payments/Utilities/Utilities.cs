namespace API_Payments.Utilities
{
    public static class Utilities
    {
        public static string OnlyNumbers(string value)
        {
            return new string(value.Where(char.IsDigit).ToArray());
        }
    }
}
