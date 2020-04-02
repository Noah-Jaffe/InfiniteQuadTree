using System;

namespace InfiniteQuadTree.QuadTree
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class QTPoint
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public dynamic X { get; set; }
        public dynamic Y { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
            {
                return false;
            } 
            return this.X.Equals(((QTPoint)obj).X) && this.Y.Equals(((QTPoint)obj).Y);
        }
        /// <summary>
        /// Returns a point and range that captures all areas contained by both points and ranges
        /// </summary>
        /// <typeparam name="N"></typeparam>
        /// <param name="point1"></param>
        /// <param name="range1"></param>
        /// <param name="point2"></param>
        /// <param name="range2"></param>
        /// <param name="ExistingPointToChange"></param>
        /// <param name="ExistingRangeToChange"></param>
        public static void Capture(QTPoint point1, dynamic range1, QTPoint point2, dynamic range2, ref QTPoint ExistingPointToChange, ref dynamic ExistingRangeToChange)
        {
            // Get the minimum X and Y's 
            ExistingPointToChange.X = point1.X < point2.X ? point1.X : point2.X;
            ExistingPointToChange.Y = point1.Y < point2.Y ? point1.Y : point2.Y;
            // Get the maximum X and Y's
            dynamic x = (point1.X + range1 > point2.X + range2 ? point1.X + range1 : point2.X + range2) - ExistingPointToChange.X;
            dynamic y = (point1.Y + range1 > point2.Y + range2 ? point1.Y + range1 : point2.Y + range2) - ExistingPointToChange.Y;
            ExistingRangeToChange = x;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}