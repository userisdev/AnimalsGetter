﻿using System.Net.Http;

namespace AnimalsGetter
{
    /// <summary> HttpClientFactory class. </summary>
    internal static class HttpClientFactory
    {
        /// <summary> The HTTP client </summary>
        private static readonly HttpClient httpClient = new HttpClient();

        /// <summary> Creates this instance. </summary>
        /// <returns> </returns>
        public static HttpClient Create()
        {
            return httpClient;
        }
    }
}
