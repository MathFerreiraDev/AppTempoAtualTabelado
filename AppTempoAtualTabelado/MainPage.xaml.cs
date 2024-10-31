using System.Diagnostics;

namespace AppTempoAtualTabelado
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Produto> list_locations_cs = new ObservableCollection<Produto>();
        CancellationTokenSource _cancelTokenSource;

        string? cidade;
        string latitude_value, longitude_value;

        public MainPage()
        {
            InitializeComponent();
            lst_locations.ItemsSource = list_locations_cs;
        }

       
        //OBTER AS COORDENADAS ATUAIS
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

                //RETORNA UM POSSIVEL VALOR DE CIDADE A PARTIR DA LONGITUDE A LATITUDE
                await GetGeocodeReverseData(latitude_value, longitude_vlaue);

                lst_locations.ItemsSource = list_locations_cs;

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

        //DECIFRAR A LOCALIZAÇÃO
          private async Task<string> GetGeocodeReverseData(latitude, longitude)
        {
            IEnumerable<Placemark> placemarks = 
                await Geocoding.Default.GetPlacemarksAsync(
                    latitude, longitude);

            Placemark? placemark = placemarks?.FirstOrDefault();

            Debug.WriteLine("-------------------------------------------");
            Debug.WriteLine(placemark?.Locality);
            Debug.WriteLine("-------------------------------------------");

            if (placemark != null)
            {
                cidade = placemark.Locality;

                return
                    $"AdminArea:       {placemark.AdminArea}\n" +
                    $"CountryCode:     {placemark.CountryCode}\n" +
                    $"CountryName:     {placemark.CountryName}\n" +
                    $"FeatureName:     {placemark.FeatureName}\n" +
                    $"Locality:        {placemark.Locality}\n" +
                    $"PostalCode:      {placemark.PostalCode}\n" +
                    $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                    $"SubLocality:     {placemark.SubLocality}\n" +
                    $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                    $"Thoroughfare:    {placemark.Thoroughfare}\n";
            }

            return "Nada";
        }

        private async void GetWeather()
        {
            try
            {
                if (!String.IsNullOrEmpty(cidade))
                {
                    CondicoesClima? previsao = await DataService.GetPrevisaoDoTempo(cidade);

                    string dados_previsao = "";

                    if (previsao != null)
                    {
                        dados_previsao = $"Humidade: {previsao.Humidity} \n" +
                                         $"Nascer do Sol: {previsao.Sunrise} \n " +
                                         $"Pôr do Sol: {previsao.Sunset} \n" +
                                         $"Temperatura: {previsao.Temperature} \n" +
                                         $"Titulo: {previsao.Title} \n" +
                                         $"Visibilidade: {previsao.Visibility} \n" +
                                         $"Vento: {previsao.Wind} \n" +
                                         $"Previsão: {previsao.Weather} \n" +
                                         $"Descrição: {previsao.WeatherDescription} \n" +
                                         $"Latitude: {previsao.Latitude} \n" +
                                         $"Longitude: {previsao.Longitude}";



                        string url_mapa = $"https://embed.windy.com/embed.html" +
                                           $"?type=map&location=coordinates&metricRain=mm" +
                                           $"&metricTemp=°C&metricWind=km/h&zoom=5&overlay=wind" +
                                           $"&product=ecmwf&level=surface" +
                                           $"&lat={previsao.Latitude}&lon={previsao.Longitude}";


                        Debug.WriteLine(url_mapa);
                        mapa.Source = url_mapa;                                        
                    }
                    else
                    {
                        dados_previsao = $"Sem dados, previsão nula.";
                    }

                    Debug.WriteLine("-------------------------------------------");
                    Debug.WriteLine(dados_previsao);
                    Debug.WriteLine("-------------------------------------------");

                    lbl_previsao.Text = dados_previsao;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro ", ex.Message, "OK");
            }
        }
    }

}
