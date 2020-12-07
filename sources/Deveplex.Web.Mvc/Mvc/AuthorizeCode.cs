using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Deveplex.Web.Mvc
{
    public enum AuthorizeCode
    {
        Authorized = 200,

        Unauthorized = HttpStatusCode.Unauthorized,

        Forbidden = HttpStatusCode.Forbidden,
    }
}
