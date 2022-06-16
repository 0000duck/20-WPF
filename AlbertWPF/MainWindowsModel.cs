using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertWPF
{
    public class MainWindowsModel
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Title { get; set; } = "AlbertZhao";

        public ButtonModel BtnModel { get; set; } = new ButtonModel();

        public CommandHelper ButtonClickCommand
        {
            get => new CommandHelper(DoButtonClick); 
        }     

        private void DoButtonClick(object obj)
        {
            // 实现界面时间的实时刷新
            Task.Run(() =>{

                while (true)
                {
                    this.BtnModel.Content = DateTime.Now.ToString();
                }
            });
            
        }

    }
}
