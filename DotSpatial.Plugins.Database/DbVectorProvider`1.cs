// ********************************************************************************************************
// Product Name: DotSpatial.Data
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and 
// limitations under the License. 
//
// The Initial Developer of this Original Code is Felix Obermaier. Created 2011.01.12 
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Topology;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Plugins.Database
{
    ///<summary>
    /// DbDataProvider base class
    ///</summary>
    ///<typeparam name="T"></typeparam>
    public abstract class DbVectorProvider<T> : IDbVectorProvider
        where T: DbConnection, new()
    {
        protected string ConnectionString;
        protected virtual string[] InitialCommands { get { return null; } }

        /// <summary>
        /// This open method is only called if this plugin has been given priority for one
        /// of the file extensions supported in the DialogReadFilter property supplied by
        /// this control.  Failing to provide a DialogReadFilter will result in this plugin
        /// being added to the list of DataProviders being supplied under the Add Other Data
        /// option in the file menu.
        /// </summary>
        /// <param name="connectionString">A string specifying the connection parameters.</param>
        /// <param name="tablespace">The name of the tablespace where the table is stored</param>
        /// <param name="schema">The name of the schema where the table is located</param>
        /// <param name="table">The name of the database table to get the data from</param>
        /// <param name="fidColumn">The name of the column that holds the unique primary key</param>
        /// <param name="geometryColumn">The name  of the column that holds the geometry</param>
        /// <returns>A List of IDataSets to be added to the Map.  These can also be groups of datasets.</returns>
        public IDataSet Open(string connectionString, string tablespace, string schema, string table, string fidColumn, string geometryColumn)
        {
            ConnectionString = connectionString;

            TableSpace = tablespace;
            Schema = schema;
            Table = table;
            FidColumn = fidColumn;
            GeometryColumn = geometryColumn;

            SetQualifyFormatStrings();

            IFeatureSet featureSet = new DbFeatureSet<DbVectorProvider<T>, T>(
                GetFeatureType(connectionString, tablespace, schema, table, geometryColumn, InitialCommands));
            //featureSet.AddFid();

            using(T c1 = OpenConnection(connectionString))
            using(T connection = OpenConnection(connectionString))
            {
                string[] otherColumns = GetOtherColumns(c1);
                string commandString =
                    string.Format("SELECT {0}, {1}, {2} FROM {3};", 
                        string.Format(AsBinaryClause, QualifiedColumn(geometryColumn)),
                        QualifiedColumn(FidColumn), QualifiedColumn(otherColumns), QualifiedTable);
                
                // Get schema information
                DataAdapter da = GetDataAdapter(commandString.Substring(0, commandString.Length-1) + LimitClause +";", connection);
                System.Data.DataSet ds = new System.Data.DataSet();
                da.FillSchema(ds, SchemaType.Source);
                DataTable dt = ds.Tables[0];
                if (dt == null)
                    throw new DbDataProviderException("Unable to get schema table");

                // Add columns to the feature sets datatable
                for (int i = 1; i < dt.Columns.Count; i++)
                {
                    DataColumn dc = dt.Columns[i];
                    DataColumn newDataColumn = featureSet.DataTable.Columns.Add(dc.ColumnName, dc.DataType);
                    newDataColumn.AllowDBNull = dc.AllowDBNull;
                    newDataColumn.AutoIncrement = dc.AutoIncrement;
                    newDataColumn.AutoIncrementSeed = dc.AutoIncrementSeed;
                    newDataColumn.AutoIncrementStep = dc.AutoIncrementStep;
                    newDataColumn.Caption = dc.Caption;
                    newDataColumn.DateTimeMode = dc.DateTimeMode;
                    newDataColumn.DefaultValue = dc.DefaultValue;
                    newDataColumn.Expression = dc.Expression;
                    newDataColumn.MaxLength= dc.MaxLength;
                    newDataColumn.ReadOnly = dc.ReadOnly;
                    newDataColumn.Prefix = dc.Prefix;
                    newDataColumn.Unique = dc.Unique;
                }
                
                // Read whole dataset
                DbCommand cmd = connection.CreateCommand();
                cmd.CommandText = commandString; 
                DbDataReader dr = cmd.ExecuteReader(CommandBehavior.Default);
                if (dr.HasRows)
                {
                    WkbReader wkbReader = new WkbReader();
                    dt = featureSet.DataTable;
                    //int fid = 0;
                    while (dr.Read())
                    {
                        if (!dr.IsDBNull(0))
                        {
                            IGeometry geom = wkbReader.Read((byte[]) dr[0]);
                            IFeature f = featureSet.AddFeature(geom);
                            DataRow row = f.DataRow = dt.NewRow();
                            //row[0] = fid++;
                            for (int i = 1; i < dt.Columns.Count; i++)
                                //row[i] = dr[i];
                                row[i-1] = dr[i];
                        }
                    }
                }
            }
            featureSet.UpdateExtent();
            featureSet.Name = string.Format(QualifiedColumn(GeometryColumn));
            // Assign projection information to feature set
            featureSet.Projection = GetProjection();
            return featureSet;
        }

        #region private/protected helpers

        protected abstract DbDataAdapter GetDataAdapter(string sql, T connection); 

        protected T OpenConnection(string connectionString)
        {
            // Create connection
            T connection = new T { ConnectionString = connectionString };
            connection.Open();
            // Perform inital commands
            if (PerformInitialCommands(connection, InitialCommands))
                return connection;
            return null;
        }

        private string[] GetOtherColumns(T connection)
        {
            using (DbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format("SELECT * FROM {0} {1}", QualifiedTable, LimitClause);
                DbDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                if (dr.HasRows)
                {
                    List<string> otherColumns = new List<string>();
                    dr.Read();
                    DataTable dt = dr.GetSchemaTable();
                    if (dt == null)
                        throw new DbDataProviderException("Unable to get schema information");

                    foreach (DataRow row in dt.Rows)
                    {
                        if (string.Compare((string)row[0], FidColumn) == 0) continue;
                        if (string.Compare((string)row[0], GeometryColumn) == 0) continue;
                        otherColumns.Add((string)row[0]);
                    }
                    return otherColumns.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// Performs a sequence of initial commands. All of these commands must not fail.
        /// </summary>
        /// <param name="connection">The connection to the database</param>
        /// <param name="initialCommands">The SQL commands to perform</param>
        /// <returns>True if all is OK</returns>
        private static bool PerformInitialCommands(T connection, IEnumerable<string> initialCommands)
        {
            if (initialCommands == null)
                return true;

            try
            {
                using (DbCommand cmd = connection.CreateCommand())
                    foreach (string initialCommand in initialCommands)
                    {
                        cmd.CommandText = initialCommand;
                        cmd.ExecuteNonQuery();
                    }
                return true;
            }
            catch
            {
            }
            return false;

        }

        private string _qualifyTable;
        private string _qualifyColumn;

        private void SetQualifyFormatStrings()
        {
            _qualifyColumn = string.Format(DecorateEntities, "{1}") + "." + string.Format(DecorateEntities, "{0}");
            _qualifyTable = string.Format(DecorateEntities, "{0}");
            if (!String.IsNullOrEmpty(Schema))
                _qualifyTable = string.Format(DecorateEntities, "{1}") + "." + _qualifyTable; 
            if (!String.IsNullOrEmpty(TableSpace))
                _qualifyTable = string.Format(DecorateEntities, "{2}") + "." + _qualifyTable;

        }

        private string QualifiedTable { get { return string.Format(_qualifyTable, Table, Schema, TableSpace); } }
        
        private string QualifiedColumn( string column )
        {
            return string.Format(_qualifyColumn, column, Table);
        }

        private string QualifiedColumn(string[] columns)
        {
            string[] qualifiedColumns = new string[columns.Length];
            for (int i = 0; i < columns.Length; i++ )
                qualifiedColumns[i] = QualifiedColumn(columns[i]);
            return string.Join(",", qualifiedColumns);
        }

        #endregion

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
        public abstract IDataSet Open(string connectionString, string schema, string table, string fidColumn, string geometryColumn);

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
        public abstract IDataSet Open(string connectionString, string table, string fidColumn, string geometryColumn);

        /// <summary>
        /// Gets a prefereably short name that identifies this data provider.  Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        abstract public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public string TableSpace { get; private set; }

        /// <summary>
        /// The name of the schema the <see cref="Table"/> is stored in, e.g. public, dbo
        /// </summary>
        public string Schema { get; private set; }

        /// <summary>
        /// The name of the table to get the data from
        /// </summary>
        public string Table { get; private set; }

        /// <summary>
        /// The name of the column that holds the unique primary key
        /// </summary>
        public string FidColumn { get; private set; }

        /// <summary>
        /// The name of the column that holds the geometry column
        /// </summary>
        public string GeometryColumn { get; private set; }

        /// <summary>
        /// Gets or sets the progess handler to use.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        /// <summary>
        /// This provides a basic description of what your provider does.
        /// </summary>
        abstract public string Description { get; }

        /// <summary>
        /// Clause to perform transfromation to WKB representation of geometry data, 
        /// e.g. "ST_AsBinary{0}" for PostGis
        /// </summary>
        public abstract string AsBinaryClause { get; }

        /// <summary>
        /// Clause to Limit the resultset
        /// </summary>
        public abstract string LimitClause { get; }

        /// <summary>
        /// Format string to decorate database entities, e.g. PostgreSql: "\"{0}\", SqlServer: "[{0}]";
        /// </summary>
        public abstract string DecorateEntities { get ; }

        /// <summary>
        /// Format string to decorate database literals, e.g. PostgreSql: "$${0}$$", SqlServer: "'{0}'";
        /// </summary>
        public abstract string DecorateLiterals { get; }

        /// <summary>
        /// Function to get the ProjectionInfo
        /// </summary>
        public abstract ProjectionInfo GetProjection();

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
        public abstract IFeatureSet CreateNew(string connectionString, string tablespace, string schema, string table, string geometryColumn, FeatureType featureType, bool inRam, IProgressHandler progressHandler, params string[] initialCommands);

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
        public abstract FeatureType GetFeatureType(string connectionString, string tablespace, string schema, string table, string geometryColumn, params string[] initialCommands);
    }
}