using Microsoft.VisualStudio.TestTools.UnitTesting;
using FencingGame.Model;
using FencingGame.Persistence;
using Moq;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FencingTest
{
    [TestClass]
    public class FencingTest
    {
        private FencingModel _model; // a tesztelendő modell
        private FencingTable _mockedTable; // mockolt játéktábla
        private Mock<IFencingDataAccess<FencingTable>> _mock; // az adatelérés mock-ja

        [TestInitialize]
        public void Initialize()
        {
            Init();
        }



        [DataTestMethod]
        [DataRow(Size.Small)]
        [DataRow(Size.Medium)]
        [DataRow(Size.Large)]
        public void FencingModelNewGame(Size size)
        {
            _model.NewGame(size);
            for (int i = 0; i < (int)size; i++)
                for (int j = 0; j < (int)size; j++)
                {
                    Assert.AreEqual(FieldType.NoPlayer, _model.Table.GetFieldType((i, j)),"Field not NoPlayer");
                }
            Assert.AreEqual(size, _model.GameSize, "Size not good");
            Assert.AreEqual(FieldType.BluePlayer, _model.Table.CurrentPlayer, "Piros kezd, de kéknek kéne");
            Assert.AreEqual(0, _model.BluePoints, "Nem 0 a pontok száma");
            Assert.AreEqual(0, _model.RedPoints, "Nem 0 a pontok száma");
        }


        [DataTestMethod]
        [DataRow(0, 5, true,  false)]
        [DataRow(5, 0, true,  false)]
        [DataRow(5, 5, true,  false)]
        [DataRow(0, 5, false, false)]
        [DataRow(3, 0, false, false)]
        [DataRow(0, 0, false, false)]


        public void FencingPlacingCanPlaceBlock(int x, int y, bool isHor, bool can)
        {
            Assert.AreEqual(can, _mockedTable.CanPlaceBlock((x,y), isHor), "Oda nem elhet lenni blokkot");
        }


        [TestMethod]

        public void FencingChangePlayers()
        {
            Assert.AreEqual(FieldType.RedPlayer, _mockedTable.CurrentPlayer);
            _model.TryStepGame((1, 3));
            Assert.AreEqual(FieldType.BluePlayer, _mockedTable.CurrentPlayer);
            _model.TryStepGame((0, 5));
            Assert.AreEqual(FieldType.BluePlayer, _mockedTable.CurrentPlayer);

        }

        [TestMethod]
        public void FencingAreaFenced()
        {
            Assert.AreEqual(FieldType.NoPlayer, _mockedTable.GetFieldType((1,1)));
            _mockedTable.CurrentPlayer = FieldType.BluePlayer;
            _mockedTable.PlaceBlock((2, 2), false, FieldType.BluePlayer);
            Assert.AreEqual(FieldType.BluePlayerFenced, _mockedTable.GetFieldType((1, 1)));
            _mockedTable.PlaceBlock((5, 4), true, FieldType.RedPlayer);
            Assert.AreEqual(FieldType.NoPlayer, _mockedTable.GetFieldType((4, 4)));


        }

        [TestMethod]
        public void FencingLoadTest()
        {
            var table = _mockedTable;
            _model.NewGame(Size.Medium);
            for (int i = 0; i < (int)Size.Medium; i++)
                for (int j = 0; j < (int)Size.Medium; j++)
                {
                    Assert.AreEqual(FieldType.NoPlayer, _model.Table.GetFieldType((i, j)), "Field not NoPlayer");
                }
            LoadAsync();
            for (int i = 0; i < (int)Size.Medium; i++)
                for (int j = 0; j < (int)Size.Medium; j++)
                {
                    Assert.AreEqual(table.GetFieldType((i, j)), _model.Table.GetFieldType((i, j)), "Field not NoPlayer");
                }




        }
  

        [TestMethod]
        public void FencingGameOver()
        {
            _mockedTable = JsonConvert.DeserializeObject<FencingTable>("{\"_field\":[[0,0,1,1,2,2],[1,1,2,2,1,1],[2,2,1,1,2,2],[1,1,2,2,1,1],[2,2,1,1,2,2],[1,1,2,2,1,1]],\"CurrentPlayer\":2,\"GameSize\":6,\"Extension\":\"sav\"}");
            _model.TryStepGame((0,0));
            _mockedTable = JsonConvert.DeserializeObject<FencingTable>("{\"_field\":[[0,0,1,1,2,2],[1,1,2,2,1,1],[2,2,1,1,2,2],[1,1,2,2,1,1],[2,2,1,1,2,2],[1,1,2,2,1,1]],\"CurrentPlayer\":2,\"GameSize\":6,\"Extension\":\"sav\"}");
            _mockedTable.PlaceBlock((0, 0), true, FieldType.BluePlayer);
        }

        public async void Init()
        {
            _mockedTable = new FencingTable(Size.Small);
            _mockedTable = JsonConvert.DeserializeObject<FencingTable>("{\"_field\":[[1,1,1,0,0,0],[1,0,0,0,0,0],[1,1,0,0,0,0],[0,0,0,1,1,0],[0,0,0,2,0,0],[0,2,2,2,2,2]],\"CurrentPlayer\":2,\"GameSize\":6,\"Extension\":\"sav\"}");

            _mock = new Mock<IFencingDataAccess<FencingTable>>();
            _mock.Setup(x => x.LoadAsync(It.IsAny<String>())).Returns(() => Task.FromResult(_mockedTable));
            _model = new FencingModel(_mock.Object);
            await _model.LoadGameAsync("");
            _model.GameOver += _model_GameOver;
        }

        private async void LoadAsync()
        {
            await _model.LoadGameAsync("");
        }

        private void _model_GameOver(object sender, FieldType e)
        {
            Assert.IsTrue(_mockedTable.IsFilled);
            if(_model.BluePoints > _model.RedPoints) Assert.AreEqual(FieldType.BluePlayer, e);
            else if (_model.RedPoints > _model.BluePoints) Assert.AreEqual(FieldType.RedPlayer,e);
            else Assert.AreEqual( FieldType.NoPlayer, e);
        }
    }
}
