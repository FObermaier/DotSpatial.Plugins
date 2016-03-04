using System;
using System.Collections.Generic;
using System.Data;
using DotSpatial.Topology;
using GeoAPI.Geometries;
using Microsoft.SqlServer.Types;
using NetTopologySuite.IO;
using NetTopologySuite;
using Npgsql;
namespace DotSpatial.Data
{
    public class FeatureSetFromPostGis
    {
        static FeatureSetFromPostGis()
        {
            GeoAPI.GeometryServiceProvider.Instance = new NetTopologySuite. NetTopologySuite.NtsGeometryServices.Instance;
        }

        public static IFeatureSet LoadFeatureSet(string connectionString, string sql)
        {
            IFeatureSet res = null;
            using (var cn = new NpgsqlConnection(connectionString))
            {
                cn.Open();
                var cmd = new NpgsqlCommand(sql, cn);
                using (var r = cmd.ExecuteReader())
                {
                    if (r.HasRows)
                    {
                        r.Read();
                        DataTable table;
                        Func<NpgsqlDataReader, int, IBasicGeometry> gr;
                        int gIndex;
                        ColumMapper columMapper;

                        res = new FeatureSet(GetFeatureType(r, out table, out gr, out gIndex, out columMapper));

                        var values = new object[r.FieldCount];
                        table.BeginLoadData();
                        do
                        {
                            var g = gr(r, gIndex);
                            var num = r.GetValues(values);
                            var f = res.AddFeature(g);
                            var dr = table.LoadDataRow(columMapper.Map(values), true);
                            f.DataRow = dr;

                        } while (r.Read());

                        table.EndLoadData();
                    }
                }
                cmd.Dispose();
            }
            return res;
        }

        private class ColumMapper
        {
            private readonly Dictionary<int, int> _map = new Dictionary<int, int>();

            public object[] Map(object[] values)
            {
                var res = new object[_map.Count];
                foreach (var t in _map)
                    res[t.Value] = values[t.Key];

                return res;
            }

            public void AddMap(int o, int t)
            {
                _map.Add(o, t);
            }

        }

        private static class PostGisServerReaderUtility
        {
            private static readonly PostGisReader GeometryReader = new PostGisReader();

            public static IBasicGeometry ReadGeometry(NpgsqlDataReader reader, int index)
            {
                return GeometryReader.Read((byte[])reader.GetValue(index)).ToDotSpatial();
            }

            public static IBasicGeometry ReadGeography(NpgsqlDataReader reader, int index)
            {
                return GeometryReader.Read((byte[]) reader.GetValue(index)).ToDotSpatial();
            }
        }

        private static FeatureType GetFeatureType(NpgsqlDataReader r, out DataTable table,
            out Func<NpgsqlDataReader, int, IBasicGeometry> gc, out int gIndex, out ColumMapper mapper)
        {
            table = new DataTable();
            mapper = new ColumMapper();
            gIndex = -1;
            gc = null;

            var res = FeatureType.Unspecified;

            for (var i = 0; i < r.FieldCount; i++)
            {
                var t = r.GetFieldType(i);
                if (t == null)
                    throw new InvalidOperationException("Could not get column type");

                if (t == typeof(byte[]))
                {
                    gc = PostGisServerReaderUtility.ReadGeometry;
                    gIndex = i;
                    res = gc(r, gIndex).FeatureType;
                }
                else
                {
                    table.Columns.Add(r.GetName(i), t);
                    mapper.AddMap(i, table.Columns.Count-1);
                }
            }

            if (gIndex == -1)
                throw new InvalidOperationException("No geometry column found");
            return res;
        }
    }
}
