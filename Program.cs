﻿using System;

namespace PhasmophobiaHelper
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Main())
                game.Run();
        }
    }
}
