using MemoryManagementExercise;
using static System.Convert;

Console.WriteLine("---\nWelcome to the C# memory management demo!\n---");

var memory = new Memory(0);

Console.WriteLine("Memory object created with id value 0");

bool replayMenu = true;

while (replayMenu)
{
    Console.WriteLine("---\nMemory Management Menu\n---");
    Console.WriteLine("1. Print memory state");
    Console.WriteLine("2. Print a memory segment");
    Console.WriteLine("3. Create a new process");
    Console.WriteLine("4. Destroy an existing process");
    Console.WriteLine("5. Destroy memory and exit");
    Console.Write("Choose an option: ");
    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            memory.ShowMemory();
            break;
        case "2":
            int start, end;
            do
            {
                Console.Write("Enter start index between 0 and 127: ");
                start = ToInt32(Console.ReadLine() ?? string.Empty);
            } while (start is < 0 or > 127);

            do
            {
                Console.Write($"Enter end index between {start} and 127: ");
                end = ToInt32(Console.ReadLine() ?? string.Empty);
            } while (end <= start || end > 127);
            
            memory.ShowMemorySegment(start, end);
            break;
        case "3":
            Console.Write("Enter process name: ");
            var processName = Console.ReadLine();

            int memoryUsage;

            do
            {
                Console.Write("Enter memory usage [MB]: ");
                memoryUsage = int.Parse(Console.ReadLine() ?? string.Empty);
            } while (memoryUsage <= 0 || memoryUsage % 100 != 0);

            int initialPosition;

            do
            {
                Console.Write("Enter initial position [Must be 100 leaps]: ");
                initialPosition = int.Parse(Console.ReadLine() ?? string.Empty);
            }while(initialPosition % 100 != 0);

            if (processName != null)
            {
                var programInstance = new ProgramInstance(processName, memoryUsage);
                memory.RegisterNewProcess(programInstance, initialPosition);
            }

            break;
        case "4":
            Console.Write("Enter process name to destroy: ");
            var processToDestroy = Console.ReadLine();
            if (processToDestroy != null) memory.FreeProcess(processToDestroy);
            break;
        case "5":
            Console.WriteLine("Exiting program and destroying memory.");
            replayMenu = false;
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}

GC.Collect();
GC.WaitForPendingFinalizers();
Console.WriteLine("Memory has been susccessfully destroyed.");
return 0;