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
    internal class HospitalizationRequest
    {
        internal static async Task HandleGetHospitalization(HttpListenerResponse response)
        {
            try
            {
                using (var db = new dbModel())
                {
                    var hospitalizations = db.Hospitalization.ToList();

                    await Settings.SendResponse(response, JsonConvert.SerializeObject(hospitalizations));
                    Settings.Log("GET запрос на получение списка госпитализированных выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
        internal static async Task HandlePostHospitalization(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                string contentBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    contentBody = await reader.ReadToEndAsync();
                }
                var hospitalizations = JsonConvert.DeserializeObject<Hospitalization>(contentBody);

                if (hospitalizations == null)
                {
                    await Settings.SendResponse(response, "Были отправлены неверные данные", code: HttpStatusCode.BadRequest);
                    Settings.Log("При POST запросе были отправлены неверные данные", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                    return;
                }

                using (var db = new dbModel())
                {
                    db.Hospitalization.Add(hospitalizations);
                    await db.SaveChangesAsync();
                    await Settings.SendResponse(response, "Новые данные в таблицу госпитализации добавлены");
                    Settings.Log("POST запрос на добавление данных госпитализации выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
        internal static async Task HandlePutHospitalization(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                string contentBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    contentBody = await reader.ReadToEndAsync();
                }
                var updatedHospitalizations = JsonConvert.DeserializeObject<Hospitalization>(contentBody);

                if (updatedHospitalizations == null)
                {
                    await Settings.SendResponse(response, "Были отправлены неверные данные", code: HttpStatusCode.BadRequest);
                    Settings.Log("При PUT запросе были отправлены неверные данные", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                    return;
                }
                using (var db = new dbModel())
                {
                    var existingHospitalization = await db.Patient.FindAsync(updatedHospitalizations.ID);
                    if (existingHospitalization == null)
                    {
                        await Settings.SendResponse(response, "При PUT запросе на обновление данных госпитализации произошла ошибка (значение не найдено)", code: HttpStatusCode.BadRequest);
                        Settings.Log("При PUT запросе на обновление данных пациента произошла ошибка (значение не найден)", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                        return;
                    }
                    db.Entry(existingHospitalization).CurrentValues.SetValues(updatedHospitalizations);
                    await db.SaveChangesAsync();
                    await Settings.SendResponse(response, "Данные госпитализации были обновлены");
                    Settings.Log("PUT запрос на обновление данных госпитализации выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
        internal static async Task HandleDeleteHospitalization(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                var hospitalizationId = Convert.ToInt32(request.QueryString["id"]);

                using (var db = new dbModel())
                {
                    var hospitalization = await db.Hospitalization.FindAsync(hospitalizationId);
                    if (hospitalization == null)
                    {
                        await Settings.SendResponse(response, "При PUT запросе на обновление данных госпитализации произошла ошибка (значение не найдено)", code: HttpStatusCode.BadRequest);
                        Settings.Log("При PUT запросе на обновление данных пациента произошла ошибка (значение не найден)", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                        return;
                    }
                    db.Hospitalization.Remove(hospitalization);
                    await db.SaveChangesAsync();
                    await Settings.SendResponse(response, "Данные госпитализации удалены из БД");
                    Settings.Log("DELETE запрос на удаление данных госпитализации выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
    }
}
