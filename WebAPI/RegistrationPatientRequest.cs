using ModelDataBase.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI
{
    internal class RegistrationPatientRequest
    {
        internal static async Task HandleGetPatient(HttpListenerResponse response)
        {
            try
            {
                using (var db = new dbModel())
                {
                    var patient = db.Patient.ToList();

                    await Settings.SendResponse(response, JsonConvert.SerializeObject(patient));
                    Settings.Log("GET запрос на получение пациентов выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
        internal static async Task HandlePostPatient(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                string contentBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    contentBody = await reader.ReadToEndAsync();
                }
                var patient = JsonConvert.DeserializeObject<Patient>(contentBody);

                if(patient == null)
                {
                    await Settings.SendResponse(response, "Были отправлены неверные данные", code: HttpStatusCode.BadRequest);
                    Settings.Log("При POST запросе были отправлены неверные данные", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                    return;
                }

                using(var db = new dbModel())
                {
                    db.Patient.Add(patient);
                    await db.SaveChangesAsync();
                    await Settings.SendResponse(response, "Новый пациент был добавлен");
                    Settings.Log("POST запрос на добавление пациента выполнен");
                }
            }
            catch(Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
        internal static async Task HandlePutPatient(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                string contentBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    contentBody = await reader.ReadToEndAsync();
                }
                var updatedPatient = JsonConvert.DeserializeObject<Patient>(contentBody);

                if (updatedPatient == null)
                {
                    await Settings.SendResponse(response, "Были отправлены неверные данные", code: HttpStatusCode.BadRequest);
                    Settings.Log("При PUT запросе были отправлены неверные данные", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                    return;
                }
                using (var db = new dbModel())
                {
                    var existingPatient = await db.Patient.FindAsync(updatedPatient.ID);
                    if (existingPatient == null)
                    {
                        await Settings.SendResponse(response, "Пациент не найден в БД", code: HttpStatusCode.BadRequest);
                        Settings.Log("При PUT запросе на обновление данных пациента произошла ошибка (пациент не найден)", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                        return;
                    }
                    db.Entry(existingPatient).CurrentValues.SetValues(updatedPatient);
                    await db.SaveChangesAsync();
                    await Settings.SendResponse(response, "Данные пациента были обновлены");
                    Settings.Log("PUT запрос на обновление данных пациента выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
        internal static async Task HandleDeletePatient(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                var patientId = Convert.ToInt32(request.QueryString["id"]);
                
                using(var db = new dbModel())
                {
                    var patient = await db.Patient.FindAsync(patientId);
                    if (patient == null)
                    {
                        await Settings.SendResponse(response, "Пациент не найден в БД", code: HttpStatusCode.BadRequest);
                        Settings.Log("При DELETE запросе на удаление данных пациента произошла ошибка (пациент не найден)", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
                        return;
                    }
                    db.Patient.Remove(patient);
                    await db.SaveChangesAsync();
                    await Settings.SendResponse(response, "Пациент удален из БД");
                    Settings.Log("DELETE запрос на удаление пациента выполнен");
                }
            }
            catch (Exception e)
            {
                Settings.Log($"Ошибка: {e.Message}", HttpStatusCode.BadRequest, ConsoleColor.DarkRed);
            }
        }
    }
}
