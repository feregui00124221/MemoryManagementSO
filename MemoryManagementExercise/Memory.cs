using System.Collections;

namespace MemoryManagementExercise;

public class Memory
{
    private int _memoryId;
    private Page[] _frame;
    private HashSet<Process> _processTable;

    public Memory(int memoryId)
    {
        MemoryId = memoryId;
        ProcessTable = new HashSet<Process?>();
        Frame = new Page[10];

        for (int i = 0; i < Frame.Length; i++)
        {
            Frame[i] = new Page(); // Instantiate each Page object
            Frame[i].PageId = i*100; // Set PageId
        }
    }

    public int MemoryId
    {
        get => _memoryId;
        set => _memoryId = value;
    }

    public Page[] Frame
    {
        get => _frame;
        set => _frame = value;
    }

    public HashSet<Process?> ProcessTable
    {
        get => _processTable;
        set => _processTable = value;
    }

    ~Memory()
    {
        Frame.Initialize();
        ProcessTable.Clear();
        Console.WriteLine($"Memory {MemoryId} has been destroyed.");
    }

    public void ShowMemory()
    {
        Console.WriteLine($"---\nMemory ID: {MemoryId}");

        Console.WriteLine("\nProcess Table:");
        if (ProcessTable.Count > 0)
        {
            foreach (var process in ProcessTable) process?.ShowProcess();
        }
        else
        {
            Console.WriteLine("No processes in memory.");
        }

        Console.WriteLine("\nMemory framing:");
        foreach (var page in Frame) page.ShowPage();
    }

    public void ShowMemorySegment(int start, int end)
    {
        Console.WriteLine("\nMemory framing:");
        for (var i = start; i <= end; i++) Frame[i].ShowPage();
    }

    private bool DoesProcessExists(string programName)
    {
        return ProcessTable.Any(process => process?.ProgramInstance.ProgramName == programName);
    }

    public Process FindProcess(int processId)
    {
        return ProcessTable.First(process => process?.ProcessId == processId)!;
    }

    public void RegisterNewProcess(ProgramInstance? programInstance, int initialPosition)
    {
        if (DoesProcessExists(programInstance.ProgramName))
        {
            Console.WriteLine("Process already exists in memory.");
            return;
        }

        var newProcess = new Process(new Random().Next(1111,9999), programInstance, initialPosition);

        try
        {
            ProcessTable.Add(newProcess);
            PutProcessInFrameMemory(newProcess);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            Console.WriteLine("Would you like to try allocating memory in any free available position? [Y/N]");
            var choice = Console.ReadLine();

            if (choice == "Y")
            {
                try
                {
                    AllocateProcessMemory(newProcess);
                    Console.WriteLine($"Process registered and memory allocated successfully.");
                }catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    return;
                }
            }
            else
            {
                ProcessTable.Remove(newProcess);
                Console.WriteLine("Process could not be registered and memory could not be allocated.");
            }

            return;
        }

        Console.WriteLine("Process registered and memory allocated successfully.");
    }

    public void PutProcessInFrameMemory(Process process)
    {
        var processSize = process.ProgramInstance.MemoryUsage;
        var processPages = processSize / 100; // Each page has 100 megabytes of memory.
        var processLocation = process.InitPosition / 100;

        for (int i = processLocation; i <= processPages; i++)
        {
            if (Frame[i].IsOccupied)
            {
                throw new Exception("Memory allocation position or space is not available.");
            }
        }

        for (int i = processLocation; i <= processPages; i++)
        {
            Frame[i].ProcessId = process.ProcessId;
            Frame[i].IsOccupied = true;
        }
    }

    public void AllocateProcessMemory(Process process)
    {
        var processSize = process.ProgramInstance.MemoryUsage;
        var processPages = processSize / 100; // Each page has 100 megabytes of memory.

        // Memory allocation must check position in which the process is trying to be allocated. If there are enough
        // free continuous pages, the process will be allocated. Otherwise, the process will be allocated in the first
        // available position with enough continuous free pages.
        var freePages = 0;
        var firstFreePage = -1;
        for (int i = 0; i < Frame.Length; i++)
        {
            if (Frame[i].IsOccupied)
            {
                freePages = 0;
                firstFreePage = -1;
                Process occupyingProcess = FindProcess(Frame[i].ProcessId);
                int initPosOccupyingProcess = occupyingProcess.InitPosition / 100;
                int occupyingProcessPages = occupyingProcess.ProgramInstance.MemoryUsage / 100;
                i = initPosOccupyingProcess + occupyingProcessPages;
            }

            if (freePages == 0) firstFreePage = i;

            freePages++;

            if (freePages == processPages) break;
        }

        for (int i = firstFreePage; i < firstFreePage + processPages; i++)
        {
            if (i == firstFreePage)
            {
                Process reformatingProcess = FindProcess(process.ProcessId);
                reformatingProcess.InitPosition = i * 100;
            }

            Frame[i].ProcessId = process.ProcessId;
            Frame[i].IsOccupied = true;
        }
    }

    //  To destroy a process, said process will be freed from Frame,leaving those spaces in blank, and then removed from ProcessTable.
    public void FreeProcess(string programName)
    {
        if (!DoesProcessExists(programName))
        {
            Console.WriteLine("Process does not exist in memory.");
            return;
        }

        var process = ProcessTable.First(process => process?.ProgramInstance?.ProgramName == programName)!;
        var processPages = process.ProgramInstance?.MemoryUsage / 100;
        int i = 0;

        foreach (var t in Frame)
        {
            if (i == processPages) break;
            if (t.ProcessId != process.ProcessId) continue;
            t.ProcessId = 0;
            t.IsOccupied = false;
            i++;
        }

        ProcessTable.Remove(process);
        Console.WriteLine("Process freed from memory.");
    }
}