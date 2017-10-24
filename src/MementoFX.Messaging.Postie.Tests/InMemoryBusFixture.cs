using System;
using Moq;
using Xunit;
using SharpTestsEx;
using MementoFX;
using MementoFX.Messaging.Postie;
using MementoFX.Persistence;

namespace MementoFX.Messaging.Postie.Tests
{
    
    public class InMemoryBusFixture
    {
        
        public class Constructor
        {
            [Fact]
            public void Ctor_should_throw_ArgumentNullException_on_null_container_and_value_of_parameter_should_be_container()
            {
                Executing.This(() => new InMemoryBus(null))
                               .Should()
                               .Throw<ArgumentNullException>()
                               .And
                               .ValueOf
                               .ParamName
                               .Should()
                               .Be
                               .EqualTo("container");
            }
        }

        
        public class RegisterSagaMethod
        {
            [Fact]
            public void RegisterSaga_should_throw_InvalidOperationException_on_type_arguments_that_do_not_implement_IAmStartedBy_interface()
            {
                var containerMock = new Mock<ITypeResolver>().Object;
                IBus bus = new InMemoryBus(containerMock);
                Executing.This(() => bus.RegisterSaga<PretendingSaga>())
                    .Should()
                    .Throw<InvalidOperationException>();   
            }

            [Fact]
            public void RegisterSaga_should_throw_InvalidOperationException_sagas_that_implements_IAmStartedBy_more_than_once()
            {
                var containerMock = new Mock<ITypeResolver>().Object;
                IBus bus = new InMemoryBus(containerMock);
                Executing.This(() => bus.RegisterSaga<OverloadedSaga>())
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Fact]
            public void RegisterSaga_should_not_throw_InvalidOperationException_on_type_arguments_that_implement_IAmStartedBy_interface()
            {
                var containerMock = new Mock<ITypeResolver>().Object;
                IBus bus = new InMemoryBus(containerMock);
                bus.RegisterSaga<DummySaga>(); 
            }

            public class PretendingSaga : Saga
            {
                public PretendingSaga(IBus bus, IEventStore eventStore, IRepository repository)
                    : base(bus, eventStore, repository)
                {

                }

            }

            public class DummySaga : Saga,
                IAmStartedBy<DummySaga.DummyMessage>
            {
                public class DummyMessage
                {

                }

                public DummySaga(IBus bus, IEventStore eventStore, IRepository repository)
                    : base(bus, eventStore, repository)
                {

                }

                public void Handle(DummySaga.DummyMessage message)
                {
                    throw new NotSupportedException();
                }
            }

            public class OverloadedSaga : Saga,
                IAmStartedBy<OverloadedSaga.FooMessage>,
                IAmStartedBy<OverloadedSaga.BarMessage>
            {
                public class FooMessage
                {

                }
                public class BarMessage
                {

                }

                public OverloadedSaga(IBus bus, IEventStore eventStore, IRepository repository)
                    : base(bus, eventStore, repository)
                {

                }

                public void Handle(OverloadedSaga.BarMessage message)
                {
                    throw new NotImplementedException();
                }

                public void Handle(OverloadedSaga.FooMessage message)
                {
                    throw new NotImplementedException();
                }
            }
        }

        
        public class SendMethod
        {
            [Fact]
            public void Send()
            {
                var command = new InMemoryBusFixture.SendMethod.FakeSaga.StartCommand();
                var containerMock = new Mock<ITypeResolver>();
                containerMock.Setup(o => o.Resolve(typeof(FakeSaga)))
                                .Returns(new FakeSaga());
                IBus bus = new InMemoryBus(containerMock.Object);
                bus.RegisterSaga<FakeSaga>();
                bus.Send(command);
            }

            public class FakeSaga : Saga, IAmStartedBy<InMemoryBusFixture.SendMethod.FakeSaga.StartCommand>
            {
                public FakeSaga()
                    : base() { }

                public void Handle(FakeSaga.StartCommand message)
                {
                    
                }

                public class StartCommand : Command
                {

                }
            }
        }
    }
}
