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
        [dr.IsVisibleInDynamoLibrary(true)]
        public IfcShapeRepresentation Get_IfcFaceBasedSurfaceModel()
        {
            return this.ifc_IfcShapeRepresentation;
        }
        /// <summary>
        /// Create IfcCartesianPoint from Autodesk.DesignScript.Geometry.Point
        /// </summary>
        /// <param name="point">Autodesk.DesignScript.Geometry.Point</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        [Obsolete]
        public as_IfcShapeRepresentation (dg.Point point)
        {
            IfcCartesianPoint pnt = new IfcCartesianPoint(ifc_db, point.X, point.Y, point.Z);
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(pnt);
        }
        /// <summary>
        /// Create IfcSphere from Autodesk.DesignScript.Geometry.Point with radius
        /// </summary>
        /// <param name="point">Autodesk.DesignScript.Geometry.Point</param>
        /// <param name="radius">double value of radius</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public as_IfcShapeRepresentation (dg.Point point, double radius)
        {
            IfcCartesianPoint pnt = new IfcCartesianPoint(ifc_db, point.X, point.Y, point.Z);
            IfcAxis2Placement3D placement = new IfcAxis2Placement3D(pnt);
            IfcSphere sphere = new IfcSphere(placement, radius);
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(sphere);
        }
        /// <summary>
        /// Create IfcBoundingBox from Autodesk.DesignScript.Geometry.BoundingBox
        /// </summary>
        /// <param name="bbox">Autodesk.DesignScript.Geometry.BoundingBox</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public as_IfcShapeRepresentation(dg.BoundingBox bbox)
        {
            IfcCartesianPoint min_point = new IfcCartesianPoint(ifc_db, bbox.MinPoint.X, bbox.MinPoint.Y, bbox.MinPoint.Z);
            IfcBoundingBox ifc_bbox = new IfcBoundingBox(min_point,
                Math.Abs(bbox.MaxPoint.X - bbox.MinPoint.X),
                Math.Abs(bbox.MaxPoint.Y - bbox.MinPoint.Y),
                Math.Abs(bbox.MaxPoint.Z - bbox.MinPoint.Z));
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(ifc_bbox);
        }
        /// <summary>
        /// Create IfcPolyline from Autodesk.DesignScript.Geometry.PolyCurve
        /// </summary>
        /// <param name="poly_curve">Autodesk.DesignScript.Geometry.PolyCurve</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public as_IfcShapeRepresentation(dg.PolyCurve poly_curve)
        {
            List<IfcCartesianPoint> points = new List<IfcCartesianPoint>();

            points.Add(new IfcCartesianPoint(ifc_db, 
                poly_curve.Curves()[0].StartPoint.X, 
                poly_curve.Curves()[0].StartPoint.Y,
                poly_curve.Curves()[0].StartPoint.Z));
            for (int counter1 = 0; counter1< poly_curve.Curves().Count(); counter1++)
            {
                dg.Curve curve = poly_curve.Curves()[counter1];
                points.Add(new IfcCartesianPoint(ifc_db, curve.EndPoint.X, curve.EndPoint.Y, curve.EndPoint.Z));
            }
            IfcPolyline ifc_pline = new IfcPolyline(points);
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(ifc_pline);
        }
        /// <summary>
        /// Create IfcFaceBasedSurfaceModel from Autodesk.DesignScript.Geometry.Mesh
        /// </summary>
        /// <param name="mesh">Autodesk.DesignScript.Geometry.Mesh</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public as_IfcShapeRepresentation (dg.Mesh mesh)
        {
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
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(faced_model);
        }
        /// <summary>
        /// Create IfcFaceBasedSurfaceModel from Autodesk.DesignScript.Geometry.Solid. Need debug!!!!
        /// </summary>
        /// <param name="solid">Autodesk.DesignScript.Geometry.Solid</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public as_IfcShapeRepresentation (dg.Solid solid)
        {
            List<IfcFace> faces = new List<IfcFace>();
            foreach (dg.Face dyn_face in solid.Faces)
            {
                
                foreach (dg.Loop dyn_loop in dyn_face.Loops)
                {
                    IfcFaceOuterBound bound;
                    IfcPolyLoop loop;
                    if (dyn_loop.IsExternal)
                    {
                        List<dg.Point> dyn_points = new List<dg.Point>();
                        foreach (dg.CoEdge coedge in dyn_loop.CoEdges)
                        {
                            //дичь какая-то ...
                            if (!dyn_points.Contains(coedge.StartVertex.PointGeometry)) dyn_points.Add(coedge.StartVertex.PointGeometry);
                            if (!dyn_points.Contains(coedge.EndVertex.PointGeometry)) dyn_points.Add(coedge.EndVertex.PointGeometry);
                        }
                        List<IfcCartesianPoint> points = dyn_points.Select(a => new IfcCartesianPoint(ifc_db, a.X, a.Y, a.Z)).ToList();
                        loop = new IfcPolyLoop(points);
                        bound = new IfcFaceOuterBound(loop, true);
                        faces.Add(new IfcFace(bound));
                    }
                }
            }
            IfcFaceBasedSurfaceModel faced_model = new IfcFaceBasedSurfaceModel(new IfcConnectedFaceSet(faces));
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(faced_model);
        }

        /// <summary>
        /// Create IfcExtrudedAreaSolid by Autodesk.DesignScript.Geometry.PolyCurve
        /// </summary>
        /// <param name="dyn_polyline">Autodesk.DesignScript.Geometry.PolyCurve</param>
        /// <param name="depth">double value of depth</param>
        [dr.IsVisibleInDynamoLibrary(true)]
        public as_IfcShapeRepresentation(dg.PolyCurve polyCurve, double depth)
        {
            List<Tuple<double, double, double>> points = new List<Tuple<double, double, double>>();
            List<IfcLineIndex> indexes = new List<IfcLineIndex>();

            int points_counter = 1;
            foreach (dg.Curve line in polyCurve.Curves())
            {
                Tuple<double, double, double> start_point = new Tuple<double, double, double>(line.StartPoint.X, line.StartPoint.Y, line.StartPoint.Z);
                Tuple<double, double, double> end_point = new Tuple<double, double, double>(line.EndPoint.X, line.EndPoint.Y, line.EndPoint.Z);
                //Реализовать потом экономичную процедуру
                points.Add(start_point);
                points.Add(end_point);
                indexes.Add(new IfcLineIndex(points_counter, points_counter + 1));
                points_counter += 2;
            }
            IfcIndexedPolyCurve polycurve = new IfcIndexedPolyCurve(new IfcCartesianPointList3D(ifc_db, points), indexes);
            IfcArbitraryClosedProfileDef arb_closed = new IfcArbitraryClosedProfileDef("area", polycurve);
            IfcExtrudedAreaSolid solid = new IfcExtrudedAreaSolid(arb_closed, depth);
            this.ifc_IfcShapeRepresentation = new IfcShapeRepresentation(solid);
        }
        
    }
}
