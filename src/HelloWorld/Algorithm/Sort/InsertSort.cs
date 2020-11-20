﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Algorithm.Sort
{
    public class InsertSort : Sort, ISort
    {
        public void Sort(int[] a)
        {
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] < a[i - 1])
                {
                    var temp = a[i];
                    int j = 0;
                    for (j = i - 1; j >= 0 && temp < a[j]; --j)
                    {
                        a[j + 1] = a[j];
                    }
                    a[j + 1] = temp;
                }
            }
        }
    }
}
