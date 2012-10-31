using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;

namespace HomeSecurity.Broker
{
    public class WorkerRole : RoleEntryPoint
    {
        Process _program = new Process();
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("$projectname$ entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.WriteLine("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            DiagnosticMonitorConfiguration diagnosticConfig =
               DiagnosticMonitor.GetDefaultInitialConfiguration();
            diagnosticConfig.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
            diagnosticConfig.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagnosticConfig);

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            string rsbroot = Path.Combine(Environment.GetEnvironmentVariable("RoleRoot") + @"\\", @"approot\\mosquitto");
            int port = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["WorkerIn"].IPEndpoint.Port;

            ProcessStartInfo pInfo = new ProcessStartInfo(Path.Combine(rsbroot, @"mosquitto.exe"))
            {
                UseShellExecute = false,
                WorkingDirectory = rsbroot,
                ErrorDialog = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            _program.StartInfo = pInfo;
            _program.OutputDataReceived += new DataReceivedEventHandler(program_OutputDataReceived);
            _program.ErrorDataReceived += new DataReceivedEventHandler(program_ErrorDataReceived);
            _program.Start();
            _program.BeginOutputReadLine();
            _program.BeginErrorReadLine();

            return true;
        }

        void program_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                string output = e.Data;
                Trace.WriteLine("[…] Standard output –> " + output, "Information");
            }
        }
        void program_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                string output = e.Data;
                Trace.WriteLine("[…] Standard output –> " + output, "Information");
            }
        }
    }
}
