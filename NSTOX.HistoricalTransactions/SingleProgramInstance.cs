using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace NSTOX.HistoricalTransactions
{
    class SingleProgramInstance
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        const int SW_RESTORE = 9;
        static readonly IntPtr HWND_BROADCAST = (IntPtr)0xffff;
        
        static public uint WakeupMessage = RegisterWindowMessage("NSTox_Wakeup");
        
        public void Register()
        {
            
        }

        public void RaiseOtherProcess()
        {
            PostMessage(HWND_BROADCAST, WakeupMessage, IntPtr.Zero, IntPtr.Zero);
            
            //Process proc = Process.GetCurrentProcess();
            //// Using Process.ProcessName does not function properly when
            //// the actual name exceeds 15 characters. Using the assembly 
            //// name takes care of this quirk and is more accruate than 
            //// other work arounds.
            //string assemblyName =
            //    Assembly.GetExecutingAssembly().GetName().Name;
            //foreach (Process otherProc in
            //    Process.GetProcessesByName(assemblyName))
            //{
            //    //ignore "this" process
            //    if (proc.Id != otherProc.Id)
            //    {
            //        // Found a "same named process".
            //        // Assume it is the one we want brought to the foreground.
            //        // Use the Win32 API to bring it to the foreground.
            //        IntPtr hWnd = otherProc.MainWindowHandle;
            //        if (IsIconic(hWnd))
            //        {
            //            ShowWindowAsync(hWnd, SW_RESTORE);
            //        }
            //        SetForegroundWindow(hWnd);
            //        break;
            //    }
            //}
        }
    }

    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public class MyMessageFilter : IMessageFilter
    {
        Form m_MainForm;
        public MyMessageFilter(Form form)
        {
            m_MainForm = form;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == SingleProgramInstance.WakeupMessage)
            {
                m_MainForm.Show();
                m_MainForm.WindowState = FormWindowState.Normal;
                m_MainForm.TopMost = true;
                m_MainForm.TopMost = false;
                m_MainForm.Activate();
                return true;
            }
            return false;
        }
    }
}
