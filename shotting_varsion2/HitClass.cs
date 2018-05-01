using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shotting_varsion2
{
    public static class HitClass
    {
        //サイコロと弾のあたり判定まとめ
        public static bool Hit(int x1, int x2, int y1, int y2, int shot1, int shotB)
        {
            if (x1 + 30 >= x2 - shot1 && x1 <= x2 + shotB - shot1)
                if (y1 + 30 >= y2 && y1 <= y2 + shotB) return true;
            if (x1 + 30 >= x2 + shot1 && x1 <= x2 + shotB + shot1)
                if (y1 + 30 >= y2 && y1 <= y2 + shotB) return true;
            if (x1 + 30 >= x2 && x1 <= x2 + shotB)
                if (y1 + 30 >= y2 + shot1 && y1 <= y2 + shot1 + shotB) return true;
            if (x1 + 30 >= x2 && x1 <= x2 + shotB)
                if (y1 + 30 >= y2 - shot1 && y1 <= y2 - shot1 + shotB) return true;

            return false;
        }
        //与謝野あたりまとめ
        public static bool HitYosano(int yx1, int yx2, int yy1, int yy2, int shotsize,int h)
        {

            if (Math.Sqrt(Math.Pow(yx1 - (yx2 - shotsize), 2) + Math.Pow(yy1 - (yy2), 2)) < h ||
                Math.Sqrt(Math.Pow(yx1 - (yx2 + shotsize), 2) + Math.Pow(yy1 - (yy2), 2)) < h ||
                Math.Sqrt(Math.Pow(yx1 - (yx2), 2) + Math.Pow(yy1 - (yy2 - shotsize), 2)) < h ||
                Math.Sqrt(Math.Pow(yx1 - (yx2), 2) + Math.Pow(yy1 - (yy2 + shotsize), 2)) < h ||
                Math.Sqrt(Math.Pow(yx1 - (yx2 + shotsize), 2) + Math.Pow(yy1 - (yy2 + shotsize), 2)) < h ||
                Math.Sqrt(Math.Pow(yx1 - (yx2 - shotsize), 2) + Math.Pow(yy1 - (yy2 + shotsize), 2)) < h ||
                Math.Sqrt(Math.Pow(yx1 - (yx2 + shotsize), 2) + Math.Pow(yy1 - (yy2 - shotsize), 2)) < h ||
                Math.Sqrt(Math.Pow(yx1 - (yx2 - shotsize), 2) + Math.Pow(yy1 - (yy2 - shotsize), 2)) < h)
                return true;

            return false;
        }
    }
}
