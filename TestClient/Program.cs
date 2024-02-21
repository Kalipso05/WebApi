using ModelDataBase.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace TestClient
{
    internal class Program
    {
        int ID { get; set; }
        static async Task Main(string[] args)
        {
            var policy = new InsuransePolicy()
            {
                NumberPolicy = "4121231",
                DateOfExpirationPolicy = DateTime.Now,
            };
            var company = new InsuranseCompany()
            {
                Title = "Test",
            };
            var passport = new Passport()
            {
                Number = "431312",
                Series = "1231231"
            };

            var patient = new Patient()
            {
                FirstName = "Test",
                LastName = "Test",
                Patronymic = "Test",
                WorkPlace = "Test",
            };

            

            var contentPolicy = new StringContent(JsonConvert.SerializeObject(policy), Encoding.UTF8, "application/json");
            var contentCompany = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");
            var contentPassport = new StringContent(JsonConvert.SerializeObject(passport), Encoding.UTF8, "application/json");
            var contentPatient = new StringContent(JsonConvert.SerializeObject(patient), Encoding.UTF8, "application/json");

            using(var client = new HttpClient())
            {
                var response = await client.PostAsync("http://localhost:8080/api/InsuransePolicy", contentPolicy);
                var response1 = await client.PostAsync("http://localhost:8080/api/InsuranseCompany", contentCompany);
                var response2 = await client.PostAsync("http://localhost:8080/api/Passport", contentPassport);
                var response3 = await client.PostAsync("http://localhost:8080/api/Patient", contentPatient);
            }
        }
    }
}
