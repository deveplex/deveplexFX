using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;

namespace Deveplex.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class MultipleActionAttribute : ActionNameSelectorAttribute
    {
        private string _Name = "Action";

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Argument;

        public string Argument
        {
            get { return _Argument; }
            set { _Argument = value; }
        }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, System.Reflection.MethodInfo methodInfo)
        {
            var isValidName = false;
            string[] argument = Argument.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string arg in argument)
            {
                var key = string.Format("{0}:{1}", Name, arg);
                if (isValidName = controllerContext.HttpContext.Request.Form.AllKeys.Contains(key, StringComparer.OrdinalIgnoreCase))
                {
                    controllerContext.Controller.ControllerContext.RouteData.Values["id"] = arg;
                    break;
                }
            }
            return isValidName;
        }
    }
}
