﻿/**
 * GWebSearcher.cs
 *
 * Copyright (C) 2008,  iron9light
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Net;

namespace Google.API.Search
{
    /// <summary>
    /// Utility class for Google Web Search service.
    /// </summary>
    public static class GWebSearcher
    {
        private static int s_Timeout = 0;

        /// <summary>
        /// Get or set the length of time, in milliseconds, before the request times out.
        /// </summary>
        public static int Timeout
        {
            get
            {
                return s_Timeout;
            }
            set
            {
                if (s_Timeout < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                s_Timeout = value;
            }
        }

        internal static SearchData<GWebResult> GSearch(string keyword, int start, ResultSizeEnum resultSize, Language language)
        {
            if (keyword == null)
            {
                throw new ArgumentNullException("keyword");
            }

            string languageCode = LanguageUtility.GetLanguageCode(language);

            GWebSearchRequest request = new GWebSearchRequest(keyword, start, resultSize, languageCode);

            WebRequest webRequest;
            if (Timeout != 0)
            {
                webRequest = request.GetWebRequest(Timeout);
            }
            else
            {
                webRequest = request.GetWebRequest();
            }

            SearchData<GWebResult> responseData;
            try
            {
                responseData = RequestUtility.GetResponseData<SearchData<GWebResult>>(webRequest);
            }
            catch (GoogleAPIException ex)
            {
                throw new SearchException(string.Format("request:\"{0}\"", request), ex);
            }
            return responseData;
        }

        /// <summary>
        /// Search.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="resultCount">The count of result itmes.</param>
        /// <returns>The result items.</returns>
        /// <exception cref="SearchException">Search failed.</exception>
        /// <remarks>Now, the max count of items Google given is <b>32</b>.</remarks>
        /// <example>
        /// This is the c# code example.
        /// <code>
        /// IList&lt;IWebResult&gt; results = GWebSearcher.Search("Google API for .NET", 8);
        /// </code>
        /// </example>
        public static IList<IWebResult> Search(string keyword, int resultCount)
        {
            return Search(keyword, resultCount, new Language());
        }

        /// <summary>
        /// Search.
        /// </summary>
        /// <param name="keyword">The keyword.</param>
        /// <param name="resultCount">The count of result itmes.</param>
        /// <param name="language">The language you want to search.</param>
        /// <returns>The result itmes.</returns>
        /// <exception cref="SearchException">Search failed.</exception>
        /// <remarks>Now, the max count of items Google given is <b>32</b>.</remarks>
        /// <example>
        /// This is the c# code example.
        /// <code>
        /// IList&lt;IWebSearchResult&gt; results = GWebSearcher.Search("Google API for .NET", 32, Language.Chinese_Simplified);
        /// </code>
        /// </example>
        public static IList<IWebResult> Search(string keyword, int resultCount, Language language)
        {
            if(keyword == null)
            {
                throw new ArgumentNullException("keyword");
            }

            SearchUtility.GSearchCallback<GWebResult> gsearch = (start, resultSize) => GSearch(keyword, start, resultSize, language);
            List<GWebResult> results = SearchUtility.Search(gsearch, resultCount);
            return results.ConvertAll<IWebResult>(item => (IWebResult)item);
        }
    }
}
