using System;
using NesEmu.Core;
using ReactiveUI;
using Avalonia.Media.Imaging;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Media;
using System.Threading;

namespace NesEmu.Avalonia.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public WriteableBitmap Screen
    {
        get => _screen;
        set => this.RaiseAndSetIfChanged(ref _screen, value);
    }

    private WriteableBitmap _screen;
    private readonly NintendoEntertainmentSystem _nes;
    private Thread _clockThread;

    public MainWindowViewModel(NintendoEntertainmentSystem nes)
    {
        _nes = nes ?? throw new ArgumentNullException("nes");

        _screen = new WriteableBitmap(new PixelSize(256, 240), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Unpremul);

        this.RaisePropertyChanged(nameof(Screen));

        _nes.FrameReady += OnFrameReady;

        _clockThread = new Thread(Clock);
        _clockThread.Start();
    }

    private void Clock()
    {
        while (true)
        {
            _nes.Step();
        }
    }

    public unsafe void OnFrameReady(object? sender, byte[] frameData)
    {
        using (ILockedFramebuffer buffer = _screen.Lock())
        {
            for (int y = 0; y < 240; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    var pixel = frameData[y * 256 + x];
                    SetPixel(buffer, x, y, new Color(255, pixel, pixel, pixel));
                }
            }
        }

        void SetPixel(ILockedFramebuffer buffer, int x, int y, Color colour)
        {
            var pixel = GetPixel(buffer, x, y);
            var alpha = colour.A / 255;

            pixel[0] = (byte)(colour.B * alpha);
            pixel[1] = (byte)(colour.G * alpha);
            pixel[2] = (byte)(colour.R * alpha);
            pixel[3] = colour.A;
        }

        Span<byte> GetPixel(ILockedFramebuffer buffer, int x, int y)
        {
            var bytesPerPixel = 4; // BGRA8888
            
            var zero = (byte*)buffer.Address;
            var offset = y * buffer.RowBytes + x * bytesPerPixel;

            return new Span<byte>(zero + offset, bytesPerPixel);
        }
    }
}
