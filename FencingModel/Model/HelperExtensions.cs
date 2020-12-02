using System;
using System.Collections.Generic;
using System.Text;

namespace FencingGame.Model
{
    public static class HelperExtensions
    {
        public static (int,int) GetNeighbor(this(int x,int y) p, bool IsHorizontal = true)
        {
            return IsHorizontal ? (p.x, p.y + 1) : (p.x - 1, p.y);
        }
    }
}
