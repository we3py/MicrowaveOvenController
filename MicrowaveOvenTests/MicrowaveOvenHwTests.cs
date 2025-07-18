using MicrowaveOven;
using Moq;

namespace MicrowaveOvenTests
{
    [TestClass]
    public class MicrowaveOvenHwTests
    {
        private Mock<Heater> _mockHeater;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockHeater = new Mock<Heater>();
        }

        [TestMethod]
        public void ShouldInitializeDoorOpenToFalse()
        {
            var sut = CreateSut();

            Assert.IsFalse(sut.DoorOpen);
        }

        [TestMethod]
        public void ShouldStopHeaterWhenTurnOffIsCalledAndHeaterIsOn()
        {
            var sut = CreateSut();

            sut.TurnOnHeater();
            Assert.AreEqual(PowerState.On, sut.HeaterState);

            sut.TurnOffHeater();

            Assert.AreEqual(PowerState.Off, sut.HeaterState);
        }

        [TestMethod]
        public void ShouldRemainOffWhenTurnOffIsCalledAndHeaterIsOff()
        {
            var sut = CreateSut();
            Assert.AreEqual(PowerState.Off, sut.HeaterState);

            sut.TurnOffHeater();

            Assert.AreEqual(PowerState.Off, sut.HeaterState);
        }

        [TestMethod]
        public void ShouldStartHeaterWhenDoorIsClosed()
        {
            var sut = CreateSut();

            sut.TurnOnHeater();

            Assert.AreEqual(PowerState.On, sut.HeaterState);
            Assert.IsTrue(_mockHeater.Object.RemainingTime > 0);
        }

        [TestMethod]
        public void ShouldNotStartHeaterWhenDoorIsOpen()
        {
            var sut = CreateSut();
            sut.ChangeStateOfMicrowaveOvenDoors();

            sut.TurnOnHeater();

            Assert.AreEqual(PowerState.Off, sut.HeaterState);
        }

        [TestMethod]
        public void ShouldChangeStateOfMicrowaveOvenDoorsProperly()
        {
            var sut = CreateSut();

            Assert.IsFalse(sut.DoorOpen);

            sut.ChangeStateOfMicrowaveOvenDoors();

            Assert.IsTrue(sut.DoorOpen);

            sut.ChangeStateOfMicrowaveOvenDoors();

            Assert.IsFalse(sut.DoorOpen);
        }

        [TestMethod]
        public void ShouldStopHeaterWhenOpeningDoor()
        {
            var sut = CreateSut();
            sut.TurnOnHeater();
            Assert.AreEqual(PowerState.On, sut.HeaterState);

            sut.ChangeStateOfMicrowaveOvenDoors();

            Assert.AreEqual(PowerState.Off, sut.HeaterState);
        }

        [TestMethod]
        public void ShouldNotAffectHeaterStateWhenClosingDoor()
        {
            var sut = CreateSut();
            sut.ChangeStateOfMicrowaveOvenDoors();
            var initialHeaterState = sut.HeaterState;

            sut.ChangeStateOfMicrowaveOvenDoors();

            Assert.AreEqual(initialHeaterState, sut.HeaterState);
        }

        [TestMethod]
        public void ShouldRaiseDoorOpenChangedEvent()
        {
            var sut = CreateSut();
            var doorState = false;
            sut.DoorOpenChanged += (isOpen) => { doorState = isOpen; };

            sut.ChangeStateOfMicrowaveOvenDoors();

            Assert.IsTrue(doorState);
        }

        [TestMethod]
        public void ShouldRaiseStartButtonPressedEvent()
        {
            var sut = CreateSut();
            var eventRaised = false;
            sut.StartButtonPressed += (sender, args) => 
            { 
                eventRaised = true; 
            };

            sut.PressStartButton();

            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void ShouldResetHeaterWhenPressStopButtonAndHeaterHasRemainingTime()
        {
            var sut = CreateSut();
            sut.TurnOnHeater();
            Assert.IsTrue(_mockHeater.Object.RemainingTime > 0);

            sut.PressStopButton();

            Assert.AreEqual(0, _mockHeater.Object.RemainingTime);
            Assert.AreEqual(PowerState.Off, sut.HeaterState);
        }

        [TestMethod]
        public void ShouldNotAffectHeaterWhenPressStopButtonAndHeaterHasNoRemainingTime()
        {
            var sut = CreateSut();
            Assert.AreEqual(0, _mockHeater.Object.RemainingTime);
            var initialState = sut.HeaterState;

            sut.PressStopButton();

            Assert.AreEqual(0, _mockHeater.Object.RemainingTime);
            Assert.AreEqual(initialState, sut.HeaterState);
        }

        [TestMethod]
        public void ShouldRaiseDoorOpenChangedEventWithCorrectState()
        {
            var sut = CreateSut();
            var doorStates = new List<bool>();
            sut.DoorOpenChanged += (isOpen) => doorStates.Add(isOpen);

            sut.ChangeStateOfMicrowaveOvenDoors();
            sut.ChangeStateOfMicrowaveOvenDoors();
            sut.ChangeStateOfMicrowaveOvenDoors();

            Assert.AreEqual(3, doorStates.Count);
            Assert.IsTrue(doorStates[0]);
            Assert.IsFalse(doorStates[1]);
            Assert.IsTrue(doorStates[2]);
        }

        [TestMethod]
        public void ShouldStopHeaterWhenOpenDoorWhileHeating()
        {
            var sut = CreateSut();
            sut.TurnOnHeater();
            Assert.AreEqual(PowerState.On, sut.HeaterState);

            sut.ChangeStateOfMicrowaveOvenDoors();

            Assert.AreEqual(PowerState.Off, sut.HeaterState);
            Assert.IsTrue(sut.DoorOpen);
        }

        [TestMethod]
        public void ShouldNotStartHeaterWhenTryToStartHeatingWithDoorOpen()
        {
            var sut = CreateSut();
            sut.ChangeStateOfMicrowaveOvenDoors();
            Assert.IsTrue(sut.DoorOpen);

            sut.TurnOnHeater();

            Assert.AreEqual(PowerState.Off, sut.HeaterState);
        }

        [TestMethod]
        public void ShouldWorkCorrectlyInFullWorkflow()
        {
            var sut = CreateSut();

            Assert.IsFalse(sut.DoorOpen);
            Assert.AreEqual(PowerState.Off, sut.HeaterState);

            sut.TurnOnHeater();
            Assert.AreEqual(PowerState.On, sut.HeaterState);

            sut.ChangeStateOfMicrowaveOvenDoors();
            Assert.IsTrue(sut.DoorOpen);
            Assert.AreEqual(PowerState.Off, sut.HeaterState);

            sut.TurnOnHeater();
            Assert.AreEqual(PowerState.Off, sut.HeaterState);

            sut.ChangeStateOfMicrowaveOvenDoors();
            Assert.IsFalse(sut.DoorOpen);

            sut.TurnOnHeater();
            Assert.AreEqual(PowerState.On, sut.HeaterState);

            sut.PressStopButton();
            Assert.AreEqual(PowerState.Off, sut.HeaterState);
            Assert.AreEqual(0, _mockHeater.Object.RemainingTime);
            
            sut.Dispose();
        }

        private MicrowaveOvenHw CreateSut()
        {
            return new MicrowaveOvenHw(_mockHeater.Object);
        }

    }
}
