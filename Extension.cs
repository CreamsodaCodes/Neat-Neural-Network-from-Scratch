using System;
using System.IO;
namespace app
{
public static class Extensions
{
    public static int ReadBigInt32(this BinaryReader br)
    {
        var bytes = br.ReadBytes(sizeof(Int32));
        if (BitConverter.IsLittleEndian) Array.Reverse(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }

    public static void ForEach<T>(this T[,] source, Action<int, int> action)
    {
        for (int w = 0; w < source.GetLength(0); w++)
        {
            for (int h = 0; h < source.GetLength(1); h++)
            {
                action(w, h);
            }
        }
    }

    public static double[] conv2dTo1d(byte[,] input, int newLenght, int height, int length){
        double[] new1D = new double[newLenght];
        int t = 0;
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                new1D[t] = (double)input[i,j];
                t++;
            }
        }
        return new1D;
    }

    


}}