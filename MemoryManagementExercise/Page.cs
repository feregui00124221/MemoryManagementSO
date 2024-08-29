namespace MemoryManagementExercise;

public class Page(int pageId, int processId)
{
    public Page() : this(0, 0)
    {
    }

    public int PageId { get; set; } = pageId;

    public int ProcessId { get; set; } = processId;

    public bool IsOccupied { get; set; } = false;

    ~Page()
    {
        Console.WriteLine($"Page {PageId} has been destroyed.");
    }

    public void ShowPage()
    {
        Console.WriteLine($"---\nPage ID: {PageId}");
        Console.WriteLine($"Process ID: {ProcessId}");
        Console.WriteLine($"Is Occupied: {IsOccupied}\n---");
    }
}