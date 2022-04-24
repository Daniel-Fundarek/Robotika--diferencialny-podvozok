using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int x,y;
        /// <summary>
        /// ///
        /// </summary>
        private double L = 0.2;  // m  rozchod kolies
        private double d = 0.1;  // m  vzdialenost medzi taziskom a kolesom
        private double r = 0.05; // m  polomer kolesa
        // zadane premenne
        private double[] velLeftWheelArray = new double[] { 2, -1, 1, 1, 1 };
        private double[] velRightWheelArray = new double[] { 2, 1, 0, 2, 1 };
        private double velRightWheel = 0;
        private double velLeftWheel = 0;
        private double[] timeStamps = new double[] { 0, 5, 10, 15, 20 };
        private int timeIndex = 0;
        private double timeDev = 0.01; //s

        private double time = 0; //s

       
        // premenne ktore pocitam
        private double rotVel; // rad/s
        private double linVel; // m/s
        private double angle;  // rad
        private double currTime; //s
        public Position prevPosition = new Position(100,10);
        public Position position = new Position(100,10);
        private Canvas myCanvas;
        /// 
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(timeDev); // TimeSpan.FromMilliseconds(100);
            timer.Tick += timerFcn;
            timer.Start();

            
        }

        public void timerFcn(object sender, EventArgs e)
        {
            changeVelOfWheel();
            linVel = calculateLinVel(velLeftWheel, velRightWheel);
            rotVel = calculateRotVel(velLeftWheel, velRightWheel, L);
            angle = calculateAngle(rotVel, angle, timeDev);
            calculatePosition(position,prevPosition, linVel, angle, timeDev);
            drawLine(MyCanvas,prevPosition, position);
           


        }
        public void drawLine(Canvas MyCanvas, Position lastPosition, Position currPosition)
        {
            // MyCanvas.ActualHeight;
            //   MyCanvas.ActualWidth;
            
            Line line = new Line();
            line.X1 = lastPosition.IntX;
            line.Y1 = MyCanvas.ActualHeight - lastPosition.IntY *2;
            line.X2 = currPosition.IntX ;
            line.Y2 = MyCanvas.ActualHeight - currPosition.IntY *2;
            line.Fill = Brushes.Red;
            line.Stroke = Brushes.Black;
            line.Width = 2;
            MyCanvas.Children.Add(line);
            currTime += timeDev;
        }
       
      /*  private double calculateLinVel(double leftWheelVel, double RightWheelVel)
        {   

            return (leftWheelVel + RightWheelVel) / 2;

        }

        private double calculateRotVel(double leftWheelVel, double rightWheelVel, double wheelBase)
        {
            return (rightWheelVel - leftWheelVel) / wheelBase;
        }

        private double calculateAngle(double rotVelocity, double previousAngle, double timeDeviation)
        {
            double angle = previousAngle + rotVelocity * timeDeviation;
            if (angle > 2* Math.PI)
            {
                angle -= 2* Math.PI;
            }
            else if (angle < 0)
            {
                angle += 2 * Math.PI;
            }

            return angle;
        }

        private void calculatePosition(Position _position,Position _prevPosition, double _linVel, double angleR, double timeDeviation)
        {
            _prevPosition.setX(_position.getX());
            _prevPosition.setY(_position.getY());
            double linDeviation = _linVel * timeDeviation;
            _position.setX(_position.getX() + 3 * linDeviation * Math.Sin(angleR)); // *3
            _position.setY(_position.getY() + 3 * linDeviation * Math.Cos(angleR)); // *3
            

        }

        private void changeVelOfWheel()
        {
            if (timeStamps[timeIndex] <= currTime)
            {
                velLeftWheel = velLeftWheelArray[timeIndex];
                velRightWheel = velRightWheelArray[timeIndex];
                if (timeIndex != timeStamps.Length-1)
                {
                    timeIndex++;
                }
                

            }
        }*/
       




    }
}
