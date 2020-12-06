using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using FencingGame.Persistence;
using FencingGame.Model;
using System.Windows.Media;

namespace FencingGame
{
    public class ViewModel : ViewModelBase
    {
        private ViewModelButton[] _greenbuttons;
        private readonly FencingModel _model;
        private string? _BluePoints;
        private string? _RedPoints;
        private string? _CurrentPlayer;
        private readonly Dictionary<(int, int), ViewModelButton> _buttons;
        public GameSize Small { get; } = GameSize.Small;
        public GameSize Medium { get; } = GameSize.Medium;
        public GameSize Large { get;} = GameSize.Large;

        public ViewModel(FencingModel model)
        {
            this._model = model;
            this.GameTable = new ObservableCollection<ViewModelButton>();
            _greenbuttons = new[] { new ViewModelButton(), new ViewModelButton() };
            _buttons = new Dictionary<(int, int), ViewModelButton>();
            NewGame = new DelegateCommand(param => NewGameEvent?.Invoke(this, param));
            SaveGame = new DelegateCommand(param => SaveGameEvent?.Invoke(this, EventArgs.Empty));
            LoadGame = new DelegateCommand(param => LoadGameEvent?.Invoke(this, EventArgs.Empty));
            ChangeOrientation = new DelegateCommand(param => ChangeOrientationEvent?.Invoke(this, EventArgs.Empty));
        }

        public DelegateCommand NewGame { get; set; }
        public DelegateCommand SaveGame { get; set; }
        public DelegateCommand LoadGame { get; set; }
        public DelegateCommand ChangeOrientation { get; set; }

        public event EventHandler<Object?>? NewGameEvent;
        public event EventHandler? LoadGameEvent;
        public event EventHandler? SaveGameEvent;
        public event EventHandler? ChangeOrientationEvent;
        public String BluePoints
        {
            get { return _BluePoints ??= ""; }
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
            get { return _RedPoints ??= ""; }
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
            get { return _CurrentPlayer ??= ""; }
            set
            {
                if (_CurrentPlayer != value)
                {
                    _CurrentPlayer = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<ViewModelButton> GameTable { get; set; }

        public void OnTableChanged(object? sender, EventArgs e)
        {
            GameTable.Clear();
            _buttons.Clear();
            BluePoints = "Kék: " + _model.BluePoints;
            RedPoints = "Piros: " + _model.RedPoints;
            for (int i = 0; i < (int)_model.GameSize; i++)
                for (int j = 0; j < (int)_model.GameSize; j++)
                {
                    (int, int) p = (i, j);
                    var button = new ViewModelButton()
                    {
                        Click = new DelegateCommand(_ => _model.TryStepGame(p)), 
                        Column = p.Item2,
                        Row = p.Item1,
                        BackColor = _model.Table.GetFieldType(p)
                };
                    _buttons.Add(p, button);
                    GameTable.Add(button);
                }
        }

        public void OnPlayerChanged(object? sender, FieldType e)
        {
            CurrentPlayer = e == Persistence.FieldType.BluePlayer ? "Kék van soron" : "Piros van soron";

        }


        public void OnGameFieldChanged(object? sender, (int, int) e)
        {
            if (_buttons.TryGetValue(e, out ViewModelButton? button))
            {
                button.BackColor = _model.Table.GetFieldType(e);
                BluePoints = "Kék: " + _model.BluePoints;
                RedPoints = "Piros: " + _model.RedPoints;
            }
        }

    }
}