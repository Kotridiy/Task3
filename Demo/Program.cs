using System;
using ATS;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            My my = new My();

        }
    }

    class My
    {
        public MyEnum Enum { get; set; }

        public My()
        {

        }

        public enum MyEnum
        {
            One,
            Two,
        }
    }
}
