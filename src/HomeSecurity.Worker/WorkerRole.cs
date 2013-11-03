using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using System.IO;

namespace HomeSecurity.Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        Process _program = new Process();
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("HomeSecurity.Worker entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.TraceInformation("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            DiagnosticMonitorConfiguration diagnosticConfig =
                   DiagnosticMonitor.GetDefaultInitialConfiguration();
            diagnosticConfig.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1);
            diagnosticConfig.Logs.ScheduledTransferLogLevelFilter = Microsoft.WindowsAzure.Diagnostics.LogLevel.Verbose;
            DiagnosticMonitor.Start("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString", diagnosticConfig);

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

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

            Trace.WriteLine("Completed OnStart", "Information");
            return base.OnStart();
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
