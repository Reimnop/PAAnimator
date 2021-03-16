using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace PAAnimator
{
    public static class Helper
    {
        public static Vector2 Bezier(Vector2[] controls, float val)
        {
            float t = MathF.Pow(val, 1.0f / (controls.Length - 1));

            float a = controls.Length - 1.0f;
            Vector2 output = Vector2.Zero;
            for (int i = 0; i < controls.Length; i++)
            {
                Vector2 v = MathF.Pow(1.0f - t, controls.Length - i - 1.0f) * MathF.Pow(t, i) * controls[i];
                if (i != 0 && i != controls.Length - 1)
                    v *= a;
                output += v;
            }
            return output;
        }
    }
}
