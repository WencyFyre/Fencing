using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using FencingGame.Persistence;

namespace FencingGame.Model
{
    public class FencingTable : IGameSave
    {
        [JsonProperty]
        private readonly FieldType[,] _field;
        private FieldType _currentPlayer;
        public FieldType CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                    PlayerChanged?.Invoke(this, value);
                }
            }
        }

        [JsonIgnore]
        public bool IsFilled
        {
            get
            {

                for (int i = 0; i < (int)GameSize; i++)
                    for (int j = 0; j < (int)GameSize; j++)
                    {
                        if (CanPlaceBlock((i, j), true) || CanPlaceBlock((i, j), false)) return false;
                    }
                return true;
            }

        }
        public event EventHandler<(int, int)>? FieldChanged;
        public event EventHandler<FieldType>? PlayerChanged;

        public Size GameSize { get; set; }

        public string Extension => "sav";

        public FieldType GetFieldType((int x, int y) p)
        {
            if (p.x > -1 && p.x < (int)GameSize && p.y > -1 && p.y < (int)GameSize)
            {
                return _field[p.x, p.y];
            }
            else
            {
                return FieldType.Wall;
            }

        }

        public void SetFieldType((int x, int y) p, FieldType type)
        {
            _field[p.x, p.y] = type;
            FieldChanged?.Invoke(this, p);
        }


        public FencingTable(Size size)
        {
            GameSize = size;
            _field = new FieldType[(int)size, (int)size];
            for (int i = 0; i < (int)size; i++)
                for (int j = 0; j < (int)size; j++)
                {
                    _field[i, j] = FieldType.NoPlayer;
                }
        }

        public bool CanPlaceBlock((int x, int y) p, bool IsHorizontal)
        {
            return GetFieldType(p) == FieldType.NoPlayer &&
                GetFieldType(p.GetNeighbor(IsHorizontal)) == FieldType.NoPlayer;

        }
        public void PlaceBlock((int x, int y) p, bool IsHorizontal, FieldType type)
        {
            SetFieldType(p, type);
            SetFieldType(p.GetNeighbor(IsHorizontal), type);
            FillFencedArea(p);
            FillFencedArea(p.GetNeighbor(IsHorizontal));
        }

        private void FillFencedArea((int x, int y) p)
        {
            Console.WriteLine(p.ToString() + "Ez az a sor amit ki ír");
            foreach (var item in GetNeighbor(p))
            {
                var values = FindWall(item, p);
                if (values is object)
                {
                    foreach (var items in values)
                    {
                        SetFieldType(items, GetFieldType(p) == FieldType.RedPlayer ? FieldType.RedPlayerFenced : FieldType.BluePlayerFenced);
                        Console.WriteLine(items);

                    }
                }
            }

        }

        private HashSet<(int, int)>? FindWall((int, int) item, (int x, int y) p)
        {
            var type = GetFieldType(p);
            HashSet<(int, int)> Paint = new HashSet<(int, int)>();
            HashSet<(int, int)> prosecced = new HashSet<(int, int)>
            {
                p
            };
            HashSet<(int, int)> remaining = new HashSet<(int, int)>
            {
                item
            };
            while (remaining.Count != 0)
            {
                var poped = remaining.First();
                remaining.Remove(poped);
                if (!prosecced.Contains(poped))
                {
                    if (FieldType.Wall == GetFieldType(poped))
                    {
                        return null;
                    }
                    else
                    {
                        prosecced.Add(poped);
                        if (GetFieldType(poped) != type)
                        {
                            Paint.Add(poped);
                        }
                        foreach (var poppy in GetNeighbor(poped))
                        {
                            if (GetFieldType(poppy) != type)
                                remaining.Add(poppy);
                        }
                    }
                }
            }
            return Paint;
        }

        public int CountPoints(FieldType field) => _field.Cast<FieldType>().Count(x => x == field);



        private static IEnumerable<(int, int)> GetNeighbor((int, int) p)
        {
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++) yield return (p.Item1 + i, p.Item2 + j);
        }

    }
}
