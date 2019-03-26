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
                        viewData.Model = new CommixViewModel<T>(renderingModel, renderingModel.Item.As<T>(
                            (pipeline, context) =>
                            {
                                if (context.Monitor != null)
                                    tracer.Attach(context.Monitor);
                            }));

                        viewData["commixTrace"] = tracer.ToJson();
                    }
                }
                else
                {
                    viewData.Model = new CommixViewModel<T>(renderingModel, renderingModel.Item.As<T>());
                }
            }
            else
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
