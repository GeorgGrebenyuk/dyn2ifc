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
    /// Class for Grouping IFC elements
    /// </summary>
    public class IfcGrouping
    {
        public IfcObjectDefinition group;
        //public IfcBuildingStorey storey;
        /// <summary>
        /// Getting IfcGroup from IfcGrouping class
        /// </summary>
        /// <returns>IfcGroup</returns>
        [dr.IsVisibleInDynamoLibrary(true)]
        public IfcObjectDefinition get_group()
        {
            return this.group;
        }
        [dr.IsVisibleInDynamoLibrary(true)]
        public IfcGrouping(dyn2ifc.IfcDoc ifc_document, string name)
        {
            group = new IfcGroup(ifc_document.ifc_db, name);
        }
        /// <summary>
        /// Create IfcBuildingStorey by name and elevation
        /// </summary>
        /// <param name="name">Name of level (storey)</param>
        /// <param name="elevation">Elevation of level (storey) in selected units in IFC</param>
        [dr.IsVisibleInDynamoLibrary(false)]
        public IfcGrouping(dyn2ifc.IfcDoc ifc_document, string name, double elevation)
        {
            group = new IfcBuildingStorey(ifc_document.ifc_site, name, elevation);
        }
        /// <summary>
        /// Add created IfcElements to group
        /// </summary>
        /// <param name="group">Result of node IfcGrouping.get_group</param>
        /// <param name="elements">Collections of IfcElement</param>
        /// <returns></returns>
        [dr.IsVisibleInDynamoLibrary(false)]
        public static int AddIdcElementsToGroup(int order, IfcObjectDefinition group, List<IfcElement> elements)
        {
            foreach (IfcElement elem in elements)
            {
                group.AddNested(elem);
            }
            return order;
        }
    }
}
