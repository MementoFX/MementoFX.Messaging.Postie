using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MementoFX.Messaging.Postie
{
    /// <summary>
    /// Provides the implementation of an in-memory synchronous bus
    /// </summary>
    public sealed class InMemoryBus : IBus, IEventDispatcher
    {
        private static IDictionary<Type, Type> registeredSagas = new Dictionary<Type, Type>();
        private static IList<Type> registeredHandlers = new List<Type>();

        /// <summary>
        /// Gets or sets the instance of the type resolver used by the bus to resolve dependencies
        /// </summary>
        public ITypeResolver TypeResolver {get; private set;}

        /// <summary>
        /// Creates a new instance of the bus
        /// </summary>
        /// <param name="container">The instance of the container used by the bus to resolve dependencies</param>
        public InMemoryBus(ITypeResolver container)
        {
            if(container==null)
                throw new ArgumentNullException(nameof(container));
            TypeResolver = container;
        }

        void IEventDispatcher.Dispatch<T>(T @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));
            this._Send(@event);
        }

        void IBus.RegisterHandler<T>()
        {
            registeredHandlers.Add(typeof(T));
        }

        void IBus.RegisterSaga<T>()
        {
            Type sagaType = typeof(T);
            if (sagaType.GetInterfaces().Where(i => i.Name.StartsWith(typeof(IAmStartedBy<>).Name)).Count() != 1)
            {
                throw new InvalidOperationException("The specified saga must implement the IAmStartedBy<T> interface only once.");
            }
            var messageType = sagaType.
                GetInterfaces().
                Where(i => i.Name.StartsWith(typeof(IAmStartedBy<>).Name)).
                First().
                GenericTypeArguments.
                First();
            registeredSagas.Add(messageType, sagaType);
        }

        void IBus.Send<T>(T command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            this._Send(command);
        }

        private void BootRegisteredSagas<T>(T message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            Type messageType = message.GetType();
            var openInterface = typeof(IAmStartedBy<>);
            var closedInterface = openInterface.MakeGenericType(messageType);
            var sagasToStartup = from s in registeredSagas.Values
                                 where closedInterface.IsAssignableFrom(s)
                                 select s;
            if (sagasToStartup == null)
                return;
            foreach (var s in sagasToStartup)
            {
                dynamic sagaInstance = TypeResolver.Resolve(s);
                sagaInstance.Handle((dynamic)message);
            }
        }

        private void DeliverMessageToAlreadyRunningSagas<T>(T message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            Type messageType = message.GetType();
            var openInterface = typeof(IHandleMessages<>);
            var closedInterface = openInterface.MakeGenericType(messageType);
            var sagasToNotify = from s in registeredSagas.Values
                                 where closedInterface.IsAssignableFrom(s)
                                 select s;
            if (sagasToNotify == null)
                return;
            foreach (var s in sagasToNotify)
            {
                dynamic sagaInstance = TypeResolver.Resolve(s);
                sagaInstance.Handle((dynamic)message);
            }
        }

        private void DeliverMessageToRegisteredHandlers<T>(T message)
        {
            Type messageType = message.GetType();
            var openInterface = typeof(IHandleMessages<>);
            var closedInterface = openInterface.MakeGenericType(messageType);
            var handlersToNotify = from h in registeredHandlers
                                   where closedInterface.IsAssignableFrom(h)
                                   select h;
            foreach (var h in handlersToNotify)
            {
                dynamic handlerInstance = TypeResolver.Resolve(h);
                handlerInstance.Handle((dynamic)message);
            }
        }

        void _Send(object message)
        {
            BootRegisteredSagas(message);
            DeliverMessageToAlreadyRunningSagas(message);
            DeliverMessageToRegisteredHandlers(message);
        }
    }
}
