using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Filminurk.Core.Dto.AccuWeather;
using Filminurk.Core.Dto.OMDB;
using Filminurk.Core.ServiceInterface;
using Environment = Filminurk.Data.Environment;

namespace Filminurk.ApplicationServices.Services
{
    public class OMDBServices : IOMDBServices
    {
        public async Task<OMDBSearchResultDTO> OMDBResult(OMDBSearchResultDTO dto)
        {
            string apikey = Environment.omdbkey;
            string baseUrl = "https://www.omdbapi.com/";

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
                var response = httpClient.GetAsync($"{baseUrl}?t={dto.Title}&apikey={apikey}").GetAwaiter().GetResult();
                var jsonResponse = await response.Content.ReadAsStringAsync();


                try
                {
                    OMDBSearchResultDTO movieData = JsonSerializer.Deserialize<OMDBSearchResultDTO>(jsonResponse);
                    dto.Title = movieData.Title;
                    dto.imdbID = movieData.imdbID;
                    dto.Year = movieData.Year;
                    dto.Released = movieData.Released;
                    dto.Rated = movieData.Rated;
                    dto.Runtime = movieData.Runtime;
                    dto.Genre = movieData.Genre;
                    dto.Director = movieData.Director;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return dto;

            }
            /*
            string idResponse = baseUrl + $"i={dto.imdbID}&apikey={apikey}";

            using (var clientMovie = new HttpClient())
            {
                var httpResponseMovie = clientMovie.GetAsync(idResponse).GetAwaiter().GetResult();
                string jsonMovie = await httpResponseMovie.Content.ReadAsStringAsync();
                OMDBRoot omdbRootDto = JsonSerializer.Deserialize<OMDBRoot>(jsonMovie);
                dto.Title = omdbRootDto.Title;
                dto.Year = omdbRootDto.Year;
                dto.imdbID = omdbRootDto.imdbID;
            }

                return dto;*/
        }
    }
}
