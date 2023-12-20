using System.Diagnostics.Contracts;
using AdventOfCode.Interfaces;

public class Day20 : IDay
{
    string[] lines;

    public static Queue<(string? Sender, string Receiver, bool Pulse)> MessageQueue { get; set; } = new Queue<(string?, string, bool)>();

    public Day20(string path)
    {
        lines = File.ReadAllLines(path);
    }

    public object SolveOne()
    {
        Machine machine = new Machine(lines);

        long highPulses = 0;
        long lowPulses = 0;

        for (int i = 0; i < 1000; i++)
        {
            MessageQueue.Enqueue((null, "broadcaster", false));

            while (MessageQueue.Count > 0)
            {
                var message = MessageQueue.Dequeue();
                
                if (message.Pulse)
                    highPulses++;
                else
                    lowPulses++;

                try
                {
                    machine.Modules[message.Receiver].ReceivePulse(message.Sender == null ? null : machine.Modules[message.Sender], message.Pulse);
                }
                catch (RxEception)
                {
                    continue;
                }
            }
        }

        return highPulses * lowPulses;
    }

    public object SolveTwo()
    {
        Machine machine = new Machine(lines);

        long highPulses = 0;
        long lowPulses = 0;

        long buttonPresses = 0;

        for (long i = 1; i < long.MaxValue; i++)
        {
            MessageQueue.Enqueue((null, "broadcaster", false));

            while (MessageQueue.Count > 0)
            {
                var message = MessageQueue.Dequeue();
                
                if (message.Pulse)
                    highPulses++;
                else
                    lowPulses++;

                try
                {
                    machine.Modules[message.Receiver].ReceivePulse(message.Sender == null ? null : machine.Modules[message.Sender], message.Pulse);
                }
                catch (RxEception)
                {
                    buttonPresses = i;
                }
            }

            if (buttonPresses > 0)
                break;
        }

        return buttonPresses;
    }

    public class Machine
    {
        public Dictionary<string, Module> Modules { get; } = new Dictionary<string, Module>();

        public Machine(string[] lines)
        {
            Dictionary<string, string[]> configuration = new Dictionary<string, string[]>();

            foreach (var line in lines)
            {
                string[] parts = line.Split(" -> ");
                string[] destinations = parts[1].Replace(" ", "").Split(",");

                configuration[parts[0]] = destinations;
            }
            
            foreach ((string name, string[] destinations) in configuration)
            {
                string nameWithoutPrefix = (name == "broadcaster" || name == "output" || name == "rx") ? name : name.Substring(1);

                if (!Modules.ContainsKey(nameWithoutPrefix))
                    Modules[nameWithoutPrefix] = CreateModule(name);

                foreach (var destination in destinations)
                {
                    if (!Modules.ContainsKey(destination))
                    {
                        if (destination == "output" || destination == "rx")
                            Modules[destination] = CreateModule(destination);
                        else
                            Modules[destination] = CreateModule(configuration.First(c => c.Key.Substring(1) == destination).Key);
                    }

                    Modules[nameWithoutPrefix].Destinations.Add(Modules[destination]);
                    Modules[destination].Inputs.Add(Modules[nameWithoutPrefix]);
                }
            }
        }

        private Module CreateModule(string name)
        {
            if (name == "broadcaster")
            {
                return new BroadcastModule(name);
            }
            else if (name == "output")
            {
                return new OutputModule(name);
            }
            else if (name == "rx")
            {
                return new RxModule(name);
            }
            else if (name.StartsWith("%"))
            {
                return new FlipFlopModule(name.Substring(1));
            }
            else
            {
                return new ConjunctionModule(name.Substring(1));
            }
        }
    }

    public abstract class Module
    {
        public string Name { get; }
        public List<Module> Inputs { get; set; } = new List<Module>();
        public List<Module> Destinations { get; set; } = new List<Module>();

        public Module(string name)
        {
            Name = name;
        }

        public void ReceivePulse(Module? sender, bool pulse)
        {
            ReceivePulseInner(sender, pulse);
        }

        protected abstract void ReceivePulseInner(Module? sender, bool pulse);
    }

    // Prefix: %
    public class FlipFlopModule : Module
    {
        public bool IsOn { get; protected set; } = false;

        public FlipFlopModule(string name) : base(name)
        {
            IsOn = false;
        }

        protected override void ReceivePulseInner(Module? sender, bool pulse)
        {
            if (pulse)
                return;
            
            IsOn = !IsOn;            
            foreach (var destination in Destinations)
                MessageQueue.Enqueue((Name, destination.Name, IsOn));
        }
    }

    // Prefix: &
    public class ConjunctionModule : Module
    {
        public Dictionary<string, bool> ReceivedPulses { get; } = new Dictionary<string, bool>();

        public ConjunctionModule(string name) : base(name)
        {
        }

        protected override void ReceivePulseInner(Module? sender, bool pulse)
        {           
            ReceivedPulses[sender.Name] = pulse;

            if (Inputs.All(i => ReceivedPulses.ContainsKey(i.Name) && ReceivedPulses[i.Name]))
                foreach (var destination in Destinations)                    
                    MessageQueue.Enqueue((Name, destination.Name, false));
            else
                foreach (var destination in Destinations)
                    MessageQueue.Enqueue((Name, destination.Name, true));

        }
    }

    // Name: broadcaster
    public class BroadcastModule : Module
    {
        public BroadcastModule(string name) : base(name)
        {
        }

        protected override void ReceivePulseInner(Module? sender, bool pulse)
        {
            foreach (var destination in Destinations)
                MessageQueue.Enqueue((Name, destination.Name, pulse));
        }        
    }

    // Name: output
    public class OutputModule : Module
    {
        public OutputModule(string name) : base(name)
        {
        }

        protected override void ReceivePulseInner(Module? sender, bool pulse)
        {
        }        
    }
    
    // Name: rx
    public class RxModule : Module
    {
        public RxModule(string name) : base(name)
        {
        }

        protected override void ReceivePulseInner(Module? sender, bool pulse)
        {
            if (!pulse)
                throw new RxEception("Rx module reached.");
        }        
    }
}

public class RxEception : Exception
{
    public RxEception(string message) : base(message)
    {
    }
}