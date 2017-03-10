using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Messaging.Postie
{
    /// <summary>
    /// Allows a saga to specify the type of its triggering event
    /// </summary>
    /// <typeparam name="T">The type of the event</typeparam>
    public interface IAmStartedBy<T>
    {
        /// <summary>
        /// Handles the triggering event
        /// </summary>
        /// <param name="message">The event</param>
        void Handle(T message); 
    }
}
