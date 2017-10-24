using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MementoFX.Persistence;
using Moq;
using Xunit;
using SharpTestsEx;

namespace MementoFX.Messaging.Postie.Tests
{
    
    public class SagaFixture
    {
        
        public class ConstructorWithSingleParameter
        {
            [Fact]
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

            [Fact]
            public void Ctor_should_properly_set_repository_property()
            {
                var mock = new Mock<IRepository>().Object;
                var saga = new TestableSaga(mock);
                Assert.Equal(mock, saga.Repository);
            }
        }

        
        public class ConstructorWithTwoParameters
        {
            [Fact]
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

            [Fact]
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

            [Fact]
            public void Ctor_should_properly_set_properties()
            {
                var bus = new Mock<IBus>().Object;
                var repository = new Mock<IRepository>().Object;
                var saga = new TestableSaga(bus, repository);
                Assert.Equal(bus, saga.Bus);
                Assert.Equal(repository, saga.Repository);
            }
        }

        
        public class ConstructorWithThreeParameters
        {
            [Fact]
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

            [Fact]
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

            [Fact]
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

            [Fact]
            public void Ctor_should_properly_set_properties()
            {
                var bus = new Mock<IBus>().Object;
                var eventStore = new Mock<IEventStore>().Object;
                var repository = new Mock<IRepository>().Object;
                var saga = new TestableSaga(bus, eventStore, repository);
                Assert.Equal(bus, saga.Bus);
                Assert.Equal(eventStore, saga.EventStore);
                Assert.Equal(repository, saga.Repository);
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
