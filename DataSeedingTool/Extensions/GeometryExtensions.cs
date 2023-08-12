using Domain.Models;
using NetTopologySuite.Geometries;

using ProjNet;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace DataSeedingTool.Extensions;

public static class GeometryExtensions
{
    private static readonly CoordinateSystemServices CoordinateSystemServices
        = new(
            new Dictionary<int, string>
            {
                // Coordinate systems:

                [Srid.Wgs84] = GeographicCoordinateSystem.WGS84.WKT,

                // This coordinate system covers the area of our data.
                // Different data requires a different coordinate system.
                [Srid.Wgs84Utm34N] =
                    @"
                        PROJCS[""WGS 84 / UTM zone 34N"",
                        GEOGCS[""WGS 84"",
                            DATUM[""WGS_1984"",
                                SPHEROID[""WGS 84"",6378137,298.257223563,
                                    AUTHORITY[""EPSG"",""7030""]],
                                AUTHORITY[""EPSG"",""6326""]],
                            PRIMEM[""Greenwich"",0,
                                AUTHORITY[""EPSG"",""8901""]],
                            UNIT[""degree"",0.0174532925199433,
                                AUTHORITY[""EPSG"",""9122""]],
                            AUTHORITY[""EPSG"",""4326""]],
                        PROJECTION[""Transverse_Mercator""],
                        PARAMETER[""latitude_of_origin"",0],
                        PARAMETER[""central_meridian"",21],
                        PARAMETER[""scale_factor"",0.9996],
                        PARAMETER[""false_easting"",500000],
                        PARAMETER[""false_northing"",0],
                        UNIT[""metre"",1,
                            AUTHORITY[""EPSG"",""9001""]],
                        AXIS[""Easting"",EAST],
                        AXIS[""Northing"",NORTH],
                        AUTHORITY[""EPSG"",""32634""]]  
                    "
            });

    public static Geometry ProjectTo(this Geometry geometry, int srid)
    {
        var transformation = CoordinateSystemServices.CreateTransformation(geometry.SRID, srid);
        
        var result = geometry.Copy();
        result.Apply(new MathTransformFilter(transformation.MathTransform));

        result.SRID = srid;

        return result;
    }

    private class MathTransformFilter : ICoordinateSequenceFilter
    {
        private readonly MathTransform _transform;

        public MathTransformFilter(MathTransform transform)
            => _transform = transform;

        public bool Done => false;
        public bool GeometryChanged => true;

        public void Filter(CoordinateSequence seq, int i)
        {
            var (x, y, z) = _transform.Transform(seq.GetX(i), seq.GetY(i), seq.GetZ(i));
            seq.SetX(i, x);
            seq.SetY(i, y);
            seq.SetZ(i, z);
        }
    }
}