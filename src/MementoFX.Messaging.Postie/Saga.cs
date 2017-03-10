using Memento.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Messaging.Postie
{
    /// <summary>
    /// This class is used to define sagas containing data and handling a message.
    /// To handle more message types, implement <see cref="IHandleMessages{T}" />
    /// for the relevant types.
    /// To signify that the receipt of a message should start this saga,
    /// implement <see cref="IAmStartedBy{T}" /> for the relevant message type.
    /// </summary>
    public abstract class Saga
    {
        /// <summary>
        /// Gets or sets the bus instance available to the saga
        /// </summary>
        public IBus Bus { get; private set; }

        /// <summary>
        /// Gets or sets a reference to the event store available to the saga
        /// </summary>
        public IEventStore EventStore { get; private set; }

        /// <summary>
        /// Gets or sets the repository instance available to the saga
        /// </summary>
        public IRepository Repository { get; private set; }

        /// <summary>
        /// Creates a new instance of a saga
        /// </summary>
        public Saga()
        {

        }

        /// <summary>
        /// Creates a new instance of a saga
        /// </summary>
        /// <param name="repository">The repository to be used by the saga instance</param>
        public Saga(IRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));

            Repository = repository;
        }

        /// <summary>
        /// Creates a new instance of a saga
        /// </summary>
        /// <param name="bus">The bus to be used by the saga instance</param>
        /// <param name="repository">The repository to be used by the saga instance</param>
        public Saga(IBus bus, IRepository repository)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            Bus = bus;
            Repository = repository;
        }

        /// <summary>
        /// Creates a new instance of a saga
        /// </summary>
        /// <param name="bus">The bus to be used by the saga instance</param>
        /// <param name="eventStore">The event store to be used by the saga instance</param>
        /// <param name="repository">The repository to be used by the saga instance</param>
        public Saga(IBus bus, IEventStore eventStore, IRepository repository)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));
            if (eventStore == null)
                throw new ArgumentNullException(nameof(eventStore));
            if (repository == null)
                throw new ArgumentNullException(nameof(repository));
            Bus = bus;
            EventStore = eventStore;
            Repository = repository;
        }
    }
}
