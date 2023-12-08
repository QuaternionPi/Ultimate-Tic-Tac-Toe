using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    public static class Clipboard
    {
        public static string Text
        {
            get
            {
                unsafe
                {
                    sbyte* raw = Raylib.GetClipboardText();
                    return raw->ToString();
                }
            }
            set
            {
                Raylib.SetClipboardText(value);
            }
        }
    }
}