using Microsoft.VisualStudio.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CCodeAI
{
    public class CCodeExplainWindow : BaseToolWindow<CCodeExplainWindow>
    {
        public override string GetTitle(int toolWindowId) => "CCodeAI";

        public override Type PaneType => typeof(Pane);

        public override Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            return Task.FromResult<FrameworkElement>(new CCodeExplainWindowControl());
        }

        [Guid("937a7d1d-d10f-4843-80ed-f668ac49cb68")]
        internal class Pane : ToolWindowPane
        {
            public Pane()
            {
                BitmapImageMoniker = KnownMonikers.ToolWindow;
            }
        }
    }
}
