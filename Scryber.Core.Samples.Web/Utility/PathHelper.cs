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
using System.IO;

namespace Scryber.Utilities
{
    public static class PathHelper
    {

        /// <summary>
        /// Maps and joins paths.
        /// </summary>
        /// <param name="origsource">The original source path that we are mapping from (original file path)</param>
        /// <param name="sourcepath">The source path that we want mapped (relative to the original source)</param>
        /// <param name="isfile">Set to true if the resultant path is a file on a local disk, otherwise false - web resource.</param>
        /// <param name="loadtype">How the components are loaded - is it a web application or executable</param>
        /// <returns>The joined and mapped path</returns>
        public static string MapPath(ParserLoadType loadtype, string origsource, string sourcepath, out bool isfile)
        {
            string result;
            string path;

            if (string.IsNullOrEmpty(sourcepath))
                throw new ArgumentNullException("sourcepath");

            if (sourcepath.StartsWith("~") && null == System.Web.HttpContext.Current)
            {
                //application rooted in an executable

                if (string.IsNullOrEmpty(sourcepath))
                    throw new ArgumentException("sourcepath");
                //Application relative source path.
                string workingDirectory = System.Environment.CurrentDirectory;
                //remove the tilde from the source
                sourcepath = sourcepath.Substring(1);
                //switch any forward slashes to DirectorySeparatorChars and remove any front slashes
                sourcepath = sourcepath.Replace('/', System.IO.Path.DirectorySeparatorChar);
                if (sourcepath[0] == System.IO.Path.DirectorySeparatorChar)
                    sourcepath = sourcepath.Substring(1);

                //combine with the working directory
                result = System.IO.Path.Combine(workingDirectory, sourcepath);
                isfile = true;
            }
            else if ((sourcepath.StartsWith("~") || sourcepath.StartsWith("/")) 
                     && null != System.Web.HttpContext.Current)
            {
                //appliction rooted in a web site
                result = System.Web.HttpContext.Current.Server.MapPath(sourcepath);
                isfile = true;
            }
            else if (Uri.IsWellFormedUriString(sourcepath, UriKind.RelativeOrAbsolute))
            {

                if (System.Web.VirtualPathUtility.IsAppRelative(sourcepath))
                {
                    System.Web.HttpContext context = System.Web.HttpContext.Current;

                    if (null == context)
                        throw new NullReferenceException(Errors.NoWebContextAvailableForRelativeUrl);

                    result = System.Web.HttpContext.Current.Server.MapPath(sourcepath);
                    isfile = true;
                }
                else if (Uri.IsWellFormedUriString(sourcepath, UriKind.Absolute))
                {
                    result = sourcepath;
                    isfile = false;
                }
                else if (!GetLocalPath(loadtype, origsource, out path, out isfile))
                    throw new FileNotFoundException(sourcepath);
                else if (isfile)
                {
                    result = System.IO.Path.Combine(path, sourcepath.Replace('/', System.IO.Path.DirectorySeparatorChar));
                    result = System.IO.Path.GetFullPath(result);
                }
                else
                    result = CombineWebUri(path, sourcepath);

            }
            else if (System.IO.Path.IsPathRooted(sourcepath))
            {
                isfile = true;
                result = System.IO.Path.GetFullPath(sourcepath); //Normalize
            }
            else if (!GetLocalPath(loadtype, origsource, out path, out isfile))
                throw new FileNotFoundException(sourcepath);

            else if (isfile)
            {
                result = System.IO.Path.Combine(path, sourcepath.Replace('/', System.IO.Path.DirectorySeparatorChar));
                result = System.IO.Path.GetFullPath(result);
            }
            else
            {
                result = CombineWebUri(path, sourcepath);
            }

            return result;
        }

        private static string CombineWebUri(string origUri, string relativeUri)
        {
            if (relativeUri.StartsWith("/")) //server relative
            {
                UriBuilder builder = new UriBuilder(origUri);
                builder.Path = relativeUri;
                return builder.ToString();
            }
            while (relativeUri.StartsWith(".."))
            {
                origUri = origUri.Substring(0, origUri.LastIndexOf("/"));
                relativeUri = relativeUri.Substring(3); //remove ../
            }
            if (relativeUri.StartsWith("./"))
                relativeUri = relativeUri.Substring(1);

            if (origUri.EndsWith("/") && relativeUri.StartsWith("/"))
                relativeUri = relativeUri.Substring(1);
            else if (!origUri.EndsWith("/") && !relativeUri.StartsWith("/"))
                relativeUri = "/" + relativeUri;

            return origUri + relativeUri;
        }

        /// <summary>
        /// Gets the full path to this local document - setting isfile to true if the path is a file rather than a uri
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isfile"></param>
        /// <returns></returns>
        private static bool GetLocalPath(ParserLoadType loadtype, string source, out string path, out bool isfile)
        {
            path = source;
            if (string.IsNullOrEmpty(path))
                path = GetRootDirectory(loadtype);
            else if (System.Uri.IsWellFormedUriString(path, UriKind.Absolute))
            {
                path = path.Substring(0, path.LastIndexOf('/'));
            }
            else
                path = System.IO.Path.GetDirectoryName(source);

            if (string.IsNullOrEmpty(path))
            {
                isfile = false;
                return false;
            }
            else if (System.Uri.IsWellFormedUriString(path, UriKind.Absolute))
            {
                isfile = false;
                return true;
            }
            else if (System.IO.Path.IsPathRooted(path))
            {
                isfile = true;
                return true;
            }
            else if (path.StartsWith("~"))
            {
                string exec = GetRootDirectory(loadtype);
                if (path.Length > 1)
                {
                    //merge current directory with the path.
                    path = path.Substring(1);
                    if (!path.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString())
                        && !exec.StartsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
                        exec += System.IO.Path.DirectorySeparatorChar.ToString();
                    path = exec + path;
                }
                else
                    path = exec;
                isfile = true;

                return true;
            }
            else
            {
                throw new FileNotFoundException(path);
            }
        }

        /// <summary>
        /// Gets the root (working) directory for this document - 
        /// root of the web application if its a web document or the current directory for executable
        /// </summary>
        /// <returns></returns>
        public static  string GetRootDirectory(ParserLoadType loadtype)
        {
            if (loadtype == ParserLoadType.WebBuildProvider)
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                if(null == context)
                    throw new NullReferenceException();
                return context.Server.MapPath("/");
            }
            else
            {
                return System.IO.Directory.GetCurrentDirectory();
            }
        }
    }
}
