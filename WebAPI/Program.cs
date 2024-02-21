using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI
{
    internal class Program
    {
        internal static async Task RouteRequest(HttpListenerResponse response, HttpListenerRequest request)
        {
            var path = request.Url.AbsolutePath;
            var method = request.HttpMethod;

            if (path.StartsWith("/api") && path.TrimEnd('/') == "/api")
            {
                if (method == "GET")
                {
                    var content = File.ReadAllText(@"C:\Users\ServerPC\source\repos\WebAPI\WebAPI\Documentation\test.html");
                    await Settings.SendResponse(response, content, "text/html");

                }
            }
            else if (path.StartsWith("/api/Patient"))
            {
                switch (method)
                {
                    case "GET":
                        await RegistrationPatientRequest.HandleGetPatient(response);
                        break;
                    case "POST":
                        await RegistrationPatientRequest.HandlePostPatient(response, request);
                        break;
                    case "PUT":
                        await RegistrationPatientRequest.HandlePutPatient(response, request);
                        break;
                    case "DELETE":
                        await RegistrationPatientRequest.HandleDeletePatient(response, request);
                        break;
                    default:
                        break;
                }
            }
            else if (path.StartsWith("/api/Hospitalization"))
            {
                switch(method)
                {
                    case "GET":
                        await HospitalizationRequest.HandleGetHospitalization(response);
                        break;
                    case "POST":
                        await HospitalizationRequest.HandlePostHospitalization(response, request);
                        break;
                    case "PUT":
                        await HospitalizationRequest.HandlePutHospitalization(response, request);
                        break;
                    case "DELETE":
                        await HospitalizationRequest.HandleDeleteHospitalization(response, request);
                        break;
                    default:
                        break;
                }
            }
            else if (path.StartsWith("/api/InsuransePolicy"))
            {
                if (method == "POST")
                {
                    await InsuransePolicyRequest.HandlePostInsuransePolicy(response, request);
                }
            }
            else if (path.StartsWith("/api/InsuranseCompany"))
            {
                if (method == "POST")
                {
                    await InsuranseCompanyRequest.HandlePostInsuranseCompany(response, request);
                }
            }
            else if (path.StartsWith("/api/MedicalCard"))
            {
                if (method == "POST")
                {
                    await MedicalCardRequest.HandlePostMedicalCard(response, request);
                }
            }
            else if (path.StartsWith("/api/Passport"))
            {
                if (method == "POST")
                {
                    await PassportRequest.HandlePostPassport(response, request);
                }
            }
            else if (path.StartsWith("/api/CodeHospitalization"))
            {
                if (method == "POST")
                {
                    await CodeHospitalizationRequest.HandlePostCodeHospitalization(response, request);
                }
            }
        }

        internal static async Task StartServer()
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8080/api/");
            listener.Start();
            Settings.Log("Сервер с IP http://localhost:8080/api/ запущен", color: ConsoleColor.DarkGray);

            while (true)
            {
                var context = await listener.GetContextAsync();
                await RouteRequest(context.Response, context.Request);
            }
        }

        static void Main(string[] args)
        {
            Task.Run(() => StartServer()).GetAwaiter().GetResult();
        }
    }
}
