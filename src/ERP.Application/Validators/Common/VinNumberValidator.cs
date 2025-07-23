using FluentValidation;

namespace ERP.Application.Validators.Common
{
    public class VinNumberValidator : AbstractValidator<string>
    {
        public VinNumberValidator()
        {
            RuleFor(vinNumber => vinNumber)
                .NotEmpty().WithMessage("VIN numarası boş olamaz")
                .Length(17).WithMessage("VIN numarası 17 karakter olmalıdır")
                .Matches(@"^[A-HJ-NPR-Z0-9]{17}$")
                .WithMessage("VIN numarası sadece rakam ve harf içerebilir (I, O, Q hariç)")
                .Must(BeValidVinChecksum).WithMessage("VIN numarası geçersiz kontrol toplamına sahip");
        }

        private bool BeValidVinChecksum(string vin)
        {
            if (string.IsNullOrEmpty(vin) || vin.Length != 17)
                return false;

            // VIN check digit validation algorithm
            var weights = new int[] { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };
            var values = new Dictionary<char, int>
            {
                {'0', 0}, {'1', 1}, {'2', 2}, {'3', 3}, {'4', 4}, {'5', 5}, {'6', 6}, {'7', 7}, {'8', 8}, {'9', 9},
                {'A', 1}, {'B', 2}, {'C', 3}, {'D', 4}, {'E', 5}, {'F', 6}, {'G', 7}, {'H', 8}, {'J', 1}, {'K', 2},
                {'L', 3}, {'M', 4}, {'N', 5}, {'P', 7}, {'R', 9}, {'S', 2}, {'T', 3}, {'U', 4}, {'V', 5}, {'W', 6},
                {'X', 7}, {'Y', 8}, {'Z', 9}
            };

            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                if (i == 8) continue; // Skip check digit position
                if (!values.ContainsKey(vin[i])) return false;
                sum += values[vin[i]] * weights[i];
            }

            int checkDigit = sum % 11;
            char expectedCheckChar = checkDigit == 10 ? 'X' : checkDigit.ToString()[0];

            return vin[8] == expectedCheckChar;
        }
    }
}