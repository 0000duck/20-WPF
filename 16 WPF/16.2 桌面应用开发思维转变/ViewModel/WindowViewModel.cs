using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _16._2_桌面应用开发思维转变.Common;
using _16._2_桌面应用开发思维转变.ControlModels;

namespace _16._2_桌面应用开发思维转变.ViewModel
{
    internal class WindowViewModel
    {
        #region Models
        public string Title { get; set; }
        public TextBoxModel TbModel { get; set; } = new TextBoxModel();
        public ButtonModel BtModel { get; set; } = new ButtonModel();
        public LbModel LbModel { get; set; } = new LbModel();
        #endregion

        #region Command
        // 匿名委托
        public CommandHelper ClickTb { get; set; } = new CommandHelper(obj => { });

        public CommandHelper ClickBt
        {
            get
            {
                // 匿名委托
                return new CommandHelper(obj =>
                {
                    // 实际的业务逻辑
                    BtModel.Width = 50;

                    Task.Run(()=>
                    {
                        while (true)
                        {
                            this.LbModel.Content = DateTime.Now.ToString();
                        }
                    });
                });
            }
        }
        #endregion
    }
}
