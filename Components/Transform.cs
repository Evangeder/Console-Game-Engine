using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame.Behaviour
{
    public class Transform
    {
        public Vector2 position = new Vector2(0, 0);

        public void translate(Vector2 newPosition)
        {
            position += newPosition;
        }
        public void translate(int left, int top)
        {
            translate(new Vector2(left, top));
        }
    }
}
