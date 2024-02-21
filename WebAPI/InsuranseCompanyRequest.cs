using ModelDataBase.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI
{
    internal class InsuranseCompanyRequest
    {
        internal static async Task HandlePostInsuranseCompany(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                string contentBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    contentBody = await reader.ReadToEndAsync();
                }
                var company = JsonConvert.DeserializeObject<InsuranseCompany>(contentBody);

                if (company == null)
                {
                    await Settings.SendResponse(response, "Были отправлены неверные данные", code: HttpStatusCode.BadRequest);
                    Settings.Log("При POST запросе были отправлены неверные данные", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                    return;
                }

                using (var db = new dbModel())
                {
                    db.InsuranseCompany.Add(company);
                    await db.SaveChangesAsync();
                    await Settings.SendResponse(response, "Данные страховой компании были добавлены");
                    Settings.Log("POST запрос на добавление данных страховой компании выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
    }
}
