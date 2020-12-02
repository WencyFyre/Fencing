using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using FencingGame.Model;
using FencingGame.ViewModel;
using FencingGame.Persistence;
using Microsoft.Win32;

namespace ViewWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private FencingModel? _model;
        private ViewModel? _viewModel;
        private MainWindow? _view;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new FencingModel(new FileDataAccess<FencingTable>());
            _model.GameOver += _model_GameOver;
            _model.PlayerChanged += _model_PlayerChanged;
            _model.TableChanged += _model_TableChanged;
            _model.GameOver += _model_GameOver;
            _viewModel = new ViewModel(_model);
            _viewModel.LoadGameEvent += _viewModel_LoadGameEvent;
            _viewModel.SaveGameEvent += _viewModel_SaveGameEvent;
            _viewModel.NewGameEvent += _viewModel_NewGameEvent;

            _model.GameFieldChanged += _viewModel.OnGameFieldChanged;


            _view = new MainWindow
            {
                DataContext = _viewModel,
            };
            _view.Show();





        }

        private void _viewModel_SaveGameEvent(object? sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "SAV files (*.sav)|*.sav",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true
            };
            if (saveDialog.ShowDialog() ?? false)
            {
                _model.SaveAsync(saveDialog.FileName);
            }
        }

        private async void _viewModel_LoadGameEvent(object? sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "SAV files (*.sav)|*.sav",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                RestoreDirectory = true
            };
            if (openDialog.ShowDialog() ?? false)
            {
                await _model.LoadGameAsync(openDialog.FileName);
            }
        }

        private void _viewModel_NewGameEvent(object? sender, object? e)
        {
            if (e is not null) _model!.NewGame((FencingGame.Model.Size)e);
        }

        private void _model_TableChanged(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _model_PlayerChanged(object? sender, FieldType e)
        {
            throw new NotImplementedException();
        }

        private void _model_GameOver(object? sender, FieldType e)
        
            {
                MessageBox.Show(
                     e switch
                     {
                         FencingGame.Persistence.FieldType.BluePlayer => "A kék nyert " + _model.BluePoints + " ponttal!",
                         FencingGame.Persistence.FieldType.RedPlayer => "A piros nyert " + _model.BluePoints + " ponttal",
                         FencingGame.Persistence.FieldType.NoPlayer => "Döntetlen!",
                         _ => "Hiba történt a kiértékelés során"
                     });
                _model!.NewGame(_model.GameSize);
            
        }

        private void _model_GameFieldChanged(object? sender, (int, int) e)
        {
            throw new NotImplementedException();
        }

        private void NewGame(FencingGame.Model.Size size)
        {
            _model.NewGame(size);
        }
    }
}
