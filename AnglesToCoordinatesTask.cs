using System;
using System.Drawing;
using NUnit.Framework;

namespace Manipulation
{
    public static class AnglesToCoordinatesTask
    {
        public static PointF[] GetJointPositions(double shoulder, double elbow, double wrist)
        {
            var elbowPos = GetSecondPointOfVector(Manipulator.UpperArm, shoulder, 0, 0);
            var foreamAngle = shoulder + Math.PI + elbow;
            var wristPos = GetSecondPointOfVector(Manipulator.Forearm, foreamAngle, elbowPos.X, elbowPos.Y);
            var palmAngle = foreamAngle + Math.PI + wrist;
            var palmEndPos = GetSecondPointOfVector(Manipulator.Palm, palmAngle, wristPos.X, wristPos.Y);
            return new PointF[]
            {
                elbowPos,
                wristPos,
                palmEndPos
            };
        }

        public static PointF GetSecondPointOfVector(float handLength, double angleToOrdinate, float x, float y)
        {
            var posY = handLength * Math.Sin(angleToOrdinate);
            var posX = handLength * Math.Sin(Math.PI / 2 - angleToOrdinate);
            return new PointF((float)posX + x, (float)posY + y);
        }
    }

    [TestFixture]
    public class AnglesToCoordinatesTask_Tests
    {
        [TestCase(Math.PI / 2, Math.PI / 2, Math.PI, Manipulator.Forearm + Manipulator.Palm, Manipulator.UpperArm)]
        [TestCase(0, Math.PI, Math.PI, Manipulator.Forearm + Manipulator.Palm + Manipulator.UpperArm, 0)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI / 2, Manipulator.Palm, Manipulator.Forearm + Manipulator.UpperArm)]
        [TestCase(Math.PI / 2, Math.PI, Math.PI, 0, Manipulator.Forearm + Manipulator.UpperArm + Manipulator.Palm)]
        public void TestGetJointPositions(double shoulder, double elbow, double wrist, double palmEndX, double palmEndY)
        {
            var joints = AnglesToCoordinatesTask.GetJointPositions(shoulder, elbow, wrist);
            Assert.AreEqual(palmEndX, joints[2].X, 1e-5, "palm endX");
            Assert.AreEqual(palmEndY, joints[2].Y, 1e-5, "palm endY");
        }
    }
}