using Autodesk.DesignScript.Runtime; // needed to add flags like [IsVisibleDynamoLibrary(false)]
using Autodesk.DesignScript.Geometry;

namespace NodeModelsEssentials.Functions
{
    [IsVisibleInDynamoLibrary(false)]

    public class NodeModelsEssentialsFunctions
    {
        public static double Multiply(double a, double b)
        {
            return a * b;
        }

        public static Point PointByCoordinates(double x = 0, double y = 0, double z = 0)
        {
            return Point.ByCoordinates(x, y, z);
        }
    }
}
