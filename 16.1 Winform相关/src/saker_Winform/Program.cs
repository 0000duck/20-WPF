using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using saker_Winform.CommonBaseModule;
using saker_Winform.SubForm;

namespace saker_Winform
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
          /*  Task.Run(() =>
            {
                CFileOperate.closePort();//关闭1111端口占用的PID
            });         */
            Form_LogIn form_LogIn = new Form_LogIn();
            form_LogIn.ShowDialog();
            if(Form_LogIn.m_adminLog||Form_LogIn.m_vistorLog)
            {
                Application.Run(new Form_Main(Form_LogIn.m_adminLog, Form_LogIn.m_vistorLog));
            }
            else
            {
                Application.Exit();
            }
            //Application.Run(new Form_ChanScaleSet());
            //Application.Run(new Form1());
            //Application.Run(new Form_ChanConfig());
            //Application.Run(new Form_ViewConfig());
            //Application.Run(new Form_RecordConfig());   
        }
    }
}
