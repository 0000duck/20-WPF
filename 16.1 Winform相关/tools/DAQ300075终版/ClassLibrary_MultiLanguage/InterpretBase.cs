using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;                   //控件遍历所需
using System.Text.RegularExpressions;      //String处理
using Newtonsoft.Json;
using System.IO;

namespace ClassLibrary_MultiLanguage
{
    public class InterpretBase
    {
        //定义字典用于储存Json配置文件资源
        public static Dictionary<string, string> resources = new Dictionary<string, string>();

        /// <summary>
        /// 当前项目文件夹Debug\Language\参数文件夹
        /// </summary>
        /// <param name="language">配置文件所在文件夹名</param>
        public static void LoadLanguage(string language = "")
        {
            if (string.IsNullOrEmpty(language))
            {
                language = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            }

            resources = new Dictionary<string, string>();
            //string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("Language"));
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format("Language"));
            if (Directory.Exists(dir))
            {
                var jaonFile = Directory.GetFiles(dir, language + ".json", SearchOption.AllDirectories);
                foreach (string file in jaonFile)
                {
                    LoadFile(file);
                }
            }
        }

        public static string textTran(string str)
        {
            return str = (resources.ContainsKey(str)) ? resources[str] : str;
        }

        /// <summary>
        /// 配置文件加载
        /// </summary>
        /// <param name="path">配置文件绝对路径（包括文件本身）</param>
        public static void LoadFile(string path)
        {
            var content = File.ReadAllText(path, Encoding.UTF8);
            if (!string.IsNullOrEmpty(content))
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                foreach (string key in dict.Keys)
                {

                    if (!resources.ContainsKey(key))
                    {
                        resources.Add(key, dict[key]);
                    }
                    else
                        resources[key] = dict[key];
                }
            }
        }

        /// <summary>
        /// 遍历翻译 窗体或控件及其子控件
        /// </summary>
        /// <param name="control">需要翻译的控件或窗体</param>
        public static void InitLanguage(Control control)
        {
            SetControlLanguage(control);
            foreach (Control ctrl in control.Controls)
            {
                InitLanguage(ctrl);
            }

            //工具栏或者菜单动态构建窗体或者控件的时候，重新对子控件进行处理
            control.ControlAdded += (sender, e) =>
            {
                InitLanguage(e.Control);
            };
        }
        /// <summary>
        /// 控件及子控件翻译
        /// </summary>
        /// <param name="control">需要翻译的控件</param>
        public static void SetControlLanguage(Control control)
        {
            if (control is ComboBox)
            {
                ComboBox combox = control as ComboBox;
                string[] NewItems = new string[combox.Items.Count];
                for (int i = 0; i < combox.Items.Count; i++)
                {
                    if (resources.ContainsKey(combox.Items[i].ToString()))
                    {
                        combox.Items[i] = resources[combox.Items[i].ToString()];
                    }
                    else
                        NewItems[i] = combox.Items[i].ToString();
                }
                combox.Text = (resources.ContainsKey(combox.Text)) ? resources[combox.Text] : combox.Text;
                //combox.Items.Clear();
                //combox.Items.AddRange(NewItems);
            }
            //control is 其他控件或者特殊控件 如：TreeView
            else if (control is TreeView)
            {
                TreeView treeView = (TreeView)control;
                foreach (TreeNode node in treeView.Nodes)
                {
                    try
                    {
                        node.Text = (resources.ContainsKey(node.Text)) ? resources[node.Text] : node.Text;
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        if (node.Nodes.Count > 0)
                        {
                            GetNodeText(node);
                        }
                    }
                }
            }
            else if (control is MenuStrip)
            {
                MenuStrip menuStrip = (MenuStrip)control;
                foreach (ToolStripMenuItem toolItem in menuStrip.Items)
                {
                    try
                    {
                        toolItem.Text = (resources.ContainsKey(toolItem.Text)) ? resources[toolItem.Text] : toolItem.Text;
                    }
                    catch (Exception)
                    {
                    }
                    finally
                    {
                        if (toolItem.DropDownItems.Count > 0)
                        {
                            GetItemText(toolItem);
                        }
                    }
                }
            }
            else if (control is TabControl)
            {
                TabControl tabCtrl = (TabControl)control;
                try
                {
                    foreach (TabPage tabPage in tabCtrl.TabPages)
                    {
                        tabPage.Text = (resources.ContainsKey(tabPage.Text)) ? resources[tabPage.Text] : tabPage.Text;
                    }
                }
                catch (Exception)
                {
                }
            }
            else if (control is ToolStrip)
            {
                ToolStrip statusStrip = (ToolStrip)control;
                foreach (ToolStripItem toolItem in statusStrip.Items)
                {
                    try
                    {
                        toolItem.Text = (resources.ContainsKey(toolItem.Text)) ? resources[toolItem.Text] : toolItem.Text;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else if (control is CheckedListBox)
            {
                CheckedListBox chkListBox = (CheckedListBox)control;
                try
                {
                    for (int n = 0; n < chkListBox.Items.Count; n++)
                    {
                        chkListBox.Items[n] = (resources.ContainsKey(chkListBox.Items[n].ToString())) ? resources[chkListBox.Items[n].ToString()] : chkListBox.Items[n].ToString();
                    }
                }
                catch (Exception)
                { }
            }
            else if (control is Label)
            {
                Label label = control as Label;
                try
                {
                    label.Text = (resources.ContainsKey(label.Text)) ? resources[label.Text] : label.Text;
                }
                catch (Exception)
                {
                }
            }
            else if (control is Button)
            {
                Button button = control as Button;
                try
                {
                    button.Text = (resources.ContainsKey(button.Text)) ? resources[button.Text] : button.Text;
                }
                catch (Exception)
                {
                }
            }
            else if (control is RadioButton)
            {
                RadioButton radioButton = (RadioButton)control;
                try
                {
                    radioButton.Text = (resources.ContainsKey(radioButton.Text)) ? resources[radioButton.Text] : radioButton.Text;
                }
                catch (Exception)
                {
                }
            }
            else if (control is DataGridView)
            {
                try
                {
                    DataGridView dataGridView = (DataGridView)control;
                    foreach (DataGridViewColumn dgvc in dataGridView.Columns)
                    {
                        try
                        {
                            if (dgvc.HeaderText.ToString() != "" && dgvc.Visible)
                            {
                                dgvc.HeaderText = (resources.ContainsKey(dgvc.HeaderText)) ? resources[dgvc.HeaderText] : dgvc.HeaderText;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                catch (Exception)
                { }
            }
            else
            {
                control.Text = (resources.ContainsKey(control.Text)) ? resources[control.Text] : control.Text;
            }

            /* if (control.HasChildren)
             {
                 foreach (System.Windows.Forms.Control c in control.Controls)
                 {
                     SetControlLanguage(c);
                 }
             }
             else
             {
                 SetControlLanguage(control);
             }*/
        }

        #region 简繁体转换
        /// <summary>
        /// 内容的语言转化
        /// </summary>
        /// <param name="parent"></param>
        public static void SetControlLanguageText(System.Windows.Forms.Control parent)
        {
            if (parent.HasChildren)
            {
                foreach (System.Windows.Forms.Control ctrl in parent.Controls)
                {
                    SetControlLanguage(ctrl);
                }
            }
            else
            {
                SetControlLanguage(parent);
            }
        }
        #endregion

        /// <summary>
        /// 局部匹配翻译，不存在则不翻译
        /// </summary>
        /// <param name="text">需要翻译的正则公式</param>
        public static void PartInterpret(ref string text)
        {
            if (resources.Keys == null && resources.Keys.Count == 0)
            {
                MessageBox.Show(InterpretBase.textTran("未添加资源文件，请及时确认或与工作人员联系"), InterpretBase.textTran("提示！！"));
                return;
            }
            foreach (var item in resources)
            {
                if (text.Contains(item.Key))
                {
                    text = text.Replace(item.Key, item.Value);
                }
            }
        }


        /// <summary>
        /// 递归转化树
        /// </summary>
        /// <param name="menuItem"></param>
        private static void GetNodeText(TreeNode node)
        {

            foreach (TreeNode treeNode in node.Nodes)
            {
                try
                {
                    treeNode.Text = (resources.ContainsKey(treeNode.Text)) ? resources[treeNode.Text] : treeNode.Text;
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (treeNode.Nodes.Count > 0)
                    {
                        GetNodeText(treeNode);
                    }
                }
            }
        }
        /// <summary>
        /// 递归转化菜单
        /// </summary>
        /// <param name="menuItem"></param>
        private static void GetItemText(ToolStripDropDownItem menuItem)
        {
            foreach (ToolStripItem toolItem in menuItem.DropDownItems)
            {
                try
                {
                    toolItem.Text = (resources.ContainsKey(toolItem.Text)) ? resources[toolItem.Text] : toolItem.Text;
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (toolItem is ToolStripDropDownItem)
                    {
                        ToolStripDropDownItem subMenuStrip = (ToolStripDropDownItem)toolItem;
                        if (subMenuStrip.DropDownItems.Count > 0)
                        {
                            GetItemText(subMenuStrip);
                        }
                    }
                }

            }
        }
    }
}
