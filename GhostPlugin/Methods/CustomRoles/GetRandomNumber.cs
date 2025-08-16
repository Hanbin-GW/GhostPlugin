using System;

namespace GhostPlugin.Methods.CustomRoles
{
    public class GetRandomNumber
    {
        static Random _random = new();
        public static int GetRandomInt()
        {
            return _random.Next();
        }
        public static int GetRandomInt(int max)
        {
            return _random.Next(max);
        }
        public static int GetRandomInt(int min, int max)
        {
            return _random.Next(min, max);
        }
        public static double GetRandomDouble()
        {
            return _random.NextDouble();
        }
    }
}