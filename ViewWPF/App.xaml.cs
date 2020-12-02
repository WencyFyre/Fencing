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


namespace ViewWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private FencingModel _model;
        private ViewModel _viewModel;
        private MainWindow _view;

        public App()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new FencingModel(new FileDataAccess<FencingTable>());
            _model.GameFieldChanged += _model_GameFieldChanged;
            _model.GameOver += _model_GameOver;
            _model.PlayerChanged += _model_PlayerChanged;
            _model.TableChanged += _model_TableChanged;

            _viewModel = new ViewModel();


            _view = new MainWindow
            {
                DataContext = _viewModel,
            };
            _view.Show();





        }

        private void _model_TableChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void _model_PlayerChanged(object sender, FieldType e)
        {
            throw new NotImplementedException();
        }

        private void _model_GameOver(object sender, FieldType e)
        {
            throw new NotImplementedException();
        }

        private void _model_GameFieldChanged(object sender, (int, int) e)
        {
            throw new NotImplementedException();
        }
    }
}
