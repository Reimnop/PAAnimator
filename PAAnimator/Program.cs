﻿using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;

namespace PAAnimator
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Window window = new Window(new GameWindowSettings(), new NativeWindowSettings()
            {
                APIVersion = new Version(4, 3),
                WindowBorder = WindowBorder.Resizable,
                API = ContextAPI.OpenGL,
                Flags = ContextFlags.ForwardCompatible,
                Profile = ContextProfile.Core,
                NumberOfSamples = 2,
                Size = new Vector2i(1600, 900),
                Title = "Project Arrhythmia Animator"
            });
            window.Run();
        }
    }
}
