using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PewPew.math;
public struct Read
{
    
    public static int BestEntity(IntPtr ClientDLL)
    {
        int curebest = -1;
        float Mindist = float.MaxValue;
        float dist;

        for (int i = 1; i < 65; i++)
        {
            IntPtr LocalPlayer = Memory.Read<IntPtr>(ClientDLL + Offsets.LocalPlayer);
            IntPtr EntityList = Memory.Read<IntPtr>(ClientDLL + Offsets.EntityList + i * 0x10);
            if (LocalPlayer == IntPtr.Zero)
                continue;
            if (EntityList == IntPtr.Zero)
                continue;
            if (Read.Team(LocalPlayer) == Read.Team(EntityList))
                continue;
            if (Memory.Read<Int32>(EntityList + Offsets.Health) < 1)
                continue;
            if (Memory.Read<Boolean>((IntPtr)EntityList + Offsets.Dormant))
                continue;


            IntPtr Bonematrix = Memory.Read<IntPtr>(EntityList + Offsets.BoneMatrix);
            Vector3 BonePos = new Vector3(Memory.Read<float>((IntPtr)Bonematrix + 0x30 * 8 + 0xC), Memory.Read<float>((IntPtr)Bonematrix + 0x30 * 8 + 0x1C), Memory.Read<float>((IntPtr)Bonematrix + 0x30 * 8 + 0x2C));

            

            Vector3 EyePos = Read.EyePos(LocalPlayer);
            Vector3 orgin = Read.Orgin(LocalPlayer);
            Vector3 idk = orgin + EyePos;

            angles Location = CalcAngle(idk, BonePos);
            dist = Getdistance(BonePos, orgin);
            if (dist > 1600)
                continue;
            
            if(dist < Mindist)
            {
                curebest = i;
                Mindist = dist;
            }

        }
        return curebest;
    }
    public static IntPtr getclient()
    {
        
        IntPtr ClientDLL = Memory.GetModuleBaseAddress("client.dll");
        return ClientDLL;
    }
    public static IntPtr getengine()
    {
        IntPtr EngineDLL = Memory.GetModuleBaseAddress("engine.dll");
        return EngineDLL;
    }
    public static Vector3 EyePos(IntPtr Entity)
    {
        Vector3 Eyepos = Memory.Read<Vector3>(Entity + Offsets.VecViewOffset);
        return Eyepos;
    }
    public static Vector3 Orgin(IntPtr Entity)
    {
        Vector3 Orgin = Memory.Read<Vector3>(Entity + Offsets.VecOrgin);
        return Orgin;
    }
    public static int Team(IntPtr Entity)
    {
        int Team = Memory.Read<Int32>((IntPtr)Entity + Offsets.Team);
        return Team;
    }
   
    public static angles CalcAngle(Vector3 src, Vector3 dst)
    {
        angles angles;
        double[] delta = { (src.X - dst.X), (src.Y - dst.Y), (src.Z - dst.Z) };
        double hyp = Math.Sqrt(delta[0] * delta[0] + delta[1] * delta[1]);
        angles.X = (float)(Math.Asin(delta[2] / hyp) * (180.0f / Math.PI));
        angles.Y = (float)(Math.Atan(delta[1] / delta[0]) * (180.0f / Math.PI));

        if (delta[0] >= 0.0)
            angles.Y += 180.0f;

        return angles;
    }
   public static float Getdistance(Vector3 to, Vector3 from)
    {
        float deltaX = to.X - from.X;
        float deltaY = to.Y - from.Y;
        float deltaZ = to.Z - from.Z;

        return (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
    }
}
