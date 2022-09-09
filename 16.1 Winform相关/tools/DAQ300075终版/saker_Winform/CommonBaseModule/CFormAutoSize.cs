using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace saker_Winform.CommonBaseModule
{
    class CFormAutoSize
    {
        /// <summary>
        /// 声明结构,只记录窗体和其控件的初始位置和大小。
        /// </summary>
        private struct CtrRectStru
        {
            public int s32Left;
            public int s32Top;
            public int s32Width;
            public int s32Height;
            public Font FontInfo;
        }

        /// <summary>
        /// 记录原始控件的位置和大小
        /// </summary>
        private List<CtrRectStru> lstOldCtrl = new List<CtrRectStru>();

        private int s32CtrlNo = 0;


        /// <summary>
        /// 记录窗体和其控件的初始位置和大小
        /// </summary>
        /// <param name="mForm"></param>
        public void formInitializeSize(Control mForm)
        {

            CtrRectStru cR;
            cR.s32Left = mForm.Left;
            cR.s32Top = mForm.Top;
            cR.s32Width = mForm.Width;
            cR.s32Height = mForm.Height;
            cR.FontInfo = mForm.Font;
            //第一个为"窗体本身",只加入一次即可
            lstOldCtrl.Add(cR);

            //窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
            AddControl(mForm);
        }

        /// <summary>
        /// 窗体内其余控件
        /// </summary>
        /// <param name="ctl"></param>
        private void AddControl(Control ctl)
        {
            foreach (Control c in ctl.Controls)
            {
                CtrRectStru objCtrl;
                objCtrl.s32Left = c.Left;
                objCtrl.s32Top = c.Top;
                objCtrl.s32Width = c.Width;
                objCtrl.s32Height = c.Height;
                objCtrl.FontInfo = c.Font;
                lstOldCtrl.Add(objCtrl);

                //**放在这里，是先记录控件本身，后记录控件的子控件
                if (c.Controls.Count > 0)
                {
                    //窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
                    AddControl(c);
                }
            }
        }


        /// <summary>
        /// 控件自适应大小
        /// </summary>
        /// <param name="mForm"></param>
        public void formAutoSize(Control mForm)
        {
            if (s32CtrlNo == 0)
            {
                //*如果在窗体的Form1_Load中，记录控件原始的大小和位置，正常没有问题，但要加入皮肤就会出现问题，因为有些控件如dataGridView的的子控件还没有完成，个数少
                //*要在窗体的Form1_SizeChanged中，第一次改变大小时，记录控件原始的大小和位置,这里所有控件的子控件都已经形成
                CtrRectStru cR;
                cR.s32Left = 0;
                cR.s32Top = 0;
                cR.s32Width = mForm.PreferredSize.Width;
                cR.s32Height = mForm.PreferredSize.Height;
                cR.FontInfo = mForm.Font;

                //第一个为"窗体本身",只加入一次即可
                lstOldCtrl.Add(cR);

                //窗体内其余控件可能嵌套其它控件(比如panel),故单独抽出以便递归调用
                AddControl(mForm);
            }

            float wScale = (float)mForm.Width / (float)lstOldCtrl[0].s32Width;//新旧窗体之间的比例，与最早的旧窗体
            float hScale = (float)mForm.Height / (float)lstOldCtrl[0].s32Height;//.Height;

            s32CtrlNo = 1;//进入=1，第0个为窗体本身,窗体内的控件,从序号1开始

            //窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
            AutoScaleControl(mForm, wScale, hScale);
        }

        /// <summary>
        /// 控件自动缩放
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="wScale"></param>
        /// <param name="hScale"></param>
        private void AutoScaleControl(Control ctl, float wScale, float hScale)
        {
            int ctrLeft0, ctrTop0, ctrWidth0, ctrHeight0;

            foreach (Control c in ctl.Controls)
            {
                ctrLeft0 = lstOldCtrl[s32CtrlNo].s32Left;
                ctrTop0 = lstOldCtrl[s32CtrlNo].s32Top;
                ctrWidth0 = lstOldCtrl[s32CtrlNo].s32Width;
                ctrHeight0 = lstOldCtrl[s32CtrlNo].s32Height;

                //新旧控件之间的线性比例。控件位置只相对于窗体，所以不能加 + wLeft1
                c.Left = (int)((ctrLeft0) * wScale);
                c.Top = (int)((ctrTop0) * hScale);//
                c.Width = (int)(ctrWidth0 * wScale);//只与最初的大小相关，所以不能与现在的宽度相乘 (int)(c.Width * w);
                c.Height = (int)(ctrHeight0 * hScale);//
                if(wScale>0&&hScale>0)
                {
                    c.Font = new Font(lstOldCtrl[s32CtrlNo].FontInfo.FontFamily, lstOldCtrl[s32CtrlNo].FontInfo.Size * System.Math.Min(wScale, hScale)); //字体缩放，取最小的比例            
                }
                s32CtrlNo++;//累加序号

                //**放在这里，是先缩放控件本身，后缩放控件的子控件
                if (c.Controls.Count > 0)
                {
                    //窗体内其余控件还可能嵌套控件(比如panel),要单独抽出,因为要递归调用
                    AutoScaleControl(c, wScale, hScale);
                }
            }
        }
    }
}
