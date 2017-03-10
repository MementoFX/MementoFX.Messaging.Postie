using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Messaging.Postie
{
    /// <summary>
    /// Provides the definition for Postie's buses
    /// </summary>
    public interface IBus
    {
        /// <summary>
        /// Sends a command in order to be dispatched
        /// </summary>
        /// <typeparam name="T">The type of the command</typeparam>
        /// <param name="command">The command to be dispatched</param>
        void Send<T>(T command) where T : Command;

        /// <summary>
        /// Registers a saga
        /// </summary>
        /// <typeparam name="T">The type of the Saga to be registered</typeparam>
        void RegisterSaga<T>() where T : Saga;

        /// <summary>
        /// Registers an handler
        /// </summary>
        /// <typeparam name="T">The type of the handler to be registered</typeparam>
        void RegisterHandler<T>();
    }
}
