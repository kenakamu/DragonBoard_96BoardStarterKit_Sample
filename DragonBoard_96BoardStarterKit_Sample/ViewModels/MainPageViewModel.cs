using System;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;

namespace DragonBoard_96BoardStarterKit_Sample.ViewModels
{
    public static class DigitalPin
    {
        public const int D1 = 36;
        public const int D2 = 13;
        public const int D3 = 115;
        public const int D4 = 24;
    }

    public class MainPageViewModel: ViewModelBase
    {
        #region Property

        /// <summary>
        /// Show message in the view
        /// </summary>
        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged(); }
        }

        /// <summary>
        /// Color for Red LED
        /// </summary>
        private SolidColorBrush ledColor = new SolidColorBrush(Windows.UI.Colors.LightGray);
        public SolidColorBrush LedColor
        {
            get { return ledColor; }
            set { ledColor = value; NotifyPropertyChanged(); }
        }
        
        private int TOUCH_PIN = DigitalPin.D1; // Touch sensor
        private int LED_PIN = DigitalPin.D2; // LED pin 
        private int BUTTON_PIN = DigitalPin.D3; // Button pin 
        private int TILT_PIN = DigitalPin.D4; // Tilt pin 
        private int ONBOARDLED_PIN1 = 21;
        private int ONBOARDLED_PIN2 = 120;
        private GpioPin ledPin;
        private GpioPinValue ledPinValue;
        private GpioPin onboardLedPin1;
        private GpioPinValue onboardLedPin1Value;
        private GpioPin onboardLedPin2;
        private GpioPinValue onboardLedPin2Value;
        private GpioPin touchPin;
        private GpioPin buttonPin;
        private GpioPin tiltPin;
        private string spiDeviceSelector = SpiDevice.GetDeviceSelector();
        private SpiDevice spiDevice;
        private byte[] tempWriteBuf = new byte[3] { 0x01, 0x80, 0x00 }; // Temperature Write Buffer
        private byte[] tempReadBuf = new byte[3]; // Store Temperature result
        private byte[] luxWriteBuf = new byte[3] { 0x01, 0xA0, 0x00 }; // Lux Write Buffer
        private byte[] luxReadBuf = new byte[3]; // Store Lux result
       
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);
    
        #endregion

        #region Method

        public async Task Initialize()
        {
            await InitSPI();
            InitGPIO();
        }

        /// <summary>
        /// Initialize SPI
        /// </summary>
        private async Task InitSPI()
        {
            Message = "Initializing SPI..";
            var spiController = await SpiController.GetDefaultAsync();
            if (spiController == null)
                return;

            // Sepcify settings for the SPI device.
            this.spiDevice = spiController.GetDevice(new SpiConnectionSettings(0)
            { 
                ClockFrequency = 10000,
                DataBitLength = 8,
                Mode = SpiMode.Mode0
            });

            Message = "SPI initialized correctly.";
        }

        /// <summary>
        /// Initialize GPIO pins.
        /// </summary>
        private void InitGPIO()
        {
            Message = "Initializing GPIO.."; 
            var gpioController = GpioController.GetDefault();

            if (gpioController == null)
                return;

            // Setup Touch sensor
            touchPin = gpioController.OpenPin(TOUCH_PIN);
            touchPin.SetDriveMode(GpioPinDriveMode.Input); // Specify the device as input
            touchPin.ValueChanged += Touch_ValueChanged; // Call back when user touches the sensor

            // Setup LED 
            ledPin = gpioController.OpenPin(LED_PIN);
            ledPinValue = GpioPinValue.Low; // LED is turned off.
            ledPin.Write(ledPinValue);
            ledPin.SetDriveMode(GpioPinDriveMode.Output); // Specify the device as output

            // Setup Button
            buttonPin = gpioController.OpenPin(BUTTON_PIN);
            buttonPin.SetDriveMode(GpioPinDriveMode.Input); // Specify the device as input
            buttonPin.ValueChanged += ButtonPin_ValueChanged;

            // Setup Tilt
            tiltPin = gpioController.OpenPin(TILT_PIN);
            tiltPin.SetDriveMode(GpioPinDriveMode.Input);
            tiltPin.ValueChanged += TiltPin_ValueChanged;

            // Setup OnboardLED
            onboardLedPin1 = gpioController.OpenPin(ONBOARDLED_PIN1);
            onboardLedPin1Value = GpioPinValue.Low; // LED is turned on.
            onboardLedPin1.Write(onboardLedPin1Value);
            onboardLedPin1.SetDriveMode(GpioPinDriveMode.Output); // Specify the device as outputonboardLedPin1 = gpioController.OpenPin(ONBOARDLED_PIN1);
            onboardLedPin2 = gpioController.OpenPin(ONBOARDLED_PIN2);
            onboardLedPin2Value = GpioPinValue.Low; // LED is turned on.
            onboardLedPin2.Write(onboardLedPin2Value);
            onboardLedPin2.SetDriveMode(GpioPinDriveMode.Output); // Specify the device as output

            Message = "GPIO pin initialized correctly.";
        }
        
        /// <summary>
        /// Called when user touches the touch sensor.
        /// </summary>
        private async void Touch_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            await GetSensorValueAndUpdateUI("TouchSensor", args);
        }

        /// <summary>
        /// Called when user pushes the button.
        /// </summary>
        private async void ButtonPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            await GetSensorValueAndUpdateUI("Button", args);
        }

        /// <summary>
        /// Called when user tilts the tilt sensor.
        /// </summary>
        private async void TiltPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            await GetSensorValueAndUpdateUI("TiltSensor", args);
        }

        /// <summary>
        /// Get sensor data and display the result
        /// </summary>
        private async Task GetSensorValueAndUpdateUI(string sensorName, GpioPinValueChangedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            // When user touched, pushed or tiled, then lit the LED and display sensor data
            if (e.Edge == GpioPinEdge.RisingEdge)
            {
                ledPinValue = GpioPinValue.High;
                ledPin.Write(ledPinValue);
                onboardLedPin1Value = GpioPinValue.High;
                onboardLedPin1.Write(onboardLedPin1Value);
                onboardLedPin2Value = GpioPinValue.High;
                onboardLedPin2.Write(onboardLedPin1Value);
                sb.Append($"{sensorName} on");

                var geoposition = await GetCurrentLocation();
                sb.Append($"Temp: {GetTemperature()}");
                sb.Append($"Lux: {GetLux()}");
                sb.Append($"Latitude: {geoposition.Coordinate.Point.Position.Latitude}");
                sb.Append($"Longitude: {geoposition.Coordinate.Point.Position.Longitude}");
            }
            else if (e.Edge == GpioPinEdge.FallingEdge)
            {
                ledPinValue = GpioPinValue.Low;
                ledPin.Write(ledPinValue);
                onboardLedPin1Value = GpioPinValue.Low;
                onboardLedPin1.Write(onboardLedPin1Value);
                onboardLedPin2Value = GpioPinValue.Low;
                onboardLedPin2.Write(onboardLedPin1Value);
                sb.Append($"Off");
            }

            // Display the result to the view.
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                LedColor = (ledPinValue == GpioPinValue.High) ?
                        redBrush : grayBrush;
                Message = sb.ToString();
            });
        }

        #region Get Sensor data

        /// <summary>
        /// Get temperature value.
        /// See more detail at http://learn.linksprite.com/project/read-temperature-using-tpm36-sensor/
        /// </summary>
        /// <returns></returns>
        private double GetTemperature()
        {
            spiDevice.TransferFullDuplex(tempWriteBuf, tempReadBuf);
            int value = (tempReadBuf[1] << 8) & 0b1100000000;
            value |= (tempReadBuf[2] & 0xff);
            return Math.Round((value * 5.0 / 1023 - 0.5) * 100, 2);
        }

        /// <summary>
        /// Get Lux value.
        /// See more detail at http://learn.linksprite.com/96-board/photoresistor/
        /// </summary>
        /// <returns></returns>
        private decimal GetLux()
        {
            spiDevice.TransferFullDuplex(luxWriteBuf, luxReadBuf);
            int value = (luxReadBuf[1] << 8) & 0b1100000000;
            value |= (luxReadBuf[2] & 0xff);
            return value;
        }

        /// <summary>
        /// Get Location from onboard GPS
        /// </summary>
        /// <returns></returns>
        private async Task<Geoposition> GetCurrentLocation()
        {
            var geolocator = new Geolocator();
            return await geolocator.GetGeopositionAsync();
        }

        #endregion

        #endregion
    }
}
