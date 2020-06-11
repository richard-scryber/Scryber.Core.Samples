/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 * 
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Scryber.Caching
{

    /// <summary>
    /// Caching provider that stores it's data in the HttpContext.WebCache
    /// </summary>
    public class PDFWebCacheProvider : IPDFCacheProvider
    {
        private static TimeSpan DefaultCacheDuration = new TimeSpan(1, 0, 0); //1 hour

        #region private HttpContext CurrentContext {get;}
 
        /// <summary>
        /// Gets the current HttpContext associated with the current request
        /// </summary>
        private HttpContext CurrentContext
        {
            get { return HttpContext.Current; }
        }

        #endregion

        #region private Cache WebCache {get;}

        /// <summary>
        /// Gets the web caching instance associated with the current request.
        /// </summary>
        private Cache WebCache
        {
            get
            {
                HttpContext current = this.CurrentContext;
                if (null == current)
                    throw new InvalidOperationException(Errors.NoWebContextAvailable);
                return current.Cache;
            }
        }

        #endregion

        //
        // IPDFCacheProvider implementation
        //

        #region public bool TryRetrieveFromCache(string type, string key, out object data)

        /// <summary>
        /// Tries to retrieve an item with the specified type and key from the cache and returns true if it exists (is not null)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool TryRetrieveFromCache(string type, string key, out object data)
        {
            string fullkey = GetFullKey(type, key);
            Cache c = this.WebCache;
            data = c[fullkey];
            bool hasdata = data != null;
            return hasdata;
        }

        #endregion

        #region public void AddToCache(string type, string key, object data)

        /// <summary>
        /// Adds the item to the cache with a no expiration
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        public void AddToCache(string type, string key, object data)
        {
            this.AddToCache(type, key, data, Cache.NoAbsoluteExpiration);
        }

        #endregion

        #region public void AddToCache(string type, string key, object data, TimeSpan duration)

        /// <summary>
        /// Adds the item to the cache with the specified expriy duration
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="duration"></param>
        public void AddToCache(string type, string key, object data, TimeSpan duration)
        {
            DateTime expires = DateTime.Now.Add(duration);
            
            this.AddToCache(type, key, data, expires);
        }

        #endregion

        #region public void AddToCache(string type, string key, object data, DateTime expires)

        /// <summary>
        /// Implements the storing in the cache until a specified expiry date / time
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="expires"></param>
        public void AddToCache(string type, string key, object data, DateTime expires)
        {
            if (expires == Cache.NoAbsoluteExpiration || expires > DateTime.Now)
            {
                Cache c = this.WebCache;
                string fullkey = GetFullKey(type, key);
                c.Insert(fullkey, data, null, expires, Cache.NoSlidingExpiration);
            }
        }

        #endregion

        #region private static string GetFullKey(string type, string key)

        /// <summary>
        /// Gets the full key based on the type (not required) and key (required)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetFullKey(string type, string key)
        {
            if (null == type)
                type = "____";
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            return String.Concat("pdf:", type, ":", key);
        }

        #endregion

    }
}
