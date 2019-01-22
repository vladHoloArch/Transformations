using System;

namespace CameraTransform
{
    public class Camera
    {
        private double[][] matrixProduct(double[][] matrixA, double[][] matrixB)
        {
            int aRows = matrixA.Length; int aCols = matrixA[0].Length;
            int bRows = matrixB.Length; int bCols = matrixB[0].Length;
            if (aCols != bRows)
                throw new Exception("Non-conformable matrices in MatrixProduct");

            double[][] result = matrixCreate(aRows, bCols);

            for (int i = 0; i < aRows; ++i) // each row of A
                for (int j = 0; j < bCols; ++j) // each col of B
                    for (int k = 0; k < aCols; ++k) // could use k less-than bRows
                        result[i][j] += matrixA[i][k] * matrixB[k][j];

            return result;
        }

        private double[][] matrixCreate(int rows, int cols)
        {
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            return result;
        }

        private double[][] translationMatrix(Vec3d t)
        {
            return new double[4][]
               {
                    new double[] { 1, 0, 0, t.x },
                    new double[] { 0, 1, 0, t.y },
                    new double[] { 0, 0, 1, t.z },
                    new double[] { 0, 0, 0, 1   }
               };
        }

        private double[][] rotateX(double theta)
        {
            return new double[4][]
                {
                    new double[] { 1, 0, 0, 0 },
                    new double[] { 0, Math.Cos(theta), -Math.Sin(theta), 0 },
                    new double[] { 0, Math.Sin(theta),  Math.Cos(theta), 0 },
                    new double[] { 0, 0, 0, 1 }
                };
        }

        private double[][] rotateY(double theta)
        {
            double[][] res = new double[4][]
                {
                    new double[] { Math.Cos(theta), 0, Math.Sin(theta), 0 },
                    new double[] { 0, 1, 0, 0 },
                    new double[] {-Math.Sin(theta), 0, Math.Cos(theta), 0 },
                    new double[] { 0, 0, 0, 1 }
                };

            return res;
        }

        private double[][] rotateZ(double theta)
        {
            return new double[4][]
                {
                    new double[] { Math.Cos(theta), -Math.Sin(theta), 0, 0 },
                    new double[] { Math.Sin(theta),  Math.Cos(theta), 0, 0 },
                    new double[] { 0, 0, 1, 0 },
                    new double[] { 0, 0, 0, 1 }
                };
        }

        private double angleBetweenAxis(Vec3d a, Vec3d b)
        {
            double res = Math.Acos(
                    a.dot(b)
                    /
                    (a.length() * b.length())
                );

            return res * 180.0 / Math.PI;
        }

        public double[] GetRotationsFromCoords(Vec3d[] newCoords)
        {
            double[] res = new double[3];

            res[0] = angleBetweenAxis(newCoords[1] - newCoords[0], new Vec3d() { x = 1, y = 0, z = 0 });
            res[1] = angleBetweenAxis(newCoords[2] - newCoords[0], new Vec3d() { x = 0, y = 1, z = 0 });
            res[2] = angleBetweenAxis(newCoords[3] - newCoords[0], new Vec3d() { x = 0, y = 0, z = 1 });

            return res;
        }

        public double[] GetRotationsFromNormals(Vec3d[] normals)
        {
            double[] res = new double[3];

            res[0] = angleBetweenAxis(normals[0], new Vec3d() { x = 1, y = 0, z = 0 });
            res[1] = angleBetweenAxis(normals[1], new Vec3d() { x = 0, y = 1, z = 0 });
            res[2] = angleBetweenAxis(normals[2], new Vec3d() { x = 0, y = 0, z = 1 });

            return res;
        }

        public double[][] GetTransformMatrix(double[] rotation, Vec3d pos)
        {
            double[][] res = matrixCreate(4, 4);
            double radX = rotation[0] * Math.PI / 180.0;
            double radY = rotation[1] * Math.PI / 180.0;
            double radZ = rotation[2] * Math.PI / 180.0;

            return matrixProduct(matrixProduct(matrixProduct(translationMatrix(pos), rotateX(radX)), rotateY(radY)), rotateZ(radZ));
        }
    }
    public struct Vec3d
    {
        public double x;
        public double y;
        public double z;

        public double dot(Vec3d b)
        {
            double res = x * b.x + y * b.y + z * b.z;

            return res;
        }

        public double length()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public static Vec3d operator -(Vec3d a, Vec3d b)
        {
            Vec3d res = new Vec3d();

            res.x = a.x - b.x;
            res.y = a.y - b.y;
            res.z = a.z - b.z;

            return res;
        }
    };
}
