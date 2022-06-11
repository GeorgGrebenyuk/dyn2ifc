using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using dg = Autodesk.DesignScript.Geometry;
using dr = Autodesk.DesignScript.Runtime;

using GeometryGym.Ifc;
using static dyn2ifc.IfcFile.IfcDoc;

namespace dyn2ifc.IfcGeometry
{
    [dr.IsVisibleInDynamoLibrary(true)]
    public class as_IfcShapeRepresentation
    {
        private GeometryGym.Ifc.IfcShapeRepresentation ifc_IfcShapeRepresentation;
        //private GeometryGym.Ifc.IfcRepresentationItem ifc_to_set_style;
        private double[] color_data = new double[3] { 0, 0, 0 };

        [Obsolete]
        private void SetColor(IfcRepresentationItem ifc_to_set_style)
        {
            //problem ... how set a color???
            IfcColourRgb ifc_color = new IfcColourRgb(ifc_db, color_data[0] / 256d, color_data[1] / 256d, color_data[2] / 256d);
            IfcSurfaceStyleRendering ifc_style1 = new IfcSurfaceStyleRendering(ifc_color);
            //IfcSurfaceStyleShading ifc_style = new IfcSurfaceStyleShading(ifc_color);
            IfcSurfaceStyle ifc_style0 = new IfcSurfaceStyle(ifc_style1);
            ifc_style0.Name = $"some_material_rgb_{color_data[0]}-{color_data[1]}-{color_data[2]}";

            IfcPresentationStyleAssignment style_assignm = new IfcPresentationStyleAssignment(ifc_style0);
            IfcStyledItem object_style = new IfcStyledItem(ifc_to_set_style, style_assignm);

        }
        [dr.IsVisibleInDynamoLibrary(true)]
        public IfcShapeRepresentation Get_IfcFaceBasedSurfaceModel()
        {
            return this.ifc_IfcShapeRepresentation;
        }
        /// <summary>
        /// Create IfcCartesianPoint from Autodesk.DesignScript.Geometry.Point with color
        /// </summary>
        /// <param name="color">Color info (double array with 3 values)</param>
        /// <param name="point">Autodesk.DesignScript.Geometry.Point</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        [Obsolete]
        public as_IfcShapeRepresentation (double [] color, dg.Point point)
        {
            this.color_data = color;
            IfcCartesianPoint pnt = new IfcCartesianPoint(ifc_db, point.X, point.Y, point.Z);
            this.SetColor(pnt);
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(pnt);
        }
        /// <summary>
        /// Create IfcSphere from Autodesk.DesignScript.Geometry.Point with color and radius
        /// </summary>
        /// <param name="color">Color info (double array with 3 values)</param>
        /// <param name="point">Autodesk.DesignScript.Geometry.Point</param>
        /// <param name="radius">double value of radius</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public as_IfcShapeRepresentation (double[] color, dg.Point point, double radius)
        {
            this.color_data = color;
            IfcCartesianPoint pnt = new IfcCartesianPoint(ifc_db, point.X, point.Y, point.Z);
            IfcAxis2Placement3D placement = new IfcAxis2Placement3D(pnt);
            IfcSphere sphere = new IfcSphere(placement, radius);
            this.SetColor(pnt);
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(sphere);
        }
        /// <summary>
        /// Create IfcBoundingBox from Autodesk.DesignScript.Geometry.BoundingBox with color
        /// </summary>
        /// <param name="color">Color info (double array with 3 values)</param>
        /// <param name="bbox">Autodesk.DesignScript.Geometry.BoundingBox</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public as_IfcShapeRepresentation(double[] color, dg.BoundingBox bbox)
        {
            this.color_data = color;
            IfcCartesianPoint min_point = new IfcCartesianPoint(ifc_db, bbox.MinPoint.X, bbox.MinPoint.Y, bbox.MinPoint.Z);
            IfcBoundingBox ifc_bbox = new IfcBoundingBox(min_point,
                Math.Abs(bbox.MaxPoint.X - bbox.MinPoint.X),
                Math.Abs(bbox.MaxPoint.Y - bbox.MinPoint.Y),
                Math.Abs(bbox.MaxPoint.Z - bbox.MinPoint.Z));
            this.SetColor(ifc_bbox);
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(ifc_bbox);
        }
        /// <summary>
        /// Create IfcPolyline from Autodesk.DesignScript.Geometry.PolyCurve with color
        /// </summary>
        /// <param name="color">Color info (double array with 3 values)</param>
        /// <param name="poly_curve">Autodesk.DesignScript.Geometry.PolyCurve</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public as_IfcShapeRepresentation(double[] color, dg.PolyCurve poly_curve)
        {
            this.color_data = color;
            List<IfcCartesianPoint> points = new List<IfcCartesianPoint>();

            points.Add(new IfcCartesianPoint(ifc_db, 
                poly_curve.Curves()[0].StartPoint.X, 
                poly_curve.Curves()[0].StartPoint.Y,
                poly_curve.Curves()[0].StartPoint.Z));
            for (int counter1 = 0; counter1<points.Count(); counter1++)
            {
                dg.Curve curve = poly_curve.Curves()[counter1];
                points.Add(new IfcCartesianPoint(ifc_db, curve.EndPoint.X, curve.EndPoint.Y, curve.EndPoint.Z));
            }
            IfcPolyline ifc_pline = new IfcPolyline(points);
            this.SetColor(ifc_pline);
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(ifc_pline);
        }
        /// <summary>
        /// Create IfcFaceBasedSurfaceModel from Autodesk.DesignScript.Geometry.Mesh with color
        /// </summary>
        /// <param name="color">Color info (double array with 3 values)</param>
        /// <param name="mesh">Autodesk.DesignScript.Geometry.Mesh</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public as_IfcShapeRepresentation (double[] color, dg.Mesh mesh)
        {
            this.color_data = color;
            List<IfcCartesianPoint> ifc_points = new List<IfcCartesianPoint>();
            foreach (dg.Point dyn_point in mesh.VertexPositions)
            {
                ifc_points.Add(new IfcCartesianPoint(ifc_db, dyn_point.X, dyn_point.Y, dyn_point.Z));
            }
            List<IfcFace> faces = new List<IfcFace>();
            foreach (dg.IndexGroup dyn_face in mesh.FaceIndices)
            {
                IfcPolyLoop loop;
                if (dyn_face.Count == 3) loop = new IfcPolyLoop(
                    ifc_points[(int)dyn_face.A],
                    ifc_points[(int)dyn_face.B],
                    ifc_points[(int)dyn_face.C]);
                else loop = new IfcPolyLoop(
                    ifc_points[(int)dyn_face.A],
                    ifc_points[(int)dyn_face.B],
                    ifc_points[(int)dyn_face.C],
                    ifc_points[(int)dyn_face.D]);
                //IfcFaceOuterBound bound = new IfcFaceOuterBound(loop, true);
                faces.Add(new IfcFace(new IfcFaceOuterBound(loop, true)));
            }
            IfcFaceBasedSurfaceModel faced_model = new IfcFaceBasedSurfaceModel(new IfcConnectedFaceSet(faces));
            this.SetColor(faced_model);
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(faced_model);
        }
        [dr.IsVisibleInDynamoLibrary(false)]
        public as_IfcShapeRepresentation (double[] color, dg.Solid solid)
        {
            this.color_data = color;
            List<IfcFace> faces = new List<IfcFace>();
            foreach (dg.Face dyn_face in solid.Faces)
            {
                foreach (dg.Loop dyn_loop in dyn_face.Loops)
                {
                    if (dyn_loop.IsExternal)
                    {
                        List<IfcCartesianPoint> points = new List<IfcCartesianPoint>();
                        foreach (dg.CoEdge coedge in dyn_loop.CoEdges)
                        {
                            //дичь какая-то ...
                        }
                    }
                }
            }
        }
    }
}
