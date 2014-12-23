using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using DotSpatial.Topology;
using Point = System.Drawing.Point;

namespace DotSpatial.Plugins.BruTileLayer.Reprojection
{
    internal class WorldFile
    {
        private readonly Matrix2D _matrix = new Matrix2D();
        private Matrix2D _inverse;
        
        /// <summary>
        /// Creates an instance of this class
        /// </summary>
        /// <param name="a11">x-component of the pixel width</param>
        /// <param name="a21">y-component of the pixel width</param>
        /// <param name="a12">x-component of the pixel height</param>
        /// <param name="a22">y-component of the pixel height</param>
        /// <param name="b1">x-ordinate of the center of the top left pixel</param>
        /// <param name="b2">y-ordinate of the center of the top left pixel</param>
        public WorldFile(double a11 = 1d, double a21 = 0d, double a12 = 0d, double a22 = -1, double b1 = 0d, double b2 = 0d)
        {
            _matrix.A11 = a11;
            _matrix.A21 = a21;
            _matrix.A12 = a12;
            _matrix.A22 = a22;
            _inverse = _matrix.Inverse();

            B1 = b1;
            B2 = b2;
        }

        /// <summary>
        /// Loads a world file
        /// </summary>
        /// <param name="file">The filename</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public void Load(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");
            if (File.Exists(file))
                throw new ArgumentException(string.Format("File '{0}' not found", file), "file");

            using (var sr = new StreamReader(file))
            {
                _matrix.A11 = double.Parse(sr.ReadLine(), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                _matrix.A21 = double.Parse(sr.ReadLine(), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                _matrix.A12 = double.Parse(sr.ReadLine(), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                _matrix.A22 = double.Parse(sr.ReadLine(), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                B1 = double.Parse(sr.ReadLine(), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
                B2 = double.Parse(sr.ReadLine(), NumberStyles.Float, NumberFormatInfo.InvariantInfo);
            }
            _inverse = _matrix.Inverse();
        }

        /// <summary>
        /// Saves a world file
        /// </summary>
        /// <param name="file">The filename</param>
        /// <exception cref="ArgumentNullException"/>
        public void Save(string file)
        {
            if (string.IsNullOrEmpty(file))
                throw new ArgumentNullException("file");

            using (var sw = new StreamWriter(file))
            {
                sw.WriteLine(A11.ToString("R", NumberFormatInfo.InvariantInfo));
                sw.WriteLine(A21.ToString("R", NumberFormatInfo.InvariantInfo));
                sw.WriteLine(A12.ToString("R", NumberFormatInfo.InvariantInfo));
                sw.WriteLine(A22.ToString("R", NumberFormatInfo.InvariantInfo));
                sw.WriteLine(B1.ToString("R", NumberFormatInfo.InvariantInfo));
                sw.WriteLine(B2.ToString("R", NumberFormatInfo.InvariantInfo));
            }
        }

        /// <summary>
        /// x-component of the pixel width
        /// </summary>
        public double A11 { get { return _matrix.A11; } }

        /// <summary>
        /// y-component of the pixel width
        /// </summary>
        public double A21 { get { return _matrix.A21; } }

        /// <summary>
        /// x-component of the pixel height
        /// </summary>
        public double A12 { get { return _matrix.A12; } }

        /// <summary>
        /// y-component of the pixel height (negative most of the time)
        /// </summary>
        public double A22 { get { return _matrix.A22; } }

        /// <summary>
        /// x-ordinate of the center of the top left pixel
        /// </summary>
        public double B1 { get; private set; }

        /// <summary>
        /// y-ordinate of the center of the top left pixel
        /// </summary>
        public double B2 { get; private set; }

        /// <summary>
        /// Gets a value indicating the point (<see cref="B1"/>, <see cref="B2"/>).
        /// </summary>
        public Coordinate Location { get { return new Coordinate(B1, B2);} }

        /// <summary>
        /// Function to compute the ground coordinate for a given <paramref name="x"/>, <paramref name="y"/> pair.
        /// </summary>
        /// <param name="x">The x pixel</param>
        /// <param name="y">The y pixel</param>
        /// <returns>The ground coordinate</returns>
        public Coordinate ToGround(int x, int y)
        {
            var resX = B1 + (A11*x + A21*y);
            var resY = B2 + (A12*x + A22*y);

            return new Coordinate(resX, resY);
        }

        /// <summary>
        /// Function to compute the ground x-ordinate for a given <paramref name="x"/>, <paramref name="y"/> pair.
        /// </summary>
        /// <param name="x">The x pixel</param>
        /// <param name="y">The y pixel</param>
        /// <returns>The ground coordinate</returns>
        public double ToGroundX(int x, int y)
        {
            return B1 + (A11 * x + A21 * y);
        }
        /// <summary>
        /// Function to compute the ground y-ordinate for a given <paramref name="x"/>, <paramref name="y"/> pair.
        /// </summary>
        /// <param name="x">The x pixel</param>
        /// <param name="y">The y pixel</param>
        /// <returns>The ground coordinate</returns>
        public double ToGroundY(int x, int y)
        {
            return B2 + (A12 * x + A22 * y);
        }

        /// <summary>
        /// Function to compute the ground bounding-ordinate for a given <paramref name="x"/>, <paramref name="y"/> pair.
        /// </summary>
        /// <param name="width">The width pixel</param>
        /// <param name="height">The height pixel</param>
        /// <returns>The ground coordinate</returns>
        public IPolygon ToGroundBounds(int width, int height)
        {
            var ringCoordinates = new List<Coordinate>(5);
            var leftTop = ToGround(0, 0);
            ringCoordinates.AddRange(new []
            {
                leftTop,
                ToGround(0, height),
                ToGround(width, 0),
                ToGround(width, height),
                leftTop
            });

            var ring = GeometryFactory.Default.CreateLinearRing(ringCoordinates);
            return GeometryFactory.Default.CreatePolygon(ring, null);
        }

        public System.Drawing.Point ToRaster(Coordinate point)
        {
            var px = point.X - B1;
            var py = point.Y - B2;

            var x = (int) Math.Round(_inverse.A11*px + _inverse.A21*py, 
                MidpointRounding.AwayFromZero);
            var y = (int) Math.Round(_inverse.A12*px + _inverse.A22*py, 
                MidpointRounding.AwayFromZero);

            return new System.Drawing.Point(x,y);
        }

        public int ToRasterX(Coordinate point)
        {
            point.X -= B1;
            point.Y -= B2;

            return (int) Math.Round(_inverse.A11*point.X + _inverse.A21*point.Y,
                MidpointRounding.AwayFromZero);
        }

        public int ToRasterY(Coordinate point)
        {
            point.X -= B1;
            point.Y -= B2;

            return (int)Math.Round(_inverse.A12 * point.X + _inverse.A22 * point.Y, 
                MidpointRounding.AwayFromZero);
        }

        private class Matrix2D
        {
            /// <summary>
            /// x-component of the pixel width
            /// </summary>
            public double A11 { get; set; }

            /// <summary>
            /// y-component of the pixel width
            /// </summary>
            public double A21 { get; set; }

            /// <summary>
            /// x-component of the pixel height
            /// </summary>
            public double A12 { get; set; }

            /// <summary>
            /// y-component of the pixel height (negative most of the time)
            /// </summary>
            public double A22 { get; set; }

            /// <summary>
            /// Gets a value indicating the determinant of this matrix
            /// </summary>
            private double Determinant { get { return A22*A11 - A21*A12; }}

            /// <summary>
            /// Gets a value indicating that <see cref="Inverse()"/> can be computed.
            /// </summary>
            /// <remarks>
            /// Shortcut for <c><see cref="Determinant"/> != 0d</c>
            /// </remarks>
            private bool IsInvertible { get { return Determinant != 0d; }}

            /// <summary>
            /// Method to compute the inverse Matrix of this matrix
            /// </summary>
            /// <returns>The inverse matrix</returns>
            /// <exception cref="Exception"/>
            public Matrix2D Inverse()
            {
                if (!IsInvertible)
                    throw new Exception("Matrix not invertible");

                var det = Determinant;

                return new Matrix2D
                {
                    A11 = A22/det,
                    A21 = -A21/det,
                    A12 = -A12/det,
                    A22 = A11/det
                };
            }
        }
    }
}