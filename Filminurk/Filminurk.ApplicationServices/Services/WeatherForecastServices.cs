using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Filminurk.Core.Dto.AccuWeather;
using Filminurk.Core.ServiceInterface;
using Environment = Filminurk.Data.Environment;

namespace Filminurk.ApplicationServices.Services
{
    public class WeatherForecastServices : IWeatherForecastServices
    {
        public WeatherForecastServices()
        {

        }

        public async Task<AccuLocationWeatherResultDTO> AccuWeatherResult(AccuLocationWeatherResultDTO dto)
        {
            string apikey = Environment.accuweatherkey; // key tuleb environmentist ega pole hardcode'tud
            var baseUrl = "https://dataservice.accuweather.com/forecasts/v1/daily/1day/";

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
                var response = await httpClient.GetAsync($"{dto.CityCode}?apikey={apikey}&details=true");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                List<AccuCityCodeRootDTO> weatherData = JsonSerializer.Deserialize<List<AccuCityCodeRootDTO>>(jsonResponse);
                dto.CityName = weatherData[0].LocalizedName;
                dto.CityCode = weatherData[0].Key;
            }

            string weatherResponse = baseUrl + $"{dto.CityCode}?apikey={apikey}&metric=true";

            using (var clientWeather = new HttpClient())
            {
                var httpResponseWeather = await clientWeather.GetAsync(weatherResponse);
                string jsonWeather = await httpResponseWeather.Content.ReadAsStringAsync();

                AccuLocationRoot weatherRootDTO = JsonSerializer.Deserialize<AccuLocationRoot>(jsonWeather);
                dto.EffectiveDate = weatherRootDTO.Headline.EffectiveDate;
                dto.EffectiveEpochDate = weatherRootDTO.Headline.EffectiveEpochDate;
                dto.Severity = weatherRootDTO.Headline.Severity;
                dto.Text = weatherRootDTO.Headline.Text;
                dto.Category = weatherRootDTO.Headline.Category;
                dto.EndDate = weatherRootDTO.Headline.EndDate;
                dto.EndEpochDate = weatherRootDTO.Headline.EndEpochDate;
            }

        }
    }
}
