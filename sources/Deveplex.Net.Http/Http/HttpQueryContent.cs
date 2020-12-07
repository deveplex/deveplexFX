using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Web;

namespace Deveplex.Net.Http
{
    public class HttpQueryContent : FormUrlEncodedContent
    {
        private IEnumerable<KeyValuePair<string, string>> _QueryString;

        //public QueryContent(NameValueCollection nameValueCollection)
        //{
        //    _QueryString = nameValueCollection;

        //    IEnumerable<KeyValuePair<string, string>> keyValueCollection = new List<KeyValuePair<string, string>>();
        //    foreach (var key in nameValueCollection.AllKeys)
        //    {
        //        _QueryString.Add(key, nameValueCollection[key]);
        //    }
        //}
        public HttpQueryContent(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
            : base(nameValueCollection)
        {
            _QueryString = nameValueCollection;
        }

        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                format = string.Empty;

            string result = string.Empty;

            if (format.ToLower().Equals("url"))
            {
                result = AppendQueryString();
            }
            else if (format.ToLower().Equals("json"))
            {
                result = AppendJsonString();
            }
            else
            {
                result = ToString();
            }

            return result;
            ;
        }

        public static HttpQueryContent From(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException("ArgumentNullException");

            IEnumerable<KeyValuePair<string, string>> queryPairs = ParseQueryString(json);

            return new HttpQueryContent(queryPairs);
        }

        public static HttpQueryContent From(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException("ArgumentNullException");

            IEnumerable<KeyValuePair<string, string>> queryPairs = ParseQueryString(uri);

            return new HttpQueryContent(queryPairs);
        }

        public static HttpQueryContent From(NameValueCollection nameValue)
        {
            if (nameValue == null)
                throw new ArgumentNullException("ArgumentNullException");

            IEnumerable<KeyValuePair<string, string>> queryPairs = nameValue.AllKeys.ToDictionary(k => k, v => nameValue[v]);

            return new HttpQueryContent(queryPairs);
        }

        protected string AppendQueryString()
        {
            StringBuilder queryString = new StringBuilder();

            bool first = true;
            foreach (var keyValue in _QueryString)
            {
                if (first == false)
                    queryString.Append("&");

                queryString.Append(HttpUtility.UrlEncode(keyValue.Key, Encoding.UTF8) + "=" + HttpUtility.UrlEncode(keyValue.Value, Encoding.UTF8));
                first = false;
            }

            return queryString.ToString();
        }

        protected string AppendJsonString()
        {
            //JObject jobject = new JObject();
            //foreach (var kv in _QueryString)
            //{
            //    jobject.Add(new JProperty(kv.Key, kv.Value));
            //}

            //return jobject.ToString(Formatting.None);
            return JsonConvert.SerializeObject(_QueryString);
        }
        protected static IEnumerable<KeyValuePair<string, string>> ParseQueryString(Uri uri)
        {
            NameValueCollection nameValue = HttpUtility.ParseQueryString(uri.Query);
            IEnumerable<KeyValuePair<string, string>> queryPairs = nameValue.AllKeys.ToDictionary(k => k, v => nameValue[v]);

            return queryPairs;
        }

        protected static IEnumerable<KeyValuePair<string, string>> ParseQueryString(string json)
        {
            IEnumerable<KeyValuePair<string, string>> queryPairs = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);

            return queryPairs;
        }
    }
}
