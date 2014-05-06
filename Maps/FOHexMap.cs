﻿namespace FOCommon.Maps
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;

    public class FOHexMap
    {
        // Base offsets.
        private PointF baseOffset = new PointF(3000, 100);
        private Size mapSize;

        // Tile offsets from scenery/protos.
        private readonly float tileOffX = -64;
        private readonly float tileOffY = -16;

        // Hex sizes
        public readonly int hexW = 32;
        public readonly int hexH = 16;
        public readonly int hexHEdge = 12; // height of left/right edge.

        public FOHexMap( ushort width, ushort height )
            : this( new Size( width, height ) )
        { }

        public FOHexMap(Size mapSize) 
        {
            this.mapSize = mapSize;
        }

        public FOHexMap(PointF baseOffset, Size mapSize)
        {
            this.baseOffset = baseOffset;
            this.mapSize = mapSize;
        }

        /// <summary>
        /// Common algoritm used for both tile placement and other objects.
        /// </summary>
        /// <returns></returns>
        private PointF GetCoords(Point hex)
        {
            float x = ((hex.Y * hexH) - (hex.X * hexW));
            float y = (Math.Abs((hex.Y + (hex.X / 2))) * hexHEdge);

            if (hex.X > 1) x += (hex.X / 2) * hexH;

            return new PointF(baseOffset.X + x, baseOffset.Y + y);
        }

        public Point GetHex(PointF coords)
        {
            for(int x = 0; x<mapSize.Width; x++)
                for(int y=0; y<mapSize.Height; y++)
                {
                    Point hex = new Point(x,y);
                    var calc = GetCoords(hex);

                    float deltx = (coords.X - calc.X);
                    float delty = (coords.Y - calc.Y);

                    if ((deltx < 33 && deltx > -1) &&
                        delty < 17 && delty > -1)
                        return hex;
                }
            return new Point(-1, -1);
        }

        /// <summary>
        /// Gets coordinates for scenery and other things that uses protoitems.
        /// </summary>
        /// <returns></returns>
        public PointF GetObjectCoords(Point hex, Size obj, Point Shift, Point Proto)
        {
            var coords = this.GetCoords(hex);

            coords.X -= obj.Width / 2;
            coords.X -= Proto.X;
            coords.X += Shift.X;
            coords.Y -= obj.Height;
            coords.Y -= Proto.Y;
            coords.Y += Shift.Y;

            return coords;
        }

        /// <summary>
        /// Gets coordinates for tiles, including roofs.
        /// </summary>
        /// <returns></returns>
        public PointF GetTileCoords(Point hex, bool isRoof)
        {
            var coords = this.GetCoords(hex);

            if (isRoof)
               coords.Y -= 92;

            return new PointF(tileOffX + coords.X, tileOffY + coords.Y);
        }
    }
}
