using System;
using System.Collections.Generic;
using Autodesk.DesignScript.Runtime; // needed to add flags like [IsVisibleDynamoLibrary(false)]
using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace NodeModelsEssentials.Functions
{
    [IsVisibleInDynamoLibrary(false)]

    public class NodeModelsEssentialsFunctions
    {
        public static double Multiply(double a, double b)
        {
            return a * b;
        }

        public static double Add(double a, double b)
        {
            return a + b;
        }

        public static double Subtract(double a, double b)
        {
            return a - b;
        }

        public static double Divide(double a, double b)
        {
            return a / b;
        }

        public static double Sum(double a, double b, double c, double d)
        {
            return a + b + c + d;
        }

        /// <summary>
        /// Appends a the provided text to a file.
        /// </summary>
        /// <param name="FilePath">The path of the file to write to.</param>
        /// <param name="Text">The string to write.</param>
        public static void WriteLineToFilePath(string FilePath, string Text)
        {
            using (StreamWriter outputFile = new StreamWriter(FilePath, true))
            {
                outputFile.WriteLine(Text);
                outputFile.Close();
            }
        }

        /// <summary>
        /// Replaces the contents of a file.
        /// </summary>
        /// <param name="FilePath">The path of the file to write to.</param>
        /// <param name="Text">The string to write.</param>
        public static void WriteToFilePath(string FilePath, string Text)
        {
            using (StreamWriter outputFile = new StreamWriter(FilePath, true))
            {
                outputFile.Write(Text);
                outputFile.Close();
            }
        }

        public static string ConcatenateThree(string a, string b, string c)
        {
            return string.Join(", ", new List<string>{ a, b, c });
        }

        public static string Error(string text)
        {
            if (text == "ERROR1")
            {
                throw new ArgumentException("ERROR1 raised ArgumentException.");
            } else if (text == "ERROR2")
            {
                throw new Exception("A sample Exception by ERROR2.");
            }
            return "Error response";
        }

        public static string Timeout(int sleepDuration = 1000, int timeoutDuration = 3000)
        {
            var taskWasCompleted = false; // wether the C# Task was completed before timeout or not
            Thread taskThread = null; // reference the thread in which we call Edge.js to abort in case of timeout

            try
            {
                taskWasCompleted = Task.Run(() =>
                {
                    taskThread = Thread.CurrentThread;
                    Thread.Sleep(sleepDuration);
                    //var client = GetClientConnection(Project);
                    //var propertyWatch = client.GetProperyWatch(Name);
                    //propertyValue = propertyWatch.Value;
                }).Wait(timeoutDuration);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
                // TODO: throw HFDM.js.NET errors (accessible in JSON as e.InnerException.InnerException.Message);
            }

            if (!taskWasCompleted && taskThread != null)
            {
                // Abort taskThread and throw exception
                taskThread.Abort();
                throw new Exception("The request timed out (" + timeoutDuration + "ms).");
            }

            return "Timeout message - " + sleepDuration + " - " + timeoutDuration ;
        }

        public static string GetDate()
        {
            return DateTime.Now.ToString("yyMMdd H:mm:ss");
        }
       
        public static object GetItem(List<object> list, int index)
        {
            return 2;
//            if(list.Count <= index)
  //              return list[index];
    //        return null;
        }

        /// <summary>
        /// A simple node to create a surface from four points.
        /// </summary>
        public static Surface SurfaceFrom4Points(Point point1, Point point2, Point point3, Point point4)
        {
            Line line1 = Line.ByStartPointEndPoint(point1, point2);
            Line line2 = Line.ByStartPointEndPoint(point3, point4);

            List<Curve> curves = new List<Curve> { line1, line2 };

            Surface surf = Surface.ByLoft(curves);

            // Remember to DISPOSE geometry elements created in the node
            // but not returned by it!
            line1.Dispose();
            line2.Dispose();

            return surf;
        }

        /// <summary>
        /// Given a surface, returns a grid of UV tangent planes.
        /// </summary>
        public static List<List<Plane>> UVPlanesOnSurface(Surface surface, int uCount, int vCount)
        {
            List<List<Plane>> planes = new List<List<Plane>>();

            double du = 1.0 / (uCount - 1);
            double dv = 1.0 / (vCount - 1);

            Point point = null;
            Vector normal = null;
            Vector tangent = null;
            for (double v = 0; v <= 1.0; v += dv)
            {
                List<Plane> pls = new List<Plane>();
                for (double u = 0; u <= 1; u += du)
                {
                    point = surface.PointAtParameter(u, v);
                    normal = surface.NormalAtParameter(u, v);
                    tangent = surface.TangentAtUParameter(u, v);
                    Plane p = Plane.ByOriginNormalXAxis(point, normal, tangent);
                    pls.Add(p);
                }
                planes.Add(pls);
            }

            // Remember to dispose unreturned geometry objects
            point.Dispose();
            normal.Dispose();
            tangent.Dispose();

            return planes;
        }

        /// <summary>
        /// Given some size parameters, returns a lofted surface with random bumps.
        /// </summary>
        public static Surface WobblySurface(
            double baseX, double baseY, double baseZ,
            double width, double length, double maxHeight,
            int uCount, int vCount)
        {
            double dx = width / (uCount - 1);
            double dy = length / (vCount - 1);

            Random rnd = new Random();
            List<Curve> curves = new List<Curve>();
            List<Point> pts = null;
            for (double y = baseY; y <= baseY + length; y += dy)
            {
                pts = new List<Point>();
                for (double x = baseX; x <= baseX + width; x += dx)
                {
                    pts.Add(Point.ByCoordinates(x, y, baseZ + maxHeight * rnd.NextDouble()));
                }
                curves.Add(NurbsCurve.ByPoints(pts));
            }

            Surface surface = Surface.ByLoft(curves);

            return surface;
        }
    }

    [IsVisibleInDynamoLibrary(false)]
    public class MyMesh : IGraphicItem
    {
        #region private members

        private static double counter;
        private Point point = Point.ByCoordinates(0, 2, 0);

        #endregion

        #region properties

        public Point Point { get { return point; } }

        #endregion


        private MyMesh(double x, double y, double z)
        {
            point = Point.ByCoordinates(x, y, z);
        }

        public static MyMesh Create(double x = 0, double y = 0, double z = 0)
        {
            counter++;
            return new MyMesh(x, y, z);
        }

        //public static MyMesh Create([DefaultArgumentAttribute("Point.ByCoordinates(0,0,0);")]Point point)
        //{
        //    return new MyMesh(point.X, point.Y, point.Z);
        //}

        #region IGraphicItem interface


        /// <summary>
        /// The Tessellate method in the IGraphicItem interface allows
        /// you to specify what is drawn when dynamo's visualization is
        /// updated.
        /// </summary>
        [IsVisibleInDynamoLibrary(false)]
        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            // This example contains information to draw a point
            package.AddPointVertex(point.X, point.Y, point.Z);
            package.AddPointVertexColor(255, 0, 0, 255);
            package.AddPointVertex(0, 1, 1);
            package.AddPointVertexColor(255, 0, 0, 255);
            package.AddPointVertex(0, 2, 2 + counter / 10.0);
            package.AddPointVertexColor(255, 0, 0, 255);
            package.AddPointVertex(0, 3, 3 + counter / 10.0);
            package.AddPointVertexColor(255, 0, 0, 255);
            package.AddPointVertex(0, 4, 4 + counter / 10.0);
            package.AddPointVertexColor(255, 0, 0, 255);
        }

        #endregion

        public override string ToString()
        {
            return string.Format("HelloDynamoZeroTouch:{0},{1},{2}", point.X, point.Y, point.Z);
        }

    }
}
