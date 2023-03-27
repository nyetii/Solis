using Solis.Output;

namespace Solis.Filtering
{
    internal class FilterHandler
    {
        private readonly Settings? _settings;
        private readonly List<Instructions> _messages;

        public FilterHandler(List<Instructions> messages, Settings? settings)
        {
            _messages = messages;
            _settings = settings;
        }

        public async Task<List<Instructions>> FilterMessages()
        {
            Console.WriteLine("Filtering...");

            await new MessageFilter(_messages, _settings).Filter();
            await new MessageReplace(_messages, _settings).Filter();

            Console.WriteLine("Filtering done!");

            return await Task.FromResult(_messages);
        }

        
    }
}
