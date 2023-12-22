#region PART1

using System.ComponentModel.DataAnnotations;

string[] lines = File.ReadAllLines("input.sql");

List<Module> modules = new();
Queue<(Module sender, Module receiver, string signal)> queue = new();

foreach(var line in lines)
{
    string[] split = line.Split(" -> ");
    string moduleName = split[0];
    Module module = new(moduleName);
    modules.Add(module);
}

foreach (var line in lines)
{
    string[] split = line.Split(" -> ");
    string moduleName = split[0];
    string[] targets = split[1].Split(", ");
    bool hasFound = false;
    AssignTargets:
    foreach(Module module in modules)
    {
        if (module.Name.Equals(moduleName.Replace("%", "").Replace("&", "")))
        {
            foreach(Module target in modules)
            {
                if (targets.Contains(target.Name))
                {
                    hasFound = true;
                    module.Targets.Add(target);
                    if (target.Type.Equals("&"))
                    {
                        target.ConnectedModulesMemory.Add(module, "low");
                    }
                }
            }
            break;
        }
    }
    if (!hasFound)
    {
        foreach(string target in targets)
        {
            modules.Add(new Module(target));
        }
        goto AssignTargets;
    }
}

long lowSignal = 0;
long highSignal = 0;

int broadcasterIndex = modules.FindIndex(module => module.Name.Equals("broadcaster"));

for(int i = 0; i < 1000; i++)
{
    queue.Enqueue((new Module("%button"), modules[broadcasterIndex], "low"));

    while (queue.Count > 0)
    {
        (Module sender, Module receiver, string signal) = queue.Dequeue();
        if (signal.Equals("low"))
            lowSignal++;
        else
            highSignal++;
        receiver.ReceiveSignal(sender, signal, queue);
    }
}

Console.WriteLine(lowSignal*highSignal);

#endregion

#region PART2

long buttonPresses = 0;
modules.ForEach(module =>
{
    module.State = "off";
    foreach(var key in module.ConnectedModulesMemory.Keys)
    {
        module.ConnectedModulesMemory[key] = "low";
    }
});
Dictionary<string, long> cycles = new();

Module hasRXasTarget = modules.Find(m => m.Targets.Where(mo => mo.Name.Equals("rx")).Any());

int cyleModule = 0;

while (true)
{
    buttonPresses++;
    queue.Enqueue((new Module("%button"), modules[broadcasterIndex], "low"));

    while (queue.Count > 0 && cyleModule < 4)
    {
        (Module sender, Module receiver, string signal) = queue.Dequeue();
        receiver.ReceiveSignal(sender, signal, queue);
        if(receiver.Name.Equals(hasRXasTarget.Name) && signal.Equals("high"))
        {
            cycles[sender.Name] = buttonPresses;
            cyleModule++;
        }
    }
    if(cyleModule > 3)
    {
        break;
    }
}

Console.WriteLine(LCM(cycles.Values.ToArray()));

#endregion

#region LCM

static long LCM(long[] numbers)
{
    return numbers.Aggregate(lcm);
}
static long lcm(long a, long b)
{
    return Math.Abs(a * b) / GCD(a, b);
}
static long GCD(long a, long b)
{
    return b == 0 ? a : GCD(b, a % b);
}

#endregion

#region MODULE

public class Module
{
    public string Name { get; set; }
    public List<Module> Targets { get; set; }
    public string Type { get; set; }
    public string State { get; set; }
    public Dictionary<Module, string> ConnectedModulesMemory { get; set; }

    public Module(string name)
    {
        if (name.Contains('%') || name.Contains('&'))
        {
            Name = name[1..];
            Type = name[0].ToString();
        }
        else
        {
            Name = name;
            Type = name;
        }
        State = "off";
        Targets = new();
        ConnectedModulesMemory = new();
    }

    public void ReceiveSignal(Module m, string signal, Queue<(Module sender, Module receiver, string signal)> queue)
    {
        switch (Type)
        {
            case "broadcaster":
                foreach (Module module in Targets)
                    queue.Enqueue((this, module, signal));
                break;
            case "%":
                if (signal.Equals("low"))
                {
                    if (State.Equals("off"))
                    {
                        State = "on";
                        foreach (Module module in Targets)
                            queue.Enqueue((this, module, "high"));
                    }
                    else
                    {
                        State = "off";
                        foreach (Module module in Targets)
                            queue.Enqueue((this, module, "low"));
                    }
                }
                break;
            case "&":
                ConnectedModulesMemory[m] = signal;
                if(!ConnectedModulesMemory.Values.Where(sig => sig.Equals("high") == false).Any())
                {
                    State = "low";
                }
                else
                {
                    State = "high";
                }
                foreach (Module module in Targets)
                    queue.Enqueue((this, module, State));
                break;
        }
    }
}

#endregion