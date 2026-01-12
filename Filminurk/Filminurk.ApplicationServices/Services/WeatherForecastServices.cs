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
            var cityUrl = "https://dataservice.accuweather.com/locations/v1/cities/search";

            // get city
            /*
            using (var HttpClient = new HttpClient())
            {
                HttpClient.BaseAddress = new Uri(cityUrl);
                HttpClient.DefaultRequestHeaders.Accept.Clear();
                HttpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );

                var response = HttpClient.GetAsync($"?apikey={apikey}&q={dto.CityName}").GetAwaiter().GetResult();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                List<AccuCityCodeRootDTO> codeData = JsonSerializer.Deserialize<List<AccuCityCodeRootDTO>>(jsonResponse);
                dto.CityCode = codeData[0].Key;
            }

            string locationResponse = cityUrl+$"?apikey={apikey}&q={dto.CityName}";

            using (var clientLocation = new HttpClient())
            {
                var httpResponseLocation = await clientLocation.GetAsync(locationResponse);
                string jsonLocation = await httpResponseLocation.Content.ReadAsStringAsync();
                AccuCityCodeRootDTO cityRootDTO = JsonSerializer.Deserialize<AccuCityCodeRootDTO>(jsonLocation);
                dto.CityCode = cityRootDTO.Key;
            }
            */
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(cityUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
                var response = httpClient.GetAsync($"?q={dto.CityCode}?apikey={apikey}&details=true").GetAwaiter().GetResult();
                var jsonResponse = await response.Content.ReadAsStringAsync();


                try
                {
                    List<AccuCityCodeRootDTO> weatherData = JsonSerializer.Deserialize<List<AccuCityCodeRootDTO>>(jsonResponse);
                    dto.CityName = weatherData[0].LocalizedName;
                    dto.CityCode = weatherData[0].Key;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            string weatherResponse = baseUrl + $"{dto.CityCode}?apikey={apikey}&metric=true";

            using (var clientWeather = new HttpClient())
            {
                var httpResponseWeather = clientWeather.GetAsync(weatherResponse).GetAwaiter().GetResult();
                string jsonWeather = await httpResponseWeather.Content.ReadAsStringAsync();

                AccuLocationRoot weatherRootDTO = JsonSerializer.Deserialize<AccuLocationRoot>(jsonWeather);

                dto.EffectiveDate = weatherRootDTO.Headline.EffectiveDate;
                dto.EffectiveEpochDate = weatherRootDTO.Headline.EffectiveEpochDate;
                dto.Severity = weatherRootDTO.Headline.Severity;
                dto.Text = weatherRootDTO.Headline.Text;
                dto.Category = weatherRootDTO.Headline.Category;
                dto.EndDate = weatherRootDTO.Headline.EndDate;
                dto.EndEpochDate = weatherRootDTO.Headline.EndEpochDate;

                dto.MobileLink = weatherRootDTO.Headline.MobileLink;
                dto.Link = weatherRootDTO.Headline.Link;

                dto.DailyForecastsDate = weatherRootDTO.DailyForecasts[0].Date;
                dto.DailyForecastsEpochDate = weatherRootDTO.DailyForecasts[0].EpochDate;

                dto.TempMinValue = weatherRootDTO.DailyForecasts[0].Temperature.Minimum.Value;
                dto.TempMinUnit = weatherRootDTO.DailyForecasts[0].Temperature.Minimum.Unit;
                dto.TempMinUnitType = weatherRootDTO.DailyForecasts[0].Temperature.Minimum.UnitType;
                
                dto.TempMaxValue = weatherRootDTO.DailyForecasts[0].Temperature.Maximum.Value;
                dto.TempMaxUnit = weatherRootDTO.DailyForecasts[0].Temperature.Maximum.Unit;
                dto.TempMaxUnitType = weatherRootDTO.DailyForecasts[0].Temperature.Maximum.UnitType;

                dto.DayIcon = weatherRootDTO.DailyForecasts[0].Day.Icon;
                dto.DayIconPhrase = weatherRootDTO.DailyForecasts[0].Day.IconPhrase;
                dto.DayHasPrecipitation = weatherRootDTO.DailyForecasts[0].Day.HasPrecipitation;
                dto.DayPrecipitationType = weatherRootDTO.DailyForecasts[0].Day.PrecipitationType;
                dto.DayPrecipitationIntensity = weatherRootDTO.DailyForecasts[0].Day.PrecipitationIntensity;

                dto.NightIcon= weatherRootDTO.DailyForecasts[0].Night.Icon;
                dto.NightIconPhrase = weatherRootDTO.DailyForecasts[0].Night.IconPhrase;
                dto.NightHasPrecipitation = weatherRootDTO.DailyForecasts[0].Night.HasPrecipitation;
                dto.NightPrecipitationType = weatherRootDTO.DailyForecasts[0].Night.PrecipitationType;
                dto.NightPrecipitationIntensity = weatherRootDTO.DailyForecasts[0].Night.PrecipitationIntensity;
            }
            return dto;
        }
    }
}
