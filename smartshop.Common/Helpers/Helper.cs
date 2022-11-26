namespace smartshop.Common.Helpers
{
    public static class Helper
    {
        public static string GetOtp(int otpLength)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            string otp = string.Empty;

            string tempCharacter;

            Random rand = new Random();

            for (int i = 0; i < otpLength; i++)

            {
                _ = rand.Next(0, saAllowedCharacters.Length);

                tempCharacter = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                otp += tempCharacter;
            }

            return otp;
        }

        public static string FilterNumber(string number)
        {
            number = number.Replace("+88", "");
            number = number.Replace("+8", "");
            number = number.Replace(" ", "");
            number = number.Replace("-", "");

            if (number.Substring(0, 2) == "88")
                number.Substring(2);

            return number;
        }
    }
}