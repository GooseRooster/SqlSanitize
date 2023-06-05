using Newtonsoft.Json;
using SqlSanitize.Shared;
using System.Net.Http.Json;

namespace SqlSanitize.Client.Services
{


    public interface IApiRepository
    {
        Task<List<SensitiveMessage>> GetAllSensitiveMessages();

        Task<bool> DeleteSensitiveMessage(string id);

        Task<bool> CreateSensitiveMessage(SensitiveMessage data);

        Task<bool> UpdateSensitiveMessage(SensitiveMessage data);
    }
    public class ApiRepository : IApiRepository
    {
        private HttpClient _http;

        public ApiRepository(HttpClient http)
        {
            _http = http;
        }


        public async Task<List<SensitiveMessage>> GetAllSensitiveMessages()
        {
            try
            {
                var url = "api/SensitiveMessages";



                List<SensitiveMessage> result = await _http.GetFromJsonAsync<List<SensitiveMessage>>(url);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return null;
            }


        }

        public async Task<bool> DeleteSensitiveMessage(string id)
        {
            try
            {
                var url = "api/SensitiveMessages/" + id;

  

                HttpResponseMessage resp = await _http.DeleteAsync(url);

                if (resp.IsSuccessStatusCode)
                {
                    

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }


        }




        public async Task<bool> CreateSensitiveMessage(SensitiveMessage data)
        {
            try
            {
                var url = "api/SensitiveMessages";


                HttpResponseMessage resp = await _http.PostAsJsonAsync(url, data);

                if (resp.IsSuccessStatusCode)
                {


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }


        }

        public async Task<bool> UpdateSensitiveMessage(SensitiveMessage data)
        {
            try
            {
                var url = "api/SensitiveMessages/" + data.Id;


                HttpResponseMessage resp = await _http.PutAsJsonAsync(url, data);

                if (resp.IsSuccessStatusCode)
                {


                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                return false;
            }


        }
    }
}
