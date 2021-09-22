using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.Metadata;
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

            context.DrawImage(Source, new Rect(new Size(1920, 1080)));

            Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Background);
        }
    }
}