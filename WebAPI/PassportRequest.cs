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
    internal class PassportRequest
    {
        internal static async Task HandlePostPassport(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                string contentBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    contentBody = await reader.ReadToEndAsync();
                }
                var passport = JsonConvert.DeserializeObject<Passport>(contentBody);

                if (passport == null)
                {
                    await Settings.SendResponse(response, "Были отправлены неверные данные", code: HttpStatusCode.BadRequest);
                    Settings.Log("При POST запросе были отправлены неверные данные", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                    return;
                }

                using (var db = new dbModel())
                {
                    db.Passport.Add(passport);
                    await db.SaveChangesAsync();
                    await Settings.SendResponse(response, "Паспортные данные были добавлены");
                    Settings.Log("POST запрос на добавление паспортных данных выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
    }
}
