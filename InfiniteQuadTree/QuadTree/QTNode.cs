using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfiniteQuadTree.QuadTree
{

    /// <summary>
    /// Quad tree node
    /// </summary>
    /// <typeparam name="N">Numerical datatype</typeparam>
    /// <typeparam name="D">Data type</typeparam>

    class QTNode<N, D>
    {
        public QTPoint Point { get; internal set; }
        public N Range { get; internal set; }
        public D Data { get; internal set; }
        // TODO: is setting a halfrange for the childQuadrant calculations worth it?
        // public N HalfRange { get; internal set; }
        public QTNode<N, D>[] Children { get; internal set; }
        public QTNode()
        {
            //Children = new QTNode<N, D>[4];
            //Children[0] = this; // TODO: think this through? 
        }
        internal bool Add(QTPoint pt, N range, D data)
        {
            // this.HalfRange = (dynamic) range * 0.5;
            
            if (Point == null && Children == null) //TODO: is Children == null check neccisary?
            {
                // first time setting
                Point = pt;
                Range = range;
                Data = data;
                ++QuadTree<N,D>.QTNodeCount;
                return true;
            } 
            else if (Point.Equals(pt)) // TODO: anything about range?
            {                            // TODO: what about if this isnt a leaf and the point matches?
                Data = data;
                return false;
            }
            else // a leaf node or a parent node
            {
                int quad = GetQuadrant(pt);
                if (Children == null)
                {
                    Children = new QTNode<N, D>[4];
                }
                if (Children[quad] == null)
                {
                    Children[quad] = new QTNode<N, D>();
                }
                return Children[quad].Add(pt, range, data);
            }
        }
        internal bool Append(QTNode<N, D> node)
        {
            // find which quadrant to add the new node to, add it in
            int quad = GetQuadrant(node.Point);
            if (Children != null)
            {
                throw new Exception("Are you sure you know what youre doing?");
            } else if (Children == null)
            {
                Children = new QTNode<N, D>[4];
                Children[quad] = node;
                ++QuadTree<N, D>.QTNodeCount;
                return true;
            }
            return false;
        }
        internal bool Query<C>(QTPoint pt, N range, ref C found) where C : System.Collections.ICollection
        {
            dynamic obj = found;
            if (this.Overlaps(pt, range)) // is this working?
            {
                obj.Add(Data);
                if (Children != null)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        if (Children[i] != null)
                        {
                            Children[i].Query(pt, range, ref found);
                        }
                    }
                }
                return true;
            }
            return false;
        }

        internal bool Find(QTPoint pt)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// ╔═════════╦══════════╗
        /// ║    3    ║     2    ║
        /// ║ TopLeft ║ TopRight ║
        /// ╠═════════╬══════════╣
        /// ║    0    ║     1    ║
        /// ║ BotLeft ║ BotRight ║
        /// ╚═════════╩══════════╝
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        internal int GetQuadrant(QTPoint pt)
        {
            // TODO: is / 2.0 or * 0.5 faster? Inital testing says they are about the same. 
            return (pt.X - Point.X > (dynamic)Range * 0.5 ? 1 : 0)  // left or right
                + (pt.Y - Point.Y > (dynamic)Range * 0.5 ? 2 : 0);  // top or bottom
        }
        /// <summary> <returns>
        /// True if the given point is inside the current node bounds, otherwise false.
        /// </returns> </summary>
        /// <param name="pt"></param>
        internal bool Contains(QTPoint pt)
        {
            return (dynamic)pt.X >= Point.X &&
                    (dynamic)pt.X <= (dynamic)Point.X + Range &&
                    (dynamic)pt.Y >= Point.Y &&
                    (dynamic)pt.Y <= (dynamic)Point.Y + Range;
        }
        /// <summary>
        /// Tests if the given point range bounds overlap this current node bounds.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="otherRange"></param>
        /// <returns>True if they overlap, otherwise false.</returns>
        internal bool Overlaps(QTPoint pt, N otherRange)
        {
            // is min.rightmost > max.leftmost?
            bool XOverlap = (dynamic)Point.X <= pt.X ?
                                (dynamic)Point.X + Range > pt.X :
                                (dynamic)pt.X + otherRange > Point.X;
            // is min.topmost > max.botommost?
            bool YOverlap = (dynamic)Point.Y <= pt.Y ?
                                (dynamic)Point.Y + Range > pt.Y :
                                (dynamic)pt.Y + otherRange > Point.Y;
            // if both are true, then the two squares overlap
            return XOverlap && YOverlap;
        }

        internal void Draw(QTPoint pt, N range, ref Graphics drawing, ref Color[] dataColor)
        {
            if (pt != null && Data != null)
            {
                // x y is upper left... 
                drawing.FillRectangle(new System.Drawing.SolidBrush(dataColor[(dynamic)Data]), (float)Point.X, (float)((dynamic)Point.Y), (float)(dynamic)Range, (float)(dynamic)Range);
            }
            if (Children != null)
            {
                for (int i = 0; i < Children.Length; ++i)
                {
                    if (Children[i] != null)
                    {
                        Children[i].Draw(pt, range, ref drawing, ref dataColor);
                    }
                }
            }
        }

        public override string ToString()
        {
            string ret = "";
            ret += $"Node: {this.GetHashCode()}\tPoint: {Point}\tRange: {Range}\tData: {Data}\tChildren: ";
            if (Children == null)
            {
                ret += "null;\n";
            }
            else
            {
                for (int i = 0; i < 4; ++i)
                {
                    ret += $"{(Children[i] == null ? "null" : Children[i].GetHashCode().ToString())}{(i == 3 ? ";\n" : ",")}";
                }
                for (int i = 0; i < 4; ++i)
                {
                    ret += Children[i] != null ? Children[i].ToString() : "";
                }
            }
            return ret;
        }
    }
}
