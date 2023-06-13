using System.Runtime.InteropServices;

namespace Classes.DA
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct StructDAL
    {
        public static int UserId = 1;
        public static int empID;
        public static bool CheckAdd;
    }
}
