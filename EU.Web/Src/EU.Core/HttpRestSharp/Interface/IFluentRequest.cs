using EU.Core.HttpRestSharp.Enum;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace EU.Core.HttpRestSharp.Interface
{
    public interface IFluentRequest
    {
		RestClient RestClient { get; }
		RestRequest RestRequest { get;}
        IFluentRequest Get();
        IFluentRequest Post();
        IFluentRequest Put();
        IFluentRequest Delete();
        IFluentRequest Patch();
        IFluentRequest AddHeader(string key, string value);
        IFluentRequest AddQueryParameter(string key, string value, bool encode = true);
        IFluentRequest AddParameter(string key, string value, bool encode = true);
        IFluentRequest AddBodyData<T>(T bodyData, BodyType type= BodyType.Json) where T :class;
        IFluentRequest AddStringBody(string body, DataFormat dateFormat);
        IFluentRequest AddFile(string name,string filePath,string contentType=null);
        IFluentRequest AddFile(string name,byte[] file,string fileName,string contentType=null);
        IFluentResult BuildRequest();

    }
}
