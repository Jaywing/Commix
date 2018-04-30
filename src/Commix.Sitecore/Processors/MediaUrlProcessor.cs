﻿using System;
using System.Linq;

using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;

namespace Commix.Sitecore.Processors
{
    public class MediaUrlProcessor : IPropertyProcesser
    {
        public static string Width = $"{typeof(MediaUrlProcessor).Name}Width";
        public static string Height = $"{typeof(MediaUrlProcessor).Name}Height";
        
        public Action Next { get; set; }
        
        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            var mediaUrlOptions = new MediaUrlOptions();

            if (processorContext.Options.ContainsKey(Width)
                && int.TryParse(processorContext.Options[Width].ToString(), out int width))
                mediaUrlOptions.Width = width;

            if (processorContext.Options.ContainsKey(Height) 
                && int.TryParse(processorContext.Options[Height].ToString(), out int height))
                mediaUrlOptions.Height = height;
            
            switch (pipelineContext.Value)
            {
                case ImageField imageField when imageField.MediaItem != null:
                    pipelineContext.Value = HashingUtils.ProtectAssetUrl(MediaManager.GetMediaUrl(imageField.MediaItem, mediaUrlOptions));
                    break;
                case MediaItem mediaItem:
                    pipelineContext.Value = HashingUtils.ProtectAssetUrl(MediaManager.GetMediaUrl(mediaItem,mediaUrlOptions));
                    break;
            }

            Next();
        }
    }
}