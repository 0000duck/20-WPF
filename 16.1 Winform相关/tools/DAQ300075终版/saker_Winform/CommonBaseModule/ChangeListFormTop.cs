using saker_Winform.SubForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace saker_Winform.CommonBaseModule
{
    class ChangeListFormTop
    {
        public static void ChangeListFormPropTop(List<Form_WaveMonitor> ts)
        {
            for (int i = 0; i < ts.Count; i++)
            {
               ts[i].TopLevel = false;
               ts[i].FormBorderStyle = FormBorderStyle.None; /*去除窗体的标题栏*/
               ts[i].ShowInTaskbar = false; /*去除窗体的标题栏*/
               ts[i].Dock = System.Windows.Forms.DockStyle.Fill;//form_WaveMonitor Dock:Fill
            }
        }

        public static void ChangeListFormPropTop(List<Form_HistoryComp> ts)
        {
            for (int i = 0; i < ts.Count; i++)
            {
                ts[i].TopLevel = false;
                ts[i].FormBorderStyle = FormBorderStyle.None; /*去除窗体的标题栏*/
                ts[i].ShowInTaskbar = false; /*去除窗体的标题栏*/
                ts[i].Dock = System.Windows.Forms.DockStyle.Fill;//form_WaveMonitor Dock:Fill
            }
        }

        public static void ChangeListFormPropTop(Form form)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None; /*去除窗体的标题栏*/
            form.ShowInTaskbar = false; /*去除窗体的标题栏*/
            form.Dock = System.Windows.Forms.DockStyle.Fill;//form_WaveMonitor Dock:Fill
        }
    }
}
