﻿using System;
using SCVE.Editor.ProjectStructure;

namespace SCVE.Editor.Editing
{
    public class ImageClip : Clip
    {
        public Guid ReferencedImageGuid { get; private set; }

        public ImageAsset ReferencedImageAsset { get; private set; }
        
        private ImageClip(Guid guid, int startFrame, int frameLength, Guid referencedImageGuid) : base(guid, startFrame, frameLength)
        {
            ReferencedImageGuid = referencedImageGuid;
            ReferencedImageAsset = EditorApp.Instance.OpenedProject.RootFolder.FindAsset<ImageAsset>(referencedImageGuid);
        }

        public static ImageClip CreateNew(int startFrame, int frameLength, Guid referencedImageGuid)
        {
            return new(Guid.NewGuid(), startFrame, frameLength, referencedImageGuid);
        }

        public override string ShortName()
        {
            return $"Image {ReferencedImageAsset.InternalName}";
        }
    }
}