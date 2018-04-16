using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Sitecore.Mvc.Presentation;

namespace Commix.Sitecore
{
    public abstract class CommixView<T> : WebViewPage<CommixViewModel<T>>
    {
        protected override void SetViewData(ViewDataDictionary viewData)
        {
            if (viewData.Model is RenderingModel renderingModel)
                viewData.Model = new CommixViewModel<T>(renderingModel, renderingModel.Item.As<T>());
            else
                throw new InvalidOperationException($"CommixView expects base model Of RenderingModel, received: ${viewData.Model}");

            base.SetViewData(viewData);
        }
    }

    public abstract class CommixView : WebViewPage
    {

    }

    public class CommixViewModel<T>
    {
        public RenderingModel Rendering { get; }
        public T View { get; }

        public CommixViewModel(RenderingModel rendering, T view)
        {
            Rendering = rendering ?? throw new ArgumentNullException(nameof(rendering));
            View = view;
        }
    }
}
