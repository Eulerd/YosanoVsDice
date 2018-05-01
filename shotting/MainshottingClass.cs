using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DxLibDLL;

namespace shotting
{
    class MainshottingClass
    {
        public bool form1close = false;
        public static void Main(string[] args)
        {
            //初期化
            DX.ChangeWindowMode(DX.TRUE);
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);//摩訶不思議!!なぜか重くなる現象回避
            DX.DxLib_Init();

            //キー入力用
            byte[] keys = new byte[256];


            //メインループ
            while (DX.ProcessMessage() == 0)
            {

                
                //キー入力
                DX.GetHitKeyStateAll(out keys[0]);
                var Pos = new ClassPosition() { X = 100, Y = 100 };


                DX.ScreenFlip();
            }

            //終了
            DX.DxLib_End();
        }
    }
}