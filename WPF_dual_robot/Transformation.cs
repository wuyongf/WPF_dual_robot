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

        // general
        public List<double[]> via_points = new List<double[]>();

        /// <summary>
        /// single circle
        /// </summary>
        public double[] center_point = new double[6];

        public double radius = 150;

        public double step_radian = 0.1;

        public Matrix<double> RMat;

        public List<double[]> measure_points = new List<double[]>();

        /// <summary>
        /// multi circle
        /// </summary>

        public double orbit_radius = 150;
        public double orbit_step_radian = Math.PI/2;
        public List<double[]> orbit_points = new List<double[]>();

        public double d2r(double degree)
        {
            double radian = Math.PI / 180.0 * degree;
            return radian;
        }

        public double r2d(double radian)
        {
            var degree = radian * 180.0 / Math.PI;
            return degree;
        }
        public Matrix<double> rpy2R(double roll, double pitch, double yaw)
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

        public List<double[]> get_measure_points(double[] center_point, Matrix<double> RMat, double radius, double step_radian)
        {
            measure_points.Clear();

            for (double r = 0; r <= Math.PI/2; r = r + step_radian)
            {
                double[] measure_point = new double[6];

                measure_point[0] = center_point[0] + Math.Sin(r) * radius * RMat[0,1] + Math.Cos(r) * radius * RMat[0,2];
                measure_point[1] = center_point[1] + Math.Sin(r) * radius * RMat[1,1] + Math.Cos(r) * radius * RMat[1,2];
                measure_point[2] = center_point[2] + Math.Sin(r) * radius * RMat[2,1] + Math.Cos(r) * radius * RMat[2,2];
                measure_point[3] = center_point[3] - r2d(r);
                measure_point[4] = center_point[4] ;
                measure_point[5] = center_point[5] ;

                measure_points.Add(measure_point);
            }

            return measure_points;
        }

        public List<double[]> get_orbit_points(double[] center_point, Matrix<double> RMat, double radius, double step_radian)
        {
            orbit_points.Clear();

            for (double r = 0; r <= 2 * Math.PI; r = r + step_radian)
            {
                double[] orbit_point = new double[6];

                orbit_point[0] = center_point[0] + Math.Sin(r) * radius * RMat[0, 0] + Math.Cos(r) * radius * RMat[0, 1];
                orbit_point[1] = center_point[1] + Math.Sin(r) * radius * RMat[1, 0] + Math.Cos(r) * radius * RMat[1, 1];
                orbit_point[2] = center_point[2] + Math.Sin(r) * radius * RMat[2, 0] + Math.Cos(r) * radius * RMat[2, 1];
                orbit_point[3] = center_point[3];
                orbit_point[4] = center_point[4];
                orbit_point[5] = center_point[5];

                orbit_points.Add(orbit_point);
            }

            return orbit_points;
        }
    }
}
