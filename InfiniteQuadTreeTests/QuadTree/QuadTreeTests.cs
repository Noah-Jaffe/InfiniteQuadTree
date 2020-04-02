using Microsoft.VisualStudio.TestTools.UnitTesting;
using InfiniteQuadTree.QuadTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfiniteQuadTree.NumericalType;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
namespace InfiniteQuadTree.QuadTree.Tests
{
    [TestClass()]
    public class QuadTreeTests
    {
        [TestMethod()]
        public void NumericsTest()
        {
            /*
            // int, double, decimal, float, long, short, uint, ulong, ushort
            object[] NumericZeroes= new object[]{(int)0, (double)0, (decimal)0M, (float)0.0f,
                (long)0, (short)0, (uint)0, (ulong)0, (ushort)0};
            Numeric n = 0.0;
            for (int i = 0; i < NumericZeroes.Length; ++i)
            {
                Assert.AreNotEqual(n, NumericZeroes[i],$"Failure @ i = {i}: type = {NumericZeroes[i].GetType()}. Numerical types are not supposed to be true for AreEqual assertions.");
                Assert.IsTrue(n.Equals((Numeric)NumericZeroes[i]),$"Failure @ i = {i}: type = {NumericZeroes[i].GetType()}. Numerical types are supposed to be true for .Equal operations.");
            }
            */
            Stopwatch stopwatch = Stopwatch.StartNew(); //creates and start the instance of Stopwatch
                                                        //your sample code

            /*
             *  d[i] = i / 2.0;
             * 4ms	    N=1140662	    285165.5cps
             * 4ms	    N=1273293	    318323.25cps
             * 19ms	    N=5440132	    286322.736842105cps
             * 34ms	    N=9761431	    287100.911764706cps
             * 50ms	    N=14113933	    282278.66cps
             * 196ms	N=55046494	    280849.459183673cps
             * 145ms	N=40606563	    280045.262068966cps
             * 84ms	    N=23234611	    276602.511904762cps
             * 22ms		N=6329055		287684.318181818cps
             * 338ms	N=94155294		278565.958579882cps
             * 335ms	N=94155294		281060.579104478cps
             * 282,278 avg over 9
             * 
             * 235ms		N=54353749		231292.54893617cps
             * 268ms		N=61502125		229485.541044776cps
             * 97ms		    N=22431079		231248.237113402cps
             * 230,675‬ for 3
             */
            /* 
             * 22ms		N=6329055		287684.318181818cps
             * 62ms		N=17429783		281125.532258065cps
             * 119ms	N=33530620		281769.915966387cps
             * 171ms	N=47901693		280126.859649123cps
             * 142ms	N=40262278		283537.169014085cps
             * 338ms	N=94155294		278565.958579882cps
             * 339ms	N=94155294		277744.230088496cps
             * 281,507 over 7
             * 
             * 249ms		N=56362515		226355.481927711cps
             * 386ms		N=87161320		225806.528497409cps
             * 75ms		N=17001039		226680.52cps
             * 226,280 for 3
             */
            /*d[i] = (d[i-1] + ++idx) * 0.5;
             * 502ms		N=79356065		158079.810756972cps
             * 81ms		N=12995420		160437.283950617cps
             * 89ms		N=14308466		160769.280898876cps
             * 
             * d[i] = (d[i-1] + ++idx) / 2.0;
             * 370ms		N=44142085		119302.932432432cps
             * 552ms		N=66299825		120108.378623188cps
             * 720ms		N=86171583		119682.754166667cps
             */
            Random r = new Random();
            double[] d = new double[new Random().Next(1000000, 100000000)];
            double idx = 10;
            idx = idx++ / 2;
            idx = 10;
            idx = ++idx / 2; 
            idx = 10;
            for (int i = 1; i < d.Length; ++i)
            {
                d[i] = (d[i-1] + ++idx) / 2.0;
            }
            stopwatch.Stop();
            double a = double.MinValue;
            for (int i = 0; i < d.Length; ++i)
            {
                a += d[i];
            }
            Trace.WriteLine($"{stopwatch.ElapsedMilliseconds}ms\t\tN={d.Length}\t\t{d.Length / (double)stopwatch.ElapsedMilliseconds}cps");
            string s = "";
            Trace.WriteLine(s);
        }
        [TestMethod()]
        public void QuadTreeTest()
        {
            QuadTree<double, int> quadTree = new QuadTree<double, int>();
            Assert.IsNotNull(quadTree);
            Assert.AreEqual(0, QuadTree<double, int>.QTNodeCount);
        }

        [TestMethod()]
        public void AddTest()
        {
            QuadTree<double, int> quadTree = new QuadTree<double, int>();
            Assert.IsNotNull(quadTree);
            Assert.AreEqual(0, QuadTree<double, int>.QTNodeCount);
            double range = 5;
            //Trace.WriteLine(quadTree.ToString());
            quadTree.Add(new QTPoint { X = 0.0, Y = 0.0 }, range, 0);
            range *= 0.5;
            //Trace.WriteLine(quadTree.ToString());
            quadTree.Add(new QTPoint { X = 1.0, Y = 1.0 }, range, 1);
            quadTree.Add(new QTPoint { X = 3.0, Y = 1.0 }, range, 2);
            quadTree.Add(new QTPoint { X = 3.0, Y = 3.0 }, range, 3);
            quadTree.Add(new QTPoint { X = 1.0, Y = 3.0 }, range, 4);
            Trace.WriteLine(quadTree.ToString());
            quadTree.Add(new QTPoint { X = 0, Y = 0 }, range, -1);
            Trace.WriteLine(quadTree.ToString());
        }

        [TestMethod()]
        public void AddTest1()
        {
            // Lets break it c:

            QuadTree<double, int> quadTree = new QuadTree<double, int>();
            Assert.IsNotNull(quadTree);
            Assert.AreEqual(0, QuadTree<double, int>.QTNodeCount);
            double range = 5;
            int index = 0;
            int[] dataToAdd = new int[] { 0, 1, 2, 4, 3, 5, 6, 7, 8, 9, 10 };
            quadTree.Add(new QTPoint { X = 0.0, Y = 0.0 }, range, dataToAdd[index++]);
            // Lets not make the range smaller and see what happens... range *= 0.5;
            quadTree.Add(new QTPoint { X = 1.0, Y = 1.0 }, range, dataToAdd[index++]);
            quadTree.Add(new QTPoint { X = 3.0, Y = 1.0 }, range, dataToAdd[index++]);
            quadTree.Add(new QTPoint { X = 3.0, Y = 3.0 }, range, dataToAdd[index++]);
            quadTree.Add(new QTPoint { X = 1.0, Y = 3.0 }, range, dataToAdd[index++]);

            quadTree.Add(new QTPoint { X = 0.5, Y = 3.5 }, range, dataToAdd[index++]);
            quadTree.Add(new QTPoint { X = 0.5, Y = 3.5 }, range, dataToAdd[index++]);
            quadTree.Add(new QTPoint { X = 0.000001, Y = 0.000001}, range, dataToAdd[index++]);
            quadTree.Add(new QTPoint { X = 0.0000001, Y = 0.0000001 }, range, dataToAdd[index++]);




            Trace.WriteLine(quadTree.ToString());
        }

        [TestMethod()]
        public void AddTest2()
        {
            // Lets break it c:

            QuadTree<double, int> quadTree = new QuadTree<double, int>();
            Assert.IsNotNull(quadTree);
            Assert.AreEqual(0, QuadTree<double, int>.QTNodeCount);
            double range = 128;
            int index = 0;
            double x = -500, y = -500;
            quadTree.Add(new QTPoint { X = 0, Y = 0 }, range, index++);
            range *= 0.5;
            quadTree.Add(new QTPoint { X = x, Y = y }, range, index++); // (260, 260)
            x += range; y += range;
            quadTree.Add(new QTPoint { X = x, Y = y }, range, index++);
            range *= 0.5;
            quadTree.Add(new QTPoint { X = x, Y = y }, range, index++);
            x += range; y += range;
            quadTree.Add(new QTPoint { X = x, Y = y }, range, index++);

            x = 0; y = 0;
            quadTree.Add(new QTPoint { X = x, Y = y }, range, index++);
            range *= 0.5;
            x = 0; y = 0;
            quadTree.Add(new QTPoint { X = x, Y = y }, range, index++);

            Trace.WriteLine(quadTree.ToString());
        }

        [TestMethod()]
        public void RemoveTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void FindTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void QueryTest()
        {
            QuadTree<double, int> quadTree = new QuadTree<double, int>();
            Assert.IsNotNull(quadTree);
            Assert.AreEqual(0, QuadTree<double, int>.QTNodeCount);
            double range = 5;
            int index = 0;
            int[] dataToAdd = new int[] { 0, 1, 2, 4, 3, 5 };
            quadTree.Add(new QTPoint { X = 0.0, Y = 0.0 }, range, dataToAdd[index++]);
            range *= 0.5;
            quadTree.Add(new QTPoint { X = 1.0, Y = 1.0 }, range, dataToAdd[index++]);
            quadTree.Add(new QTPoint { X = 3.0, Y = 1.0 }, range, dataToAdd[index++]);
            quadTree.Add(new QTPoint { X = 3.0, Y = 3.0 }, range, dataToAdd[index++]);
            quadTree.Add(new QTPoint { X = 1.0, Y = 3.0 }, range, dataToAdd[index++]);
            range *= 0.5;
            quadTree.Add(new QTPoint { X = 0.5, Y = 3.5 }, range, dataToAdd[index++]);

            Trace.WriteLine(quadTree.ToString());
            List<int> data = new List<int>();
            quadTree.Query(new QTPoint { X = 0, Y = 0 }, 2, ref data);
            for (int i = 0; i < 2; ++i) {
                Assert.AreEqual(dataToAdd[i], data.IndexOf(i));
            }
            quadTree.Add(new QTPoint { X = 0, Y = 0 }, range, -1);

            data = new List<int>();
            dataToAdd = new int[]{ -1, 1, 2, 4, 3, 5 };
            quadTree.Query(new QTPoint { X = 0, Y = 0 }, 5, ref data);
            for (int i = 0; i < 2; ++i)
            {
                Assert.AreEqual(dataToAdd[i], data.IndexOf(i));
            }


        }

        [TestMethod()]
        public void DrawTest()
        {
            double d = 1.0;
            ulong count = 0;
            while (d > 0)
            {
                d /= 10.0;
                ++count;
            }
            Trace.WriteLine($"{count} iterations");
            QuadTree<double, int> quadTree = new QuadTree<double, int>();
            Assert.IsNotNull(quadTree);
            Assert.AreEqual(0, QuadTree<double, int>.QTNodeCount);
            double range = 4096;
            int index = 0;
            
            while (range > 0.1)
            {
                quadTree.Add(new QTPoint { X = index, Y = index }, range, index++);
                range *= 0.5;
            }

            Trace.WriteLine(quadTree.ToString());
            
            //Color[] colors = ColorPaletteGenerator.SelectInterval(ColorPaletteGenerator.GenerateIterationColors(), QuadTree<double, int>.QTNodeCount + 1);
            Color[] colors = ColorPaletteGenerator.GetBWFlipped(QuadTree<double, int>.QTNodeCount + 1);

            foreach (Color c in colors)
            {
                Trace.WriteLine(c);

            }
            Image p = quadTree.Draw(new QTPoint { X = 0.0, Y = 0.0 }, 4096, colors);
            p.Save("C:\\Users\\PC\\Pictures\\yeet.png", ImageFormat.Bmp);


        }

    }
    public static class ColorPaletteGenerator
    {

        /// <summary>
        /// Generates a list of RGB values in rainbow order ending in black.
        /// </summary>
        /// <param name="maxR">The maximum red component. Valid values are 0 through 255. Must be greater or equal to minR.</param>
        /// <param name="maxG">The maximum green component. Valid values are 0 through 255. Must be greater or equal to minG.</param>
        /// <param name="maxB">The maximum blue component. Valid values are 0 through 255. Must be greater or equal to minB.</param>
        /// <param name="minR">The minimum red component. Valid values are 0 through 255. Must be less than or equal to maxR.</param>
        /// <param name="minG">The minimum green component. Valid values are 0 through 255. Must be less than or equal to maxG.</param>
        /// <param name="minB">The minimum blue component. Valid values are 0 through 255. Must be less than or equal to maxB.</param>
        /// <returns>An array of <c>Color</c>'s in rainbow order.</returns>
        public static Color[] GenerateIterationColors(Int32 maxR = 255, Int32 maxG = 255, Int32 maxB = 255, Int32 minR = 0, Int32 minG = 0, Int32 minB = 0)
        {
            Debug.Print($"Generating colors R <{minR},{maxR}> G<{minG},{maxG}> B<{minB},{maxB}>");
            DateTime startTime = DateTime.Now;
            startTime = DateTime.Now;
            // [rainbow_id,rgb_index]
            Color[] colors = new Color[(maxR + maxG + maxB - minR - minG - minB) * 2];
            int index = 0;
            // r,0,b^
            for (int i = minB; i < maxB; ++i, ++index)
            {
                colors[index] = Color.FromArgb(maxR, minG, i);
            }

            // r_,0,b
            for (int i = maxR; i > minR; --i, ++index)
            {
                colors[index] = Color.FromArgb(i, minG, maxB);
            }

            // 0,g^,b
            for (int i = minG; i < maxG; ++i, ++index)
            {
                colors[index] = Color.FromArgb(minR, i, maxB);
            }

            // 0,g,b_
            for (int i = maxB; i > minB; --i, ++index)
            {
                colors[index] = Color.FromArgb(minR, maxG, i);
            }

            // r^,g,0
            for (int i = minR; i < maxR; ++i, ++index)
            {
                colors[index] = Color.FromArgb(i, maxG, minB);
            }

            // r,g/2 _,0
            int x = maxG;
            while (x > (double)(1 + maxG - minG) / 2.0)
            {
                colors[index] = Color.FromArgb(maxR, x, minB);
                --x;
                ++index;
            }

            // down to black
            int expectedLen = colors.Length;
            int remainingVals = expectedLen - minB - index;
            if (remainingVals == 0)
            {
                return colors;
            }
            double rateR = Math.Abs((double)maxR / (double)remainingVals);
            double rateG = Math.Abs((double)(maxG - minG) / (double)(2.0 * remainingVals));

            for (int i = 1; i <= remainingVals; ++i, ++index)
            {
                colors[index] = Color.FromArgb((int)(maxR - (i * rateR)), (int)(x - (i * rateG)), minB);
            }
            return colors;
        }

        public static Color[] GetBWFlipped(int num)
        {
            Color[] intervals = new Color[num];
            for (int i = 0; i < num; ++i)
            {
                intervals[i] = (i % 2 == 0 ? Color.Black : Color.White);
            }
            return intervals;
        }
        public static Color[] SelectInterval(Color[] colors, int finalLen)
        {
            Color[] intervals = new Color[finalLen];
            int scale = colors.Length / finalLen; 
            for (int i = 0; i < finalLen; ++i)
            {
                intervals[i] = colors[scale * i];
            }
            return intervals;
        }

    }
}