using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Deveplex.Web
{
    public static class SecurityAuthentication
    {
        public static void SetFormsAuthenticationTicket<T>(string ticketId, T value, TimeSpan expires, bool isPersistent)
        {
            var authTicket = new FormsAuthenticationTicket(2, ticketId, DateTime.Now, DateTime.Now.AddSeconds(expires.TotalSeconds), isPersistent, "", FormsAuthentication.FormsCookiePath);
            string encryTicket = FormsAuthentication.Encrypt(authTicket);

            HttpCookie authCookie;
            if (isPersistent)   //是否在设置的过期时间内一直有效
            {
                authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryTicket)
                {
                    HttpOnly = true,
                    Path = FormsAuthentication.FormsCookiePath,
                    Secure = FormsAuthentication.RequireSSL,
                    Expires = authTicket.Expiration,
                    //Domain = authTicket.CookiePath  //这里设置认证的域名，同域名下包括子域名如aa.cnblogs.com或bb.cnblogs.com都保持相同的登录状态
                };
            }
            else
            {
                authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryTicket)
                {
                    HttpOnly = true,
                    Path = FormsAuthentication.FormsCookiePath,
                    Secure = FormsAuthentication.RequireSSL,
                    //Expires = ticket.Expiration,  //无过期时间的，浏览器关闭后失效
                    //Domain = authTicket.CookiePath  //这里设置认证的域名，同域名下包括子域名如aa.cnblogs.com或bb.cnblogs.com都保持相同的登录状态
                };
            }

            HttpContext.Current.Cache.Insert(encryTicket, value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, expires, System.Web.Caching.CacheItemPriority.NotRemovable, Logout);
            HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public static T GetFormsAuthenticationTicket<T>(string ticketId)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    object value = HttpContext.Current.Cache.Get(cookie.Value);
                    if (value != null)
                    {
                        return (T)value;
                    }
                }
            }
            return default(T);
        }

        private static void Logout(string key, object value, System.Web.Caching.CacheItemRemovedReason reason)
        {
            //var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            //if (key.Trim().Equals(cookie.Value.Trim()))
            //{
            //    FormsAuthentication.SignOut();
            //}
        }
    }
}
