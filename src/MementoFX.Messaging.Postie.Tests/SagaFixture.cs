using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Memento.Persistence;
using Moq;
using NUnit.Framework;
using SharpTestsEx;

namespace Memento.Messaging.Postie.Tests
{
    [TestFixture]
    public class SagaFixture
    {
        [TestFixture]
        public class ConstructorWithSingleParameter
        {
            [Test]
            public void Ctor_should_throw_ArgumentNullException_on_null_repository_and_value_of_parameter_should_be_repository()
            {
                Executing.This(() => new TestableSaga(null))
                               .Should()
                               .Throw<ArgumentNullException>()
                               .And
                               .ValueOf
                               .ParamName
                               .Should()
                               .Be
                               .EqualTo("repository");
            }

            [Test]
            public void Ctor_should_properly_set_repository_property()
            {
                var mock = new Mock<IRepository>().Object;
                var saga = new TestableSaga(mock);
                Assert.AreEqual(mock, saga.Repository);
            }
        }

        [TestFixture]
        public class ConstructorWithTwoParameters
        {
            [Test]
            public void Ctor_should_throw_ArgumentNullException_on_null_bus_and_value_of_parameter_should_be_bus()
            {
                Executing.This(() => new TestableSaga(null, null))
                               .Should()
                               .Throw<ArgumentNullException>()
                               .And
                               .ValueOf
                               .ParamName
                               .Should()
                               .Be
                               .EqualTo("bus");
            }

            [Test]
            public void Ctor_should_throw_ArgumentNullException_on_null_repository_and_value_of_parameter_should_be_repository()
            {
                var mock = new Mock<IBus>().Object;
                Executing.This(() => new TestableSaga(mock, null))
                               .Should()
                               .Throw<ArgumentNullException>()
                               .And
                               .ValueOf
                               .ParamName
                               .Should()
                               .Be
                               .EqualTo("repository");
            }

            [Test]
            public void Ctor_should_properly_set_properties()
            {
                var bus = new Mock<IBus>().Object;
                var repository = new Mock<IRepository>().Object;
                var saga = new TestableSaga(bus, repository);
                Assert.AreEqual(bus, saga.Bus);
                Assert.AreEqual(repository, saga.Repository);
            }
        }

        [TestFixture]
        public class ConstructorWithThreeParameters
        {
            [Test]
            public void Ctor_should_throw_ArgumentNullException_on_null_bus_and_value_of_parameter_should_be_bus()
            {
                Executing.This(() => new TestableSaga(null, null, null))
                               .Should()
                               .Throw<ArgumentNullException>()
                               .And
                               .ValueOf
                               .ParamName
                               .Should()
                               .Be
                               .EqualTo("bus");
            }

            [Test]
            public void Ctor_should_throw_ArgumentNullException_on_null_eventStore_and_value_of_parameter_should_be_eventStore()
            {
                var mock = new Mock<IBus>().Object;
                var repository = new Mock<IRepository>().Object;
                Executing.This(() => new TestableSaga(mock, null, repository))
                               .Should()
                               .Throw<ArgumentNullException>()
                               .And
                               .ValueOf
                               .ParamName
                               .Should()
                               .Be
                               .EqualTo("eventStore");
            }

            [Test]
            public void Ctor_should_throw_ArgumentNullException_on_null_repository_and_value_of_parameter_should_be_repository()
            {
                var mock = new Mock<IBus>().Object;
                var eventStore = new Mock<IEventStore>().Object;
                Executing.This(() => new TestableSaga(mock, eventStore, null))
                               .Should()
                               .Throw<ArgumentNullException>()
                               .And
                               .ValueOf
                               .ParamName
                               .Should()
                               .Be
                               .EqualTo("repository");
            }

            [Test]
            public void Ctor_should_properly_set_properties()
            {
                var bus = new Mock<IBus>().Object;
                var eventStore = new Mock<IEventStore>().Object;
                var repository = new Mock<IRepository>().Object;
                var saga = new TestableSaga(bus, eventStore, repository);
                Assert.AreEqual(bus, saga.Bus);
                Assert.AreEqual(eventStore, saga.EventStore);
                Assert.AreEqual(repository, saga.Repository);
            }
        }

        public class TestableSaga : Saga
        {
            public TestableSaga(IRepository repository) : base(repository)
            {

            }

            public TestableSaga(IBus bus, IRepository repository) : base(bus, repository)
            {

            }

            public TestableSaga(IBus bus, IEventStore eventStore, IRepository repository) : base(bus, eventStore, repository)
            {
            }
        }
    }
}
