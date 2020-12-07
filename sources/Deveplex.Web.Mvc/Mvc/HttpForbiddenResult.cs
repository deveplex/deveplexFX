using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;

namespace Deveplex.Web.Mvc
{
    class HttpForbiddenResult: HttpStatusCodeResult
    {
        public HttpForbiddenResult()
            : this(null)
        {
        }

        // Forbidden is equivalent to HTTP status 403, the status code for Forbidden
        // access. Other code might intercept this and perform some special logic. For
        // example, the FormsAuthenticationModule looks for 403 responses and instead
        // redirects the user to the login page.
        public HttpForbiddenResult(string statusDescription)
            : base(HttpStatusCode.Forbidden, statusDescription)
        {
        }
    }
}
