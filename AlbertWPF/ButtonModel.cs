using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbertWPF
{
    public class ButtonModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string content = "Test";

        public string Content
        {
            get { return content; }
            set 
            { 
                content = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("content"));
            }
        }

        public int Width { get; set; } = 200;
        public int Height { get; set; } = 200;

        
    }
}
