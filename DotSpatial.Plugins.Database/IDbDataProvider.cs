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

using DotSpatial.Data;

namespace DotSpatial.Plugins.Database
{
    /// <summary>
    /// IDbDataProvider is what you implement to expand the data handling methods of DotSpatial with databases
    /// </summary>
    public interface IDbDataProvider
    {
        #region Methods
        /*
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
        /// <param name="initialCommands">A list of initial SQL commands to be executed prior to using the database</param>
        /// <returns>A List of IDataSets to be added to the Map.  These can also be groups of datasets.</returns>
        IDataSet Open(string connectionString, string tablespace, string schema, string table, params string[] initialCommands);
        */
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
        /// <returns>A List of IDataSets to be added to the Map. These can also be groups of datasets.</returns>
        IDataSet Open(string connectionString, string tablespace, string schema, string table, string fidColumn, string geometryColumn);

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
        IDataSet Open(string connectionString, string schema, string table, string fidColumn, string geometryColumn);

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
        IDataSet Open(string connectionString, string table, string fidColumn, string geometryColumn);

        #endregion

        #region Properties

        /// <summary>
        /// Gets a prefereably short name that identifies this data provider.  Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The name of the tablespace where the table is stored. Default is empty
        /// </summary>
        string TableSpace { get; }
        
        /// <summary>
        /// The name of the schema the <see cref="Table"/> is located in, e.g. public, dbo
        /// </summary>
        string Schema { get; }

        /// <summary>
        /// The name of the table to get the data from
        /// </summary>
        string Table { get;  }

        /// <summary>
        /// Gets or sets the progess handler to use.
        /// </summary>
        IProgressHandler ProgressHandler{ get; set; }

        /// <summary>
        /// This provides a basic description of what your provider does.
        /// </summary>
        string Description { get; }


        /// <summary>
        /// Clause to Limit the resultset
        /// </summary>
        string LimitClause { get; }

        /// <summary>
        /// Format string to decorate database entities, e.g. PostgreSql: "\"{0}\", SqlServer: "[{0}]";
        /// </summary>
        string DecorateEntities { get; }

        /// <summary>
        /// Format string to decorate database literals, e.g. PostgreSql: "$${0}$$", SqlServer: "'{0}'";
        /// </summary>
        string DecorateLiterals { get; }

        #endregion
    }
}

