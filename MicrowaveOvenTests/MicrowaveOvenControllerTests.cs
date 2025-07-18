using Moq;
using MicrowaveOven;

namespace MicrowaveOvenTests
{
    [TestClass]
    public class MicrowaveOvenControllerTests
    {
        private Mock<IMicrowaveOvenHw> _mockMicrowaveOvenHw;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockMicrowaveOvenHw = new Mock<IMicrowaveOvenHw>();
        }

        [TestMethod]
        public void ShouldInitializeLightStateToOff()
        {
            var sut = CreateSut();

            Assert.AreEqual(PowerState.Off, sut.LightState);
        }

        [TestMethod]
        public void ShouldSubscribeToStartButtonPressed()
        {
            CreateSut();

            _mockMicrowaveOvenHw.VerifyAdd(hw => hw.StartButtonPressed += It.IsAny<EventHandler>(), Times.Once);
        }

        [TestMethod]
        public void ShouldSubscribeToDoorOpenChanged()
        {
            CreateSut();

            _mockMicrowaveOvenHw.VerifyAdd(hw => hw.DoorOpenChanged += It.IsAny<Action<bool>>(), Times.Once);
        }

        [TestMethod]
        public void ShouldSetLightStateToOnWhenDoorIsOpened()
        {
            var sut = CreateSut();

            _mockMicrowaveOvenHw.Raise(hw => hw.DoorOpenChanged += null, true);

            Assert.AreEqual(PowerState.On, sut.LightState);
        }

        [TestMethod]
        public void ShouldSetLightStateToOffWhenDoorIsClosed()
        {
            var sut = CreateSut();
            _mockMicrowaveOvenHw.Raise(hw => hw.DoorOpenChanged += null, true);

            _mockMicrowaveOvenHw.Raise(hw => hw.DoorOpenChanged += null, false);

            Assert.AreEqual(PowerState.Off, sut.LightState);
        }

        [TestMethod]
        public void ShouldTurnOffHeaterWhenDoorIsOpened()
        {
            CreateSut();

            _mockMicrowaveOvenHw.Raise(hw => hw.DoorOpenChanged += null, true);

            _mockMicrowaveOvenHw.Verify(hw => hw.TurnOffHeater(), Times.Once);
        }

        [TestMethod]
        public void ShouldNotTurnOffHeaterWhenDoorIsClosed()
        {
            CreateSut();

            _mockMicrowaveOvenHw.Raise(hw => hw.DoorOpenChanged += null, false);

            _mockMicrowaveOvenHw.Verify(hw => hw.TurnOffHeater(), Times.Never);
        }

        [TestMethod]
        public void ShouldNotTurnOnHeaterWhenStartButtonPressedAndDoorIsOpen()
        {
            CreateSut();
            _mockMicrowaveOvenHw.Setup(hw => hw.DoorOpen).Returns(true);

            _mockMicrowaveOvenHw.Raise(hw => hw.StartButtonPressed += null, EventArgs.Empty);

            _mockMicrowaveOvenHw.Verify(hw => hw.TurnOnHeater(), Times.Never);
        }

        [TestMethod]
        public void ShouldTurnOnHeaterWhenStartButtonPressedAndDoorIsClosed()
        {
            CreateSut();
            _mockMicrowaveOvenHw.Setup(hw => hw.DoorOpen).Returns(false);

            _mockMicrowaveOvenHw.Raise(hw => hw.StartButtonPressed += null, EventArgs.Empty);

            _mockMicrowaveOvenHw.Verify(hw => hw.TurnOnHeater(), Times.Once);
        }

        [TestMethod]
        public void ShouldHandleStartButtonPressedWithSenderAndEventArgs()
        {
            var sut = CreateSut();
            _mockMicrowaveOvenHw.Setup(hw => hw.DoorOpen).Returns(false);
            var sender = new object();
            var eventArgs = EventArgs.Empty;

            _mockMicrowaveOvenHw.Raise(hw => hw.StartButtonPressed += null, sender, eventArgs);

            _mockMicrowaveOvenHw.Verify(hw => hw.TurnOnHeater(), Times.Once);
        }

        [TestMethod]
        public void ShouldReflectCurrentDoorStateAfterMultipleDoorChanges()
        {
            var sut = CreateSut();

            _mockMicrowaveOvenHw.Raise(hw => hw.DoorOpenChanged += null, true);
            Assert.AreEqual(PowerState.On, sut.LightState);

            _mockMicrowaveOvenHw.Raise(hw => hw.DoorOpenChanged += null, false);
            Assert.AreEqual(PowerState.Off, sut.LightState);

            _mockMicrowaveOvenHw.Raise(hw => hw.DoorOpenChanged += null, true);
            Assert.AreEqual(PowerState.On, sut.LightState);

            _mockMicrowaveOvenHw.Raise(hw => hw.DoorOpenChanged += null, false);
            Assert.AreEqual(PowerState.Off, sut.LightState);
        }

        private MicrowaveOvenController CreateSut()
        {
            return new MicrowaveOvenController(_mockMicrowaveOvenHw.Object);
        }
    }
}
