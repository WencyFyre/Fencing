using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using FencingGame.Persistence;

namespace FencingGame
{
    public class ViewModelButton : ViewModelBase
    {
        private FieldType _BackColor;
        private bool _Enabled = true;
        private int _Row;
        private int _Column;
        public int Row {
            get => _Row; set
            {
                if (_Row != value)
                {
                    _Row = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Column {
            get => _Column; set
            {
                if (_Column != value)
                {
                    _Column = value;
                    OnPropertyChanged();
                }
            }
        }
        public FieldType BackColor
        {
            get => _BackColor; set
            {
                if (_BackColor != value)
                {
                    _BackColor = value ;
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


        public DelegateCommand? Click { get; set; }
        public DelegateCommand? Hovered { get; set; }
        public DelegateCommand? HoverLeaved { get; set; }
    }
   
}


