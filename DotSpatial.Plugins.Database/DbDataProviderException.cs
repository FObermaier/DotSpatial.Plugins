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

namespace DotSpatial.Plugins.Database
{
    /// <summary>
    /// DbDataProviderException class
    /// </summary>
    [Serializable]
    public class DbDataProviderException : Exception
    {
        /// <inheritdoc/>
        public DbDataProviderException() { }
        /// <inheritdoc/>
        public DbDataProviderException(string message) : base(message) { }
        /// <inheritdoc/>
        public DbDataProviderException(string message, Exception inner) : base(message, inner) { }
        /// <inheritdoc/>
        protected DbDataProviderException( 
            System.Runtime.Serialization.SerializationInfo info, 
            System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }
    }
}