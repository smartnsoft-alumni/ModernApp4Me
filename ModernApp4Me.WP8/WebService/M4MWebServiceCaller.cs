﻿using System.Net;
using System.Threading.Tasks;
using Microsoft.Phone.Net.NetworkInformation;
using ModernApp4Me.Core.App;
using ModernApp4Me.Core.Log;
using RestSharp;
using ModernApp4Me.Core.LifeCycle;

namespace ModernApp4Me.WP8.WebService
{

    /// <summary>
    /// A basis class for making web service calls easier which uses RestSharp.
    /// When invoking an HTTP method, the caller goes through the following workflow:
    /// The <see cref="M4MWebServiceCaller.ExecuteHttpRequest()"/> methods is invoked : if the response <see cref="HttpStatusCode"/> is OK, the body is returned ad a <see cref="string"/>.
    /// if the <see cref="HttpStatusCode"/> is not OK, the private method <see cref="M4MWebServiceCaller.OnHttpStatusCodeNotOk()"/> is invoked.
    /// if the <see cref="HttpStatusCode"/> is NotFound and the <see cref="DeviceNetworkInformation.IsNetworkAvailable"/> is false, a <see cref="M4MConnectivityException"/> is thrown.
    /// Otherwife, a <see cref="M4MCallException"/> is thrown.
    /// </summary>
    /// 
    /// <author>Ludovic ROLAND</author>
    /// <since>2014.03.21</since>
    // TODO : Add some logs
    public abstract class M4MWebServiceCaller
    {

        public RestClient Client { get; private set; }

        /// <summary>
        /// Basis constructor.
        /// </summary>
        /// <param name="baseUrl">The base URL of the web service API</param>
        protected M4MWebServiceCaller(string baseUrl)
        {
            Client = new RestClient(baseUrl);
        }

        /// <summary>
        /// Executes a call to a web method without any persistence.
        /// </summary>
        /// <param name="restRequest"></param>
        /// <returns>The content of the </returns>
        protected async Task<string> ExecuteHttpRequest(RestRequest restRequest)
        {
            var rawResponse = await Client.ExecuteTaskAsync(restRequest);

            if (rawResponse.StatusCode != HttpStatusCode.OK)
            {
                OnHttpStatusCodeNotOk(rawResponse);
            }

            return rawResponse.Content;
        }

        /// <summary>
        /// Private function that raises an exception when the result code to a web method id not OK (not 20X).
        /// </summary>
        /// <param name="rawResponse"></param>
        protected void OnHttpStatusCodeNotOk(IRestResponse rawResponse)
        {
            var message = "The error code of the call to the web method '" + rawResponse.Request.Resource +
                          "' is not OK (not 20X). Status: '" + rawResponse.StatusCode + "'";

            if (rawResponse.StatusCode == HttpStatusCode.NotFound && DeviceNetworkInformation.IsNetworkAvailable == false)
            {
                throw new M4MConnectivityException(message);
            }

            throw new M4MCallException(message);
        }

    }

}