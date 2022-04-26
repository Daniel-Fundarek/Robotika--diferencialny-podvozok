using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp1
{   
    
    internal class Controller
    {
        private const int OFFSETX = 200; // pozicia na canvase na zaciatku
        private const int OFFSETY = 200; // pozicia na canvase na zaciatku

        private const int SCALE = 100;
        private double L = 0.2;  // m  rozchod kolies
        private const double D = 0.1;  // m  vzdialenost medzi taziskom a kolesom
        private double r = 0.05; // m  polomer kolesa
        // zadane premenne
        private double[] velLeftWheelArray = new double[] { 1.25, 1, 1, 2, 1, 1, 0};
        private double[] velRightWheelArray = new double[] { 1, 1, 0, 1, 1, 2, 0};
        private double[] timeStamps = new double[] { 0, 5, 6, 15, 24, 25, 30 };
        /// <summary>
        /// /////////
        /// </summary>
        private double velRightWheel = 0;
        private double velLeftWheel = 0;
        
        private int timeIndex = 0;
        public double timeDev = 0.001; //s

        

         
        // premenne ktore pocitam
        private double rotVel; // rad/s
        private double linVel; // m/s
        private double angle;  // rad
        private double currTime; //s
        public Position prevPosition = new Position(OFFSETX, OFFSETY);
        public Position position = new Position(OFFSETX, OFFSETY);

        public Position prevLeftWheel =new Position();
        public Position prevRightWheel = new Position();
        public Position leftWheel= new Position(OFFSETX - SCALE*D, OFFSETY);
        public Position rightWheel= new Position(OFFSETX + SCALE * D, OFFSETY);

        private hTransform transform;

        public Controller()
        {
            this.transform = new hTransform();
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
         /*   prevLeftWheel.setX(leftWheel.getX());
            prevRightWheel.setX(rightWheel.getX());
            prevLeftWheel.setY(leftWheel.getX());
            prevRightWheel.setY(rightWheel.getY());
            leftWheel = transform.rotateMatrix(angle - Math.PI/2, d * 100, position);
            rightWheel = transform.rotateMatrix(angle + Math.PI/2, d * 100, position);*/


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
    }
}
