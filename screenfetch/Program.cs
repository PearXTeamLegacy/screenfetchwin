using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace screenfetch
{
    class Program
    {
        static void Main(string[] args)
        {
            string proc = GetFromProcessor("Name");
            string os = @"
                 __
            ,-~¨^  ^¨-,           _,    {0}
           /          / ;^-._...,¨/     OS: {1}
          /          / /         /      Uptime: {2}
         /          / /         /       Resolution: {3}
        /          / /         /        DE: {4}
       /,.-:''-,_ / /         /         Version: {5}
       _,.-:--._ ^ ^:-._ __../          Font: {6}
     /^         / /¨:.._¨__.;           CPU: {7}
    /          / /      ^  /            GPU: {8}
   /          / /         /             RAM: {10}MB / {9}MB
  /          / /         /              Manufacturer: {11}
 /_,.--:^-._/ /         /
^            ^¨¨-.___.:^  
";
            Console.WriteLine(os,
                Environment.UserName + "@" + Environment.MachineName,
                GetFromPC("Caption"),
                TimeSpan.FromMilliseconds(Environment.TickCount).ToString(@"dd\dhh\hmm\m"),
                Screen.PrimaryScreen.Bounds.Width + "x" + Screen.PrimaryScreen.Bounds.Height,
                GetDE(),
                GetFromPC("Version"),
                SystemFonts.DefaultFont.FontFamily.Name,
                GetFromProcessor("Name"),
                GetFromGPU("Name"),
                Convert.ToInt64(GetFromPC("TotalVisibleMemorySize")) / 1024,
                Convert.ToInt64(GetFromPC("FreePhysicalMemory")) / 1024,
                GetFromPC("Manufacturer"));
        }

        public static string GetFromPC(string what)
        {
            ManagementObjectSearcher ser = new ManagementObjectSearcher("SELECT " + what + " FROM Win32_OperatingSystem");
            foreach (ManagementObject mo in ser.Get())
            {
                return mo[what].ToString();
            }
            return null;
        }

        public static string GetFromProcessor(string what)
        {
            ManagementObjectSearcher ser = new ManagementObjectSearcher("SELECT " + what + " FROM Win32_Processor");
            foreach (ManagementObject mo in ser.Get())
            {
                return mo[what].ToString();
            }
            return null;
        }

        public static string GetFromGPU(string what)
        {
            ManagementObjectSearcher ser = new ManagementObjectSearcher("SELECT " + what + " FROM Win32_VideoController");
            foreach (ManagementObject mo in ser.Get())
            {
                return mo[what].ToString();
            }
            return null;
        }

        public static string GetDE()
        {
            string s = GetFromPC("Version");
            if (s.StartsWith("10") || s.StartsWith("6.2") || s.StartsWith("6.3"))
            {
                return "Metro";
            }
            else if (s.StartsWith("6.1") || s.StartsWith("6.0"))
            {
                return "Aero";
            }
            else return "Luna";
        }
    }
}
