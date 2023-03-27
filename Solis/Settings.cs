namespace Solis
{
    internal class Settings
    {
        public Filter filter { get; init; } = new();
        public Replace replace { get; init; } = new();
        public Limits limits { get; init; } = new();
        public KeyWords keyWords { get; init; } = new();
    }

    internal class Filter
    {
        public bool url { get; init; } = true;
        public bool codeBlocks { get; init; } = true;
        public bool nullMessages { get; init; } = true;
        public bool smallMessages { get; init; } = true;
        public bool hugeMessages { get; init; } = true;
        public bool emotes { get; init; } = true;
        public bool specificWords { get; init; } = false;
    }

    internal class Replace
    {
        public bool pings { get; init; } = false;
        public bool mentions { get; init; } = false;
        public bool specificWords { get; init; } = false;
    }

    internal class Limits
    {
        public int minChars { get; init; } = 2;
        public int maxChars { get; init; } = 2000;
    }

    internal class KeyWords
    {
        public string[]? pingWords { get; init; } = null;
        public string? pingReplacement { get; init; } = null;
        public string[]? mentionWords { get; init; } = null;
        public string? mentionReplacement { get; init; } = null;
        public string[]? specificWords { get; init; } = null;
        public string? specificReplacement { get; init; } = null;
    }
}
