namespace HotelProject.HelperClass
{
	public class Helper
	{
		public static string NumberToWordsIndian(decimal amount)
		{
			long rupees = (long)Math.Floor(amount);
			int paise = (int)Math.Round((amount - rupees) * 100);

			string words = ConvertToWords(rupees);

			if (paise > 0)
				words += " and " + ConvertToWords(paise) + " Paise";

			return "Rupees " + words + " Only";
		}

		private static string ConvertToWords(long number)
		{
			if (number == 0)
				return "Zero";

			string[] ones =
			{
		"", "One", "Two", "Three", "Four", "Five", "Six",
		"Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve",
		"Thirteen", "Fourteen", "Fifteen", "Sixteen",
		"Seventeen", "Eighteen", "Nineteen"
	};

			string[] tens =
			{
		"", "", "Twenty", "Thirty", "Forty", "Fifty",
		"Sixty", "Seventy", "Eighty", "Ninety"
	};

			string result = "";

			if (number >= 10000000)
			{
				result += ConvertToWords(number / 10000000) + " Crore ";
				number %= 10000000;
			}

			if (number >= 100000)
			{
				result += ConvertToWords(number / 100000) + " Lakh ";
				number %= 100000;
			}

			if (number >= 1000)
			{
				result += ConvertToWords(number / 1000) + " Thousand ";
				number %= 1000;
			}

			if (number >= 100)
			{
				result += ConvertToWords(number / 100) + " Hundred ";
				number %= 100;
			}

			if (number > 0)
			{
				if (result != "")
					result += "and ";

				if (number < 20)
					result += ones[number];
				else
				{
					result += tens[number / 10];
					if (number % 10 > 0)
						result += " " + ones[number % 10];
				}
			}

			return result.Trim();
		}
	}
}
