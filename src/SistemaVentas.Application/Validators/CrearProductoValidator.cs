using FluentValidation;
using SistemaVentas.Application.DTOs;

namespace SistemaVentas.Application.Validators;

public class CrearProductoValidator
    : AbstractValidator<CrearProductoDto>
{
    public CrearProductoValidator()
    {
        
        RuleFor(x => x.Codigo)
            .NotEmpty()
            .WithMessage("El código es obligatorio.")
            .MaximumLength(20);
        
        RuleFor(x => x.Nombre)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio.")
            .MaximumLength(100);

        RuleFor(x => x.Descripcion)
            .MaximumLength(500);

        RuleFor(x => x.Precio)
            .GreaterThan(0)
            .WithMessage("El precio debe ser mayor que cero.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("El stock no puede ser negativo.");
    }
}