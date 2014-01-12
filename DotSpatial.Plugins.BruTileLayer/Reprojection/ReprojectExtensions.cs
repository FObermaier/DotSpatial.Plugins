using System;
using System.Collections.Generic;
using DotSpatial.Projections;
using DotSpatial.Topology;

namespace DotSpatial.Plugins.BruTileLayer.Reprojection
{
    public static class ReprojectExtensions
    {
        public static Coordinate Reproject(this Coordinate c, ProjectionInfo source, ProjectionInfo target)
        {
            var ordinates = new[] { c.X, c.Y };
            var z = new[] {double.IsNaN(c.Z) ? 0 : c.Z};
            Projections.Reproject.ReprojectPoints(ordinates, z, source, target, 0, 1);
            return new Coordinate(ordinates);
        }

        public static IList<Coordinate> Reproject(this IList<Coordinate> seq, ProjectionInfo source, ProjectionInfo target)
        {
            var ords = new double[seq.Count * 2];
            var z = new Double[seq.Count];
            var j = 0;
            for (var i = 0; i < seq.Count; i++)
            {
                var c = seq[i];
                ords[j++] = c.X;
                ords[j++] = c.Y;
                z[i] = double.IsNaN(c.Z) ? 0 : c.Z;
            }

            Projections.Reproject.ReprojectPoints(ords, z, source, target, 0, seq.Count);

            var lst = new List<Coordinate>(seq.Count);
            j = 0;
            for (var i = 0; i < seq.Count; i++)
                lst.Add(new Coordinate(ords[j++], ords[j++]));

            return lst;
        }

        public static ILinearRing Reproject(this ILinearRing ring, ProjectionInfo source, ProjectionInfo target)
        {
            var seq = Reproject(ring.Coordinates, source, target);
            return ring.Factory.CreateLinearRing(seq);
        }

        public static IPolygon Reproject(this IPolygon polygon, ProjectionInfo source, ProjectionInfo target)
        {
            var shell = Reproject(polygon.Shell, source, target);
            ILinearRing[] holes = null;
            if (polygon.NumHoles > 0)
            {
                holes = new ILinearRing[polygon.NumHoles];
                var i = 0;
                foreach (var hole in polygon.Holes)
                    holes[i++] = Reproject(hole, source, target);
            }

            return polygon.Factory.CreatePolygon(shell, holes);
        }
    }
}