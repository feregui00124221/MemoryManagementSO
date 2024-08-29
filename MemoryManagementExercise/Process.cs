namespace MemoryManagementExercise;

public class Process
{
    private int _processId;
    private ProgramInstance _programInstance;
    private int _initPosition;

    public Process(int processId, ProgramInstance programInstance, int initPosition)
    {
        _programInstance = programInstance;
        _processId = processId;
        _initPosition = initPosition;
    }

    public Process() : this(0, new ProgramInstance(), 0)
    {
    }

    public int ProcessId
    {
        get => _processId;
        set => _processId = value;
    }

    public ProgramInstance ProgramInstance 
    { 
        get => _programInstance;
        set => _programInstance = value; 
    } 

    public int InitPosition
    {
        get => _initPosition;
        set => _initPosition = value;
    }

    ~Process()
    {
        ProgramInstance = null!;
        Console.WriteLine($"Process {ProcessId} has been destroyed.");
    }

    public void ShowProcess()
    {
        Console.WriteLine($"---\nProcess ID: {ProcessId}");
        Console.WriteLine("Program Instance:");
        ProgramInstance.ShowProgramInstance();
        Console.WriteLine($"Initial Position: {InitPosition}\n---");
    }
}