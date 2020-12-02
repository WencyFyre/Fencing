using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FencingGame.ViewModel
{
    public class ViewModelButton : ViewModelBase
    {
        private Color _BackColor;
        private bool _Enabled;
        public Color BackColor
        {
            get => _BackColor; set
            {
                if (_BackColor != value)
                {
                    _BackColor = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool Enabled
        {
            get => _Enabled; set
            {
                if (_Enabled != value)
                {
                    _Enabled = value;
                    OnPropertyChanged();
                }
            }
        }
        public DelegateCommand? Click { get; init; }
        public DelegateCommand? Hovered { get; init; }
        public DelegateCommand? HoverLeaved { get; init; }
    }
   
}


