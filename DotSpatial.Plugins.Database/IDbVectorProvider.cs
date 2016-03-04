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
using DotSpatial.Projections;
using DotSpatial.Topology;

namespace DotSpatial.Plugins.Database
{
    /// <summary>
    /// IDbVectorProvider
    /// </summary>
    public interface IDbVectorProvider : IDbDataProvider
    {
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
        IFeatureSet CreateNew(string connectionString, string tablespace, string schema, string table, string geometryColumn, FeatureType featureType, bool inRam, IProgressHandler progressHandler, params string[] initialCommands);

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
        FeatureType GetFeatureType(string connectionString, string tablespace, string schema, string table, string geometryColumn, params string[] initialCommands);

        /// <summary>
        /// Clause to perform transfromation to WKB representation of geometry data, 
        /// e.g. "ST_AsBinary{0}" for PostGis
        /// </summary>
        string AsBinaryClause { get; }

        /// <summary>
        /// Function to get the ProjectionInfo
        /// </summary>
        ProjectionInfo GetProjection();
        
    }
}