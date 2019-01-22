using System;

namespace CameraTransform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Camera cam = new Camera();
            Vec3d[] arch = new Vec3d[4];

            arch[0].x = 0d;
            arch[0].y = 0d;
            arch[0].z = 0d;

            arch[1].x = -15d;
            arch[1].y = 0d;
            arch[1].z = 15d;

            arch[2].x = 1.7d;
            arch[2].y = 4d;
            arch[2].z = 1.7d;

            arch[3].x = 1.7d;
            arch[3].y = 1.7d;
            arch[3].z = 4f;

            double[] res = cam.GetRotationsFromCoords(arch);
            double[][] tMatrix = cam.GetTransformMatrix(new double[] { 180.0, 90.0, 0 }, new Vec3d() { x = 1.5, y = 1.0, z = 1.5 });

            foreach(double d in res)
            {
                Console.WriteLine(d);
            }
        }
    }
}
