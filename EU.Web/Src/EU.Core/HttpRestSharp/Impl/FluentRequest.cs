using EU.Core.HttpRestSharp.Enum;using EU.Core.HttpRestSharp.Interface;using RestSharp;using System;using System.Collections.Generic;using System.Text;namespace EU.Core.HttpRestSharp.Impl{    public class FluentRequest : IFluentRequest    {        public RestRequest RestRequest => restRequest;        public RestClient RestClient=> restClient;		private RestRequest restRequest;        private RestClient restClient;        public FluentRequest(RestClient restClient)        {            this.restRequest = new RestRequest();            this.restClient = restClient;        }        public IFluentResult BuildRequest()        {            return new FluentResult(restClient, restRequest);        }        public IFluentRequest Delete()        {            restRequest.Method = Method.Delete;            return this;        }        public IFluentRequest Get()        {            restRequest.Method = Method.Get;            return this;        }        public IFluentRequest Post()        {            restRequest.Method = Method.Post;            return this;        }        public IFluentRequest Put()        {            restRequest.Method = Method.Put;            return this;        }        public IFluentRequest Patch()        {            restRequest.Method = Method.Patch;            return this;        }        public IFluentRequest AddBodyData<T>(T bodyData, BodyType type = BodyType.Json) where T : class        {            switch (type)            {                case BodyType.Json:                    restRequest.AddJsonBody(bodyData);                    break;                case BodyType.Xml:                    restRequest.AddXmlBody(bodyData);                    break;                default:                    throw new ArgumentException("�����쳣��");            }            return this;        }        public IFluentRequest AddStringBody(string body, DataFormat dataFormat)        {            restRequest.AddStringBody(body, dataFormat);            return this;        }        public IFluentRequest AddFile(string name, string filePath, string contentType = null)        {            restRequest.AddFile(name, filePath, contentType);            return this;        }        public IFluentRequest AddFile(string name, byte[] file, string fileName, string contentType = null)        {            restRequest.AddFile(name, file, fileName, contentType);            return this;        }        public IFluentRequest AddHeader(string key, string value)        {            restRequest.AddOrUpdateHeader(key, value);            return this;        }        public IFluentRequest AddParameter(string key, string value,bool encode = true)        {            restRequest.AddParameter(key, value, encode);            return this;        }        public IFluentRequest AddQueryParameter(string key, string value, bool encode = true)        {            restRequest.AddQueryParameter(key, value);            return this;        }            }}