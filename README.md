# Magellanic.Sensors.HMC5883L
This is a C# implementation of code which integrates the HMC5883L digital compass sensor with Windows 10 IoT Core on the Raspberry Pi 3.

## Getting started
To build this project, you'll need the Magellanic.I2C project also (this is a NuGet package which is referenced in the project, so you may need to restore NuGet packages in your solution).

You should reference the Magellanic.Sensors.HMC5883L in your Visual Studio solution. The HMC5883L can be used with the following sample code in a standard Windows 10 UWP app:

```C#
public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();

        Loaded += MainPage_Loaded;
    }

    private async void MainPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        try
        {
            var compass = new HMC5883L();

            await compass.Initialize();
            
            if (compass.IsConnected())
            {
                compass.SetOperatingMode(OperatingMode.CONTINUOUS_OPERATING_MODE);

                while (true)
                {
                    var direction = compass.GetRawData();

                    Debug.WriteLine($"X = {direction.X}, Y = {direction.Y}, Z = {direction.Z}");
                    
                    Task.Delay(1000).Wait();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
```
