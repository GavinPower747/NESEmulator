using System;
using NesEmu.Core;
using ReactiveUI;
using Avalonia.Media.Imaging;
using Avalonia;
using Avalonia.Platform;
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
        _nes.PPU.FrameReady += OnFrameReady;

        _screen = new WriteableBitmap(new PixelSize(256, 240), new Vector(96, 96), PixelFormat.Bgra8888, AlphaFormat.Unpremul);

        this.RaisePropertyChanged(nameof(Screen));

        _clockThread = new Thread(Clock);
        _clockThread.Start();
    }

    public void Clock()
    {
        while (true)
        {
            _nes.PPU.Tick();
        }
    }

    public unsafe void OnFrameReady(byte[] frameData)
    {
        using (var l = Screen.Lock())
        {
            var bitmapBuffer = (byte*)l.Address.ToPointer();

            for (var i = 0; i < frameData.Length; i++)
            {
                bitmapBuffer[i] = frameData[i];
            }
        }
    }
}
