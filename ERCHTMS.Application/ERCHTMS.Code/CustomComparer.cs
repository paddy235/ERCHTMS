using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERCHTMS.Code
{
    public class CustomComparer : System.Collections.IComparer
    {
        /// <summary>
        /// 字符串排序用
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            string s1 = (string)x;
            string s2 = (string)y;
            if (s1.Length > s2.Length) return 1;
            if (s1.Length < s2.Length) return -1;
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] > s2[i]) return 1;
                if (s1[i] < s2[i]) return -1;
            }

            return 0;
        }
    }
}
