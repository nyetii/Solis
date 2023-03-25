using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
#pragma warning disable CS8618

namespace Solis
{
    public class Attachment
    {
        public string id { get; set; }
        public string url { get; set; }
        public string fileName { get; set; }
        public int fileSizeBytes { get; set; }
    }

    public class Author
    {
        public string id { get; set; }
        public string name { get; set; }
        public string discriminator { get; set; }
        public string nickname { get; set; }
        public string color { get; set; }
        public bool isBot { get; set; }
        public string avatarUrl { get; set; }
    }

    public class Channel
    {
        public string id { get; set; }
        public string type { get; set; }
        public string categoryId { get; set; }
        public string category { get; set; }
        public string name { get; set; }
        public string topic { get; set; }
    }

    public class DateRange
    {
        public object after { get; set; }
        public object before { get; set; }
    }

    public class Embed
    {
        public string title { get; set; }
        public string url { get; set; }
        public object timestamp { get; set; }
        public string description { get; set; }
        public Thumbnail thumbnail { get; set; }
        public List<object> images { get; set; }
        public List<object> fields { get; set; }
    }

    public class Emoji
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool isAnimated { get; set; }
        public string imageUrl { get; set; }
    }

    public class Guild
    {
        public string id { get; set; }
        public string name { get; set; }
        public string iconUrl { get; set; }
    }

    public class Mention
    {
        public string id { get; set; }
        public string name { get; set; }
        public string discriminator { get; set; }
        public string nickname { get; set; }
        public bool isBot { get; set; }
    }

    public class Message
    {
        public string id { get; set; }
        public string type { get; set; }
        public DateTime timestamp { get; set; }
        public object timestampEdited { get; set; }
        public object callEndedTimestamp { get; set; }
        public bool isPinned { get; set; }
        public string content { get; set; }
        public Author author { get; set; }
        public List<Attachment> attachments { get; set; }
        public List<Embed> embeds { get; set; }
        public List<Sticker> stickers { get; set; }
        public List<Reaction> reactions { get; set; }
        public List<Mention> mentions { get; set; }
        public Reference? reference { get; set; }
    }

    public class Reaction
    {
        public Emoji emoji { get; set; }
        public int count { get; set; }
    }

    public class Root
    {
        public Guild guild { get; set; }
        public Channel channel { get; set; }
        public DateRange dateRange { get; set; }
        public List<Message> messages { get; set; }
        public int messageCount { get; set; }
    }

    public class Sticker
    {
        public string id { get; set; }
        public string name { get; set; }
        public string format { get; set; }
        public string sourceUrl { get; set; }
    }

    public class Thumbnail
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class Reference
    {
        public string messageId { get; set; }
        public string channelId { get; set; }
        public string guildId { get; set; }
    }
}
