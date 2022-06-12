using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using dg = Autodesk.DesignScript.Geometry;
using dr = Autodesk.DesignScript.Runtime;

using GeometryGym.Ifc;

namespace dyn2ifc.IfcFile
{
    [dr.IsVisibleInDynamoLibrary(false)]
    public class IfcDoc
    {
        [dr.IsVisibleInDynamoLibrary(false)]
        public static DatabaseIfc ifc_db = new DatabaseIfc();
        public static IfcSite ifc_site = null;

        /// <summary>
        /// Save ifc file from database-in-memory to file (ifc 2x3)
        /// </summary>
        /// <param name="directory_path">File path to directory where save file</param>
        /// <param name="file_name">Name of ifc file to save without extension</param>
        /// <param name="Overwrite"></param>
        /// <param name="aux_order">Link with node InitIfcDatabase or other operation before it</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public static void SaveIfc (string directory_path, string file_name, bool Overwrite, int aux_order)
        {
            string file_path = directory_path + "\\" + file_name + ".ifc";
            if (!Directory.Exists(directory_path)) return;
            if (Overwrite) ifc_db.WriteFile(file_path);
            else
            {
                file_path = file_path.Replace(".ifc", $"_{Guid.NewGuid()}.ifc");
                ifc_db.WriteFile(file_path);
            }
        }
        /// <summary>
        /// Determine ifc specifications that supported in GeometryGym library
        /// </summary>
        /// <returns>GeometryGeym.Ifc.ModelView enum</returns>
        [dr.MultiReturn("Ifc2x3Coordination", "Ifc2x3NotAssigned", "Ifc4DesignTransfer",
            "Ifc4NotAssigned", "Ifc4Reference", "Ifc4X1NotAssigned", "Ifc4X2NotAssigned", "Ifc4X3NotAssigned")]
        [dr.IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, ModelView> ifc_document_specs()
        {
            return new Dictionary<string, ModelView>()
            {

                {"Ifc2x3Coordination", ModelView.Ifc2x3Coordination},
                {"Ifc2x3NotAssigned", ModelView.Ifc2x3NotAssigned},
                {"Ifc4DesignTransfer", ModelView.Ifc4DesignTransfer},
                {"Ifc4NotAssigned", ModelView.Ifc4NotAssigned},
                {"Ifc4Reference", ModelView.Ifc4Reference},
                {"Ifc4X1NotAssigned", ModelView.Ifc4X1NotAssigned},
                {"Ifc4X2NotAssigned", ModelView.Ifc4X2NotAssigned},
                {"Ifc4X3NotAssigned", ModelView.Ifc4X3NotAssigned}
            };
        }
        /// <summary>
        /// Determine internal data length units
        /// </summary>
        /// <returns>GeometryGeym.Ifc.IfcUnitAssignment.Length enum</returns>
        [dr.MultiReturn("Millimetre", "Centimetre", "Inch", "Foot", "Metre")]
        [dr.IsVisibleInDynamoLibrary(true)]
        public static Dictionary<string, IfcUnitAssignment.Length> ifc_units()
        {
            return new Dictionary<string, IfcUnitAssignment.Length>()
            {
                {"Millimetre", IfcUnitAssignment.Length.Millimetre},
                {"Centimetre", IfcUnitAssignment.Length.Centimetre},
                {"Inch", IfcUnitAssignment.Length.Inch},
                {"Foot", IfcUnitAssignment.Length.Foot},
                {"Metre", IfcUnitAssignment.Length.Metre}
            };
        }
        /// <summary>
        /// Compalsory node that need execute firstly; it create empty structure of IfcDatabase and set units to all data
        /// </summary>
        /// <param name="ifc_type">Result of node ifc_document_specs</param>
        /// <param name="units">Result of node ifc_units</param>
        /// <returns>Arbitary integer value for linking with other node (create order for data)</returns>
        [dr.IsVisibleInDynamoLibrary(true)]
        public static int InitIfcDatabase (ModelView ifc_type, IfcUnitAssignment.Length units)
        {
            ifc_db = new DatabaseIfc(ifc_type);
            ifc_site = new IfcSite(ifc_db, "ifc_site");

            IfcProject ifc_project = new IfcProject(ifc_site, "ifc_project", units);
            return 1;
        }

        

    }
}
