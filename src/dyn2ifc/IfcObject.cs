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
    /// Class for combinind data (materials, properties, geometry) to setting them to IfcElement
    /// </summary>
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
        [dr.IsVisibleInDynamoLibrary(false)]
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
        public static IfcElement to_IfcBuildingElementProxy(dyn2ifc.IfcDoc ifc_document, 
            IfcShapeRepresentation ifcShapeRepresentation, dyn2ifc.IfcGrouping group = null)
        {
           if (group == null) return new IfcBuildingElementProxy(ifc_document.ifc_site,
               ifc_document.ifc_site.ObjectPlacement, new IfcProductDefinitionShape(ifcShapeRepresentation));
           else return new IfcBuildingElementProxy(group.group, ifc_document.ifc_site.ObjectPlacement, 
               new IfcProductDefinitionShape(ifcShapeRepresentation));
        }
        /// <summary>
        /// Create IfcOpeningElement element from parent element (where will be cutting, using IfcRelVoidsElement). 
        /// At first need parent elemet, where cut was created!
        /// </summary>
        /// <param name="ifcShapeRepresentation">Use node Get_IfcFaceBasedSurfaceModel</param>
        /// <param name="parent_element"></param>
        /// <returns></returns>
        [dr.IsVisibleInDynamoLibrary(true)]
        public static IfcElement to_IfcOpeningElement (dyn2ifc.IfcDoc ifc_document, 
            IfcShapeRepresentation ifcShapeRepresentation, IfcElement parent_element)
        {
            IfcOpeningElement elem = new IfcOpeningElement(parent_element, ifc_document.ifc_site.ObjectPlacement, 
                new IfcProductDefinitionShape(ifcShapeRepresentation));
            IfcRelVoidsElement parent_link = new IfcRelVoidsElement(parent_element, elem);
            return elem;
        }
        /// <summary>
        /// Link data with base IfcElement-structure (Name, Description GlobalId, IfcMaterial,IfcPropertySet)
        /// </summary>
        /// <param name="properties"></param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public IfcObject(IfcElement ifc_element, string GlobalId = null, string name = null, 
            dyn2ifc.IfcMaterialSet material = null, string description = null,
            IfcProperties properties = null)
        {
            if (properties != null) new IfcPropertySet(ifc_element, "ifc_properties", properties.ifc_props);
            if (GlobalId!= null) ifc_element.GlobalId = GlobalId;
            if (name != null) ifc_element.Name = name;
            if (description != null) ifc_element.Description = description;
            ifc_element.Guid = Guid.NewGuid();

            if (material != null) ifc_element.SetMaterial(material.material);
            //IfcRelAssociatesMaterial ass_material = new IfcRelAssociatesMaterial(material, new List<IfcElement> { ifc_element });
        }
    }
}
