using System.Text.RegularExpressions;
using Solis.Output;

namespace Solis.Filtering
{
    internal class MessageFilter : FilterHandler
    {
        private readonly List<Instructions> _messages;
        private readonly Settings? _settings;

        public MessageFilter(List<Instructions> messages, Settings? settings) : base(messages, settings)
        {
            _messages = messages;
            _settings = settings;
        }
        
        internal async Task<List<Instructions>> Filter()
        {
            if (_settings.filter.url) await FilterUrl();
            if (_settings.filter.codeBlocks) await FilterCodeBlocks();
            if (_settings.filter.nullMessages) await FilterNullMessages();
            if (_settings.filter.smallMessages) await FilterSmallMessages();
            if (_settings.filter.hugeMessages) await FilterHugeMessages();
            if (_settings.filter.emotes) await FilterEmotes();
            if (_settings.filter.specificWords) await FilterSpecificWords("sona");
            return _messages;
        }

        private async Task FilterUrl()
        {
            _messages.RemoveAll(message => Regex.IsMatch(message.instruction, @"http[^\s]+")
                                           || Regex.IsMatch(message.output, @"http[^\s]+"));
            await Task.CompletedTask;
        }

        private async Task FilterCodeBlocks()
        {
            _messages.RemoveAll(message => Regex.IsMatch(message.instruction, @"```")
                                           || Regex.IsMatch(message.output, @"```"));
            await Task.CompletedTask;
        }

        private async Task FilterEmotes()
        {
            _messages.RemoveAll(message => Regex.IsMatch(message.instruction, @"(?:\S{2,32}:)")
                                           || Regex.IsMatch(message.output, @"(?::\S{2,32}:)"));
            await Task.CompletedTask;
        }

        private async Task FilterNullMessages()
        {
            _messages.RemoveAll(message =>
                string.IsNullOrWhiteSpace(message.instruction) || string.IsNullOrWhiteSpace(message.output));
            await Task.CompletedTask;
        }

        private async Task FilterSmallMessages()
        {
            _messages.RemoveAll(message => message.instruction.Length < _settings?.limits.minChars || message.output.Length < _settings?.limits.minChars);
            await Task.CompletedTask;
        }

        private async Task FilterHugeMessages()
        {
            _messages.RemoveAll(message => message.instruction.Length > _settings?.limits.maxChars || message.output.Length > _settings?.limits.maxChars);
            await Task.CompletedTask;
        }

        private async Task FilterSpecificWords(string test)
        {
            _messages.RemoveAll(message => Regex.IsMatch(message.instruction,
                                               $@"\b(?:{Regex.Escape(test)})", RegexOptions.IgnoreCase)
                                           || Regex.IsMatch(message.output,
                                               $@"\b(?:{Regex.Escape(test)})", RegexOptions.IgnoreCase));
            await Task.CompletedTask;
        }
    }
}
