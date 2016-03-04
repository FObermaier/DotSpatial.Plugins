using System.Data.Common;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace DotSpatial.Plugins.Database
{
    /// <summary>
    /// Database appropriate variant of FeatureSet
    /// </summary>
    /// <typeparam name="TVectorProvider">The vector provider type</typeparam>
    /// <typeparam name="TConnection">The connection type</typeparam>
    public class DbFeatureSet<TVectorProvider, TConnection> : FeatureSet
        where TVectorProvider : DbVectorProvider<TConnection>
        where TConnection : DbConnection, new() 
    {
        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="featureType"></param>
        internal DbFeatureSet(FeatureType featureType)
            :base(featureType)
        {
        }


        public override void SaveAs(string fileName, bool overwrite)
        {
            //ATM do nothing
        }
    }
}