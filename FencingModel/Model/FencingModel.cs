
using FencingGame.Persistence;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace FencingGame.Model
{
    /// <summary>
    /// Pályaméret
    /// </summary>

    public class FencingModel
    {
        private FencingTable? _table;
        private readonly IFencingDataAccess<FencingTable> _dataAccess;


        public bool IsHorizontal { get; private set; }
        public int BluePoints => Table.CountPoints(FieldType.BluePlayer) + Table.CountPoints(FieldType.BluePlayerFenced);
        public int RedPoints => Table.CountPoints(FieldType.RedPlayer) + Table.CountPoints(FieldType.RedPlayerFenced);
        public FencingTable Table
        {
            get
            {
                if (_table is null) Table = new FencingTable(Size.Small);
                return _table!;
            }

            private set
            {
                if (_table != null)
                {
                    Table.FieldChanged -= FieldChanged;
                }
                _table = value;
                Table.FieldChanged += FieldChanged;
                Table.PlayerChanged += (sender, e) => PlayerChanged?.Invoke(this, e);
                PlayerChanged?.Invoke(this, Table.CurrentPlayer);

                TableChanged?.Invoke(this, new EventArgs());
            }
        }
        public Size GameSize => Table.GameSize;



        public event EventHandler<(int, int)>? GameFieldChanged;
        public event EventHandler<FieldType>? GameOver;
        public event EventHandler<FieldType>? PlayerChanged;
        public event EventHandler? TableChanged;

        public FencingModel(IFencingDataAccess<FencingTable> dataAccess)
        {
            _dataAccess = dataAccess;
            IsHorizontal = true;
            Table.CurrentPlayer = FieldType.BluePlayer;
        }

        public bool TryStepGame((int x, int y) p)
        {
            if (!Table.CanPlaceBlock(p, IsHorizontal)) return false;
            Table.PlaceBlock(p, IsHorizontal, Table.CurrentPlayer);
            ChangePlayers();
            CheckGame();
            return true;
        }

        public void CheckGame()
        {
            if (Table.IsFilled)
            {
                GameOver?.Invoke(this, (BluePoints > RedPoints) ? FieldType.BluePlayer : (RedPoints > BluePoints ? FieldType.RedPlayer : FieldType.NoPlayer));
            }
        }

        public void NewGame(Size size = Size.Small)
        {
            Table = new FencingTable(size);
            IsHorizontal = true;
            Table.CurrentPlayer = FieldType.BluePlayer;
        }

        public async void SaveAsync(String path) => await _dataAccess.SaveAsync(path, Table);
        public async Task LoadGameAsync(String path) => Table = await _dataAccess.LoadAsync(path);
        public void ChangeOrientation() => IsHorizontal = !IsHorizontal;


        private void ChangePlayers() => Table.CurrentPlayer = (Table.CurrentPlayer == FieldType.BluePlayer) ? FieldType.RedPlayer : FieldType.BluePlayer;
        private void FieldChanged(object? sender, (int, int) e) => GameFieldChanged?.Invoke(this, e);

    }
}
