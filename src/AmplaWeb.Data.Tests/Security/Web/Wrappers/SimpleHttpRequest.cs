﻿using System;
using System.Collections.Specialized;
using System.Web;
using AmplaWeb.Security.Web.Interfaces;

namespace AmplaWeb.Security.Web.Wrappers
{
    public class SimpleHttpRequest : IHttpRequestWrapper
    {
        public SimpleHttpRequest(string requestUrl, HttpCookieCollection cookies)
        {
            Url = new Uri(requestUrl);
            QueryString = HttpUtility.ParseQueryString(Url.Query);
            Cookies = cookies ?? new HttpCookieCollection();
        }

        public NameValueCollection QueryString { get; private set; }
        public Uri Url { get; private set; }
        public HttpCookieCollection Cookies { get; private set; }
    }
}