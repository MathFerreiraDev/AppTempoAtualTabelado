using System.Diagnostics;

namespace AppTempoAtualTabelado
{
    public partial class MainPage : ContentPage
    {

        CancellationTokenSource _cancelTokenSource;
        bool _isCheckingLocation;

        string? cidade;
        string latitude_value, longitude_value;

        public MainPage()
        {
            InitializeComponent();
        }

       

        private async void btn_getLocation_Clicked(object sender, EventArgs e)
        {
            try
            {
                _cancelTokenSource = new CancellationTokenSource();

                GeolocationRequest request =
                    new GeolocationRequest(GeolocationAccuracy.Medium,
                    TimeSpan.FromSeconds(10));

                Location? location = await Geolocation.Default.GetLocationAsync(
                    request, _cancelTokenSource.Token);


                if (location != null)
                {
                    latitude_value = location.Latitude.ToString();
                    longitude_value = location.Longitude.ToString();


                    Debug.WriteLine("---------------------------------------");
                    Debug.WriteLine(location);
                    Debug.WriteLine("---------------------------------------");
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Erro: Dispositivo não Suporta",
                    fnsEx.Message, "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Erro: Localização Desabilitada",
                    fneEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Erro: Permissão", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro: ", ex.Message, "OK");
            }
        }
    }

}
