using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using G2O_Proxy;
/// <summary>
/// Sprawdzacz aktualizacji multiplayera
/// przykład: new Checker("http://api.gothic-online.com.pl/update/version/").Check(out downloadLink) true lub false
/// </summary>
namespace UpdateChecker
{
    class Checker
    {
        private enum VersionCode
        {
            UP_TO_DATE,
            OUTDATED,
            NOT_SUPPORTED,
            ERROR
        };

        private readonly string url;
        /// <summary>
        /// Struktura prywatna zawierająca to co zwróci serwer updatera
        /// </summary>
        private struct ResponseData
        {
            public int code { get; set; }
            public string link { get; set; }
        }
        public Checker(in string url)
        {
            this.url = url;
        }
        /// <summary>
        /// Sprawdza czy jest nowsza wersja od wczytanej z G2O proxy
        /// </summary>
        /// <param name="downloadLink">Zwraca link do pobrania</param>
        /// <returns>Zwraca true gdy ma pobrać aktualizacje, false gdy nie ma</returns>
        public bool Check(out string downloadLink)
        {
            downloadLink = string.Empty;
            try
            {
                var serializer = new JavaScriptSerializer();
                var json = serializer.Serialize(new Proxy().version);
                var versionCode = VersionCode.ERROR;
                using (var client = new HttpClient())
                {
                    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                    var jsonResponse = client.PutAsync(new Uri(url), stringContent).Result.Content.ReadAsStringAsync().Result;
                    var response = serializer.Deserialize<ResponseData>(jsonResponse);
                    versionCode = (VersionCode)response.code;
                    downloadLink = response.link.Length > 0 ? response.link : "";
                }
                switch (versionCode)
                {
                    case VersionCode.UP_TO_DATE: return false;
                    case VersionCode.OUTDATED:  return true;
                    case VersionCode.NOT_SUPPORTED: return false; 
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
