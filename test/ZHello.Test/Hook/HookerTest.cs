﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZHello.Hook;

namespace ZHello.Test.Hook
{
    [TestClass]
    public class HookerTest
    {
        public class OrignClass
        {
            public int Add(int x,int y)
            {
                Trace.WriteLine($"Call Add X:{x} Y:{y}");
                return x + y;
            }

            public int Max(int x,int y,int z)
            {
                Trace.WriteLine($"OrignClass Call Max X:{x} Y:{y} Z:{z}");
                return Math.Max(Math.Max(x, y), z);
            }
        }

        public class HookClass
        {
            public int HookAdd(int x,int y)
            {
                Trace.WriteLine($"Before Call Add ");
                return Add(x, y);
            }

            public int Add(int x,int y)
            {
                Trace.WriteLine($"Repea Call Add X:{x} Y:{y}");
                return (x + y) * 100;
            }


            public int Min(int x, int y, int z)
            {
                Trace.WriteLine($"OrignClass Call Min X:{x} Y:{y} Z:{z}");
                return Math.Min(Math.Min(x, y), z);
            }

        }

        [TestMethod]
        public void Main()
        {
            InLineHooker hooker = new InLineHooker();
            var m1 = typeof(OrignClass).GetMethod("Add");
            var m2 = typeof(HookClass).GetMethod("HookAdd");
            var m3 = typeof(HookClass).GetMethod("Add");

            var max = typeof(OrignClass).GetMethod("Max");
            var min = typeof(HookClass).GetMethod("Min");

            hooker.HookMethod(m1, m2, m3);

            var orig = new OrignClass();
            int t0 = orig.Add(11, 22);
            int t1 = orig.Add(11, 22);
            int t2 = orig.Add(33, 44);

            var hc = new HookClass();
            int tt = hc.Add(123, 123);

            int zz = orig.Max(10, 20, 30);
            int ww = hc.Min(10, 20, 30);

            hooker.ReplaceMethod(max, min);

            zz = orig.Max(10, 20, 30);
            ww = hc.Min(10, 20, 30);
        }

    }
}