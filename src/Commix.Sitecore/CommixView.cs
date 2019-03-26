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
                var commixOptions = ServiceLocator.ServiceProvider?.GetService<CommixOptions>();
                if (commixOptions != null)
                {
                    if (commixOptions.Diagnostics)
                    {
                        var jsonTracer = ServiceLocator.ServiceProvider?.GetService<IJsonTracer>();
                        if (jsonTracer != null)
                        {
                            viewData.Model = new CommixViewModel<T>(renderingModel, renderingModel.Item.As<T>(
                                (pipeline, context) =>
                                {
                                    if (context.Monitor != null)
                                        jsonTracer.Attach(context.Monitor);
                                }));

                            viewData["commixTrace"] = jsonTracer.ToJson();
                        }
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
