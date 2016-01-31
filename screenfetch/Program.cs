using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
   /          / /         /             RAM: {9}
  /          / /         /              OS HDD: {10}
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
                GetRAM(),
                GetSpace());
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

        public static string GetSpace()
        {
            int used;
            foreach(DriveInfo d in DriveInfo.GetDrives())
            {
                if(d.Name == @"C:\")
                {
                    used = (int)((d.TotalSize - d.AvailableFreeSpace) / (1024 * 1024 * 1024));
                    int total = (int)(d.TotalSize / (1024 * 1024 * 1024));
                    double inPercents = used * 100 / total;
                    return used + "GB / " + total + "GB [" + inPercents + "%]";
                }
            }
            return "";
        }

        public static string GetRAM()
        {
            int total = Convert.ToInt32(GetFromPC("TotalVisibleMemorySize")) / 1024;
            int free = Convert.ToInt32(GetFromPC("FreePhysicalMemory")) / 1024;
            int used = total - free;
            double inPercents = used * 100 / total;
            return used + "MB / " + total + "MB [" + inPercents + "%]";
        }
    }
}
