using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Diagnostics.CodeAnalysis;

namespace Deveplex.Web.Mvc
{
    [SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Justification = "Unsealed so that subclassed types can set properties in the default constructor or override our behavior.")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly object _typeId = new object();

        private string _roles;
        private string[] _rolesSplit = new string[0];
        private string _users;
        private string[] _usersSplit = new string[0];

        public string Roles
        {
            get { return _roles ?? String.Empty; }
            set
            {
                _roles = value;
                _rolesSplit = SplitString(value);
            }
        }

        public override object TypeId
        {
            get { return _typeId; }
        }

        public string Users
        {
            get { return _users ?? String.Empty; }
            set
            {
                _users = value;
                _usersSplit = SplitString(value);
            }
        }

        public AuthorizationAttribute()
        {
        }

        protected virtual AuthorizeCode AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            IPrincipal user = httpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                return AuthorizeCode.Unauthorized;
            }

            if (_usersSplit.Length > 0 && !_usersSplit.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
            {
                return AuthorizeCode.Forbidden;
            }

            if (_rolesSplit.Length > 0 && !_rolesSplit.Any(user.IsInRole))
            {
                return AuthorizeCode.Forbidden;
            }

            return AuthorizeCode.Authorized;
        }

        private void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        public virtual void OnAuthorization(AuthorizationContext filterContext)
        {
            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                throw new InvalidOperationException("Cannot Use With in Child Action Cache");
                //throw new InvalidOperationException(MvcResources.AuthorizeAttribute_CannotUseWithinChildActionCache);
            }

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                     || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization)
            {
                return;
            }

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;

            List<string> Roles = GetRoles(actionName, controllerName);
            if (Roles != null && Roles.Count() > 0)
            {
                _rolesSplit = Roles.Union(_rolesSplit).ToArray();
                //if (!string.IsNullOrWhiteSpace(this.Roles))
                //{
                //    List<string> roles = this.Roles.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //    this.Roles = string.Join(",", Roles.Union(roles).ToArray());
                //}
                //else
                //{
                //    this.Roles = string.Join(",", Roles.ToArray());
                //}
            }

            List<string> Users = GetUsers(actionName, controllerName);
            if (Users != null && Users.Count() > 0)
            {
                _usersSplit = Users.Union(_usersSplit).ToArray();
                //if (!string.IsNullOrWhiteSpace(this.Users))
                //{
                //    List<string> users = this.Users.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //    this.Users = string.Join(",", Users.Union(users).ToArray());
                //}
                //else
                //{
                //    this.Users = string.Join(",", Users.ToArray());
                //}
            }

            switch (AuthorizeCore(filterContext.HttpContext))//根据验证判断进行处理
            {
                case AuthorizeCode.Authorized:
                    HandleAuthorizedRequest(filterContext);
                    break;
                case AuthorizeCode.Unauthorized:
                    HandleUnauthorizedRequest(filterContext);
                    break;
                case AuthorizeCode.Forbidden:
                    HandleForbiddenRequest(filterContext);
                    break;
            }
        }

        protected virtual void HandleAuthorizedRequest(AuthorizationContext filterContext)
        {
            HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
            cachePolicy.SetProxyMaxAge(new TimeSpan(0));
            cachePolicy.AddValidationCallback(CacheValidateHandler, null /* data */);
        }

        protected virtual void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Returns HTTP 401 - see comment in HttpUnauthorizedResult.cs.
            filterContext.Result = new HttpUnauthorizedResult();
        }

        protected virtual void HandleForbiddenRequest(AuthorizationContext filterContext)
        {
            // Returns HTTP 403 - see comment in HttpForbiddenResult.cs.
            filterContext.Result = new HttpForbiddenResult();
        }

        // This method must be thread-safe since it is called by the caching module.
        protected virtual HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            bool isAuthorized = AuthorizeCore(httpContext) == AuthorizeCode.Authorized;
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

        protected virtual List<string> GetRoles(string actionName, string controllerName)
        {
            return new List<string>();
        }

        protected virtual List<string> GetUsers(string actionName, string controllerName)
        {
            return new List<string>();
        }
    }
}