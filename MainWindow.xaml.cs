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
        private int mode = 3; // nastavenie modu 0 - zakladny rezim pohyb pomocou dopredu naprogramovanych vektorov rychlosti kolies a casovych vektorov
                              //                 1 - kreslenie stvorca pri kresleni stvorca aby sme vykreslili pravouhly stvorec musime zmensit frekvenciu casovaca
                              //                     alebo zmensit mierku pretoze dochadz k zaokruhlovaniu
                              //                 2 - kreslenie S line
                              //                 3 - hra
        private Canvas myCanvas;
        private Controller controller;
        /// 
        public MainWindow()
        {
            InitializeComponent();
            this.controller = new Controller();
            switch (mode)
            {
                case 0:
                    break;
                case 1:
                    controller.drawRect(1); // rozmer strany stvorca
                    break;
                case 2:
                    controller.drawSLine(3,2,1);  // nastavenie polomerov S krivky
                    break;
                default:
                    break;
            }
            // controller.drawRect(1);
            //controller.drawSLine(2,3,1);
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(controller.timeDev); // TimeSpan.FromMilliseconds(100);
            timer.Tick += timerFcn;
            timer.Start();

            
        }

        public void timerFcn(object sender, EventArgs e)
        {
            if (mode == 3)
            {
                controller.runGame();
            }
            else
            {
                controller.runLogic();
            }
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
            // MyCanvas.Children.RemoveAt(MyCanvas.Children.Count);    // na zmazanie robota

        }
       


    }
}
