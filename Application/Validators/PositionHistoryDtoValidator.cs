using Application.Dtos;
using FluentValidation;
using System.Globalization;

namespace Application.Validators
{
    /// <summary>
    /// Se valida el formato de las fechas que ingresa el usuario para almacenar un registro en PositionHistories. Se utiliza la librería FluentValidation para la validación
    /// </summary>
    public class PositionHistoryDtoValidator: AbstractValidator<PositionHistoryDto>
    {
        public PositionHistoryDtoValidator()
        {
            RuleFor(x => x.StartDate)
                .NotEmpty().Must(ValidDate)
                .WithMessage("El formato de fecha para startDate es yyyyMMdd");

            RuleFor(x => x.EndDate)
                .Must(ValidDate)
                .When(x => !string.IsNullOrWhiteSpace(x.EndDate))
                .WithMessage("El formato de fecha para endDate es yyyyMMdd");
        }

        private bool ValidDate(string date)
        {
            return DateTime.TryParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
