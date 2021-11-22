using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRRJIf;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace WPF_dual_robot
{
    public class Transformation
    {
        // x, y, z, r, v1, v2

        public static double[] center_point = new double[6]; 

        public static double radius = 0;

        public static Matrix<double> RMat;

        public static List<double[]> via_points = new List<double[]>();

        public static double d2r(double degree)
        {
            double radian = Math.PI / 180.0 * degree;
            return radian;
        }

        public static double r2d(double radian)
        {
            var degree = radian * 180.0 / Math.PI;
            return degree;
        }
        public static Matrix<double> rpy2R(double roll, double pitch, double yaw)
        {
            var alpha = d2r(yaw);
            var beta = d2r(pitch);
            var gamma = d2r(roll);

            var ca = Math.Cos(alpha);
            var cb = Math.Cos(beta);
            var cg = Math.Cos(gamma);
            var sa = Math.Sin(alpha);
            var sb = Math.Sin(beta);
            var sg = Math.Sin(gamma);

            Matrix<double> Ra = DenseMatrix.OfArray(new double[,] {
                {ca,-sa,0},
                {sa,ca,0},
                {0,0,1}});

            Matrix<double> Rb = DenseMatrix.OfArray(new double[,] {
                {cb,0,sb},
                {0,1,0},
                {-sb,0,cb}});

            Matrix<double> Rg = DenseMatrix.OfArray(new double[,] {
                {1,0,0},
                {0,cg,-sg},
                {0,sg,cg}});

            var R = Ra * Rb * Rg;

            return R;
        }

        // 1. get Rotation Matrix
        // 2. get radius
        // 3. get x,y,z
        // 4. get the via points.

        public static void get_via_points(double[] center_point, Matrix<double> RMat, double radius, double step_radian)
        {
            for (double r = 0; r <= 2 * Math.PI; r = r + step_radian)
            {
                double[] via_point = new double[6];

                via_point[0] = center_point[0] + Math.Sin(r) * radius * RMat[0,1] + Math.Cos(r) * radius * RMat[0,2];
                via_point[1] = center_point[1] + Math.Sin(r) * radius * RMat[1,1] + Math.Cos(r) * radius * RMat[1,2];
                via_point[2] = center_point[2] + Math.Sin(r) * radius * RMat[2,1] + Math.Cos(r) * radius * RMat[2,2];
                via_point[3] = center_point[3];
                via_point[4] = center_point[4];
                via_point[5] = center_point[5];

                via_points.Add(via_point);
            }
        }
    }
}
