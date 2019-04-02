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

using Sitecore_Context = Sitecore.Context;

namespace Commix.Sitecore
{
    public abstract class CommixView<T> : WebViewPage<CommixViewModel<T>>
    {
        protected override void SetViewData(ViewDataDictionary viewData)
        {
            if (viewData.Model is RenderingModel renderingModel)
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
            else if (Sitecore_Context.Item != null)
            {
                throw new InvalidOperationException($"CommixView expects base model Of RenderingModel, received: ${viewData.Model}");
            }

            base.SetViewData(viewData);
        }
    }

    public abstract class CommixView : WebViewPage
    {

    }
}
