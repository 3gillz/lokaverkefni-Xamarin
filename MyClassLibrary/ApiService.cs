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
            return "http://192.168.10.100:3000/" + subpath;
        }

        public async Task<HttpResponseMessage> Login(string name, string password)
        {
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("grant_type", "password"));
            name = "trainee2@trainingmaster.is";
            password = "Trainee2!";
            nvc.Add(new KeyValuePair<string, string>("username", name));
            nvc.Add(new KeyValuePair<string, string>("password", password));

            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, ApiPath("token") ) { Content = new FormUrlEncodedContent(nvc) };
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

    }
}
