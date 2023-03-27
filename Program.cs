using System.Diagnostics;
using Newtonsoft.Json;
using Solis.Output;
using Solis.Filtering;

namespace Solis
{
    internal class Program
    {
        private Settings? _settings;
        private Root? _deserialized = new();
        private readonly List<string> _files = new();
        private readonly List<Message> _list = new();
        private readonly Dataset _dataset = new()
        {
            dataset = new List<Instructions>()
        };

        private static Task Main() => new Program().MainAsync();
        private async Task MainAsync()
        {
            Console.Title = "Solis";
            await Setup();

            var stopwatch = Stopwatch.StartNew();
            await Deserialization();
            var elapsed1 = $"{stopwatch.Elapsed:hh\\:mm\\:ss}";
            
            
            stopwatch.Restart();
            await Sorting();
            var elapsed2 = $"{stopwatch.Elapsed:hh\\:mm\\:ss}";

            stopwatch.Restart();
            await new FilterHandler(_dataset.dataset, _settings).FilterMessages();
            var elapsed3 = $"{stopwatch.Elapsed:hh\\:mm\\:ss}";

            stopwatch.Restart();
            await Serialization();
            var elapsed4 = $"{stopwatch.Elapsed:hh\\:mm\\:ss}";

            stopwatch.Stop();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Number of items in dataset: {_dataset.dataset.Count}");
            Console.WriteLine($"Deserialization elapsed: {elapsed1}");
            Console.WriteLine($"Sorting elapsed: {elapsed2}");
            Console.WriteLine($"Filtering elapsed: {elapsed3}");
            Console.WriteLine($"Serialization elapsed: {elapsed4}\n");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Press any key to exit.");
            Console.ResetColor();
            Console.ReadKey();
        }

        private async Task Setup()
        {
            using var sr = new StreamReader("Settings.json");
            await using var reader = new JsonTextReader(sr);
            var serializer = new JsonSerializer();

            _settings = serializer.Deserialize<Settings>(reader);

            sr.Dispose();
            reader.Close();

            Console.ForegroundColor = ConsoleColor.Yellow;
            if(_settings == null)
                Console.WriteLine("Warning: Settings null, using default parameters.");

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Provide the directory of the JSON files. Press");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(" ESC ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("when you're done.");
            Console.ResetColor();

            while (true)
            {
                _files.Add(Console.ReadLine() ?? throw new InvalidOperationException());

                if (Console.ReadKey().Key == ConsoleKey.Escape)
                    break;
            }
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            await Task.CompletedTask;
        }

        private async Task Deserialization()
        {
            Console.WriteLine("Deserializing...");

            foreach (var chat in _files)
            {
                using var sr = new StreamReader(chat);
                await using var reader = new JsonTextReader(sr);
                var serializer = new JsonSerializer();

                _deserialized = serializer.Deserialize<Root>(reader);

                if (_deserialized == null) continue;
                
                foreach (var item in _deserialized.messages.Where(item => item.type is "Default" or "Reply" && !item.author.isBot))
                    _list.Add(item);
            }
            

            Console.WriteLine("Deserialization done!");
            await Task.CompletedTask;
        }

        private async Task Sorting()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: This might take several minutes.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sorting...");

            var query =
                from item in _list.AsParallel().AsOrdered()
                select item;

            query.ForAll(item =>
            {
                Instructions instructions = new();

                var a = _list.Find(x => x.id.Equals(item.reference?.messageId));

                if (a?.content == null) return;

                instructions.instruction = a.content;
                instructions.output = item.content;
                _dataset.dataset.Add(instructions);
            });

            Console.WriteLine("Sorting done!");
            await Task.CompletedTask;
        }

        private async Task Serialization()
        {
            Console.WriteLine("Serializing...");

            await using var sw = new StreamWriter($@"{Environment.CurrentDirectory}/output-{DateTime.Now:yyyy-MM-ddThh-mm-ss}.json");
            await using var jw = new JsonTextWriter(sw);
            JsonSerializer serializer = new();

            jw.Formatting = Formatting.Indented;

            serializer.Serialize(jw, _dataset.dataset);

            await sw.FlushAsync();

            Console.WriteLine("Serialization done!");
            Console.WriteLine();
        }
    }
}