using MovieFav.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieFav.Services
{
    public class MovieDataStore : IDataStore<Models.Movie>
    {
        List<Movie> movies { get; set; }
        HttpClient client;
        string URLWepAPI = @"https://10.0.2.2:5001/api/Movie";
        string URLWepAPIId = @"https://10.0.2.2:5001/api/Movie/{0}";

        public async Task<bool> AddItemAsync(Movie item)
        {
            bool isAdd = false;
#if DEBUG
            var httpHandler = new HttpClientHandler 
            {
                ServerCertificateCustomValidationCallback = (o , cert , chain ,error) => true
            };
#else
            var httpClientHandler = new HttpClientHandler();
#endif


            using (client = new HttpClient(httpHandler))
            {
                Uri uri = new Uri(string.Format(URLWepAPI, string.Empty));

                JsonSerializerOptions serializerOptions =
                    new JsonSerializerOptions() { WriteIndented = true };

                string json = JsonSerializer.Serialize<Movie>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;

                response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    isAdd = true;
                    Debug.WriteLine(@"\t Movie successfully saved.");
                }
            }

            return isAdd;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            bool isDelete = false;

#if DEBUG
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (o, cert, chain, error) => true
            };
#else
            var httpClientHandler = new HttpClientHandler();
#endif

            using (client = new HttpClient(httpHandler))
            {
                Uri uri = new Uri(string.Format(URLWepAPIId, id));

                HttpResponseMessage response = await client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    isDelete = true;
                    Debug.WriteLine(@"\t Movie successfully deleted.");
                }
            }

            return isDelete;
        }

        public async Task<Movie> GetItemAsync(string id)
        {
            Movie movie = null;

#if DEBUG
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (o, cert, chain, error) => true
            };
#else
            var httpClientHandler = new HttpClientHandler();
#endif

            using (client = new HttpClient(httpHandler))
            {
                Uri uri = new Uri(string.Format(URLWepAPIId, id));

                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    movie = JsonSerializer.Deserialize<Movie>(content,
                        new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });
                }
            }
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetItemsAsync(bool forceRefresh = false)
        {
            try
            {
                var httpClientHandler = new HttpClientHandler();

                httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, cert, chain, errors) => { return true; };


                using (client = new HttpClient(httpClientHandler))
                {
                    Uri uri = new Uri(string.Format(URLWepAPI, string.Empty));

                    HttpResponseMessage response = await client.GetAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        movies = JsonSerializer.Deserialize<List<Movie>>(content,
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            });
                        if(movies!=null && movies.Count > 0)
                        {
                            foreach (var m in movies)
                                m.ImageURL = @"https://img.youtube.com/vi/"+ m.MovieCode + "/hqdefault.jpg";
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\t " + ex.Message);
            }
            return movies;
        }

        public async Task<bool> UpdateItemAsync(Movie item)
        {
            bool isUpdate = false;

#if DEBUG
            var httpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (o, cert, chain, error) => true
            };
#else
            var httpClientHandler = new HttpClientHandler();
#endif

            using (client = new HttpClient(httpHandler))
            {
                Uri uri = new Uri(string.Format(URLWepAPIId, item.MovieId));

                JsonSerializerOptions serializerOptions =
                    new JsonSerializerOptions() { WriteIndented = true };

                string json = JsonSerializer.Serialize<Movie>(item, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8,
                    "application/json");

                HttpResponseMessage response = null;

                response = await client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    isUpdate = true;
                    Debug.WriteLine(@"\t Movie successfully updated.");
                }
            }

            return isUpdate;
        }
    }
}
