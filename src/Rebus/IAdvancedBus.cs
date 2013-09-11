using System;

namespace Rebus
{
    /// <summary>
    /// Extends the capabilities of <see cref="IBus"/> with some more advanced features.
    /// </summary>
    public interface IAdvancedBus
    {
        /// <summary>
        /// Gives access to all the different event hooks that Rebus exposes.
        /// </summary>
        IRebusEvents Events { get; }

        /// <summary>
        /// Gives access to Rebus' batch operations.
        /// </summary>
        IRebusBatchOperations Batch { get; }

        /// <summary>
        /// Gives access to Rebus' routing operations.
        /// </summary>
        IRebusRouting Routing { get; }

        /// <summary>
        /// Gives access to Rebus' diagnostics operations and information.
        /// </summary>
        IRebusDiagnostics Diagnostics { get; }
    }

    public interface IRebusDiagnostics
    {
        
    }

    public class RebusDiagnostics : IRebusDiagnostics
    {
        
    }
}