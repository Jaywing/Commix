using System;
using System.Collections.Generic;
using System.Linq;

using Commix.Exceptions;
using Commix.Pipeline.Property;
using Commix.Schema;

using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Commix.Sitecore.Processors
{
    public class PickerProcessor : IPropertyProcesser
    {
        public Action Next { get; set; }
        public void Run(PropertyContext pipelineContext, ProcessorSchema processorContext)
        {
            try
            {
                if (!pipelineContext.Faulted)
                {
                    switch (pipelineContext.Context)
                    {
                        case LookupField lookupField:
                            pipelineContext.Context = lookupField.TargetItem;
                            break;
                        case ReferenceField referenceField:
                            pipelineContext.Context = referenceField.TargetItem;
                            break;
                        case LinkField linkField:
                            pipelineContext.Context = linkField.TargetItem;
                            break;
                        case MultilistField treeField:
                            pipelineContext.Context = treeField.GetItems();
                            break;
                        case string stringValue:
                            pipelineContext.Context = Context.Item.Database.GetItem(stringValue);
                            break;
                        case ID idValue:
                            pipelineContext.Context = Context.Item.Database.GetItem(idValue);
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