using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyClassLibrary
{
    public class ApiService
    {
        MemoryService memService = new MemoryService();

        public string ApiPath(string subpath)
        {
            return "http://192.168.1.6:3000/" + subpath;
        }

        public async Task<HttpResponseMessage> Login(string name, string password)
        {
            name = "trainee2@trainingmaster.is";
            password = "Trainee2!";
            var nvc = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", name),
                new KeyValuePair<string, string>("password", password)
            };
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, ApiPath("token") ) { Content = new FormUrlEncodedContent(nvc) };
            var res = await client.SendAsync(req);
            client.Dispose();

            return res;
        }
        
        public async Task<HttpResponseMessage> GetTrainingProgramTPID()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/*+xml;version=5.1");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + memService.GetString("trainingmaster_token"));
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPath("api/TrainingProgramDate/GetCurrentTrainingProgram"));
            var res = await client.SendAsync(req);
            client.Dispose();
            return res;
        }

        public async Task<HttpResponseMessage> GetTrainings(int TPID)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/*+xml;version=5.1");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + memService.GetString("trainingmaster_token"));
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPath("api/Training/Get/" + TPID));
            var res = await client.SendAsync(req);
            client.Dispose();
            return res;
        }
        
        public async Task<Exercise> GetExercise(int EID)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/*+xml;version=5.1");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + memService.GetString("trainingmaster_token"));
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPath("api/Exercise/" + EID));
            var res = await client.SendAsync(req);
            client.Dispose();
            var contents = await res.Content.ReadAsStringAsync();
            Exercise exercise = JsonConvert.DeserializeObject<Exercise>(contents);
            return exercise;
        }

        public async Task<HttpResponseMessage> GetFoodProgramFPMID()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/*+xml;version=5.1");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + memService.GetString("trainingmaster_token"));
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPath("api/FoodProgramDate/GetCurrentByCID"));
            var res = await client.SendAsync(req);
            client.Dispose();
            return res;
        }

        public async Task<HttpResponseMessage> GetFoodPortions(int FPMID)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/*+xml;version=5.1");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + memService.GetString("trainingmaster_token"));
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPath("api/FoodPortion/Get/" + FPMID));
            var res = await client.SendAsync(req);
            client.Dispose();
            return res;
        }

        public async Task<HttpResponseMessage> GetUserInfo()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/*+xml;version=5.1");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + memService.GetString("trainingmaster_token"));
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPath("api/Customer/GetCurrentCustomer") );
            var res = await client.SendAsync(req);
            client.Dispose();
            return res;
        }
        public async Task<bool> Logout()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/*+xml;version=5.1");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + memService.GetString("trainingmaster_token"));
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPath("api/Account/Logout"));
            var res = await client.SendAsync(req);
            client.Dispose();
            return res.IsSuccessStatusCode;
        }

        public async Task<bool> CheckToken(string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/*+xml;version=5.1");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token );
            var req = new HttpRequestMessage(HttpMethod.Get, ApiPath("api/Account/ValidateToken"));
            var res = await client.SendAsync(req);
            client.Dispose();
            return res.IsSuccessStatusCode;
        }

    }
}
