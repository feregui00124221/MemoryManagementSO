namespace MemoryManagementExercise;

public class ProgramInstance
{
    private int _programId;
    private string _programName;
    private int _memoryUsage;

    public int ProgramId
    {
        get => _programId;
        set => _programId = value;
    }

    public string ProgramName
    {
        get => _programName;
        set => _programName = value;
    }

    public int MemoryUsage
    {
        get => _memoryUsage;
        set => _memoryUsage = value;
    }

    public ProgramInstance(int programId, string programName, int memoryUsage)
    {
        ProgramId = programId;
        ProgramName = programName;
        MemoryUsage = memoryUsage;
    }

    public ProgramInstance(string programName, int memoryUsage)
    {
        ProgramId = new Random().Next(1111,9999);
        ProgramName = programName;
        MemoryUsage = memoryUsage;
    }

    public ProgramInstance()
    {
        ProgramId = 0;
        ProgramName = "Default Program";
        MemoryUsage = 0;
    }

    ~ProgramInstance()
    {
        Console.WriteLine($"{ProgramName} with {ProgramId} instance destroyed");
    }

    public void ShowProgramInstance()
    {
        Console.WriteLine($"Program ID: {ProgramId}");
        Console.WriteLine($"Program Name: {ProgramName}");
        Console.WriteLine($"Memory Usage: {MemoryUsage}");
    }
}