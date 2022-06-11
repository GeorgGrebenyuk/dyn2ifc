using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using dg = Autodesk.DesignScript.Geometry;
using dr = Autodesk.DesignScript.Runtime;

using GeometryGym.Ifc;

namespace dyn2ifc
{
    [dr.IsVisibleInDynamoLibrary(false)]
    public class About
    {
        /// <summary>
        /// About package - link to GitHub repository
        /// </summary>
        /// <returns></returns>
        [dr.IsVisibleInDynamoLibrary(true)]
        public static string GetInfoAboutPackage()
        {
            return "Look repository of package https://github.com/GeorgGrebenyuk/dyn2ifc";
        }
    }
}
