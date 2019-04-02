using System;
using System.Linq;

using Commix.Sitecore.Diagnostics;

using Sitecore.Mvc.Presentation;

namespace Commix.Sitecore
{
    public class CommixViewModel<T>
    {
        public IJsonTracer Trace { get; set; }
        public RenderingModel Rendering { get; }
        public T View { get; }

        public CommixViewModel(RenderingModel rendering, T view)
        {
            Rendering = rendering ?? throw new ArgumentNullException(nameof(rendering));
            View = view;
        }
    }
}