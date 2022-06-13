# dyn2ifc
Source code for Autodesk Dynamo's package "dyn2ifc" to convert standard dynamo's geometry and standard data types to IFC via GeometryGym IFC library

# Source enviromental
Dynamo Core Runtime (tested on 12 version) => will be work with all product's Dynamo's applications (Revit, Civil3D, FormIt)

# How import?
There are four ways to using:
1. (Most simple) install via Dynamo package manager (package's name - ```dyn2ifc```)
2. From [Releases](https://github.com/GeorgGrebenyuk/dyn2ifc/releases) download latest version from ZIP archive "dyn2ifc_dyn-package_\*\*.zip" and unpack to Dynamo's packages folder:
```%APPDATA%\Dynamo\Dynamo Core\2.12\packages```, where 2.12 - release of Dynamo Runtime library
3. From [Releases](https://github.com/GeorgGrebenyuk/dyn2ifc/releases) download latest version from ZIP archive "dyn2ifc_ver-\*\*.zip" and import "dyn2ifc.dll" to Dynamo as File-Import library
4. Compile source code in repository (via Visual Studio) and load library to Dynamo, look step 3 above;

# What it include?
## Class IfcFile.IfcDoc
Main nodes that determine ```DatabaseIfc``` structure (Ifc specification, Length units, save path);

## Class IfcGeometry
Convert Autodesk.DesignScript.Geometry data to IfcShapeRepresentation geometry class.
Support next structures:
Dynamo geometry item | Ifc class item
--- | ---
Point | IfcPoint
Point and double-radius | IfcSphere
BoundingBox | IfcBoundingBox
PolyCurve | IfcPolyline
Mesh | IfcFaceBasedSurfaceModel
Solid | IfcFaceBasedSurfaceModel
PolyCurve and double-depth | IfcExtrudedAreaSolid

## Class IfcGrouping
Contains Ifc classes for group elements in file. Now two classes - IfcBuildingStorey (actual for buildings-CAD software) and IfcGroup

## Class IfcMaterialSet
Create IfcMaterial item from RGB code. NO WORK now.

## Class IfcObject
Combine properties, geometry representation, other data (name, guid) to IfcClass item. Supported two classes -- IfcBuildingElementProxy and IfcOpeningElement (need for itself also "parent" structure for crating holes). IfcBuildingElementProxy most stable.

## Class IfcProperties
Convert List of List<string> (2 items) to list<IfcPropertySingleValue> -- ifc properties to object, only in string representation.

  
# Bugfixing
Because of it's debug-version of package and process of converting Dynamo's data to IFC is no-trivial task, there will be an errors in data processing. Pleae, write in [Issues](https://github.com/GeorgGrebenyuk/dyn2ifc/issues) about all problems in data conversion. 
