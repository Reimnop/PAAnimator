﻿using System;
using System.Collections.Generic;

namespace PAAnimator.Logic
{
    public static class UndoManager
    {
        private static Stack<Action> undoStack = new Stack<Action>();

        public static void PushUndo(Action func)
        {
            undoStack.Push(func);
        }

        public static void Undo()
        {
            if (undoStack.Count == 0)
                return;

            undoStack.Pop().Invoke();
        }
    }
}
