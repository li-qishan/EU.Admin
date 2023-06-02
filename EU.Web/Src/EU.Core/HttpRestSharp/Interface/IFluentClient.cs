using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EU.Core.HttpRestSharp.Interface
{
    public interface IFluentClient
    {
        RestClient RestClient { get; }
        IFluentClient AddDefaultHeader(string name, string value);
        IFluentClient AddCookie(string name, string value,string path,string domain);
        IFluentClient AddDefaultQueryParameter(string name, string value);
        IFluentClient AddDefaultParameter(string name, string value);
        IFluentClient AddDefaultUrlSegment(string name, string value);
        IFluentClient UseAuthenticator(IAuthenticator authenticator);
        IFluentRequest BuildClient(string baseUrl);
		IFluentRequest BuildClient(Uri baseUrl);
		IFluentRequest BuildClient(RestClientOptions options, ConfigureHeaders configureDefaultHeaders = null);
        IFluentRequest BuildClient(HttpClient httpClient, bool disposeHttpClient = false);
        IFluentRequest BuildClient(HttpClient httpClient, RestClientOptions options, bool disposeHttpClient = false);
        IFluentRequest BuildClient(HttpMessageHandler handler, bool disposeHandler = true);
	}
}
