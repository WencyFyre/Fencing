using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using FencingGame.Persistence;

namespace FencingGame.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        private string _BluePoints;
        private string _RedPoints;
        private string _CurrentPlayer;

        public DelegateCommand NewGame{ get; set; }
        public DelegateCommand SaveGame { get; set; }
        public DelegateCommand LoadGame { get; set; }
        public String BluePoints
        {
            get { return _BluePoints; }
            set
            {
                if (_BluePoints != value)
                {
                    _BluePoints = value;
                    OnPropertyChanged();
                }
            }
        }
        public String RedPoints
        {
            get { return _RedPoints; }
            set
            {
                if (_RedPoints != value)
                {
                    _RedPoints = value;
                    OnPropertyChanged();
                }
            }
        }
        public String CurrentPlayer
        {
            get { return _CurrentPlayer; }
            set
            {
                if (_CurrentPlayer != value)
                {
                    _CurrentPlayer = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<ViewModelButton> GameTable{ get; set; }

        public void OnTableChanged(object sender, EventArgs e)
        {
            
        }

        public void OnPlayerChanged(object sender, FieldType e)
        {
            throw new NotImplementedException();
        }

        public void OnGameOver(object sender, FieldType e)
        {
            throw new NotImplementedException();
        }

        public void OnGameFieldChanged(object sender, (int, int) e)
        {
            throw new NotImplementedException();
        }

    }
}
