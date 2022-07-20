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
    /// <summary>
    /// Class for work with properties (as string-data representation)
    /// </summary>
    public class IfcProperties
    {
        public List<IfcPropertySingleValue> ifc_props;
        /// <summary>
        /// Create properties for IfcElement (linking to object after) as couple of "name:value" collection (both string-data)
        /// </summary>
        /// <param name="properties"></param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public IfcProperties(dyn2ifc.IfcDoc ifc_document, List <List<string>> properties)
        {
            ifc_props = new List<IfcPropertySingleValue>();
            foreach (List<string> name2value in properties)
            {
                ifc_props.Add(new IfcPropertySingleValue(ifc_document.ifc_db, name2value[0], name2value[1]));
            }
        }
    }
    
}
