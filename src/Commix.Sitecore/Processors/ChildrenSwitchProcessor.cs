﻿using System;
using System.Linq;

using Commix.Exceptions;
using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    public class ChildrenSwitchProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }

        public void Run(PropertyContext pipelineContext, PropertyProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    switch (pipelineContext.Context)
                    {
                        case Item parent:
                            pipelineContext.Context = parent.GetChildren();
                            break;
                        default:
                            pipelineContext.Faulted = true;
                            break;
                    }
                }
            }
            catch
            {
                pipelineContext.Faulted = true;
                throw;
            }
            finally
            {
                Next();
            }
        }
    }
}