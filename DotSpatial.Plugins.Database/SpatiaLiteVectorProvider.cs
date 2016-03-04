using System;
using System.Data.Common;
using System.Data.SQLite;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Topology;

namespace DotSpatial.Plugins.Database
{
    /// <summary>
    /// SpatiaLite Vector Provider
    /// </summary>
    public class SpatiaLiteVectorProvider : DbVectorProvider<SQLiteConnection>
    {
        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control.  Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="connectionString">A string specifying the connection parameters.</param>
        /// <param name="schema">The name of the schema where the table is located</param>
        /// <param name="table">The name of the database table to get the data from</param>
        /// <param name="fidColumn">The name of the column that holds the unique primary key</param>
        /// <param name="geometryColumn">The name  of the column that holds the geometry</param>
        /// <returns>A List of IDataSets to be added to the Map.  These can also be groups of datasets.</returns>
        public override IDataSet Open(string connectionString, string schema, string table, string fidColumn, string geometryColumn)
        {
            return Open(connectionString, string.Empty, schema, table, fidColumn,geometryColumn);
        }

        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control.  Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="connectionString">A string specifying the connection parameters.</param>
        /// <param name="table">The name of the database table to get the data from</param>
        /// <param name="fidColumn">The name of the column that holds the unique primary key</param>
        /// <param name="geometryColumn">The name  of the column that holds the geometry</param>
        /// <returns>A List of IDataSets to be added to the Map.  These can also be groups of datasets.</returns>
        public override IDataSet Open(string connectionString, string table, string fidColumn, string geometryColumn)
        {
            return Open(connectionString, string.Empty, string.Empty, table, fidColumn, geometryColumn);
        }

        /// <summary>
        /// Gets a prefereably short name that identifies this data provider.  Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        public override string Name
        {
            get { return "SpatiaLite"; }
        }

        /// <summary>
        /// This provides a basic description of what your provider does.
        /// </summary>
        public override string Description
        {
            get { return "Provides access to spatial layers in spatialite databases"; }
        }

        /// <summary>
        /// Clause to perform transfromation to WKB representation of geometry data, 
        /// e.g. "ST_AsBinary{0}" for PostGis
        /// </summary>
        public override string AsBinaryClause
        {
            get { return "ST_AsBinary({0})"; }
        }

        /// <summary>
        /// Clause to Limit the resultset
        /// </summary>
        public override string LimitClause
        {
            get { return "LIMIT 1"; }
        }

        /// <summary>
        /// Format string to decorate database entities, e.g. PostgreSql: "\"{0}\", SqlServer: "[{0}]";
        /// </summary>
        public override string DecorateEntities
        {
            get { return "\"{0}\""; }
        }

        /// <summary>
        /// Format string to decorate database entities, e.g. PostgreSql: "\"{0}\", SqlServer: "[{0}]";
        /// </summary>
        public override string DecorateLiterals
        {
            get { return "'{0}'"; }
        }

        /// <summary>
        /// Function to get the ProjectionInfo
        /// </summary>
        public override ProjectionInfo GetProjection()
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string command =
                    string.Format(
                        "SELECT \"proj4text\" AS a FROM \"spatial_ref_sys\" " +
                        "INNER JOIN \"geometry_columns\" as b USING (\"srid\") "+
                        "WHERE b.\"f_table_name\"={0} AND \"f_geometry_column\"={1};",
                        string.Format(DecorateLiterals, Table), string.Format(DecorateLiterals, GeometryColumn));
                SQLiteCommand cmd = new SQLiteCommand(command, connection);
                return ProjectionInfo.FromProj4String((string)cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// This create new method implies that this provider has the priority for creating a new file.
        /// An instance of the dataset should be created and then returned.  By this time, the fileName
        /// will already be checked to see if it exists, and deleted if the user wants to overwrite it.
        /// </summary>
        /// <param name="connectionString">The string fileName to test</param>
        /// <param name="tablespace">The name of the tablespace where the table is stored</param>
        /// <param name="schema">The name of the schema where the table is located</param>
        /// <param name="table">The name of the database table to get the data from</param>
        /// <param name="geometryColumn">The name  of the column that holds the geometry</param>
        /// <param name="featureType">Point, Line, Polygon etc.  Sometimes this will be specified, sometimes it will be "Unspecified"</param>
        /// <param name="inRam">Boolean, true if the dataset should attempt to store data entirely in ram</param>
        /// <param name="progressHandler">An IProgressHandler for status messages.</param>
        /// <param name="initialCommands">A list of initial SQL commands to be executed prior to using the database</param>
        /// <returns>An IFeatureSet</returns>
        public override IFeatureSet CreateNew(string connectionString, string tablespace, string schema, string table, string geometryColumn, FeatureType featureType, bool inRam, IProgressHandler progressHandler, params string[] initialCommands)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This tests the specified file in order to determine what type of vector the file contains.
        /// This returns unspecified if the file format is not supported by this provider.
        /// </summary>
        /// <param name="connectionString">The string fileName to test</param>
        /// <param name="tablespace">The name of the tablespace where the table is stored</param>
        /// <param name="schema">The name of the schema where the table is located</param>
        /// <param name="table">The name of the database table to get the data from</param>
        /// <param name="geometryColumn">The name  of the column that holds the geometry</param>
        /// <param name="initialCommands">A list of initial SQL commands to be executed prior to using the database</param>
        /// <returns>A FeatureType clarifying what sort of features are stored on the data type.</returns>
        public override FeatureType GetFeatureType(string connectionString, string tablespace, string schema, string table, string geometryColumn, params string[] initialCommands)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string command =
                    string.Format(
                        "SELECT \"type\" FROM \"geometry_columns\" WHERE \"f_table_name\"={0} and \"f_geometry_column\"={1};",
                        string.Format(DecorateLiterals, table), string.Format(DecorateLiterals, geometryColumn));
                SQLiteCommand cmd = new SQLiteCommand(command, connection);
                switch ((string)cmd.ExecuteScalar())
                {
                    case "POINT":
                        return FeatureType.Point;
                    case "MULTIPOINT":
                        return FeatureType.MultiPoint;
                    case "LINESTRING":
                    case "MULTILINESTRING":
                        return FeatureType.Line;
                    case "POLYGON":
                    case "MULTIPOLYGON":
                        return FeatureType.Polygon;
                    default:
                        return FeatureType.Unspecified;
                }
            }
        }

        protected override string[] InitialCommands
        {
            get
            {
                //              SELECT load_extension('{0}') 
                return new[] { "SELECT load_extension('libspatialite-2.dll');" };
            }
        }

        protected override DbDataAdapter GetDataAdapter(string command, SQLiteConnection connection)
        {
            return new SQLiteDataAdapter(command, connection);
        }
    }
}