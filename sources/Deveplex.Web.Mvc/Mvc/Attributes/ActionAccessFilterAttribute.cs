using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

namespace Deveplex.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ActionAccessFilterAttribute : FilterAttribute, IActionFilter
    {
        private string[] _userPermissionsSplit = new string[0];

        private string _permissions;
        private string[] _permissionsSplit = new string[0];

        public string Permissions
        {
            get { return _permissions ?? String.Empty; }
            set
            {
                _permissions = value;
                _permissionsSplit = SplitString(value);
            }
        }

        public ActionAccessFilterAttribute()
        {
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            List<string> UserPermissions = GetCurrentPermissions();
            if (UserPermissions != null && UserPermissions.Count() > 0)
            {
                _userPermissionsSplit = UserPermissions.Union(_userPermissionsSplit).ToArray();
                //if (!string.IsNullOrWhiteSpace(this.Permissions))
                //{
                //    List<string> users = this.Permissions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //    this.Permissions = string.Join(",", Permissions.Union(users).ToArray());
                //}
                //else
                //{
                //    this.Permissions = string.Join(",", Permissions.ToArray());
                //}
            }

            List<string> Permissions = GetPermissions(actionName, controllerName);
            if (Permissions != null && Permissions.Count() > 0)
            {
                _permissionsSplit = Permissions.Union(_permissionsSplit).ToArray();
                //if (!string.IsNullOrWhiteSpace(this.Permissions))
                //{
                //    List<string> users = this.Permissions.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //    this.Permissions = string.Join(",", Permissions.Union(users).ToArray());
                //}
                //else
                //{
                //    this.Permissions = string.Join(",", Permissions.ToArray());
                //}
            }

            if (AuthorizeCore(filterContext.HttpContext))//根据验证判断进行处理
            {
                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
            }
            else
            {
                HandleNopermissionsResult(filterContext);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }

        //权限判断业务逻辑
        protected virtual bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            if (_permissionsSplit.Length > 0 && !_permissionsSplit.Any(HasPermissions))
            {
                return false;
            }

            return true;
        }

        protected virtual void HandleNopermissionsResult(ActionExecutingContext filterContext)
        {
            // Returns HTTP 403 - see comment in HttpNopermissionsResult.cs.
            filterContext.Result = new HttpNopermissionsResult();
        }

        // This method must be thread-safe since it is called by the caching module.
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            bool isAuthorized = AuthorizeCore(httpContext);
            return (isAuthorized) ? HttpValidationStatus.Valid : HttpValidationStatus.IgnoreThisRequest;
        }

        private string[] SplitString(string original)
        {
            if (String.IsNullOrEmpty(original))
            {
                return new string[0];
            }

            var split = from piece in original.Split(',')
                        let trimmed = piece.Trim()
                        where !String.IsNullOrEmpty(trimmed)
                        select trimmed;
            return split.ToArray();
        }

        private bool HasPermissions(string p)
        {
            if (_userPermissionsSplit.Length > 0 && _userPermissionsSplit.Contains(p, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        protected virtual List<string> GetCurrentPermissions()
        {
            return new List<string>();
        }

        protected virtual List<string> GetPermissions(string actionName, string controllerName)
        {
            return new List<string>();
        }
    }
}