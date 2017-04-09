//2



using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading;
using System.Windows.Forms;


/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplicationMIDI
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}*/


namespace WindowsFormsApplicationMIDI
{
    
    public class InputPort
    {
        private NativeMethods.MidiInProc midiInProc;
        private IntPtr handle;


        public InputPort()
        {
            midiInProc = new NativeMethods.MidiInProc(MidiProc);
            handle = IntPtr.Zero;
        }


        public static int InputCount
        {
            get { return NativeMethods.midiInGetNumDevs(); }
        }


        public bool Close()
        {
            bool result = NativeMethods.midiInClose(handle)
                == NativeMethods.MMSYSERR_NOERROR;
            handle = IntPtr.Zero;
            return result;
        }


        public bool Open(int id)
        {
            return NativeMethods.midiInOpen(
                out handle,   //HMIDIIN
                id,           //id
                midiInProc,   //CallBack
                IntPtr.Zero,  //CallBack Instance
                NativeMethods.CALLBACK_FUNCTION)  //flag
                    == NativeMethods.MMSYSERR_NOERROR;
        }


        public bool Start()
        {
            return NativeMethods.midiInStart(handle)
                == NativeMethods.MMSYSERR_NOERROR;
        }


        public bool Stop()
        {
            return NativeMethods.midiInStop(handle)
                == NativeMethods.MMSYSERR_NOERROR;
        }

        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        public const int KEYEVENTF_KEYUP = 2;
        bool tanqi = true;
        private void MidiProc(IntPtr hMidiIn,
                uint wMsg,
                IntPtr dwInstance,
                uint dwParam1,
                uint dwParam2)
        {
            // Receive messages here
            /* Console.WriteLine("{0} {1} {2}", wMsg, dwParam1, dwParam2);*/

            
            if (wMsg == 963)
            {
                if (tanqi == true)
                {
                    dwParam1 = dwParam1 & 0xFFFF;
                    uint h_dw1 = 0;
                    uint l_dw1 = 0;
                    h_dw1 = dwParam1 & 0xFF;
                    l_dw1 = (dwParam1 >> 8) & 0xFF;




                    /* Console.WriteLine(Convert.ToString(wMsg, 16));
                     Console.WriteLine(Convert.ToString(h_dw1, 16));*/
                    Console.WriteLine(Convert.ToString(l_dw1, 16));
                    //int x = (int)l_dw1;



                    switch (Convert.ToString(l_dw1, 16))
                    {
                        case "15":

                            /* Form1 form1 = new Form1();
                             form1.Show();*/
                            //webBrowser1.Focus();
                            keybd_event(Keys.ControlKey, 0, 0, 0);
                            keybd_event(Keys.A, 0, 0, 0);
                            keybd_event(Keys.ControlKey, 0, KEYEVENTF_KEYUP, 0);
                            break;
                        case "17":
                            keybd_event(Keys.ControlKey, 0, 0, 0);
                            keybd_event(Keys.C, 0, 0, 0);
                            keybd_event(Keys.ControlKey, 0, KEYEVENTF_KEYUP, 0);
                            break;
                        case "18":
                            keybd_event(Keys.ControlKey, 0, 0, 0);
                            keybd_event(Keys.V, 0, 0, 0);
                            keybd_event(Keys.ControlKey, 0, KEYEVENTF_KEYUP, 0);
                            break;
                        case "19":
                            Form1 form1 = new Form1();
                            form1.process1.StartInfo.FileName = @"C:\WINDOWS\system32\taskmgr.exe";
                            form1.process1.Start();
                            break;
                        case "5e":
                            keybd_event(Keys.Space, 0, 0, 0);
                            keybd_event(Keys.Space, 0, KEYEVENTF_KEYUP, 0);
                            break;


                    }
                    tanqi = !tanqi;

                }
                else { tanqi = !tanqi; }
                /*Console.WriteLine(Convert.ToString(dwParam2, 16));
                Console.WriteLine("-------------------------------");*/
            }
            else
            {
                /* Console.WriteLine(Convert.ToString(wMsg, 16));
                 Console.WriteLine(Convert.ToString(dwParam1, 16));
                 /*Console.WriteLine(Convert.ToString(dwParam2, 16));*/
                Console.WriteLine("-------------------------------");
            }
        }


        public static void Main(String[] args)
        {
            Console.WriteLine("Hello");
          
            InputPort ip = new InputPort();
            /*Console.WriteLine("devices-sum:{0}", InputPort.InputCount);*/
            ip.Open(0);
            ip.Start();
            try
            {
                while (true)
                {
                    Thread.Sleep(1);
                }
            }
            catch (Exception e)
            {


            }
            finally
            {
                ip.Stop();
                ip.Close();
                Console.WriteLine("Bye~");
            }



        }
    }


    internal static class NativeMethods
    {
        internal const int MMSYSERR_NOERROR = 0;
        internal const int CALLBACK_FUNCTION = 0x00030000;


        internal delegate void MidiInProc(
            IntPtr hMidiIn,
            uint wMsg,
            IntPtr dwInstance,
            uint dwParam1,
            uint dwParam2);


        [DllImport("winmm.dll")]
        internal static extern int midiInGetNumDevs();


        [DllImport("winmm.dll")]
        internal static extern int midiInClose(
            IntPtr hMidiIn);


        [DllImport("winmm.dll")]
        internal static extern int midiInOpen(
            out IntPtr lphMidiIn,
            int uDeviceID,
            MidiInProc dwCallback,
            IntPtr dwCallbackInstance,
            int dwFlags);


        [DllImport("winmm.dll")]
        internal static extern int midiInStart(
            IntPtr hMidiIn);


        [DllImport("winmm.dll")]
        internal static extern int midiInStop(
            IntPtr hMidiIn);
    }
}

