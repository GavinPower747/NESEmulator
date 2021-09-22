using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Metadata;
using Avalonia.Platform;
using Avalonia.Threading;

namespace NesEmu.Avalonia.Controls
{
    public class GameScreenControl : Control
    {
        public static readonly StyledProperty<IImage> SourceProperty =
            AvaloniaProperty.Register<Image, IImage>(nameof(Source));

        [Content]
        public IImage Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private readonly Stopwatch _st = Stopwatch.StartNew();

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);
        }

        protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromLogicalTree(e);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            context.DrawImage(Source,
                new Rect(0, 0, 640, 480),
                new Rect(256, 0, 640, 480));

            Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
        }
    }
}