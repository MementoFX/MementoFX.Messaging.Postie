using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Messaging.Postie
{
    /// <summary>
    /// Defines the interface of a type resolver 
    /// </summary>
    public interface ITypeResolver
    {
        /// <summary>
        /// Resolves an instance of the requested type with the given name from the container.
        /// </summary>
        /// <param name="t">Type of object to get from the container.</param>
        /// <returns>The retrieved object.</returns>
        object Resolve(Type t);
    }
}
