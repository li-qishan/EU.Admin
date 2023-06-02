using EU.Core.HttpRestSharp.Interface;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EU.Core.HttpRestSharp.Impl
{
    public class FluentClient : IFluentClient
    {
        public RestClient RestClient => restClient;
        private RestClient restClient;
        private List<(string name, string value, string path, string domain)> cookieList = new List<(string name, string value, string path, string domain)>();
        private List<(string name, string value)> defaultHeaderList = new List<(string name, string value)>();
        private List<(string name, string value)> defaultParameterList = new List<(string name, string value)>();
        private List<(string name, string value)> defaultQueryParameter = new List<(string name, string value)>();
        private List<(string name, string value)> defaultUrlSegment = new List<(string name, string value)>();
        private IAuthenticator authenticator;

        public IFluentClient AddCookie(string name, string value, string path, string domain)
        {
            cookieList.Add((name, value, path, domain));
            return this;
        }

        public IFluentClient AddDefaultHeader(string name, string value)
        {
            defaultHeaderList.Add((name, value));
            return this;
        }


        public IFluentClient AddDefaultParameter(string name, string value)
        {
            defaultParameterList.Add((name, value));
            return this;
        }


        public IFluentClient AddDefaultQueryParameter(string name, string value)
        {
            defaultQueryParameter.Add((name, value));
            return this;
        }


        public IFluentClient AddDefaultUrlSegment(string name, string value)
        {
            defaultUrlSegment.Add((name, value));
            return this;
        }

        public IFluentRequest BuildClient(string baseUrl)
        {
            this.restClient = new RestClient(baseUrl);
            return AddClientParamters();

        }
        public IFluentRequest BuildClient(Uri baseUrl)
        {
            restClient = new RestClient(baseUrl);
            return AddClientParamters();
        }

        public IFluentRequest BuildClient(RestClientOptions options, ConfigureHeaders configureDefaultHeaders = null)
        {
            restClient = new RestClient(options, configureDefaultHeaders);
            return AddClientParamters();
        }

        public IFluentRequest BuildClient(HttpClient httpClient, bool disposeHttpClient = false)
        {
            restClient = new RestClient(httpClient, disposeHttpClient);
            return AddClientParamters();
        }

        public IFluentRequest BuildClient(HttpClient httpClient, RestClientOptions options, bool disposeHttpClient = false)
        {
            restClient = new RestClient(httpClient, disposeHttpClient);
            return AddClientParamters();
        }

        public IFluentRequest BuildClient(HttpMessageHandler handler, bool disposeHandler = true)
        {
            restClient = new RestClient(handler, disposeHandler);
            return AddClientParamters();
        }
        private FluentRequest AddClientParamters()
        {
            if (restClient == null)
            {
                throw new ArgumentNullException("client is null!");
            }

            foreach (var header in defaultHeaderList)
            {
                restClient.AddDefaultHeader(header.name, header.value);
            }
            foreach (var parameter in defaultParameterList)
            {
                restClient.AddDefaultParameter(parameter.name, parameter.value);
            }
            foreach (var queryParameter in defaultQueryParameter)
            {
                restClient.AddDefaultQueryParameter(queryParameter.name, queryParameter.value);
            }
            foreach (var item in defaultUrlSegment)
            {
                restClient.AddDefaultUrlSegment(item.name, item.value);
            }
            if (authenticator != null)
            {
                restClient.UseAuthenticator(authenticator);
            }
            var request = new FluentRequest(restClient);
            foreach (var cookie in cookieList)
            {
                request.RestClient.Options.CookieContainer.Add(new Cookie
                {
                    Name = cookie.name,
                    Value = cookie.value,
                    Path = cookie.path,
                    Domain = cookie.domain
                });
            }
            return request;
        }
        public IFluentClient UseAuthenticator(IAuthenticator authenticator)
        {
            this.authenticator = authenticator;
            return this;
        }


    }
}
