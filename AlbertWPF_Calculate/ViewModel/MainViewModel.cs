using AlbertWPF_Calculate.Command;
using AlbertWPF_Calculate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertWPF_Calculate.ViewModel
{
    public class MainViewModel
    {
        public string Title { get; set; }
        public ShowNumModel Btn_1 { get; set; } = new ShowNumModel("1");
        public ShowNumModel Btn_2 { get; set; } = new ShowNumModel("2");
        public ShowNumModel Btn_3 { get; set; } = new ShowNumModel("3");
        public ShowNumModel Btn_4 { get; set; } = new ShowNumModel("4");
        public ShowNumModel Btn_5 { get; set; } = new ShowNumModel("5");
        public ShowNumModel Btn_6 { get; set; } = new ShowNumModel("6");
        public ShowNumModel Btn_7 { get; set; } = new ShowNumModel("7");
        public ShowNumModel Btn_8 { get; set; } = new ShowNumModel("8");
        public ShowNumModel Btn_9 { get; set; } = new ShowNumModel("9");

        public ShowResultModel Label_ShowResult { get; set; } = new ShowResultModel(""); 

        public CommandHelper ClickShowNumModelCommand
        {
            get => new CommandHelper(obj =>
            {
               Label_ShowResult.Content = (obj as ShowNumModel).Content;
               Label_ShowResult.Content = Btn_1.Content;
            });
        }



    }
}
