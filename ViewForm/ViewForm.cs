using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FencingGame.Model;
using FencingGame.Persistence;

namespace FencingGame
{
    public partial class ViewForm : Form
    {
        
        private readonly FencingModel _model;
        private readonly Dictionary<(int, int), Button> _buttons;
        
        public ViewForm(FencingModel model)
        {
            InitializeComponent();
            _model = model;
            _buttons = new Dictionary<(int, int), Button>() ;
            

            this.toolStripLargeNewGame.Click += (sender, e) => NewGame(Persistence.GameSize.Large);
            this.toolStripMediumNewGame.Click += (sender, e) => NewGame(Persistence.GameSize.Medium);
            this.toolStripSmallNewGame.Click += (sender, e) => NewGame(Persistence.GameSize.Small);
            
            this.toolStripSave.Click += (sender, e) =>
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "SAV files (*.sav)|*.sav",
                    InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments),
                    RestoreDirectory = true
                };
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    _model.SaveAsync(saveDialog.FileName);
                }
            };

            this.toolStripLoad.Click += async (sender, e) =>
            {
                OpenFileDialog openDialog = new OpenFileDialog
                {
                    Filter = "SAV files (*.sav)|*.sav",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    RestoreDirectory = true
                };
                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    await _model.LoadGameAsync(openDialog.FileName);
                }
            };
                

            if(_model.Table.CurrentPlayer == Persistence.FieldType.BluePlayer) this.toolStripStatusLabel3.Text = "Kék van soron";
            else { this.toolStripStatusLabel3.Text = "Piros van soron"; }
            _model.GameFieldChanged +=(sender,e) =>ChangeColor(e);
            _model.GameOver += Model_GameOver;
            _model.TableChanged += (sender, e) => SetupTable();
            SetupTable();


            _model.PlayerChanged += (sender, e) =>
            {
                this.toolStripStatusLabel3.Text = e == Persistence.FieldType.BluePlayer ? "Kék van soron" : "Piros van soron";
                Console.WriteLine(e.ToString());
                };
        }

        private void Model_GameOver(object sender, Persistence.FieldType e)
        {
            MessageBox.Show(
                 e switch
                 {
                     Persistence.FieldType.BluePlayer => "A kék nyert " + _model.BluePoints + " ponttal!",
                     Persistence.FieldType.RedPlayer => "A piros nyert " + _model.BluePoints + " ponttal",
                     Persistence.FieldType.NoPlayer => "Döntetlen!",
                     _ => "Hiba történt a kiértékelés során"
                 }); 
            NewGame(_model.GameSize);
        }

        private void SetupTable()
        {
            this.flowLayoutPanel.Controls.Clear();
            _buttons.Clear();
            this.toolStripStatusLabel1.Text = "Kék: " + _model.BluePoints;
            this.toolStripStatusLabel2.Text = "Piros: " + _model.RedPoints;
            for (int i = 0; i < (int)_model.GameSize; i++)
                    for (int j = 0; j < (int)_model.GameSize; j++)
                    {
                    (int, int) p = (i, j);
                    var button = new Button
                    {
                        Size = new System.Drawing.Size(500 / (int)_model.GameSize, 500 / (int)_model.GameSize)
                    };



                    void MouseEvent(object sender, EventArgs e)
                    {
                        if (_model.Table.CanPlaceBlock(p, _model.IsHorizontal))
                        {
                            button.BackColor = Color.LightGreen;
                            if (_buttons.TryGetValue(p.GetNeighbor(_model.IsHorizontal), out Button buttonpair))
                            {
                                buttonpair.BackColor = Color.LightGreen;
                            }
                        }

                    }
                    button.MouseEnter += (o, e) => MouseEvent(o, e);

                    button.MouseUp += (sender, e) =>
                    {
                        if (e is MouseEventArgs ev)
                        {
                            if (ev.Button == MouseButtons.Left) _model.TryStepGame(p);
                            if (ev.Button == MouseButtons.Right)
                            {
                                _model.ChangeOrientation();
                                ChangeColor(p);
                                ChangeColor(p.GetNeighbor(!_model.IsHorizontal));
                                MouseEvent(null, null);
                                

                            }
                        }
                    };
                    button.MouseLeave += (sender, e) =>
                    {
                        ChangeColor(p);
                        ChangeColor(p.GetNeighbor(_model.IsHorizontal));
                    };

                    this.flowLayoutPanel.Controls.Add(button);
                    _buttons.Add(p, button);
                    ChangeColor(p);
                }         
        }
        private void ChangeColor((int,int) e)
        {

            if (_buttons.TryGetValue(e, out Button button))
            {
                button.BackColor = _model.Table.GetFieldType(e) switch
                {
                    Persistence.FieldType.BluePlayer => Color.Blue,
                    Persistence.FieldType.BluePlayerFenced => Color.LightBlue,
                    Persistence.FieldType.RedPlayer => Color.Red,
                    Persistence.FieldType.RedPlayerFenced => Color.Pink,
                    Persistence.FieldType.NoPlayer => Color.LightGray,
                    _ => Color.Black
                };
                this.toolStripStatusLabel1.Text = "Kék: " + _model.BluePoints;
                this.toolStripStatusLabel2.Text = "Piros: " + _model.RedPoints;
                
                
            }
        }

        private void NewGame(FencingGame.Persistence.GameSize size)
        {
            _model.NewGame(size);
            SetupTable();
        }

    }
}
