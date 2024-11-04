using AppTempoAtualTabelado.Model;
using AppTempoAtualTabelado.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AppTempoAtualTabelado
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<DadosEndereco> list_locations_cs = new ObservableCollection<DadosEndereco>();
        CancellationTokenSource _cancelTokenSource;

        string? cidade;
        

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
                    string? latitude_value = location.Latitude.ToString();
                    string? longitude_value = location.Longitude.ToString();


                    Debug.WriteLine("---------------------------------------");
                    Debug.WriteLine(location);
                    Debug.WriteLine("---------------------------------------");

                    await GetGeocodeReverseData(latitude_value, longitude_value);

                    lst_locations.ItemsSource = list_locations_cs;
                    lbl_coordenadas.Text = $"Latitude: {location.Latitude}; Longitude: {location.Longitude}";

                    string url_mapa = $"https://embed.windy.com/embed.html" +
                                           $"?type=map&location=coordinates&metricRain=mm" +
                                           $"&metricTemp=°C&metricWind=km/h&zoom=5&overlay=wind" +
                                           $"&product=ecmwf&level=surface" +
                                           $"&lat={location.Latitude}&lon={location.Longitude}";


                    Debug.WriteLine(url_mapa);

                    web_mapa.Source = url_mapa;
                }

                //RETORNA UM POSSIVEL VALOR DE CIDADE A PARTIR DA LONGITUDE A LATITUDE
                

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
          private async Task<string> GetGeocodeReverseData(string? latitude, string? longitude)
        {
            IEnumerable<Placemark> placemarks = 
                await Geocoding.Default.GetPlacemarksAsync(
                    double.Parse(latitude), double.Parse(longitude));

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


        private void MenuItem_Clicked(object sender, EventArgs e)
        {

        }

        private async Task btn_getweather_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(cidade))
                {
                    CondicoesClima? previsao = await DataServiceCep.GetPrevisaoDoTempo(cidade);

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


                        Console.WriteLine(dados_previsao);
                        
                        frm_weatherform.IsVisible = true;
                        lbl_resultCity.Text = cidade;
                        lbl_temperature.Text = previsao.Temperature;
                        lbl_wind.Text = previsao.Wind;
                        lbl_humidty.Text = previsao.Humidity;
                        lbl_visibility.Text = previsao.Visibility;
                        lbl_sunrise.Text = previsao.Sunrise;
                        lbl_sunset.Text = previsao.Sunset;
                        lbl_weather.Text = previsao.Weather;

                    }
                    else
                    {
                        await DisplayAlert("Alerta", "Sem dados, previsão nula.", "OK");
                    }

                    Debug.WriteLine("-------------------------------------------");
                    Debug.WriteLine(dados_previsao);
                    Debug.WriteLine("-------------------------------------------");


                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro ", ex.Message, "OK");
            }
        }
    }

}
