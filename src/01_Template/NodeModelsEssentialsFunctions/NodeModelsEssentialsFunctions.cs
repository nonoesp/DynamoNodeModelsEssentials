using Autodesk.DesignScript.Runtime; // needed to add flags like [IsVisibleDynamoLibrary(false)]

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
    }
}
