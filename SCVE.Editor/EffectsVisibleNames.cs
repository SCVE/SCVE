﻿using System;
using System.Collections.Generic;
using SCVE.Editor.Editing.Effects;

namespace SCVE.Editor
{
    public class EffectsVisibleNames
    {
        public static Dictionary<Type, string> Names = new()
        {
            { typeof(TranslateEffect), "Translate" },
            { typeof(ScaleEffect), "Scale" },
        };
    }
}