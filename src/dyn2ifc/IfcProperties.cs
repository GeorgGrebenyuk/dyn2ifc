using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using dg = Autodesk.DesignScript.Geometry;
using dr = Autodesk.DesignScript.Runtime;

using GeometryGym.Ifc;
using static dyn2ifc.IfcFile.IfcDoc;

namespace dyn2ifc
{
    [dr.IsVisibleInDynamoLibrary(true)]
    public class IfcProperties
    {
        private List<IfcPropertySingleValue> ifc_props;
        /// <summary>
        /// Getting an List<IfcPropertySingleValue> from IfcProperties class
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [dr.IsVisibleInDynamoLibrary(true)]
        public List<IfcPropertySingleValue> get_properties()
        {
            return this.ifc_props;
        }
        /// <summary>
        /// Convert standard Dictionary to list with IfcPropertySingleValue
        /// </summary>
        /// <param name="properties"></param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public IfcProperties(List <List<string>> properties)
        {
            ifc_props = new List<IfcPropertySingleValue>();
            foreach (List<string> name2value in properties)
            {
                ifc_props.Add(new IfcPropertySingleValue(ifc_db, name2value[0], name2value[1]));
            }
        }
    }
}
