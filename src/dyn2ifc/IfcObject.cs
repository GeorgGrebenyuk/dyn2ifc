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
    public class IfcObject
    {
        /// <summary>
        /// Create IfcBuildingElementProxy by IfcShapeRepresentation (as IfcProductDefinitionShape) and 
        /// couple of IfcPropertySingleValue; record it to ifc_database automatically
        /// </summary>
        /// <param name="ifcShapeRepresentation">Use node Get_IfcFaceBasedSurfaceModel</param>
        /// <param name="properties"></param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public IfcObject (IfcShapeRepresentation ifcShapeRepresentation, List<IfcPropertySingleValue> properties = null)
        {
            IfcProductDefinitionShape result_geometry = new IfcProductDefinitionShape(ifcShapeRepresentation);
            IfcBuildingElementProxy proxy = new IfcBuildingElementProxy(ifc_site, ifc_site.ObjectPlacement, result_geometry);

            if (properties != null)
            {
                new IfcPropertySet(proxy, "ifc_properties", properties);
            }
        }
    }
}
