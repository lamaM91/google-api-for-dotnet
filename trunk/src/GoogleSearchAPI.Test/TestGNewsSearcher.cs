﻿/**
 * TestGNewsSearcher.cs
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
using NUnit.Framework;

namespace Google.API.Search.Test
{
    [TestFixture]
    public class TestGNewsSearcher
    {
        [Test]
        public void GSearchTest()
        {
            string keyword = "Olympic";
            int start = 0;
            ResultSizeEnum resultSize = new ResultSizeEnum();
            string geo = "Beijing China";
            SortType sortBy = new SortType();

            SearchData<GNewsResult> results =
                GNewsSearcher.GSearch(keyword, start, resultSize, geo, sortBy);

            Assert.IsNotNull(results);
            Assert.IsNotNull(results.Results);
            Assert.Greater(results.Results.Length, 0);
            foreach (GNewsResult result in results.Results)
            {
                Assert.IsNotNull(result);
                Assert.AreEqual("GnewsSearch", result.GSearchResultClass);
                Console.WriteLine(result);
                Console.WriteLine();
            }
        }

        [Test]
        public void SearchTest()
        {
            string keyword = "NBA";
            int count = 15;
            IList<INewsResult> results = GNewsSearcher.Search(keyword, count);
            Assert.IsNotNull(results);
            Assert.AreEqual(count, results.Count);
        }

        [Test]
        public void SearchTest2()
        {
            string keyword = "earthquake";
            string geo = "China";
            int count = 32;
            IList<INewsResult> results = GNewsSearcher.Search(keyword, count, geo);
            Assert.IsNotNull(results);
            Assert.AreEqual(count, results.Count);
        }

        [Test]
        public void SearchTest3()
        {
            string keyword = "Obama";
            int count = 32;
            IList<INewsResult> resultsByRelevance = GNewsSearcher.Search(keyword, count, SortType.relevance);
            IList<INewsResult> resultsByDate = GNewsSearcher.Search(keyword, count, SortType.date);
            Assert.IsNotNull(resultsByRelevance);
            Assert.IsNotNull(resultsByDate);
            Assert.AreEqual(resultsByRelevance.Count, resultsByDate.Count);
            bool areSame = true;
            for(int i = 0; i < resultsByRelevance.Count;++i)
            {
                if(resultsByRelevance[i].ToString() != resultsByDate[i].ToString())
                {
                    areSame = false;
                    break;
                }
            }
            Assert.IsFalse(areSame);
        }

        [Test]
        public void SearchLocalTest()
        {
            int count = 16;
            IList<INewsResult> results1 = GNewsSearcher.SearchLocal("Tokyo", count);
            IList<INewsResult> results2 = GNewsSearcher.SearchLocal("Japan", count);
            Assert.IsNotNull(results1);
            Assert.IsNotNull(results2);
            Assert.AreEqual(count, results1.Count);
            Assert.AreEqual(count, results2.Count);

            bool areSame = true;
            for(int i = 0; i < results1.Count; ++i)
            {
                if(results1[i].ToString() != results2[i].ToString())
                {
                    areSame = false;
                    break;
                }
            }
            Assert.IsFalse(areSame);
        }
    }
}
