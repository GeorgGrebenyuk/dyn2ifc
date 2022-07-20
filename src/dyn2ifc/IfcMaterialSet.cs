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
    /// Class for creating materials definition (linking to object after)
    /// </summary>
    [dr.IsVisibleInDynamoLibrary(false)]

    public class IfcMaterialSet
    {
        public IfcMaterial material;
      
        /// <summary>
        /// Create IfcMaterial by color-data and material's name
        /// </summary>
        /// <param name="color_data">Color info (double array with 3 values)</param>
        /// <param name="material_name">Name of your material</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        [Obsolete]
        public IfcMaterialSet(dyn2ifc.IfcDoc ifc_document, double[] color_data, string material_name)
        {
            //problem ... how set a color???
            IfcColourRgb ifc_color = new IfcColourRgb(ifc_document.ifc_db, color_data[0] / 256d, color_data[1] / 256d, color_data[2] / 256d);
            IfcSurfaceStyleRendering ifc_style1 = new IfcSurfaceStyleRendering(ifc_color);
            //IfcSurfaceStyleShading ifc_style = new IfcSurfaceStyleShading(ifc_color);
            IfcSurfaceStyle ifc_style0 = new IfcSurfaceStyle(ifc_style1);
            ifc_style0.Name = $"some_material_rgb_{color_data[0]}-{color_data[1]}-{color_data[2]}";

            this.material = new IfcMaterial(ifc_document.ifc_db, material_name);
            IfcPresentationStyleAssignment style_assignm = new IfcPresentationStyleAssignment(ifc_style0);
            IfcStyledItem object_style = new IfcStyledItem(style_assignm);
            IfcStyledRepresentation repr = new IfcStyledRepresentation(object_style);

            IfcMaterialDefinitionRepresentation ifc_def = new IfcMaterialDefinitionRepresentation(repr, material);
        }

    }
}
