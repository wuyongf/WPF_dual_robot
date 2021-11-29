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

        /// rb: single circle
        public double[] rb_center_point = new double[6];
        public double rb_measure_radius = 150;
        public double rb_measure_step_radian = 0.1;
        public Matrix<double> RMat;
        public List<double[]> rb_measure_points = new List<double[]>();

        /// rb: multi circle
        public double rb_orbit_radius = 150;
        public double rb_orbit_step_angle = Math.PI/2;
        public List<double[]> rb_orbit_points = new List<double[]>();

        /// uf: measure circle
        public double[] uf_center_point = new double[6];
        public double uf_measure_radius = 150;
        public double uf_measure_arc;
        public double uf_measure_step_angle = 0.1;
        public List<double[]> uf_measure_points = new List<double[]>();
        public int uf_measure_points_no;

        public List<double[]> status_measure_points = new List<double[]>();

        /// uf: orbit circle
        public double uf_orbit_radius = 150;
        public double uf_orbit_step_angle = 1;
        public List<double[]> uf_orbit_points = new List<double[]>();

        public List<double[]> status_orbit_points = new List<double[]>();

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
        // 2. get rb_measure_radius
        // 3. get x,y,z
        // 4. get the via points.

        public List<double[]> get_rb_measure_points(double[] rb_center_point, Matrix<double> RMat, double radius, double step_radian)
        {
            rb_measure_points.Clear();

            for (double r = 0; r <= Math.PI/2; r = r + step_radian)
            {
                double[] measure_point = new double[6];

                measure_point[0] = rb_center_point[0] + Math.Sin(r) * radius * RMat[0,1] + Math.Cos(r) * radius * RMat[0,2];
                measure_point[1] = rb_center_point[1] + Math.Sin(r) * radius * RMat[1,1] + Math.Cos(r) * radius * RMat[1,2];
                measure_point[2] = rb_center_point[2] + Math.Sin(r) * radius * RMat[2,1] + Math.Cos(r) * radius * RMat[2,2];
                measure_point[3] = rb_center_point[3] - r2d(r);
                measure_point[4] = rb_center_point[4] ;
                measure_point[5] = rb_center_point[5] ;

                rb_measure_points.Add(measure_point);
            }

            return rb_measure_points;
        }

        public List<double[]> get_rb_orbit_points(double[] rb_center_point, Matrix<double> RMat, double radius, double step_radian)
        {
            rb_orbit_points.Clear();

            for (double r = 0; r <= 2 * Math.PI; r = r + step_radian)
            {
                double[] orbit_point = new double[6];

                orbit_point[0] = rb_center_point[0] + Math.Sin(r) * radius * RMat[0, 0] + Math.Cos(r) * radius * RMat[0, 1];
                orbit_point[1] = rb_center_point[1] + Math.Sin(r) * radius * RMat[1, 0] + Math.Cos(r) * radius * RMat[1, 1];
                orbit_point[2] = rb_center_point[2] + Math.Sin(r) * radius * RMat[2, 0] + Math.Cos(r) * radius * RMat[2, 1];
                orbit_point[3] = rb_center_point[3];
                orbit_point[4] = rb_center_point[4];
                orbit_point[5] = rb_center_point[5];

                rb_orbit_points.Add(orbit_point);
            }

            return rb_orbit_points;
        }

        public List<double[]> get_uf_orbit_points(double radius, double step_degree)
        {
            // init
            uf_orbit_points.Clear();

            var n = 360.0 / step_degree;

            for (double m = 0; m <= n; m = m + step_degree)
            {
                var r = d2r(m);

                double[] orbit_point = new double[6];

                orbit_point[0] = Math.Cos(r) * radius;
                orbit_point[1] = Math.Sin(r) * radius;
                orbit_point[2] = 0;
                orbit_point[3] = 0;
                orbit_point[4] = 0;
                orbit_point[5] = 0;

                uf_orbit_points.Add(orbit_point);
            }

            return uf_orbit_points;
        }

        public List<double[]> get_uf_measure_points(double radius, double arc, double step_angle)
        {
            uf_measure_points.Clear();

            List<double[]> uf_measure_points_sub1 = new List<double[]>();
            List<double[]> uf_measure_points_sub2 = new List<double[]>();

            var sub_arc = arc / 2;

            var n = sub_arc / step_angle;

            // sub_1
            //  1. list out the points
            for (double m = 0; m <= n; m = m + step_angle)
            {
                var r = d2r(m);
            
                double[] measure_point = new double[6];
            
                measure_point[0] = 0;
                measure_point[1] = Math.Sin(r) * radius;
                measure_point[2] = Math.Cos(r) * radius;
                measure_point[3] = -m;
                measure_point[4] = 0;
                measure_point[5] = 0;
            
                uf_measure_points_sub1.Add(measure_point);
            }
            // 2. rearrange
            uf_measure_points_sub1.Reverse();

            // half_2
            for (double m = 0; m <= n; m = m + step_angle)
            {
                var r = d2r(m);

                double[] measure_point = new double[6];

                measure_point[0] = 0;
                measure_point[1] = -Math.Sin(r) * radius;
                measure_point[2] = Math.Cos(r) * radius;
                measure_point[3] = m;
                measure_point[4] = 0;
                measure_point[5] = 0;

                uf_measure_points_sub2.Add(measure_point);
            }

            // uf_measure_points
            uf_measure_points.AddRange(uf_measure_points_sub1);
            uf_measure_points.AddRange(uf_measure_points_sub2);

            return uf_measure_points;
        }

    }
}
