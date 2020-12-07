using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

namespace Deveplex.Web.Mvc
{
    public class ActionPermissionFilterAttribute// : FilterAttribute, IResultFilter
    {
        //private string _permissions;
        //private string[] _permissionsSplit = new string[0];

        //public string Permissions
        //{
        //    get { return _permissions ?? String.Empty; }
        //    set
        //    {
        //        _permissions = value;
        //        _permissionsSplit = SplitString(value);
        //    }
        //}

        //private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        //{
        //    validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        //}

        //public void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    if (filterContext == null)
        //    {
        //        throw new ArgumentNullException("filterContext");
        //    }

        //    if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
        //    {
        //        throw new InvalidOperationException("Cannot Use With in Child Action Cache");
        //        //    throw new InvalidOperationException(MvcResources.AuthorizeAttribute_CannotUseWithinChildActionCache);
        //    }

        //    bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
        //                             || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

        //    if (skipAuthorization)
        //    {
        //        return;
        //    }

        //    string controllerName = filterContext.RouteData.Values["controller"].ToString();
        //    string actionName = filterContext.RouteData.Values["action"].ToString();

        //    List<string> Permissions = GetPermissions(actionName, controllerName);
        //    if (Permissions != null && Permissions.Count() > 0)
        //    {
        //        _permissionsSplit = Permissions.Union(_permissionsSplit).ToArray();
        //        //if (!string.IsNullOrWhiteSpace(this.Permissions))
        //        //{
        //        //    List<string> users = this.Permissions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //        //    this.Permissions = string.Join(",", Permissions.Union(users).ToArray());
        //        //}
        //        //else
        //        //{
        //        //    this.Permissions = string.Join(",", Permissions.ToArray());
        //        //}
        //    }

        //    if (this.AuthorizeCore(filterContext.HttpContext))//根据验证判断进行处理
        //    {
        //        HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
        //        cachePolicy.SetProxyMaxAge(new TimeSpan(0));
        //        cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
        //    }
        //    else
        //    {
        //        HttpUnauthorizedResult(filterContext);
        //    }
        //}

        ////权限判断业务逻辑
        //protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    if (httpContext == null)
        //    {
        //        throw new ArgumentNullException("httpContext");
        //    }

        //    IPrincipal user = httpContext.User;
        //    if (!user.Identity.IsAuthenticated)
        //    {
        //        return false;//判定用户是否登录
        //    }

        //    if (_permissionsSplit.Length > 0 && !_permissionsSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //protected virtual void HttpUnauthorizedResult(ResultExecutingContext filterContext)
        //{
        //    // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
        //    filterContext.Result = new HttpUnauthorizedResult();
        //}

        //// This method must be thread-safe since it is called by the caching module.
        //protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        //{
        //    if (httpContext == null)
        //    {
        //        throw new ArgumentNullException("httpContext");
        //    }

        //    bool isAuthorized = AuthorizeCore(httpContext);
        //    return (isAuthorized) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        //}

        //private string[] SplitString(string original)
        //{
        //    if (String.IsNullOrEmpty(original))
        //    {
        //        return new string[0];
        //    }

        //    var split = from piece in original.Split(',')
        //                let trimmed = piece.Trim()
        //                where !String.IsNullOrEmpty(trimmed)
        //                select trimmed;
        //    return split.ToArray();
        //}

        //protected virtual List<string> GetPermissions(string actionName, string controllerName)
        //{
        //    return new List<string>();
        //}
    }
}