using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DxLibDLL;

namespace shotting_varsion2
{
    class MainShotting2Class
    {

        public static void Main(string[] args)
        {
            //X -> 640 , Y -> 480

            
            //初期化
            DX.ChangeWindowMode(DX.TRUE);
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);//摩訶不思議!!なぜか重くなる現象回避
            DX.DxLib_Init();
            DX.SetMainWindowText("Yosano VS Dice");
            //終わり
            bool End = false;

            //キー用
            byte[] keys = new byte[256];
            bool joypadflag = false;

            //自機の場所(Class使ってみたよ!)
            var crossPos = new ClassPosition2() { X = 600, Y = 400 };
            var wasdPos = new ClassPosition2() { X = 100, Y = 100 };
            //北条用
            var hPos = new ClassPosition2() { X = -10, Y = -10 };
            bool houzyoub = false;
            int hshot = 5;
            double hangle = 0.05;
            int detaa = DX.LoadSoundMem("detaa.mp3");
            //与謝野用
            var yPos = new ClassPosition2() { X = 640, Y = 480 };
            double yspin = 0.0;
            //速さと回転もランダム
            int RandX = 1;
            int RandY = 1;
            double RandSpin = 0;
            //与謝野BGM
            int yBGM = DX.LoadMusicMem("battle1.mp3");
            int yBGMb = 0;
            //与謝野HP
            int yHP = 10;

            //弾が大きくなるアイテム用
            int shotbigX = DX.GetRand(600) + 20;
            int shotbigY = DX.GetRand(400) + 70;
            //時折だすようにするお
            int shotbigrand = 1;

            //HP
            int[] hp = new int[4] { 100, 100, 100, 100 };
            int[] R = new int[2] { 0, 0 };
            int[] G = new int[2] { 0, 0 };
            int[] B = new int[2] { 255, 255 };


            //弾の大きさ
            int[] shots = new int[2] { 7, 7 };
            //弾の速さ
            var Speed = new ClassShot() { Speed = 5 };
            bool shotR = false;
            bool shotL = false;
            bool yosanomode = false;
            int shotr = 0;
            int shotl = 0;
            //弾の音用
            int shotHandle = DX.LoadSoundMem("shot1.mp3");

            //弾が当たったかどうかの奴
            bool shothitwasd = false;
            bool shothitcross = false;

            //Hな自機に変わっちゃう
            int hziki = 0;
            int hTimer = 100;
            bool hzikiB = false;

            //自機読み込み
            int[] Handle = { DX.LoadGraph("dice1.png"), DX.LoadGraph("dice1e.png"), DX.LoadGraph("与謝野晶子.JPG"),DX.LoadGraph("yosanobreak.png") ,
                            DX.LoadGraph("与謝野晶子.JPG"), DX.LoadGraph("北条丸.png"),DX.LoadGraph("与謝野_敗北時.png"),DX.LoadGraph("battledoommode.png")};
            int[] diceHandle = new int[16];
            int[] Arrow = new int[4];
            DX.LoadDivGraph("arrow.png", 4, 1, 4, 50, 50, out Arrow[0]);
            DX.LoadDivGraph("dicelist.png", 12, 4, 3, 30, 30,out diceHandle[0]);
            for (int di = 0; di < 4; di++) diceHandle[12 + di] = DX.LoadGraph("自機" + di + ".png");
            /*
            int[] diceHandle = {DX.LoadGraph("diceup.png"),DX.LoadGraph("dicedown.png"),DX.LoadGraph("diceleft.png"),
                                DX.LoadGraph("diceright.png"),DX.LoadGraph("diceupe.png"),DX.LoadGraph("dicedowne.png"),
                                DX.LoadGraph("dicelefte.png"),DX.LoadGraph("dicerighte.png"),DX.LoadGraph("dicebreak.png"),
                                DX.LoadGraph("dice1.png"),DX.LoadGraph("dice1e.png"),DX.LoadGraph("dicepoison.png"),
                                DX.LoadGraph("自機0.png"),DX.LoadGraph("自機1.png"),DX.LoadGraph("自機2.png")};*/
            int[] menuHandle = { DX.LoadGraph("menudice.png"), DX.LoadGraph("menu与謝野晶子.JPG"), DX.LoadGraph("battledoom.png") };
            string[] menuName = { "ノーマルモード", "与謝野モード", "バトルドーム" };
            string[] dicename = { "白", "緑" };

            //爆発用
            int bomnum = 16;
            int bomnumber = 20;
            bool[] boms = new bool[bomnumber];
            int[] bomX = new int[bomnumber];
            int[] bomY = new int[bomnumber];
            int[] bomInt = new int[bomnum];
            for (int i = 0; i < bomnumber; i++) boms[i] = false;
            int[] bom = new int[bomnum];
            DX.LoadDivGraph("bom.png", bomnum, 8, 2, 96, 86, out bom[0]);
            //DX.LoadDivGraph("bom3.png", bomnum, 8, 8, 32, 32, out bom[0]);
            //0 = up,1 = down,2 = left,3 = right,4 = upE,5 = downE,6 = leftE,7 = rightE,8 = break

            //ゲームモード用
            int gamemode = 0;
            int modepoint = 0;
            bool play1 = true;


            //SAKURA
            DX.PlayMusic("sakura-sakura (GameOver)1.mid", DX.DX_PLAYTYPE_LOOP);

            //ゲームモード変更
            DX.SetFontSize(25);
            while (true)
            {
                if (DX.CheckHitKey(DX.KEY_INPUT_SPACE) != 0 || DX.GetJoypadInputState(DX.DX_INPUT_PAD1) == 4)
                {
                    modepoint++;
                    for (int i = 0; i < 100; i++)
                    {
                        DX.WaitTimer(3);
                        DX.ClearDrawScreen();
                        DX.DrawString(200, 80, menuName[modepoint % 3], DX.GetColor(255, 255, 255));
                        //消えてく奴
                        if (modepoint % 3 == 0) DX.DrawRotaGraph(320 - (i * 2), 240, ((100 - i) * 0.01), 0, menuHandle[2], 0);
                        else DX.DrawRotaGraph(320 - (i * 2), 240, ((100 - i) * 0.01), 0, menuHandle[modepoint % 3 - 1], 0);
                        //あがって来る奴
                        DX.DrawRotaGraph(520 - (i * 2), 240,(i * 0.01), 0, menuHandle[modepoint % 3], 0);
                        DX.ScreenFlip();
                    }
                }
                if(DX.GetJoypadInputState(DX.DX_INPUT_PAD1) == 2)
                {
                    if (modepoint % 3 == 0) modepoint = 2;
                    else modepoint--;
                    for(int i = 0;i < 100; i++)
                    {
                        DX.WaitTimer(3);
                        DX.ClearDrawScreen();
                        DX.DrawString(200, 80, menuName[modepoint % 3], DX.GetColor(255, 255, 255));
                        //消えてく奴
                        if (modepoint % 3 == 2) DX.DrawRotaGraph(320 + (i * 2), 240, ((100 - i) * 0.01), 0, menuHandle[0], 0);
                        else DX.DrawRotaGraph(320 + (i * 2), 240, ((100 - i) * 0.01), 0, menuHandle[modepoint % 3 + 1], 0);
                        //あがって来る奴
                        DX.DrawRotaGraph(120 + (i * 2), 240, (i * 0.01), 0, menuHandle[modepoint % 3], 0);
                        DX.ScreenFlip();
                    }
                }
                DX.ClearDrawScreen();
                //モード
                DX.DrawString(100, 50, "ゲームモードを選んでください", DX.GetColor(255, 255, 255));
                DX.DrawString(200, 80, menuName[modepoint % 3], DX.GetColor(255, 255, 255));
                DX.DrawString(100, 350, "スペースでモード変更,ENTERで開始", DX.GetColor(200, 200, 200));
                DX.DrawString(280, 400, "二人用", DX.GetColor(255, 255, 255));
                DX.DrawRotaGraph(320, 240, 1, 0, menuHandle[modepoint % 3], DX.FALSE);
                DX.WaitTimer(100);
                if (DX.CheckHitKey(DX.KEY_INPUT_RETURN) != 0 || DX.GetJoypadInputState(DX.DX_INPUT_PAD1) == 128)
                {
                    if (modepoint % 3 == 1) yosanomode = true;
                    gamemode = modepoint % 3;
                    break;
                }
            }
            DX.SetFontSize(18);
            DX.PlayMusic("sakura-sakura (vs kotoBBA)2.mid", DX.DX_PLAYTYPE_LOOP);

            //メインループ
            while (DX.ProcessMessage() == 0)
            {

                //画面をキレイキレイする
                DX.ClearDrawScreen();


                //キー入力
                DX.GetHitKeyStateAll(out keys[0]);
                int joypad = DX.GetJoypadInputState(DX.DX_INPUT_PAD1);

                //確認
                DX.DrawString(200, 100, DX.GetJoypadInputState(DX.DX_INPUT_PAD1).ToString(), DX.GetColor(255, 255, 255));

                //ESCで落とす
                if (keys[DX.KEY_INPUT_ESCAPE] != 0 || joypad == 32768)
                {
                    DX.ClearDrawScreen();
                    DX.DrawString(200, 240, "中断されました", DX.GetColor(255, 255, 255));
                    DX.DrawString(200, 290, "何かキーを押してください", DX.GetColor(255, 255, 255));
                    DX.WaitKey();
                    break;
                }

                //DX.DrawGraph(400, 400, bom[bomInt], DX.TRUE);
                //bomInt++;
                //if (bomInt == 16) bomInt = 0;
                ///////////////////////////////////////////////////
                if (joypad == 12288) hzikiB = true;
                //与謝野モード解放
                if (keys[DX.KEY_INPUT_Y] != 0 || yosanomode || joypad == 65536)
                {
                    yosanomode = true;
                    yBGMb++;
                }
                //与謝野用
                if (yosanomode)
                {
                    if (yBGMb == 1)
                    {
                        while (true)
                        {
                            DX.ClearDrawScreen();
                            DX.DrawGraph(300, 200, Arrow[0], DX.TRUE);
                            DX.DrawGraph(300, 280, Arrow[2], DX.TRUE);
                            DX.DrawString(200, 140, "与謝野モード突入!!HPを入力してね!", DX.GetColor(255, 255, 255));
                            DX.DrawString(100, 340, "弱い:～100,普通:100～500,強い:500～", DX.GetColor(255, 255, 255));
                            if (yHP < 0) yHP = 0;
                            if (yHP > 1000) yHP = 1000;
                            DX.DrawString(300, 240, yHP.ToString(), DX.GetColor(255, 255, 255));
                            DX.WaitTimer(100);
                            DX.GetHitKeyStateAll(out keys[0]);
                            joypad = DX.GetJoypadInputState(DX.DX_INPUT_PAD1);
                            if (joypad == 8 || keys[DX.KEY_INPUT_UP] != 0) yHP += 100;
                            if (joypad == 1 || keys[DX.KEY_INPUT_DOWN] != 0) yHP -= 100;
                            if (joypad == 128 || keys[DX.KEY_INPUT_RETURN] != 0) break;
                            if(joypad == 12288)
                            {
                                yHP = 810;
                                DX.ClearDrawScreen();
                                DX.DrawString(300, 240, yHP.ToString(), DX.GetColor(255, 255, 255));
                                DX.WaitTimer(1000);
                                hzikiB = true;
                                break;
                            }
                            DX.ScreenFlip();
                        }

                        DX.WaitTimer(500);
                        DX.ClearDrawScreen();
                        int play = 25;
                        while (true)
                        {
                            DX.GetHitKeyStateAll(out keys[0]);
                            joypad = DX.GetJoypadInputState(DX.DX_INPUT_PAD1);
                            if (joypad == 128 || keys[DX.KEY_INPUT_RETURN] != 0) break;
                            if (joypad == 8 || keys[DX.KEY_INPUT_UP] != 0) play = -25;
                            if (joypad == 1 || keys[DX.KEY_INPUT_DOWN] != 0) play = 25;
                            DX.ClearDrawScreen();
                            DX.DrawString(200, 100, "1PLAY", DX.GetColor(255, 255, 255));
                            DX.DrawString(200, 150, "2PLAY", DX.GetColor(255, 255, 255));
                            DX.DrawCircle(180, 133 + play, 5, DX.GetColor(255, 0, 0));
                            DX.WaitTimer(100);
                            DX.ScreenFlip();
                        }
                        if(play == -25) play1 = false;
                        gamemode = 1;
                        crossPos.X = 600;
                        crossPos.Y = 140;
                        wasdPos.X = 100;
                        wasdPos.Y = 100;
                        hp[0] = 100;
                        hp[1] = 100;
                        DX.PlayMusicMem(yBGM, DX.DX_PLAYTYPE_LOOPBIT);
                        yBGMb++;
                    }

                    if (300 < yPos.X && yPos.X < 340 || 230 < yPos.Y && yPos.Y < 250)
                    {
                        if (!houzyoub)
                        {
                            DX.PlaySoundMem(detaa, DX.DX_PLAYTYPE_BACK);
                            hPos.X = yPos.X;
                            hPos.Y = yPos.Y;
                            houzyoub = true;
                        }
                    }
                    if (houzyoub)
                    {
                        //北条の描写
                        DX.DrawRotaGraph(hPos.X, hPos.Y - hshot, 0.3, hangle, Handle[5], DX.TRUE);
                        DX.DrawRotaGraph(hPos.X, hPos.Y + hshot, 0.3, hangle, Handle[5], DX.TRUE);
                        DX.DrawRotaGraph(hPos.X - hshot, hPos.Y, 0.3, hangle, Handle[5], DX.TRUE);
                        DX.DrawRotaGraph(hPos.X + hshot, hPos.Y, 0.3, hangle, Handle[5], DX.TRUE);
                        DX.DrawRotaGraph(hPos.X + hshot, hPos.Y + hshot, 0.3, hangle, Handle[5], DX.TRUE);
                        DX.DrawRotaGraph(hPos.X + hshot, hPos.Y - hshot, 0.3, hangle, Handle[5], DX.TRUE);
                        DX.DrawRotaGraph(hPos.X - hshot, hPos.Y + hshot, 0.3, hangle, Handle[5], DX.TRUE);
                        DX.DrawRotaGraph(hPos.X - hshot, hPos.Y - hshot, 0.3, hangle, Handle[5], DX.TRUE);
                        hshot += 8;
                        hangle += 0.05;
                    }

                    if (yPos.X <= -10 || yPos.Y <= -10)
                    {
                        RandX = DX.GetRand(7);
                        RandY = DX.GetRand(7);
                        RandSpin = DX.GetRand(100) / 50;
                        hPos.X = 0;
                        hPos.Y = 0;
                        hshot = 5;
                        houzyoub = false;
                        if (DX.GetRand(50) % 2 == 0)
                        {
                            yPos.X = 640 + DX.GetRand(500);
                            yPos.Y = 480;
                        }
                        else
                        {
                            yPos.Y = 480 + DX.GetRand(350);
                            yPos.X = 640;
                        }
                    }
                    else
                    {
                        //横98,縦129
                        //yPos.X = 320;
                        //yPos.Y = 240;
                        //与謝野の描写
                        DX.DrawRotaGraph(yPos.X, yPos.Y, 0.25, yspin, Handle[2], DX.TRUE);
                        Handle[2] = Handle[4];
                        yPos.X -= RandX + 1;
                        yPos.Y -= RandY + 1;
                        //yspin += 0.01;

                        if (RandSpin % 2 == 0) yspin += RandSpin;
                        else yspin -= RandSpin;

                    }
                    //スピンを戻す
                    if (yspin >= 2 * Math.PI) yspin = 0;

                    DX.DrawString(280, 10, "与謝野モード", DX.GetColor(150, 150, 150));

                }
                else DX.DrawString(280, 10, "ノーマルモード", DX.GetColor(150, 150, 150));


                //弾(Class使ってry)
                var TwasdPos = new ClassPosition2() { X = crossPos.X + 11, Y = crossPos.Y + 11 };
                var TcrossPos = new ClassPosition2() { X = wasdPos.X + 11, Y = wasdPos.Y + 11 };

                //あたり判定？
                //wasdサイコロ　cross弾
                
                if (!yosanomode)
                {
                    //やったねたえちゃん！こんなに短くなったよ!!
                    //wasdサイコロ cross弾
                    shothitcross = HitClass.Hit(wasdPos.X, TwasdPos.X, wasdPos.Y, TwasdPos.Y, shotl, shots[1]);


                    //crossサイコロ　wasd弾
                    /*
                                if (x1 + 30 >= x2 - shot1 && x1 <= x2 + shotB - shot1)
                if (y1 + 30 >= y2 && y1 <= y2 + shotB) return true;
                    */
                    if (hzikiB)
                    {
                        if (crossPos.X + 30 >= TcrossPos.X - shotr && crossPos.X <= TcrossPos.X + shots[0] - shotr)
                            if (0 < crossPos.Y && crossPos.Y < TcrossPos.Y)
                                shothitwasd = true;
                        DX.DrawFillBox(TcrossPos.X + 10, TcrossPos.Y, TcrossPos.X + 15, 0, DX.GetColor(255, 255, 255));
                    }
                    else shothitwasd = HitClass.Hit(crossPos.X, TcrossPos.X, crossPos.Y, TcrossPos.Y, shotr, shots[0]);



                }
                if (wasdPos.X + 30 >= crossPos.X && wasdPos.X <= crossPos.X + 30)
                {
                    if (wasdPos.Y + 30 >= crossPos.Y && wasdPos.Y <= crossPos.Y + 30)
                    {
                        shothitcross = false;
                        shothitwasd = false;
                    }
                }

                

                //勝ち判定
                for (int i = 0; i < 2; i++)
                {
                    if (hp[i] <= 0)
                    {
                        DX.ClearDrawScreen();
                        DX.PlayMusic("bomb1.mp3", DX.TRUE);
                        DX.DrawString(100, 150, "スペースキーを押してください", DX.GetColor(255, 255, 255));
                        if (yosanomode)
                        {
                            for (int i3 = 0; i3 < 255; i3++)
                            {
                                DX.WaitTimer(5);
                                DX.SetDrawBright(i3, i3, i3);
                                DX.DrawRotaGraph(320, 270, 0.0065 * i3, 0, Handle[2], DX.TRUE);
                                DX.ScreenFlip();
                            }
                            DX.SetFontSize(64);
                            DX.DrawString(60, 240, "与謝野晶子の勝ち", DX.GetColor(255, 0, 0));
                        }
                        else
                        {
                            if (hzikiB)
                            {
                                DX.DrawString(100, 100, "Hな自機の勝ち", DX.GetColor(255, 255, 255));
                                DX.DrawRotaGraph(320, 240, 3.0, 0, diceHandle[13], DX.TRUE);
                            }
                            else
                            {
                                DX.DrawString(100, 100, dicename[i] + "DICEの勝ち", DX.GetColor(255, 255, 255));
                                DX.DrawRotaGraph(320, 240, 3.0, 0, diceHandle[i + 9], DX.TRUE);
                            }
                        }
                        End = true;
                    }
                }
                if (yosanomode && yHP < 0)
                {
                    ///////////////////////////////////////////////////
                    DX.PlayMusic("uwaaaaa.mp3", DX.TRUE);
                    for (int iloop = 0; iloop < 200; iloop++)
                    {
                        DX.ClearDrawScreen();
                        DX.DrawRotaGraph(320, 240, (300 - (iloop * 2)) * 0.005, Math.PI, Handle[2], 0);
                        for (int i = 0; i < bomnumber; i++)
                        {
                            if (!boms[i])
                            {
                                bomX[i] = DX.GetRand(600);
                                bomY[i] = DX.GetRand(400) + 60;
                                boms[i] = true;
                                break;
                            }
                        }

                        for (int i = 0; i < bomnumber; i++)
                        {
                            if (boms[i])
                            {
                                bomInt[i]++;
                                if (bomInt[i] == bomnum)
                                {
                                    bomInt[i] = 0;
                                    boms[i] = false;
                                }
                                DX.DrawGraph(bomX[i], bomY[i], bom[bomInt[i]], 1);
                            }
                        }
                        DX.ScreenFlip();
                    }

                    DX.ClearDrawScreen();
                    DX.DrawString(100, 100, "スペースキーを押してください", DX.GetColor(255, 255, 255));
                    DX.DrawString(100, 50, "DICEの勝ち", DX.GetColor(255, 255, 255));
                    DX.DrawRotaGraph(320, 300, 0.3, 0, Handle[6], 0);              
                    //DX.DrawRotaGraph(260, 240, 3.0, 0, diceHandle[9], DX.TRUE);
                    //DX.DrawRotaGraph(350, 240, 3.0, 0, diceHandle[10], DX.TRUE);
                    DX.PlayMusic("exciting.mp3",DX.DX_PLAYTYPE_NORMAL);
                    End = true;
                }
                if (End)
                {
                    DX.WaitKey();
                    DX.DxLib_End();
                    break;
                }




                //画面外に行かないようにするIT

                //crossで動くほうの奴
                    if (crossPos.X <= 0) crossPos.X = 0;
                    if (crossPos.X >= 640 - 30) crossPos.X = 640 - 30;
                    if (crossPos.Y <= 70) crossPos.Y = 70;
                    if (crossPos.Y >= 480 - 30) crossPos.Y = 480 - 30;
                //wasdで動くほうの奴
                if (wasdPos.X <= 0) wasdPos.X = 0;
                if (wasdPos.X >= 640 - 30) wasdPos.X = 640 - 30;
                if (wasdPos.Y <= 70) wasdPos.Y = 70;
                if (wasdPos.Y >= 480 - 30) wasdPos.Y = 480 - 30;
                ////////////////////////////////////////

                if (joypad >= 129)
                {
                    joypad -= 128;
                    joypadflag = true;
                }

                if (keys[DX.KEY_INPUT_UP] != 0)
                {
                    Handle[1] = diceHandle[4];
                    crossPos.Y -= 3;
                }

                if (keys[DX.KEY_INPUT_DOWN] != 0)
                {
                    crossPos.Y += 3;
                    Handle[1] = diceHandle[5];
                }

                if (keys[DX.KEY_INPUT_LEFT] != 0)
                {
                    crossPos.X -= 3;
                    Handle[1] = diceHandle[6];
                }

                if (keys[DX.KEY_INPUT_RIGHT] != 0)
                {
                    crossPos.X += 3;
                    Handle[1] = diceHandle[7];
                }
                if (keys[DX.KEY_INPUT_W] != 0 || joypad == 8)
                {
                    wasdPos.Y -= 3;
                    Handle[0] = diceHandle[0];
                    if (hzikiB) Handle[0] = diceHandle[12];
                }
                if (keys[DX.KEY_INPUT_S] != 0 || joypad == 1)
                {
                    wasdPos.Y += 3;
                    Handle[0] = diceHandle[1];
                    if (hzikiB) Handle[0] = diceHandle[12];
                }
                if (keys[DX.KEY_INPUT_A] != 0 || joypad == 2)
                {
                    wasdPos.X -= 3;
                    Handle[0] = diceHandle[2];
                    if (hzikiB) Handle[0] = diceHandle[14];
                }
                if (keys[DX.KEY_INPUT_D] != 0 || joypad == 4)
                {
                    wasdPos.X += 3;
                    Handle[0] = diceHandle[3];
                    if (hzikiB) Handle[0] = diceHandle[13];
                }
                //joypad用
                if(joypad == 3)//左下
                {
                    wasdPos.X -= 2;
                    wasdPos.Y += 2;
                    Handle[0] = diceHandle[2];
                    if (hzikiB) Handle[0] = diceHandle[14];
                } 
                if(joypad == 5)//右下
                {
                    wasdPos.X += 2;
                    wasdPos.Y += 2;
                    Handle[0] = diceHandle[3];
                    if (hzikiB) Handle[0] = diceHandle[13];
                } 
                if(joypad == 10)//左上
                {
                    wasdPos.X -= 2;
                    wasdPos.Y -= 2;
                    Handle[0] = diceHandle[2];
                    if (hzikiB) Handle[0] = diceHandle[14];
                }
                if(joypad == 12)//右上
                {
                    wasdPos.X += 2;
                    wasdPos.Y -= 2;
                    Handle[0] = diceHandle[3];
                    if (hzikiB) Handle[0] = diceHandle[13];
                } 


                //与謝野のあたり判定
                if (yosanomode)
                {
                    //HP表示
                    DX.DrawString(280, 30, "HP:" + yHP.ToString(), DX.GetColor(150, 150, 150));
                    //crossサイコロにあたったとき
                    if (play1)
                    {
                        if (Math.Sqrt(Math.Pow(yPos.X - (crossPos.X + 15), 2) + Math.Pow(yPos.Y - (crossPos.Y + 15), 2)) < 50)
                        {
                            shothitwasd = true;
                        }
                    }
                    //wasdサイコロにあったとき
                    if (Math.Sqrt(Math.Pow(yPos.X - (wasdPos.X + 15), 2) + Math.Pow(yPos.Y - (wasdPos.Y + 15), 2)) < 50)
                    {
                        shothitcross = true;
                    }
                    //wasd && cross弾にあったら(できるかな…)
                    if (HitClass.HitYosano(yPos.X, wasdPos.X, yPos.Y, wasdPos.Y, shotr,60))
                    {
                        Handle[2] = Handle[3];
                        yHP--;
                    }
                    if(HitClass.HitYosano(yPos.X, crossPos.X, yPos.Y, crossPos.Y, shotl, 60) && play1)
                    {
                        Handle[2] = Handle[3];
                        yHP--;
                    }
                    //北条にwasdサイコロにあたったら
                    if(HitClass.HitYosano(hPos.X,wasdPos.X + 15,hPos.Y,wasdPos.Y + 15,hshot,40))
                    {
                        shothitcross = true;
                        hp[1] += 1;
                    }
                    if(HitClass.HitYosano(hPos.X, crossPos.X + 15, hPos.Y, crossPos.Y + 15, hshot, 40))
                    {
                        shothitwasd = true;
                        hp[0] += 1;
                    }
                    /*
                    if (Math.Sqrt(Math.Pow((hPos.X + hshot) - (wasdPos.X + 15), 2) + Math.Pow(hPos.Y - (wasdPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow((hPos.X - hshot) - (wasdPos.X + 15), 2) + Math.Pow(hPos.Y - (wasdPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow(hPos.X - (wasdPos.X + 15), 2) + Math.Pow((hPos.Y + hshot) - (wasdPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow(hPos.X - (wasdPos.X + 15), 2) + Math.Pow((hPos.Y - hshot) - (wasdPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow((hPos.X + hshot) - (wasdPos.X + 15), 2) + Math.Pow((hPos.Y + hshot) - (wasdPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow((hPos.X - hshot) - (wasdPos.X + 15), 2) + Math.Pow((hPos.Y - hshot) - (wasdPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow((hPos.X - hshot) - (wasdPos.X + 15), 2) + Math.Pow((hPos.Y + hshot) - (wasdPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow((hPos.X + hshot) - (wasdPos.X + 15), 2) + Math.Pow((hPos.Y - hshot) - (wasdPos.Y + 15), 2)) < 40)
                    {
                        shothitcross = true;
                        hp[1] += 1;
                    }
                    */
                    //北条にcrossサイコロが当たったら
                    /*
                    if (Math.Sqrt(Math.Pow((hPos.X + hshot) - (crossPos.X + 15), 2) + Math.Pow(hPos.Y - (crossPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow((hPos.X - hshot) - (crossPos.X + 15), 2) + Math.Pow(hPos.Y - (crossPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow(hPos.X - (crossPos.X + 15), 2) + Math.Pow((hPos.Y + hshot) - (crossPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow(hPos.X - (crossPos.X + 15), 2) + Math.Pow((hPos.Y - hshot) - (crossPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow((hPos.X - hshot) - (crossPos.X + 15), 2) + Math.Pow((hPos.Y - hshot) - (crossPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow((hPos.X + hshot) - (crossPos.X + 15), 2) + Math.Pow((hPos.Y - hshot) - (crossPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow((hPos.X - hshot) - (crossPos.X + 15), 2) + Math.Pow((hPos.Y + hshot) - (crossPos.Y + 15), 2)) < 40 ||
                        Math.Sqrt(Math.Pow((hPos.X + hshot) - (crossPos.X + 15), 2) + Math.Pow((hPos.Y + hshot) - (crossPos.Y + 15), 2)) < 40)
                    {
                        shothitwasd = true;
                        hp[0] += 0;
                    }
                    */
                    //あたり判定の場所を赤色で表示するお
                    //DX.DrawCircle(yPos.X, yPos.Y, 50, DX.GetColor(255, 0, 0));
                }

                //ダメージをまとめてこ↑こ↓で処理&&描写
                if (play1)
                {
                    if (shothitwasd)
                    {
                        hp[0] -= 2;
                        Handle[1] = diceHandle[8];
                        shothitwasd = false;
                    }
                }
                if (shothitcross)
                {
                    hp[1] -= 2;
                    Handle[0] = diceHandle[8];
                    if (hzikiB) Handle[0] = diceHandle[15];
                    shothitcross = false;
                }
                ////////////////////////////////////////////////////////////////////////

                //アイテムあたり判定
                if (Math.Sqrt(Math.Pow(shotbigX - (crossPos.X + 15), 2) + Math.Pow(shotbigY - (crossPos.Y + 15), 2)) < 15)
                {
                    shotbigX = DX.GetRand(600) + 20;
                    shotbigY = DX.GetRand(400) + 70;
                    shots[1]++;
                    shotbigrand++;
                }
                if (Math.Sqrt(Math.Pow(shotbigX - (wasdPos.X + 15), 2) + Math.Pow(shotbigY - (wasdPos.Y + 15), 2)) < 15)
                {
                    shotbigX = DX.GetRand(600) + 20;
                    shotbigY = DX.GetRand(400) + 70;
                    shots[0]++;
                    shotbigrand++;
                }
                //アイテムの描写
                //DX.DrawString(400, 10, shotbigrand.ToString(), DX.GetColor(255, 255, 255));
                if (shotbigrand == 1)
                {
                    DX.DrawCircle(shotbigX, shotbigY, 4, DX.GetColor(255, 255, 0));
                }
                else shotbigrand = DX.GetRand(200);
                    //こ↑こ↓でたまに出すようにしとる


                //さいころの描写
                if(gamemode == 2)
                {
                    Handle[0] = Handle[7];
                    Handle[1] = Handle[7];
                }
                if(play1) DX.DrawGraph(crossPos.X, crossPos.Y, Handle[1], DX.TRUE);
                DX.DrawGraph(wasdPos.X, wasdPos.Y, Handle[0], DX.TRUE);

                Handle[0] = diceHandle[9];
                Handle[1] = diceHandle[10];
                if (hzikiB) Handle[0] = diceHandle[12];


                //弾の描写
                if (keys[DX.KEY_INPUT_LSHIFT] != 0 || joypad == 128 || joypadflag)
                {
                    shotR = true;
                    hziki++;
                }
                if (keys[DX.KEY_INPUT_RETURN] != 0 || keys[DX.KEY_INPUT_RSHIFT] != 0)
                {
                    shotL = true;
                }
                joypadflag = false;



                if (shotR)
                {
                    shotr += 10;

                    if (shotr < 480)
                    {
                        if (gamemode == 2)
                        {
                            DX.DrawGraph(TcrossPos.X - shotr, TcrossPos.Y, Handle[7], DX.TRUE);
                            DX.DrawGraph(TcrossPos.X + shotr, TcrossPos.Y, Handle[7], DX.TRUE);
                            DX.DrawGraph(TcrossPos.X, TcrossPos.Y - shotr, Handle[7], DX.TRUE);
                            DX.DrawGraph(TcrossPos.X, TcrossPos.Y + shotr, Handle[7], DX.TRUE);
                        }
                        else　if(!hzikiB)
                        {
                            DX.DrawFillBox(TcrossPos.X - shotr, TcrossPos.Y, TcrossPos.X + shots[0] - shotr, TcrossPos.Y + shots[0],
                                DX.GetColor(255, 255, 255));
                            DX.DrawFillBox(TcrossPos.X + shotr, TcrossPos.Y, TcrossPos.X + shots[0] + shotr, TcrossPos.Y + shots[0],
                                DX.GetColor(255, 255, 255));
                            DX.DrawFillBox(TcrossPos.X, TcrossPos.Y + shotr, TcrossPos.X + shots[0], TcrossPos.Y + shots[0] + shotr,
                                DX.GetColor(255, 255, 255));
                            DX.DrawFillBox(TcrossPos.X, TcrossPos.Y - shotr, TcrossPos.X + shots[0], TcrossPos.Y + shots[0] - shotr,
                                DX.GetColor(255, 255, 255));
                        }
                    }
                    else
                    {
                        shotr = 0;
                        shotR = false;
                    }
                }

                if (shotL)
                {
                    shotl += 10;
                    if (shotl < 480)
                    {
                        if (play1)
                        {
                            if(gamemode == 2)
                            {
                                DX.DrawGraph(TwasdPos.X - shotl, TwasdPos.Y, Handle[7], DX.TRUE);
                                DX.DrawGraph(TwasdPos.X + shotl, TwasdPos.Y, Handle[7], DX.TRUE);
                                DX.DrawGraph(TwasdPos.X, TwasdPos.Y - shotl, Handle[7], DX.TRUE);
                                DX.DrawGraph(TwasdPos.X, TwasdPos.Y + shotl, Handle[7], DX.TRUE);
                            }
                            else
                            {
                                DX.DrawFillBox(TwasdPos.X - shotl, TwasdPos.Y, TwasdPos.X + shots[1] - shotl, TwasdPos.Y + shots[1],
                                    DX.GetColor(0, 255, 0));
                                DX.DrawFillBox(TwasdPos.X + shotl, TwasdPos.Y, TwasdPos.X + shots[1] + shotl, TwasdPos.Y + shots[1],
                                    DX.GetColor(0, 255, 0));
                                DX.DrawFillBox(TwasdPos.X, TwasdPos.Y + shotl, TwasdPos.X + shots[1], TwasdPos.Y + shots[1] + shotl,
                                    DX.GetColor(0, 255, 0));
                                DX.DrawFillBox(TwasdPos.X, TwasdPos.Y - shotl, TwasdPos.X + shots[1], TwasdPos.Y + shots[1] - shotl,
                                    DX.GetColor(0, 255, 0));
                            }
                        }
                    }
                    else
                    {
                        shotl = 0;
                        shotL = false;
                    }
                }

                //行くなよ！絶対行くなよ！の線
                DX.DrawLine(0, 60, 640, 60, DX.GetColor(255, 255, 255));


                //HP描写処理
                for (int i = 0; i < 2; i++)
                {
                    if (hp[i] <= hp[i + 2] / 2)
                    {
                        R[i] = 255;
                        G[i] = 255;
                        B[i] = 0;
                    }
                    if (hp[i] <= hp[i + 2] / 4)
                    {
                        R[i] = 255;
                        G[i] = 0;
                        B[i] = 0;
                    }
                }
                DX.DrawString(10, 10, hp[1].ToString(), DX.GetColor(R[1], G[1], B[1]));
                DX.DrawFillBox(wasdPos.X, wasdPos.Y - 5, wasdPos.X + (hp[1] / 3), wasdPos.Y, DX.GetColor(R[1], G[1], B[1]));
                if (play1)
                {
                    DX.DrawString(530, 10, hp[0].ToString(), DX.GetColor(R[0], G[0], B[0]));
                    DX.DrawFillBox(crossPos.X, crossPos.Y - 5, crossPos.X + (hp[0] / 3), crossPos.Y, DX.GetColor(R[0], G[0], B[0]));
                }

                //弾の大きさ描写
                DX.DrawString(10, 30, "ShotSize:" + shots[0], DX.GetColor(255, 255, 255));
                if(play1) DX.DrawString(530, 30, "ShotSize:" + shots[1], DX.GetColor(0, 255, 0));
                /*
                if (gamemode == 0)
                {
                    DX.ClearDrawScreen();
                }
                */
                DX.ScreenFlip();
            }

            DX.DxLib_End();
        }
    }

}