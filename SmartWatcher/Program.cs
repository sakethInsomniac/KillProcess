using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace KillProcess
{
    class Program
    {
        void BindToRunningProcesses()
        {
            int count = 0;
            // Get the current process.
            Process currentProcess = Process.GetCurrentProcess();

            // Get all processes running on the local computer.
            Process[] localAll = Process.GetProcesses();
            ;
            foreach (Process localprocess in localAll)
            {
                count++;
                Console.WriteLine($"Process ID-> {localprocess.Id}: {localprocess.ToString()}");
            }


            // Get all instances of Notepad running on the local computer.
            // This will return an empty array if notepad isn't running.
            //Process[] localByName = Process.GetProcessesByName("notepad");

            // Get a process on the local computer, using the process id.
            // This will throw an exception if there is no such process.
            //Process localById = Process.GetProcessById(1234);


            // Get processes running on a remote computer. Note that this
            // and all the following calls will timeout and throw an exception
            // if "myComputer" and 169.0.0.0 do not exist on your local network.

            // Get all processes on a remote computer.
            //Process[] remoteAll = Process.GetProcesses("myComputer");

            // Get all instances of Notepad running on the specific computer, using machine name.
            //Process[] remoteByName = Process.GetProcessesByName("notepad", "myComputer");

            // Get all instances of Notepad running on the specific computer, using IP address.
            // Process[] ipByName = Process.GetProcessesByName("notepad", "169.0.0.0");

            // Get a process on a remote computer, using the process id and machine name.
            // Process remoteById = Process.GetProcessById(2345, "myComputer");
        }
        static void Main(string[] args)
        {
            Program myProcess = new Program();
            myProcess.BindToRunningProcesses();
            Console.WriteLine("Please enter the PID number from the above list");
            int PID = Convert.ToInt32(Console.ReadLine());
            KillProcessAndChildren(PID);
            Console.ReadKey();
        }


        /// <summary>
        /// Kill a process, and all of its children, grandchildren, etc.
        /// </summary>
        /// <param name="pid">Process ID.</param>
        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                    ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();

            }
            catch (ArgumentException e)
            {
                // Process already exited.
                Console.WriteLine(e);
            }
        }


    }
}
