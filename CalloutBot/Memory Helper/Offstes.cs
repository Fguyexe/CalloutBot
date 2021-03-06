using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public struct Offsets
{
    public static int LocalPlayer;
    public static int Team;
    public static int EntityList;
    public static int Dormant;
    public static int Health;
    public static int flag;
    public static int forceJump;
    public static int clientstate;
    public static int ViewAngles;
    public static int VecOrgin;
    public static int VecViewOffset;
    public static int BoneMatrix;
    public static void Activateoffsets()
    {

        var path2 = Path.Combine(Directory.GetCurrentDirectory(), @"Offsets.txt");
        string[] lines = File.ReadAllLines(path2);
        Console.Beep();
        LocalPlayer = Convert.ToInt32(lines[1], 16);
        Team = Convert.ToInt32(lines[3], 16);
        EntityList = Convert.ToInt32(lines[5], 16);
        Dormant = Convert.ToInt32(lines[7], 16);
        Health = Convert.ToInt32(lines[9], 16);
        flag = Convert.ToInt32(lines[11], 16);
        forceJump = Convert.ToInt32(lines[13], 16);
        clientstate = Convert.ToInt32(lines[15], 16);
        ViewAngles = Convert.ToInt32(lines[17], 16);
        VecOrgin = Convert.ToInt32(lines[19], 16);
        VecViewOffset = Convert.ToInt32(lines[21], 16);
        BoneMatrix = Convert.ToInt32(lines[23], 16);
        Console.Beep();


    }


}

