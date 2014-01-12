using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using BruTile.Wmts.Generated;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;

namespace DotSpatial.Plugins.BruTileLayer.Reprojection
{
    internal class TileReprojector
    {
        private readonly ProjectionInfo _source;
        private readonly ProjectionInfo _target;
        private readonly MapArgs _mapArgs;

        public TileReprojector(MapArgs mapArgs, ProjectionInfo source, ProjectionInfo target)
        {
            _source = source;
            _target = target;
            _mapArgs = mapArgs;
        }

        public void Reproject(WorldFile inReference, Bitmap inTile, out WorldFile outReference, out Bitmap outTile)
        {
            // Shortcut when no projections have been assigned or they are of the same reference
            if (_source == null || _target == null || _source == _target)
            {
                outReference = inReference;
                outTile = inTile;
                return;
            }

            // Bounding polygon on the ground
            var ps = inReference.ToGroundBounds(inTile.Width, inTile.Height);

            // Bounding polygon on the ground in target projection
            var pt = ps.Shell.Reproject(_source, _target);

            // The target extent
            var ptExtent = pt.EnvelopeInternal.ToExtent();

            // The target extent projected to the current viewport
            var ptRect = _mapArgs.ProjToPixel(ptExtent);

            // Get the intersection with the current viewport
            ptRect.Intersect(_mapArgs.ImageRectangle);
            
            // Is it empty, don't return anything
            if (ptRect.Width == 0 || ptRect.Height == 0)
            {
                outTile = null;
                outReference = null;
                return;
            }

            var offX = ptRect.X;
            var offY = ptRect.Y;

            // Prepare the result tile
            outTile = new Bitmap(ptRect.Size.Width, ptRect.Size.Height, 
                                 PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(outTile))
            {
                g.Clear(Color.Transparent);
            }

            var caIn  = ColorAccess.Create(inTile);
            // not needed anymore
            var inSize = inTile.Size;
            inTile.Dispose();
            var caOut = ColorAccess.Create(outTile);

            // Copy values to output buffer
            for (var i = 0; i < outTile.Height; i++)
            {
                foreach (Tuple<Point, Point> ppair in
                         GetValidPoints(offY + i, offY, offX, offX + outTile.Width, inReference, inSize))
                {
                    var c = caIn[ppair.Item1.X, ppair.Item1.Y];
                    caOut[ppair.Item2.X, ppair.Item2.Y] = c;
                }

            }
            // Copy to output tile
            SetBitmapBuffer(outTile, caOut.Buffer);

            // Compute the reference
            var outExtent = _mapArgs.PixelToProj(ptRect);
            outReference = new WorldFile(
                outExtent.Width / ptRect.Width, 0,
                0, -outExtent.Height / ptRect.Height,
                outExtent.X, outExtent.Y);
        }

        private IEnumerable<Tuple<Point, Point>> GetValidPoints(int y, int y1, int x1, int x2, WorldFile inReference, Size checkSize)
        {
            var len = (x2 - x1);
            var len2 = len*2;
            var xy = new double[len2];
            var i = 0;
            for (var x = x1; x < x2; x++)
            {
                var c = _mapArgs.PixelToProj(new Point(x, y));
                xy[i++] = c.X;
                xy[i++] = c.Y;
            }

            Projections.Reproject.ReprojectPoints(xy, null, _target, _source, 0, len);

            i = 0;
            y -= y1;
            x2 -= x1;
            for (var x = 0; x < x2; x++)
            {
                var inPoint = inReference.ToRaster(new Coordinate(xy[i++], xy[i++]));
                if (IsValid(checkSize, inPoint))
                    yield return Tuple.Create(inPoint, new Point(x, y));
            }
        }

        private static void SetBitmapBuffer(Bitmap bitmap, byte[] buffer, Rectangle? rect = null)
        {
            if (rect == null) rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var bitmapData = bitmap.LockBits(rect.Value, ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(buffer, 0, bitmapData.Scan0, buffer.Length);
            bitmap.UnlockBits(bitmapData);
        }

        /// <summary>
        /// Function to check if a
        /// </summary>
        /// <param name="size"></param>
        /// <param name="inPoint"></param>
        /// <returns></returns>
        private static bool IsValid(Size size, Point inPoint)
        {
            if (inPoint.X < 0 || inPoint.Y < 0) return false;
            if (inPoint.X < size.Width) 
                if (inPoint.Y < size.Height) return true;
            return false;
        }

        private static int GetPixelSize(PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    return 8;
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    return 16;
                case PixelFormat.Format24bppRgb:
                    return 24;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    return 32;
                case PixelFormat.Format48bppRgb:
                    return 48;
                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    return 64;
                case PixelFormat.Format4bppIndexed:
                    return 4;
                case PixelFormat.Format1bppIndexed:
                    return 1;
                
                default:
                    throw new ArgumentException("Pixelformat not supported", "pixelFormat");
            }
        }

        //private static byte[] GetBitmapBuffer(Bitmap bitmap, out int stride, Rectangle? rect = null)
        //{
        //    if (rect == null) rect = new Rectangle(0, 0, bitmap.Width,bitmap.Height);
        //    var bitmapData = bitmap.LockBits(rect.Value, ImageLockMode.ReadOnly, bitmap.PixelFormat);
        //    stride = bitmapData.Stride;
        //    var res = new byte[stride * bitmapData.Height];
        //    Marshal.Copy(bitmapData.Scan0, res, 0, res.Length);
        //    bitmap.UnlockBits(bitmapData);

        //    return res;
        //}

        /*
        private Coordinate[] Reproject(IList<Coordinate> coordList)
        {
            var ords = new double[coordList.Count*2];
            var i = 0;
            foreach (var c in coordList)
            {
                ords[i++] = c.X;
                ords[i++] = c.Y;
            }

            Projections.Reproject.ReprojectPoints(ords, null, _source, _target, 0, ords.Length);

            var res = new Coordinate[coordList.Count];
            i = 0;
            for (var j = 0; j <coordList.Count; j++)
               res[j] = new Coordinate(ords[i++], ords[i++]);

            return res;
        }
         */

        private class ColorAccess
        {
            public static ColorAccess Create(Bitmap bitmap, Rectangle? rect = null)
            {
                if (rect == null) rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bmData = bitmap.LockBits(rect.Value, ImageLockMode.ReadOnly, bitmap.PixelFormat);
                var res = new ColorAccess(bmData, bitmap.Palette);
                bitmap.UnlockBits(bmData);

                return res;
            }
            
            private static readonly byte[] BitMask;

            static ColorAccess()
            {
                BitMask = new[] { (byte)1, (byte)2, (byte)4, (byte)8, (byte)16, (byte)32, (byte)64, (byte)128 };
            }

            private readonly byte[] _buffer;
            private readonly int _stride;

            private readonly PixelFormat _format;
            private readonly int _bitsPerPixel;

            private readonly ColorPalette _palette;

            private ColorAccess(BitmapData data, ColorPalette palette = null)
            {
                _stride = data.Stride;

                _buffer = new byte[_stride * data.Height];
                Marshal.Copy(data.Scan0, _buffer, 0 , _buffer.Length);
                _format = data.PixelFormat;
                _bitsPerPixel = GetPixelSize(_format);
                _palette = palette;
            }

            private int GetIndex(int x, int y, out int mod)
            {
                var offsetRow = y* _stride;
                var offsetColBits = x*_bitsPerPixel;
                var offsetCol = offsetColBits/8;
                mod = offsetColBits - offsetCol*8;

                return offsetRow + offsetCol;
            }

            public Color this[int x, int y]
            {
                get
                {
                    int mod;
                    int pIndex;
                    var index = GetIndex(x, y, out mod);
                    switch (_format)
                    {
                        case PixelFormat.Format1bppIndexed:
                            return _palette.Entries[(_buffer[index] & BitMask[mod]) == 0 ? 0 : 1];

                        case PixelFormat.Format4bppIndexed:
                            pIndex = _buffer[index];
                            mod /= 4;
                            if (mod != 0) pIndex >>= 4;
                            return _palette.Entries[pIndex & 0x7];

                        case PixelFormat.Format8bppIndexed:
                            pIndex = _buffer[index];
                            return _palette.Entries[pIndex];

                        case PixelFormat.Format24bppRgb:
                            return Color.FromArgb(_buffer[index + 2], _buffer[index + 1], 
                                                  _buffer[index]);

                        case PixelFormat.Format32bppRgb:
                            return Color.FromArgb(_buffer[index + 3], _buffer[index + 2], 
                                                  _buffer[index + 1], _buffer[index]);
                    }
                    return Color.Transparent;

                }

                set
                {
                    if (_format != PixelFormat.Format32bppArgb)
                        throw new NotSupportedException();

                    int mod;
                    var index = GetIndex(x, y, out mod);
                    _buffer[index++] = value.B;
                    _buffer[index++] = value.G;
                    _buffer[index++] = value.R;
                    _buffer[index] = value.A;

                }
            }

            public byte[] Buffer { get { return _buffer; } }

        }
    }
}