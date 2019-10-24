using System;
using System.Runtime.InteropServices;

/// <summary>
/// Most pomiędzy launcherem a aplikacją G2O
/// Przykład:
/*
            try
            {
                var proxy = new Proxy();
                Console.WriteLine(proxy.version);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
 */
/// </summary>
namespace G2O_Proxy
{
    enum Error
    {
        OK,
        MISSING_VERSION,
        GOTHIC_NOT_FOUND,
        UNKNOWN
    }
    struct RunResult
    {
        public Error result { get; }
        public int error { get; }
    }
    public struct Version
    {
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public int Build { get; }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}.{Build}";
        }
    }
    /// <summary>
    /// Funkcje związane z obsługą G2O_Proxy.dll
    /// </summary>
    public class Proxy
    {
        [DllImport("G2O_Proxy.dll", EntryPoint = "G2O_Version", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
            private static extern Version G2O_Version();
        [DllImport("G2O_Proxy.dll", EntryPoint = "G2O_Run", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
            private static extern RunResult G2O_Run(int major, int minor, int patch);

        public Version version { get; }

        public Proxy()
        {
            //Z powodu użycia DllImport może tu wyskoczyć wyjątek, polecam go łapać trochę wyżej ;)
            version = G2O_Version();
        }
 
        public void Run(int major, int minor, int patch)
        {
            var result = G2O_Run(major, minor, patch);

            if(result.result != Error.OK)
            {
                string errorMsg = "";
                switch(result.result)
                {
                    case Error.MISSING_VERSION: errorMsg = $"You can't join to this server, because you don't have required version!\nCode: { result.error}"; break;
                    case Error.GOTHIC_NOT_FOUND: errorMsg = $"Could not open Gothic2.exe.\nDid you install G2O to, a folder with Gothic 2: Night of the Raven?\nTry to run launcher with admin rights.\nCode: {result.error}"; break;
                    case Error.UNKNOWN: errorMsg = $"Possible solution: {result.error}\nCode: {result.error}"; break;
                }
                throw new Exception(errorMsg);
            }
        }
    }
}
