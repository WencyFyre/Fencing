using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FencingGame.Model;

namespace FencingGame.Model
{
    public class FencingEventArgs:EventArgs
    {
        public Size Tableize{ get; set; }
    }
}
