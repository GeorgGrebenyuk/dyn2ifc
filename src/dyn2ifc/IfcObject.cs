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
        /// Create random Guid as string
        /// </summary>
        /// <returns>random Guid as string</returns>
        [dr.IsVisibleInDynamoLibrary(true)]
        public static string get_guid()
        {
            return Guid.NewGuid().ToString();
        }
        [dr.MultiReturn("IfcBuildingElementProxy", "IfcOpeningElement")]
        public static Dictionary<string,string> ifc_classes_objects()
        {
            return new Dictionary<string, string>()
            {
                { "IfcBuildingElementProxy","IfcBuildingElementProxy" },
                { "IfcOpeningElement","IfcOpeningElement" }
                
            };
        }
        /// <summary>
        /// Create IfcBuildingElementProxy element from IfcGroup
        /// </summary>
        /// <param name="ifcShapeRepresentation">Use node Get_IfcFaceBasedSurfaceModel</param>
        /// <param name="group"></param>
        /// <returns></returns>
        [dr.IsVisibleInDynamoLibrary(true)]
        public static IfcElement to_IfcBuildingElementProxy(IfcShapeRepresentation ifcShapeRepresentation, IfcObjectDefinition group = null)
        {
           if (group == null) return new IfcBuildingElementProxy(ifc_site, ifc_site.ObjectPlacement, new IfcProductDefinitionShape(ifcShapeRepresentation));
           else return new IfcBuildingElementProxy(group, ifc_site.ObjectPlacement, new IfcProductDefinitionShape(ifcShapeRepresentation));
        }
        /// <summary>
        /// Create IfcOpeningElement element from parent element (where will be cutting, using IfcRelVoidsElement). At first need parent elemet, where cut was created!
        /// </summary>
        /// <param name="ifcShapeRepresentation">Use node Get_IfcFaceBasedSurfaceModel</param>
        /// <param name="parent_element"></param>
        /// <returns></returns>
        [dr.IsVisibleInDynamoLibrary(true)]
        public static IfcElement to_IfcOpeningElement (IfcShapeRepresentation ifcShapeRepresentation, IfcElement parent_element)
        {
            IfcOpeningElement elem = new IfcOpeningElement(parent_element, ifc_site.ObjectPlacement, new IfcProductDefinitionShape(ifcShapeRepresentation));
            IfcRelVoidsElement parent_link = new IfcRelVoidsElement(parent_element, elem);
            return elem;
        }
        /// <summary>
        /// Create IfcBuildingElementProxy by IfcShapeRepresentation (as IfcProductDefinitionShape) and 
        /// couple of IfcPropertySingleValue; record it to ifc_database automatically
        /// </summary>
        /// <param name="properties"></param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public IfcObject(IfcElement ifc_element, string GlobalId, string name, IfcMaterial material, string description = "", 
            List<IfcPropertySingleValue> properties = null)
        {
            if (properties != null)
            {
                new IfcPropertySet(ifc_element, "ifc_properties", properties);
            }
            ifc_element.Name = name;
            ifc_element.Description = description;
            ifc_element.Guid = Guid.NewGuid();
            ifc_element.GlobalId = GlobalId;

            ifc_element.SetMaterial(material);

            //IfcRelAssociatesMaterial ass_material = new IfcRelAssociatesMaterial(material, new List<IfcElement> { ifc_element });
        }
    }
}
