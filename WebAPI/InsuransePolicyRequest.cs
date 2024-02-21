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
    internal class InsuransePolicyRequest
    {
        internal static async Task HandlePostInsuransePolicy(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                string contentBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    contentBody = await reader.ReadToEndAsync();
                }
                var policy = JsonConvert.DeserializeObject<InsuransePolicy>(contentBody);

                if (policy == null)
                {
                    await Settings.SendResponse(response, "Были отправлены неверные данные", code: HttpStatusCode.BadRequest);
                    Settings.Log("При POST запросе были отправлены неверные данные", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                    return;
                }

                using (var db = new dbModel())
                {
                    db.InsuransePolicy.Add(policy);
                    await db.SaveChangesAsync();
                    await Settings.SendResponse(response, "Данные страхового полиса были добавлены");
                    Settings.Log("POST запрос на добавление данных страхового полиса выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
    }
}
