using MicrowaveOven;

namespace MicrowaveOvenTests
{
    [TestClass]
    public class HeaterTests
    {
        private Heater CreateSut()
        {
            return new Heater();
        }

        [TestMethod]
        public void ShouldSetPowerStateOfHeaterToOn()
        {
            var sut = CreateSut();

            sut.StartHeater();

            Assert.AreEqual(PowerState.On, sut.PowerState);
        }

        [TestMethod]
        public void ShouldSetRemainingTimeOfHeaterTo60SecondsOnFreshStart()
        {
            var sut = CreateSut();

            sut.StartHeater();

            Assert.AreEqual(60, sut.RemainingTime);
        }

        [TestMethod]
        public void ShouldAddDefaultHeatingTimeWhenHeaterIsAlreadyOn()
        {
            var sut = CreateSut();

            sut.StartHeater(); 
            var firstTime = sut.RemainingTime;
            sut.StartHeater(); 

            Assert.AreEqual(firstTime + 60, sut.RemainingTime);
        }

        [TestMethod]
        public void ShouldNotSetTheHeatingTimeMoreThanMaximum()
        {
            const int startHeaterPressCount = 11;
            const int maximumSetupTimeInSeconds = 600;

            var sut = CreateSut();
            for (int i = 0; i <= startHeaterPressCount; i++)
            {
                sut.StartHeater();
            }

            var heaterTime = sut.RemainingTime;
            Assert.IsTrue(heaterTime <= maximumSetupTimeInSeconds);
        }

        [TestMethod]
        public void ShouldTurnOnHeaterWithoutChangingTimeWhenWasStartedAndTheStop()
        {
            var sut = CreateSut();

            sut.StartHeater();
            sut.StopHeater(); 
            var remainingTime = sut.RemainingTime;

            sut.StartHeater();

            Assert.AreEqual(PowerState.On, sut.PowerState);
            Assert.AreEqual(remainingTime, sut.RemainingTime);
        }

        [TestMethod]
        public void ShouldSetPowerStateOfHeaterToOffAndStopTimerOnStop()
        {
            var sut = CreateSut();

            sut.StartHeater();

            sut.StopHeater();

            Assert.AreEqual(PowerState.Off, sut.PowerState);
            Assert.IsFalse(sut.Timer.Enabled);
        }

        [TestMethod]
        public void ShouldSetPowerStateToOffAndSetRemainingTimeToZeroOnReset()
        {
            var sut = CreateSut();

            sut.StartHeater();

            sut.ResetHeater();

            Assert.AreEqual(PowerState.Off, sut.PowerState);
            Assert.AreEqual(0, sut.RemainingTime);
            Assert.IsFalse(sut.Timer.Enabled);
        }

        [TestMethod]
        public void ShouldRaisedTimerElapsedWhenResetHeaterIsCalled()
        {
            var sut = CreateSut();

            var eventRaised = false;
            double eventTime = -1;
            sut.TimerElapsed += (time) => { eventRaised = true; eventTime = time; };
            sut.StartHeater();

            sut.ResetHeater();

            Assert.IsTrue(eventRaised);
            Assert.AreEqual(0, eventTime);
        }

        [TestMethod]
        public void ShouldDecrementTimeOnTimerTick()
        {
            var sut = CreateSut();

            sut.StartHeater();
            var initialTime = sut.RemainingTime;

            Thread.Sleep(1100); 

            Assert.IsTrue(sut.RemainingTime < initialTime || sut.RemainingTime == 0);
        }
    }
}
