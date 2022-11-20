using System;
using NUnit.Framework;

namespace Manipulation
{
    public static class ManipulatorTask
    {
        /// <summary>
        /// Возвращает массив углов (shoulder, elbow, wrist),
        /// необходимых для приведения эффектора манипулятора в точку x и y 
        /// с углом между последним суставом и горизонталью, равному alpha (в радианах)
        /// См. чертеж manipulator.png!
        /// </summary>
        public static double[] MoveManipulatorTo(double x, double y, double alpha)
        {
            // Используйте поля Forearm, UpperArm, Palm класса Manipulator
            var palmStartY = (Manipulator.Palm * Math.Sin(Math.PI - alpha)) + y; 
            var palmStartX = (Manipulator.Palm * Math.Sin((Math.PI / 2) - (Math.PI - alpha))) + x;
            var rangeToPalmStart = Math.Sqrt(palmStartX * palmStartX + palmStartY * palmStartY);
            var elbow = TriangleTask.GetABAngle(Manipulator.UpperArm, Manipulator.Forearm, rangeToPalmStart);
            if (elbow == double.NaN) return new[] { double.NaN, double.NaN, double.NaN };
            var shoulder = TriangleTask.GetABAngle(rangeToPalmStart, Manipulator.UpperArm, Manipulator.Forearm)
                + Math.Atan2(palmStartY, palmStartX);
            if (shoulder == double.NaN) return new[] { double.NaN, double.NaN, double.NaN };
            var wrist = -alpha - shoulder - elbow;
            if (wrist == double.NaN) return new[] { double.NaN, double.NaN, double.NaN };
            return new[] { shoulder, elbow, wrist };
        }
    }

    [TestFixture]
    public class ManipulatorTask_Tests
    {
        [Test]
        public void TestMoveManipulatorTo()
        {
            var random = new Random();
            double x;
            double y;
            double alpha;
            for (int i = 0; i < 3; i++)
            {
                x = random.NextDouble() * 100;
                y = random.NextDouble() * 100;
                alpha = random.NextDouble() * Math.PI;
                var joints = ManipulatorTask.MoveManipulatorTo(x, y, alpha);
                if (x + y > Manipulator.Palm + Manipulator.UpperArm + Manipulator.Forearm)
                    Assert.AreEqual(double.NaN, joints[0]);
            }
            Assert.Fail("Write randomized test here!");
        }
    }
}