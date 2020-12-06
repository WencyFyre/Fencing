using FencingGame.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FencingGame.Model
{
    public class FencingEventArgs:EventArgs
    {
        public GameSize Tableize{ get; set; }
    }
}
