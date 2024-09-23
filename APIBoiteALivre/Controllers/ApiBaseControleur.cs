using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;


namespace APIBoiteALivre.Controllers
{
    public abstract class ApiBaseControleur : ControllerBase
    {
        protected async Task<BadRequestObjectResult> ValiderRequete<R, V>(R requete, V validator)where V : AbstractValidator<R>
        {
            ValidationResult validationResult = await validator.ValidateAsync(requete);
            if(!validationResult.IsValid)
            {
                ValidationProblemDetails problemDetails = new(validationResult.ToDictionary())
                {
                    Type = "https://tolls.ietf.org/html/rfc72318section-6.5.1"
                };

                BadRequest(problemDetails);
            }

            return null;
        }
    }
}
