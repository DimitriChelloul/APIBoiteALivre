using Domain.Entites;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace APIBoiteALivre.Filtre
{
    public class ApiExceptionsFiltre : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionsFiltre()
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                {typeof(NotFoundEntityException), HandleNotFoundException },
                {typeof(SuppressionUtilisateurImpossible), GererSuppressionUtilisateurImpossible },
                {typeof(UtilisateurExistantException), GererUtilisateurExistantException }

            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);
            base.OnException(context);
        }
        /// <summary>
        /// Gere la bonne exception dans le context
        /// </summary>
        /// <param name="context">Context de l exception</param>
        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();

            Console.WriteLine($"Exception captée : {type.Name}");

            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }
        /// <summary>
        /// Gere une exception de type "Inconnu"
        /// </summary>
        /// <param name="context">Context de l exception</param>
        private void HandleUnknownException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Une erreur s'est produite durant le traitement de votre requete.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            context.ExceptionHandled = true;
        }
        /// <summary>
        /// Gere une exception de type "Acces interdit"
        /// </summary>
        /// <param name="context">Context de l exception</param>
        private void HandleForbiddenAccessException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };

            context.ExceptionHandled = true;

        }



        /// <summary>
        /// Gere une exception de type "acces non autorisé"
        /// </summary>
        /// <param name="context">context de l exception</param>
        private void HandleUnauthorizedAccessException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            context.ExceptionHandled = true;
        }



        /// <summary>
        /// Gere une exception de type "Introuvable"
        /// </summary>
        /// <param name="context">Context de l exception</param>
        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundEntityException;

            var details = new ProblemDetails()
            {
                Detail = exception?.Message,
                Title = "La ressource n'a pas été trouvée.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            };

            context.Result = new NotFoundObjectResult(details);
           

            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };
            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        /// <summary>
        /// Gere une exception de type "Suppression d utilisateur impossible"
        /// </summary>
        /// <param name="context">Context de l exception</param>
        private void GererSuppressionUtilisateurImpossible(ExceptionContext context)
        {
            var exception = context.Exception as SuppressionUtilisateurImpossible;

            var details = new ProblemDetails()
            {
                Detail = exception?.Message,
                Title = "L'utilisateur selectionné ne peut etre supprimer car il possede des livres.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            
            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status400BadRequest,
            };

            context.ExceptionHandled = true;

        }

        private void GererUtilisateurExistantException(ExceptionContext context)
        {
            var exception = context.Exception as UtilisateurExistantException;

            var details = new ProblemDetails()
            {
                Detail = exception?.Message,
                Title = "L'utilisateur que vous voulez ajouter existe déjà.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };


            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status400BadRequest,
            };

            context.ExceptionHandled = true;

        }

    }
}
