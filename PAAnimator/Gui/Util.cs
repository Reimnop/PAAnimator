﻿using OpenTK.Graphics.OpenGL4;
using System;

namespace PAAnimator.Gui
{
    static class Util
    {
        public static void CheckGLError(string title)
        {
            var error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                Console.WriteLine($"{title}: {error}");
            }
        }
    }
}
