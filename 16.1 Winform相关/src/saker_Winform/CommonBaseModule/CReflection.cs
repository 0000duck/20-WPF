using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace saker_Winform.CommonBaseModule
{
    class CReflection
    {
        public static void callObjectEvent(Object obj, string EventName, EventArgs e = null)
        {
            //建立一个类型      
            Type t = Type.GetType(obj.GetType().AssemblyQualifiedName);
            //产生方法      
            MethodInfo m = t.GetMethod(EventName, BindingFlags.NonPublic | BindingFlags.Instance);
            //参数赋值。传入函数      
            //获得参数资料  
            ParameterInfo[] para = m.GetParameters();
            //根据参数的名字，拿参数的空值。  
            //参数对象      
            object[] p = new object[1];
            if (e == null)
            {
                p[0] = Type.GetType(para[0].ParameterType.BaseType.FullName).GetProperty("Empty");
            }
            else
            {
                p[0] = e;
            }
            //调用  
            m.Invoke(obj, p);
            return;
        }

        public static void callObjectEvent(Button button)
        {
            //建立一个类型  
            Type t = typeof(Button);
            //参数对象  
            object[] p = new object[1];
            //产生方法  
            MethodInfo m = t.GetMethod("OnClick", BindingFlags.NonPublic | BindingFlags.Instance);
            //参数赋值。传入函数  
            p[0] = EventArgs.Empty;
            //调用  
            m.Invoke(button, p);
            return;
        }
    }
}
