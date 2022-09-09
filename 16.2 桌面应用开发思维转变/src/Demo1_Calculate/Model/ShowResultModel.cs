using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertWPF_Calculate.Model
{
    public class ShowResultModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string content;

        public string Content
        {
            get { return content; }
            set 
            { 
                content = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("content"));
            }
        }


        public ShowResultModel(string content)
        {
            this.Content = content;  
        }
    }
}
