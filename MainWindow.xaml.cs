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
       // int x,y;
        /// <summary>
        /// ///
        /// </summary>
      /*  private double L = 0.2;  // m  rozchod kolies
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
        public Position position = new Position(100,10);*/
        private Canvas myCanvas;
        private Controller controller;
        /// 
        public MainWindow()
        {
            InitializeComponent();
            this.controller = new Controller();

             controller.drawRect(1);
            //controller.drawSLine(2,3,1);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(controller.timeDev); // TimeSpan.FromMilliseconds(100);
            timer.Tick += timerFcn;
            timer.Start();

            
        }

        public void timerFcn(object sender, EventArgs e)
        {
            controller.runLogic();
            drawLine(MyCanvas,controller.prevPosition, controller.position,0);
            drawLine(MyCanvas, controller.prevLeftWheel, controller.leftWheel, 1);
            drawLine(MyCanvas, controller.prevRightWheel, controller.rightWheel, 2);



        }
        public void drawLine(Canvas MyCanvas, Position lastPosition, Position currPosition,int color)
        {
            // MyCanvas.ActualHeight;
            //   MyCanvas.ActualWidth;
            
            Line line = new Line();
            line.X1 = lastPosition.IntX;
            line.Y1 = MyCanvas.ActualHeight - lastPosition.IntY;
            line.X2 = currPosition.IntX ;
            line.Y2 = MyCanvas.ActualHeight - currPosition.IntY ;
            if (color == 0)
            {
                line.Fill = Brushes.Red;
                line.Stroke = Brushes.Red;
            }
            else if (color == 1)
            {
                line.Fill = Brushes.Green;
                line.Stroke = Brushes.Green;
            }
            else
            {
                line.Fill = Brushes.Blue;
                line.Stroke = Brushes.Blue;
            }
            
           
            MyCanvas.Children.Add(line);
            
        }
       


    }
}
