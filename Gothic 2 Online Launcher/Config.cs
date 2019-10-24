using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gothic_2_Online_Launcher
{
    public static class Config
    {
        public static string registryKey = "HKEY_CURRENT_USER\\Software\\G2O";

        public static void SetNickname(string nickname)
        {
            Registry.SetValue(registryKey, "nickname", nickname);
        }

        public static void SetWorld(string world)
        {
            Registry.SetValue(registryKey, "world", world);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">IP:PORT</param>
        public static void SetAddress(string address)
        {
            Registry.SetValue(registryKey, "ip_port", address);
        }

        public static void SetLang(string lang)
        {
            Registry.SetValue(registryKey, "lang", lang);
        }

        public static string GetNickname()
        {
            return (string)Registry.GetValue(registryKey, "nickname", "Nickname");
        }

        public static string GetWorld()
        {
            return (string)Registry.GetValue(registryKey, "world", "NEWWORLD\\NEWWORLD.ZEN");
        }

        public static string GetAddress()
        {
            return (string)Registry.GetValue(registryKey, "ip_port", "127.0.0.1:28970");
        }

        public static string GetLang()
        {
            return (string)Registry.GetValue(registryKey, "lang", "en");
        }
    }
}
