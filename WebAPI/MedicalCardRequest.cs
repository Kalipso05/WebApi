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
    internal class MedicalCardRequest
    {
        internal static async Task HandlePostMedicalCard(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                string contentBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    contentBody = await reader.ReadToEndAsync();
                }
                var medicalCard = JsonConvert.DeserializeObject<MedicalCard>(contentBody);

                if (medicalCard == null)
                {
                    await Settings.SendResponse(response, "Были отправлены неверные данные", code: HttpStatusCode.BadRequest);
                    Settings.Log("При POST запросе были отправлены неверные данные", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                    return;
                }

                using (var db = new dbModel())
                {
                    db.MedicalCard.Add(medicalCard);
                    await db.SaveChangesAsync();
                    await Settings.SendResponse(response, "Данные медицинской карты были добавлены");
                    Settings.Log("POST запрос на добавление данных  медицинской карты выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
    }
}
