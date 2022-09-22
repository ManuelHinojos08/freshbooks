using Application.Common.Exceptions;
using Application.Common.Extensions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;
using System.Net;

namespace Infrastructure.Filters;

public sealed class BaseExceptionFilter : ExceptionFilterAttribute, IExceptionFilter, IActionFilter
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
    private readonly IModelMetadataProvider _modelMetadataProvider;
    private const string _ActionArgsKey = "ActionArguments";

    public BaseExceptionFilter(IModelMetadataProvider modelMetadataProvider)
    {
        _modelMetadataProvider = modelMetadataProvider;
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ForbiddenContentException), HandleExceptionWithRedirect },
                { typeof(NotFoundException), HandleExceptionWithRedirect },
                { typeof(ValidationException), HandleValidationException }
            };
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        context.HttpContext.Items[_ActionArgsKey] = context.ActionArguments;
    }

    #region dummy methods to comply with the action/page filter interfaces
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
    #endregion
    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }
        HandleUnknownException(context);
    }


    /// <summary>
    /// This method redirects user to a custom error page, like 404 - Not found, 
    /// 403 - Forbid, and so on.
    /// </summary>
    /// <param name="context"></param>
    private void HandleExceptionWithRedirect(ExceptionContext context)
    {
        string actionName = context.Exception switch
        {
            ForbiddenContentException => "Forbid",
            NotFoundException => "NotFound"
        };
        var routeData = new RouteValueDictionary(new
        {
            controller = context.RouteData.Values["controller"],
            action = actionName
        });
        context.ExceptionHandled = true;
        context.Result = new RedirectToRouteResult(routeData);
        context.Result.ExecuteResultAsync(context);
    }

    /// <summary>
    /// This method handles exception when it's thrown byfluent validation
    /// </summary>
    /// <param name="context"></param>
    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException)context.Exception;
        //clearing model state errors
        foreach (var key in context.ModelState.Keys)
        {
            context.ModelState[key].Errors.Clear();
        }
        //as fluent validation failures is the only error feedback we want
        foreach (var error in exception.Errors)
        {
            context.ModelState.AddModelError(error.Key, string.Join(Environment.NewLine, error.Value));
        }
        context.ExceptionHandled = true;

        var args = (Dictionary<string, object>)context.HttpContext.Items[_ActionArgsKey];
        var viewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
        if (args != null && args.Count > 0)
        {
            viewData.Model = args.FirstOrDefault().Value;
        }
        context.Result = new ViewResult { ViewData = viewData };
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    }

    /// <summary>
    /// Method call when an unknow exception (not thrown on porpuse by programmer) is thrown
    /// </summary>
    /// <param name="context"></param>
    private void HandleUnknownException(ExceptionContext context)
    {
        Exception exception = context.Exception.Unwrap();
        string message = exception.Message ??
            $"An exception of type {exception.GetType()} happened.";
        ViewResult result = new() { ViewName = "Error" };
        ErrorViewModel model = new()
        {
            RequestId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier,
            Message = message
        };
        result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState)
        {
            Model = model
        };
        context.Result = result;
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    }
}