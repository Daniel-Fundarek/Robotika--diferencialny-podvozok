using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;


namespace WpfApp1
{
    internal class hTransform
    {
        
        

        public Position rotateMatrix(double angle, double yTranslation, Position position)
        {
            Point p = new Point();
            p.X = position.IntX;
            p.Y = position.IntY;
            Matrix matrix = new Matrix();
            matrix.Rotate(angle);
            matrix.Translate(0,yTranslation);
            matrix.Transform(p);
            Position newPosition = new Position();
            newPosition.setX(p.X);
            newPosition.setY(p.Y);
            return newPosition;

        }
    }
}
