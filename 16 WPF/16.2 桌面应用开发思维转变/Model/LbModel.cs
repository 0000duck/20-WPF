using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using _16._2_桌面应用开发思维转变.Annotations;

namespace _16._2_桌面应用开发思维转变.ControlModels
{
    internal class LbModel:INotifyPropertyChanged
    {
		private string content = "我是时间";

		public string Content
		{
			get { return content; }
			set
            {
                content = value;
                OnPropertyChanged("Content");
            }
		}

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
