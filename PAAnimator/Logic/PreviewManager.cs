using System;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ImGuiNET;
using System.Collections.Generic;
using System.Text;

namespace PAAnimator.Logic
{
    public static class PreviewManager
    {
        private static AudioSource audioSource;

        public static void Init()
        {
            Window.Main.Closing += Closing;
        }

        private static void Closing(System.ComponentModel.CancelEventArgs obj)
        {
            audioSource?.Dispose();
        }

        public static void LoadAudio(string path)
        {
            audioSource?.Dispose();

            audioSource = new AudioSource(path);
        }

        public static void Seek(float time)
        {
            audioSource?.Seek(time);
        }

        public static void Update()
        {
            if (audioSource == null)
                return;

            if (Input.GetKeyDown(Keys.Space))
            {
                if (!audioSource.IsPlaying)
                    audioSource.Play();
                else
                    audioSource.Stop();
            }

            if (audioSource.IsPlaying)
                ProjectManager.CurrentProject.Time = audioSource.GetPosition();
        }

        public static void RenderImGui()
        {
            if (ImGui.Begin("Music"))
            {
                if (audioSource == null)
                {
                    ImGui.Text("No audio file");
                    goto NoAudio;
                }

                Project prj = ProjectManager.CurrentProject;

                float t = prj.Time;
                float _t = t;
                ImGui.SliderFloat("Time", ref t, 0.0f, audioSource.GetLength());

                if (_t != t)
                    audioSource.Seek(t);

                ImGui.SameLine();

                if (ImGui.Button("Play"))
                    audioSource.Play();

                ImGui.SameLine();

                if (ImGui.Button("Stop"))
                    audioSource.Stop();

                NoAudio:
                ImGui.End();
            }
        }
    }
}
