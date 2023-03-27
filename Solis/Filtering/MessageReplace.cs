using Solis.Output;
using System.Text.RegularExpressions;

namespace Solis.Filtering
{
    internal class MessageReplace : FilterHandler
    {
        private readonly List<Instructions> _messages;
        private readonly Settings? _settings;

        public MessageReplace(List<Instructions> messages, Settings? settings) : base(messages, settings)
        {
            _messages = messages;
            _settings = settings;
        }

        internal async Task<List<Instructions>> Filter()
        {
            if (_settings.replace.pings) await ReplacePings();
            if (_settings.replace.mentions) await ReplaceMentions();
            if (_settings.replace.specificWords) await ReplaceSpecific();
            

            return _messages;
        }

        private async Task ReplacePings()
        {
            if (_settings?.keyWords.pingWords is null || _settings.keyWords.pingReplacement is null)
                return;

            var messages = _messages;

            foreach (var keyword in _settings.keyWords.pingWords)
            {
                var query = messages.AsParallel().AsOrdered()
                    .Where(message =>
                        Regex.IsMatch(message.instruction, $@"\@\b(?:{Regex.Escape(keyword)})",
                            RegexOptions.IgnoreCase));

                query.ForAll(message =>
                {
                    //_messages.Remove(message);
                    message.instruction = Regex.Replace(message.instruction, $@"\@\b(?:{Regex.Escape(keyword)})",
                        $"@{_settings.keyWords.pingReplacement}",
                        RegexOptions.IgnoreCase);
                    _messages.Add(message);
                });
            }

            await Task.CompletedTask;
        }

        private async Task ReplaceMentions()
        {
            if (_settings?.keyWords.mentionWords is null || _settings.keyWords.mentionReplacement is null)
                return;

            var messages = _messages;

            foreach (var keyword in _settings.keyWords.mentionWords)
            {
                var query = messages.AsParallel().AsOrdered()
                    .Where(message => Regex.IsMatch(message.instruction,
                        $@"(?:{Regex.Escape(keyword)})", RegexOptions.IgnoreCase));
                
                query.ForAll(message =>
                {
                    message.instruction = Regex.Replace(message.instruction, $@"\b(?:{Regex.Escape(keyword)})", $"{_settings.keyWords.mentionReplacement}",
                        RegexOptions.IgnoreCase);
                    _messages.Add(message);
                });
            }

            await Task.CompletedTask;
        }

        private async Task ReplaceSpecific()
        {
            if(_settings?.keyWords.specificWords is null || _settings.keyWords.specificReplacement is null)
                return;

            var messages = new List<Instructions>();

            foreach (var keyword in _settings.keyWords.specificWords)
            {
                var query = _messages.AsParallel().AsOrdered()
                    .Where(message => Regex.IsMatch(message.instruction,
                        $@"\b(?:{Regex.Escape(keyword)})", RegexOptions.IgnoreCase));

                query.ForAll(message =>
                {
                    _messages.Remove(message);
                    message.instruction = Regex.Replace(message.instruction, $@"\b(?:{Regex.Escape(keyword)})", _settings.keyWords.specificReplacement,
                        RegexOptions.IgnoreCase);
                    
                    _messages.Add(message);
                });
            }

            await Task.CompletedTask;
        }
    }
}
