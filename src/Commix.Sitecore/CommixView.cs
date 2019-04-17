using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Commix.Diagnostics;
using Commix.Sitecore.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;

namespace Commix.Sitecore
{
    public abstract class CommixView<T> : WebViewPage<CommixViewModel<T>>
    {
        protected override void SetViewData(ViewDataDictionary viewData)
        {
            switch (viewData.Model)
            {
                case RenderingModel pipelineRenderingModel:
                    SetFromRenderingModel(viewData, pipelineRenderingModel);
                    break;
                case T mappedModel:
                {
                    RenderingModel renderingModel = null;
                    if (RenderingContext.Current?.Rendering != null)
                    {
                        renderingModel = new RenderingModel();
                        renderingModel.Initialize(RenderingContext.Current.Rendering);
                    }

                    viewData.Model = new CommixViewModel<T>(renderingModel, mappedModel);
                    break;
                }
                default:
                {
                    if (RenderingContext.Current?.Rendering != null)
                    {
                        var renderingModel = new RenderingModel();
                        renderingModel.Initialize(RenderingContext.Current.Rendering);
                        SetFromRenderingModel(viewData, renderingModel);
                    }
                    else
                    {
                        throw new InvalidOperationException($"CommixView expects base model Of RenderingModel, received: ${viewData.Model}");
                    }

                    break;
                }
            }

            base.SetViewData(viewData);
        }

        private static void SetFromRenderingModel(ViewDataDictionary viewData, RenderingModel renderingModel)
        {
            IJsonTracer tracer = null;

            var commixOptions = ServiceLocator.ServiceProvider?.GetService<CommixOptions>();

            if (commixOptions != null && commixOptions.Diagnostics)
                tracer = ServiceLocator.ServiceProvider?.GetService<IJsonTracer>();

            if (tracer != null)
            {
                using (tracer)
                {
                    // If we are running diagnostics attach a trace to the pipeline monitor

                    T mappedModel = renderingModel.Item.As<T>(
                        (pipeline, context) =>
                        {
                            if (context.Monitor != null)
                                tracer.Attach(context.Monitor);
                        });

                    viewData.Model = new CommixViewModel<T>(renderingModel, mappedModel)
                    {
                        Trace = tracer
                    };
                }
            }
            else
            {
                viewData.Model = new CommixViewModel<T>(renderingModel, renderingModel.Item.As<T>());
            }
        }
    }

    public abstract class CommixView : WebViewPage
    {

    }
}
