using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1
{   
    
    internal class Controller
    {
        private const int OFFSETX = 1000;//200; // pozicia na canvase na zaciatku
        private const int OFFSETY = 500;//200; // pozicia na canvase na zaciatku

        private const int SCALE = 100;   // mierka
        private double L = 0.2;          // m  rozchod kolies
        private const double D = 0.1;    // m  vzdialenost medzi taziskom a kolesom
        private double r = 0.05;         // m  polomer kolesa
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////// zadane premenne ///////
        private double[] velLeftWheelArray = new double[] { 1.25, 1, 1, -1, 1, 1, 0}; // vektor rychlosti laveho kolesa                    ///  
        private double[] velRightWheelArray = new double[] { 1, -1, 1, 1, 1, 0,5, 0};   // vektor rychlosti praveho kolesa                   ///
        private double[] timeStamps = new double[] { 0, 5, 6, 7, 9, 10, 11 };      // vektor casov                                        ///
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        private double velRightWheel = 0; // rychlost praveho kolesa
        private double velLeftWheel = 0;  // rychlost laveho kolesa
        private int timeIndex = 0;      
        public double timeDev = 0.01;     //  rychlost simulacie/frekvencia casovaca v sekundach     
        private int leftRightArrow;       // Keyboard sipky LEFT = -1 || RIGHT = 1
        private int upDownArrow;          // Keyboard sipky DOWN = -1 || UP = 1
        private double rotVel;            // rad/s
        private double linVel;            // m/s
        private double angle;             // rad
        private double currTime;          //s
        public Position prevPosition = new Position(OFFSETX, OFFSETY);                  // predchadzajuca pozicia Taziska
        public Position position = new Position(OFFSETX, OFFSETY);                      // pozicia Taziska
        public Position prevLeftWheel =new Position();                                      // predchadzajuca pozicia laveho kolesa
        public Position prevRightWheel = new Position();                                    // predchadzajuca pozicia praveho kolesa
        public Position leftWheel= new Position(OFFSETX - SCALE*D, OFFSETY);            //  pozicia laveho kolesa
        public Position rightWheel= new Position(OFFSETX + SCALE * D, OFFSETY);         //  pozicia praveho kolesa


        public Controller()
        {

        }


        public void runGame()
        {
            
            KeyListener();
            Debug.WriteLine(leftRightArrow); 
            changeRotVel();
            angle = calculateAngle(rotVel, angle, timeDev);
            calculatePosition(position, prevPosition, linVel, angle, timeDev);
            calculateWheelPosition(position, rightWheel, prevRightWheel, D, angle + Math.PI / 2);
            calculateWheelPosition(position, leftWheel, prevLeftWheel, D, angle - Math.PI / 2);
        }

        public void runLogic()
        {
            changeVelOfWheel();
            linVel = calculateLinVel(velLeftWheel, velRightWheel);
            rotVel = calculateRotVel(velLeftWheel, velRightWheel, L);
            angle = calculateAngle(rotVel, angle, timeDev);
            calculatePosition(position, prevPosition, linVel, angle, timeDev);
            
            calculateWheelPosition(position, rightWheel, prevRightWheel, D,angle +Math.PI/2);
            
            calculateWheelPosition(position,leftWheel, prevLeftWheel, D, angle -Math.PI / 2);




            currTime += timeDev;
        }
        private double calculateLinVel(double leftWheelVel, double RightWheelVel)
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
            if (angle > 2 * Math.PI)
            {
                angle -= 2 * Math.PI;
            }
            else if (angle < 0)
            {
                angle += 2 * Math.PI;
            }

            return angle;
        }

        private void calculatePosition(Position _position, Position _prevPosition, double _linVel, double angleR, double timeDeviation)
        {
            _prevPosition.setX(_position.getX());
            _prevPosition.setY(_position.getY());
            double linDeviation = _linVel * timeDeviation * SCALE;
            _position.setX(_position.getX() +  linDeviation * Math.Sin(angleR)); // *3
            _position.setY(_position.getY() +  linDeviation * Math.Cos(angleR)); // *3
        }

        private void calculateWheelPosition(Position reference,Position _position, Position _prevPosition, double linDeviation, double angleR)
        {
            _prevPosition.setX(_position.getX());
            _prevPosition.setY(_position.getY());
            
            _position.setX(reference.getX() + SCALE * linDeviation * Math.Sin(angleR)); // *3
            _position.setY(reference.getY() + SCALE * linDeviation * Math.Cos(angleR)); // *3
        }
        private void changeVelOfWheel()
        {
            if (timeStamps[timeIndex] <= currTime)
            {
                velLeftWheel = velLeftWheelArray[timeIndex];
                velRightWheel = velRightWheelArray[timeIndex];
                if (timeIndex != timeStamps.Length - 1)
                {
                    timeIndex++;
                }


            }
        }

        public void drawRect(int size)
        {
        velLeftWheelArray = new double[] { 2, 2, 2, 2, 2, 2, 2, 2, 0 };
        velRightWheelArray = new double[] { 2, -2, 2, -2, 2, -2, 2, -2, 0 };
        timeStamps = new double[velLeftWheelArray.Length];
        for (int i = 0; i < velLeftWheelArray.Length - 1; i++)
        {
            if (i == 0)
            {
                timeStamps[0] = 0;
            }
            if (calculateLinVel(velLeftWheelArray[i], velRightWheelArray[i]) != 0)
            {
                timeStamps[i+1] = timeStamps[i] + size / Math.Abs(calculateLinVel(velLeftWheelArray[i], velRightWheelArray[i]));
            }
            else if (calculateRotVel(velLeftWheelArray[i], velRightWheelArray[i],L)!= 0)
            {
                timeStamps[i+1] = timeStamps[i] +(Math.PI / 2) / Math.Abs(calculateRotVel(velLeftWheelArray[i], velRightWheelArray[i],L));
            }
            else
            {
                timeStamps[i+1]=  0;
            }
            
        }
        
        }

        public void drawSLine(double R1, double L1, double R2)
        {
            velLeftWheelArray = new double[4];
            velRightWheelArray = new double[4];
            timeStamps = new double[4];
            double vt = 2;
            double vr = R1 * vt * 2 / L - vt;
            double vl = vr - vt * 2;
            double speedLimit = (vr + vl / 2) / vt;
            velLeftWheelArray[0] = vl/speedLimit;
            velRightWheelArray[0] = vr/speedLimit;

            timeStamps[0] = 0;
            timeStamps[1] = (Math.PI / 2) /calculateRotVel(vl,vr,L)*speedLimit;

            velLeftWheelArray[1] = 2;
            velRightWheelArray[1] = 2;
            timeStamps[2] = timeStamps[1]+ L1 / calculateLinVel(2, 2);
            
            vl = R2 * vt * 2 / L - vt;
            vr = vl - vt * 2;
            
            velLeftWheelArray[2] = vl/speedLimit;
            velRightWheelArray[2] = vr/speedLimit;
                
            velLeftWheelArray[3] = 0;
            velRightWheelArray[3] = 0;
            timeStamps[3] = timeStamps[2] + (-Math.PI / 2) / calculateRotVel(vl, vr, L)*speedLimit;


        }

        private void KeyListener()
        {
         
            if ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) > 0)
            {
                leftRightArrow = -1;
            }
            if ((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down)>0)
            {
                leftRightArrow = 1;
            }
            if (((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down) == 0) && ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) == 0) )
            {
                leftRightArrow = 0;
            }

            if ((Keyboard.GetKeyStates(Key.Down) & KeyStates.Down) > 0)
            {
                upDownArrow = -1;
            }
            if ((Keyboard.GetKeyStates(Key.Up) & KeyStates.Down) > 0)
            {
                upDownArrow = 1;
            }
            if (((Keyboard.GetKeyStates(Key.Up) & KeyStates.Down) == 0) && ((Keyboard.GetKeyStates(Key.Down) & KeyStates.Down) == 0))
            {
                upDownArrow = 0;
            }
        }
        public void changeRotVel()
        {
          
                // rot velocity
                //arrow 1 = right
                //arrow -1 = left
                // arrow 0 = no change
               
                if (upDownArrow == 1 && linVel < 4)
                {
                    linVel += 0.05;
                }

                if (upDownArrow == -1 && linVel > -4)
                {
                    linVel -= 0.05;
                }

                if (upDownArrow == 0 && linVel > 0)
                {
                    linVel -= 0.05;
                }
                else if (upDownArrow == 0 && linVel < 0)
                {
                    linVel += 0.05;
                }
                //////////////
                /// lin velocity
                if (leftRightArrow == 1 && rotVel < 20)
                {
                    rotVel += 0.2;
                }

                if (leftRightArrow == -1 && rotVel > -20)
                {
                    rotVel -= 0.2;
                }

                if (leftRightArrow == 0 && rotVel > 0)
                {
                    rotVel -= 0.2;
                }
                else if (leftRightArrow == 0 && rotVel < 0)
                {
                    rotVel += 0.2;
                }

                
            


        }





    }
}
