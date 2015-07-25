using System.Web.UI;

namespace IUtility
{
    public class MessageBox : Control
    {
        private MessageType _messageType = MessageType.Unknown;
        public MessageType Type
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        public string Text { get; set; }

        public string Title { get; set; }

        protected override void Render(HtmlTextWriter writer)
        {
            if (Text != null && Type != MessageType.Unknown)
            {
                string cls = null;

                switch (Type)
                {
                    case MessageType.Success:
                        cls = "success";
                        break;
                    case MessageType.Error:
                        cls = "error";
                        break;
                    case MessageType.Warning:
                        cls = "warning";
                        break;
                    case MessageType.Information:
                        cls = "information";
                        break;
                }

                writer.WriteLine("<div class=\"status {0}\"><h2>{1}</h2><p>{2}</p></div>", cls, Title, Text);

            }
        }
    }

    public enum MessageType
    {
        Success,
        Error,
        Warning,
        Information,
        Unknown
    }

}
