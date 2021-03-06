using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew.math;
using System.Threading;
using System.Media;
using System.IO;
using System.Diagnostics;

namespace CalloutBot
{
    class Program
    {
        // if your reading this i hope your having a good day :)
        public static IntPtr ClientDLL, EngineDLL;
        static void Main(string[] args)
        {

            while (true)
            {
                Process[] p = Process.GetProcessesByName("csgo");
                if (p.Length > 0)
                {
                    Memory.Initialize("csgo");
                    Offsets.Activateoffsets();
                    ClientDLL = Read.getclient();
                    EngineDLL = Read.getengine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\r{0}  ", "csgo found!            ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(2000);
                    Console.Clear();
                    Thread Start = new Thread(Bhop);
                    Start.Start();
                    Thread Start2 = new Thread(Callouts);
                    Start2.Start();
                    break;
                }
                else
                {
                    Console.Write("\r{0}  ", "Looking For csgo");
                    Thread.Sleep(300);
                    Console.Write("\r{0}  ", "Looking For csgo.");
                    Thread.Sleep(300);
                    Console.Write("\r{0}  ", "Looking For csgo..");
                    Thread.Sleep(300);
                    Console.Write("\r{0}  ", "Looking For csgo...");
                    Thread.Sleep(300);
                }

            }
        }
        static void Callouts()
        {


            var path = Path.Combine(Directory.GetCurrentDirectory(), @"Callouts");

            var pathfront = Path.Combine(path, @"Front.wav");
            var pathBehind = Path.Combine(path, @"Behind.wav");
            var pathLeft = Path.Combine(path, @"Left.wav");
            var pathRight = Path.Combine(path, @"Right.wav");
            var pathdead = Path.Combine(path, @"Death.wav");
            SoundPlayer Front = new SoundPlayer(pathfront);
            SoundPlayer Behind = new SoundPlayer(pathBehind);
            SoundPlayer Left = new SoundPlayer(pathLeft);
            SoundPlayer Right = new SoundPlayer(pathRight);
            SoundPlayer Death = new SoundPlayer(pathdead);

            

            
            Console.Write("Callout Bot Started:                                                                    Features: Callouts and bhop");
            Console.WriteLine("");
            Console.WriteLine("");
            bool alive = false;
            while (true)
            {
                Thread.Sleep(10);



                for (int i = 1; i < 64; i++)
                {

                    int bestEntity = Read.BestEntity(ClientDLL);
                    IntPtr EntityList = Memory.Read<IntPtr>(ClientDLL + Offsets.EntityList + bestEntity * 0x10);
                    IntPtr LocalPlayer = Memory.Read<IntPtr>(ClientDLL + Offsets.LocalPlayer);
                    if (LocalPlayer == IntPtr.Zero)
                        continue;
                    if (Memory.Read<Int32>(LocalPlayer + Offsets.Health) < 1 || Memory.Read<Int32>(LocalPlayer + Offsets.Health) > 100)
                    {
                        if (alive == true)
                        {

                            int chance = new Random().Next(1, 100);

                            if (chance == 50)
                            {
                                Death.Play();
                                Thread.Sleep(5000);
                            }



                        }
                        alive = false;
                        continue;
                    }
                    else
                    {
                        alive = true;
                    }

                    if (Memory.Read<Int32>(EntityList + Offsets.Team) == Memory.Read<Int32>(LocalPlayer + Offsets.Team))
                        continue;
                    if (Memory.Read<Int32>(EntityList + Offsets.Health) < 1)
                        continue;
                    if (Memory.Read<Boolean>((IntPtr)EntityList + Offsets.Dormant))
                        continue;
                    int BoneId = 8;
                    IntPtr Bonematrix = Memory.Read<IntPtr>(EntityList + Offsets.BoneMatrix);
                    Vector3 BonePos = new Vector3(Memory.Read<float>((IntPtr)Bonematrix + 0x30 * BoneId + 0xC), Memory.Read<float>((IntPtr)Bonematrix + 0x30 * BoneId + 0x1C), Memory.Read<float>((IntPtr)Bonematrix + 0x30 * BoneId + 0x2C));

                    Vector3 Localorgin = Read.Orgin(LocalPlayer);
                    Vector3 LocalView = Read.EyePos(LocalPlayer);
                    Vector3 idk = Localorgin + LocalView;

                    IntPtr Clientstate = Memory.Read<IntPtr>(EngineDLL + Offsets.clientstate);
                    Vector3 ViewAngles = Memory.Read<Vector3>(Clientstate + Offsets.ViewAngles);
                    Vector3 PlayerOrgin = Read.Orgin(EntityList);

                    angles Aimat = Read.CalcAngle(idk, BonePos);



                    float difference = (Aimat.Y - (ViewAngles.Y));
                    if (difference > 180) difference -= 360;
                    if (difference < -180) difference += 360;


                    if (difference >= -45 && difference <= 45)
                    {
                        Front.Play();
                        Console.Write("\r{0}  ", "The Enemy is infront of you");

                    }
                    else if (difference >= 45 && difference <= 135)
                    {
                        Left.Play();
                        Console.Write("\r{0}  ", "The Enemy is to your left");
                    }
                    else if (difference >= -135 && difference <= -45)
                    {
                        Right.Play();
                        Console.Write("\r{0}  ", "The Enemy is to your right");
                    }
                    else if (difference >= 135 && difference <= 225)
                    {
                        Behind.Play();
                        Console.Write("\r{0}  ", "The Enemy is behind of you");
                    }
                    else if (difference <= -135 && difference >= -225)
                    {
                        Behind.Play();
                        Console.Write("\r{0}  ", "The Enemy is behind of you");
                    }
                    else if (difference <= -235 && difference >= -325)
                    {
                        Left.Play();
                        Console.Write("\r{0}  ", "The Enemy is to your left");
                    }


                    //    else if()





                    Thread.Sleep(5000);
                }
               
            }
        }
    
        static void Bhop()
        {
            IntPtr fJump = ClientDLL + Offsets.forceJump;
            while (true)
            {
                
                if (API.GetAsyncKeyState(0x20))
                {
                    int LocalPlayer = Memory.Read<Int32>(ClientDLL + Offsets.LocalPlayer);
                    if (LocalPlayer == 0)
                        continue;
                    int flags = Memory.Read<int>((IntPtr)LocalPlayer + Offsets.flag);
                    if (flags == 257)
                    {
                        Memory.Write<Int32>((IntPtr)fJump, 5);
                        Thread.Sleep(5);
                        Memory.Write<Int32>((IntPtr)fJump, 4);
                    }
                }
                
            }
        }
       
       
    }
}
