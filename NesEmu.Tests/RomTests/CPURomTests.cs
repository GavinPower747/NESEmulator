using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NesEmu.Core;
using NesEmu.Devices.CPU;
using Xunit;

namespace NesEmu.Tets.RomTests;

public class RomTests
{
    private TestLoggingCpu _cpu;
    private IBus _cpuBus;

    private byte[] _romBytes;

    [Fact]
    public void NesEmu_Passes_NesTest()
    {
        _romBytes = new ArraySegment<byte>(
            File.ReadAllBytes("./Roms/nestest.nes"),
            16,
            16384
        ).ToArray();

        byte[] ramData = new byte[1024 * 64];

        Array.Copy(_romBytes, 0, ramData, 0x8000, _romBytes.Length);
        Array.Copy(_romBytes, 0, ramData, 0xC000, _romBytes.Length);

        _cpu = new TestLoggingCpu();
        _cpuBus = new TestBus(_cpu, ramData);

        _cpu.Reset();
        _cpu.Registers.ProgramCounter = 0xC000;
        _cpu.Registers.StatusRegister = new StatusRegister(0x24);

        _cpu.RunToEnd();

        var log = _cpu.Log;
        var logLines = log.ToString().Split(Environment.NewLine);

        // nestest.log has 5003 official opcode tests, then unofficial opcodes start at line 5004
        const int officialOpcodeTestCount = 5003;

        using var reader = new StreamReader("./Roms/nestest.expected.log");
        for (var i = 1; i <= officialOpcodeTestCount; i++)
        {
            var expectedLine = reader.ReadLine();
            CpuTestLogLine parsedExpectedLine;
            CpuTestLogLine actualLine;

            try
            {
                parsedExpectedLine = CpuTestLogLine.Parse(expectedLine);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse expected log line {i}: '{expectedLine}'", ex);
            }

            try
            {
                actualLine = CpuTestLogLine.Parse(logLines[i]);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Failed to parse actual log line {i}: '{logLines[i]}'\nExpected: '{expectedLine}'",
                    ex
                );
            }

            actualLine
                .Address.Should()
                .Be(
                    parsedExpectedLine.Address,
                    $"Address mismatch on line {i}\nExpected: {expectedLine}\nActual:   {logLines[i]}"
                );
            actualLine
                .Registers.Should()
                .BeEquivalentTo(
                    parsedExpectedLine.Registers,
                    $"Register mismatch on line {i}\nExpected: {expectedLine}\nActual:   {logLines[i]}"
                );
            actualLine
                .BytesRead.Should()
                .BeEquivalentTo(
                    parsedExpectedLine.BytesRead,
                    $"Bytes read mismatch on line {i}\nExpected: {expectedLine}\nActual:   {logLines[i]}"
                );
        }
    }

    internal class TestLoggingCpu : Cpu
    {
        public readonly LogBuilder Log = new();
        internal TestBus TestBus;

        public void RunToEnd()
        {
            try
            {
                int i = 0;
                do
                {
                    using (var logScope = Log.StartScope())
                    {
                        var instructionStart = Registers.ProgramCounter;
                        logScope.LogLine.Address = instructionStart;
                        logScope.LogLine.Registers = SnapshotRegisters();

                        TestBus.StartInstructionFetch(instructionStart);

                        do
                        {
                            Tick();
                        } while (!OpComplete());

                        i++;
                    }
                } while (Registers.ProgramCounter != 0xC66E && i < 10_000);
            }
            finally
            {
                LogBuilder.ActiveScope = null;
            }
        }

        private CpuRegisters SnapshotRegisters()
        {
            return new CpuRegisters
            {
                Accumulator = Registers.Accumulator,
                X = Registers.X,
                Y = Registers.Y,
                StackPointer = Registers.StackPointer,
                ProgramCounter = Registers.ProgramCounter,
                StatusRegister = new StatusRegister((byte)Registers.StatusRegister),
            };
        }
    }

    public class TestBus : IBus
    {
        private readonly Cpu _cpu;
        private readonly byte[] _memory;
        private ushort _nextExpectedFetchAddress;
        private bool _stillFetchingInstruction;

        internal TestBus(Cpu cpu, byte[] memory)
        {
            _cpu = cpu;
            _cpu.ConnectBus(this);
            _memory = memory;

            if (cpu is TestLoggingCpu loggingCpu)
                loggingCpu.TestBus = this;
        }

        internal void StartInstructionFetch(ushort startAddress)
        {
            _nextExpectedFetchAddress = startAddress;
            _stillFetchingInstruction = true;
        }

        public byte ReadByte(ushort address)
        {
            var logScope = LogBuilder.ActiveScope;
            var readData = _memory[address];

            // Only log consecutive reads starting from instruction address
            // Once we read from elsewhere, instruction fetch is complete
            if (_stillFetchingInstruction && address == _nextExpectedFetchAddress)
            {
                logScope?.LogLine.BytesRead.Add(readData);
                _nextExpectedFetchAddress++;
            }
            else
            {
                _stillFetchingInstruction = false;
            }

            return readData;
        }

        public ushort ReadWord(ushort address)
        {
            var logScope = LogBuilder.ActiveScope;
            var lo = (ushort)_memory[address];
            var hi = (ushort)_memory[(ushort)(address + 1)];

            // Only log if this is consecutive instruction fetch
            if (_stillFetchingInstruction && address == _nextExpectedFetchAddress)
            {
                logScope?.LogLine.BytesRead.AddRange(new[] { (byte)lo, (byte)hi });
                _nextExpectedFetchAddress += 2;
            }
            else
            {
                _stillFetchingInstruction = false;
            }

            return (ushort)(hi << 8 | lo);
        }

        public void Write(ushort address, byte data)
        {
            _memory[address] = data;
        }

        public void ConnectDevice(IAddressableDevice device)
        {
            //We don't really use this for testing
        }
    }

    public class LogBuilder
    {
        public static LogScope ActiveScope;

        private readonly StringBuilder _log = new();

        public LogScope StartScope()
        {
            ActiveScope = new LogScope(_log);
            return ActiveScope;
        }

        public override string ToString()
        {
            return _log.ToString();
        }
    }

    public class LogScope(StringBuilder log) : IDisposable
    {
        public readonly CpuTestLogLine LogLine = new();

        private readonly StringBuilder _log = log;

        public void Dispose()
        {
            _log.AppendLine(LogLine.ToString());
            LogBuilder.ActiveScope = null;
        }
    }

    public class CpuTestLogLine
    {
        public int Address { get; set; }
        public CpuRegisters Registers { get; set; }
        public List<byte> BytesRead { get; set; } = [];
        public string Instruction { get; set; }

        private const string OverallFormatString = "{0,-6:X4}{1,-10}{2,-32}{3}";
        private const string RegistersFormatString =
            "A:{0:X2} X:{1:X2} Y:{2:X2} P:{3:X2} SP:{4:X2} CYC:{5,3:X2} SL:{6,-3}";

        public override string ToString()
        {
            string registers = string.Format(
                RegistersFormatString,
                Registers.Accumulator,
                Registers.X,
                Registers.Y,
                (byte)Registers.StatusRegister,
                Registers.StackPointer,
                0,
                0
            );
            string data = string.Join(" ", BytesRead.Select(x => x.ToString("X2")));

            return string.Format(OverallFormatString, Address, data, Instruction, registers);
        }

        public static CpuTestLogLine Parse(string logLine) =>
            new()
            {
                Address = int.Parse(
                    logLine[..6].Trim(),
                    System.Globalization.NumberStyles.HexNumber
                ),
                BytesRead =
                [
                    .. logLine[6..16]
                        .Trim()
                        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => byte.Parse(x, System.Globalization.NumberStyles.HexNumber)),
                ],
                Instruction = logLine[16..48].Trim(),
                Registers = new CpuRegisters
                {
                    Accumulator = byte.Parse(
                        logLine[50..52].Trim(),
                        System.Globalization.NumberStyles.HexNumber
                    ),
                    X = byte.Parse(
                        logLine[55..57].Trim(),
                        System.Globalization.NumberStyles.HexNumber
                    ),
                    Y = byte.Parse(
                        logLine[60..62].Trim(),
                        System.Globalization.NumberStyles.HexNumber
                    ),
                    StatusRegister = new StatusRegister(
                        byte.Parse(
                            logLine[65..67].Trim(),
                            System.Globalization.NumberStyles.HexNumber
                        )
                    ),
                    StackPointer = byte.Parse(
                        logLine[71..73].Trim(),
                        System.Globalization.NumberStyles.HexNumber
                    ),
                },
            };
    }
}
