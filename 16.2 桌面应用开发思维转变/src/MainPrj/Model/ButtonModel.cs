using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace _16._2_桌面应用开发思维转变.ControlModels
{
    internal class ButtonModel:INotifyPropertyChanged
    {
        private int width = 200; 

        public int Width
        {
            get { return width; }
            set
            {
                width  = value;
                OnPropertyChanged("width");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
