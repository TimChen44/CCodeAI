using Microsoft.VisualStudio.Imaging;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            return Task.FromResult<FrameworkElement>(new CCodeExplainWindowControl());
        }

        private System.Reflection.Assembly CurrentDomain_AssemblyResolve(
            object sender, 
            ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name + ".dll";
            var extensionLocation = Path.GetDirectoryName(typeof(CCodeExplainWindow).Assembly.Location);

            try
            {

                var file = Path.Combine(extensionLocation, assemblyName);

                if (File.Exists(file))
                {
                    return Assembly.LoadFrom(file);
                }
                else
                {
                    Debug.Print(file);
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("The location of the assembly, {0} could not be resolved for loading.",
                    extensionLocation), ex);
            }
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
