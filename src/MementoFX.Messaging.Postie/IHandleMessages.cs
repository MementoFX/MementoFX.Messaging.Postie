using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MementoFX.Messaging.Postie
{
    /// <summary>
    /// Allows an handler or saga to request notifications 
    /// for a specific message type
    /// </summary>
    /// <typeparam name="T">The type of the message</typeparam>
    public interface IHandleMessages<in T>
    {
        /// <summary>
        /// Allows for the management of the notified message
        /// </summary>
        /// <param name="message">The message</param>
        void Handle(T message);
    }
}
