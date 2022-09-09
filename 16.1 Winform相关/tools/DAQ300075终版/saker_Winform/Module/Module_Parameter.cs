using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saker_Winform.Module
{
    public class Module_Parameter
    {
        public Module_Parameter() { }
        public string ID { get; set; }
        // 显示界面是否勾选
        public bool IsShow { get; set; }
        public string No { get; set; }
        //通道比较
        public string Tag { get; set; }
        //Y轴编号
        public string Y { get; set; }
        public string ScaleMin { get; set; }
        public string ScaleMax { get; set; }
        //波形颜色
        public Color WaveColor { get; set; }
        //波形类型，默认趋势图
        public string WaveType { get; set; }
        //显示配置是否显示此tag
        public bool IsChoose { get; set; }
     
    }
}
