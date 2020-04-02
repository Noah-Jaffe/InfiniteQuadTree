using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfiniteQuadTree.NumericalType;
/// <summary>
/// I want to query a numerical range from (x1,y1) to (x2,y2), 
/// and get at most back a single node point per pixle in range from (0,0) to (windowX, windowY)
/// meaning the values i should be getting live in a node with xy range no more than ((x2-x1)/windowX, (y2-y1)/windowY), call this OptimalLvl
/// i should find all points in that range and then find the OptimalLvl and use those datapoints for the final image. 
/// if the datapoint in the range has no OptimalLvl value, then use the value from the level above it
/// 
/// if writing to the node:
/// node lock = true
/// when checking if being written to: 
/// if node locked, or any of its parents are locked... 
/// </summary>    
namespace InfiniteQuadTree.QuadTree
{


    /// <summary>
    /// Quad tree
    /// </summary>
    /// <typeparam name="N">Numerical datatype</typeparam>
    /// <typeparam name="D">Data type</typeparam>

    public class QuadTree<N, D>
    {
        private QTNode<N, D> RootNode { get; set; }

        // todo: is height realistic to store?
        public int Height { get; private set; }

        public static int QTNodeCount { get;  set; }

        public QuadTree()
        {
            // TODO: Is this useless as they are the defaults?
            Height = 0;
            QTNodeCount = 0;
            RootNode = null;
        }
        ~QuadTree()
        {
            Debug.WriteLine($"QuadTree Destructor called");
        }
        /// <summary>
        /// Adds a point data range to the data structure.
        /// </summary>
        /// <param name="data">The data to add</param>
        /// <param name="pt">The botom left corner for the point to add</param>
        /// <param name="range">The optimal size of the box</param>
        /// <returns>True if a new point was added, false if it was a replacement or failure to add.</returns>
        public bool Add(QTPoint pt, N range, D data)
        {
            if (RootNode == null)
            {
                RootNode = new QTNode<N, D>();
                return RootNode.Add(pt, range, data);
            } 
            // If the point to add is in the current bounds, then add it.
            if (RootNode.Contains(pt)) // TODO: do i need to check if it overlaps as well or something?
            {
                return RootNode.Add(pt, range, data);
            } else
            {
                // The point is out of the current bounds, we need to move the root down a level.

                // Get the range and the point for the new root node
                QTPoint NewRootPt = new QTPoint();
                dynamic NewRootRange = 0;
                QTPoint.Capture(RootNode.Point, RootNode.Range, pt, range, ref NewRootPt, ref NewRootRange);

                // Add the current root to the new node
                QTNode<N, D> NewRoot = new QTNode<N, D>();
                if (!NewRoot.Add(pt, range, data)) 
                {
                    throw new Exception("How did we get here? I thought we were adding to an empty node?");
                }
                bool worked = NewRoot.Append(RootNode);
                // Update out root node
                // TODO: can optimize some lines out once we pass testing
                if (worked)
                {
                    RootNode = NewRoot;
                    return true;
                } else
                {
                    throw new Exception("Why did that not work?");
                }

            }
        }

        public bool Remove()
        {

            throw new NotImplementedException();
        }

        public bool Find(QTPoint pt)
        {
            return RootNode != null ? RootNode.Find(pt) : false;
        }

        /// <summary>
        /// Gets the data values in the given point range and puts them into the found object. 
        /// </summary>
        /// <param name="pt">The bottom left point to start the search from</param>
        /// <param name="range">the size of the range tosearch for</param>
        /// <param name="found">TODO: Some sort of collection to add the found data to.</param>
        /// <returns>True if any found, false otherwise.</returns>
        public bool Query<C>(QTPoint pt, N range, ref C found) where C : System.Collections.ICollection
        {
            return RootNode != null ? RootNode.Query(pt, range, ref found) : false;
        }

        public Image Draw(QTPoint pt, N range, Color[] DataColor)
        {
            Image img = new Bitmap((int)Math.Ceiling((dynamic)range), (int)Math.Ceiling((dynamic)range));
            Graphics drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(Color.Black);
            if (RootNode != null)
            {
                RootNode.Draw(pt, range, ref drawing, ref DataColor);
            }
            drawing.DrawImage(img,0,0);
            drawing.Save();
            drawing.Dispose();
            return img;
        }
        public override string ToString()
        {
            return $"Quad tree\nItem count: {QTNodeCount}\nNodes:\n{RootNode}";
        }
    }
}
