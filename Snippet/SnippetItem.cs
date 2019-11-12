using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snippet
{
    public class SnippetItem
    {
        public string Trigger { get; set; } = "";
        public string Script { get; set; } = "";
        public bool IsHotKey { get; set; } = true;
        public bool IsSnippet { get; set; } = true;
        public bool ClipboardOrSend { get; set; } = true;

        const string HotKeyTemplate = @"
{0}::
{1}
Return
";
        const string HotStringTemplate = @"
::{0}::{1}
";
        const string ClipboardTemplate = @"tmp = %Clipboard%
Clipboard = {0}
Sleep, 100
send, ^v
Sleep, 100
Clipboard = %tmp%";
        const string SendTemplate = @"Send, {0}";

        public override string ToString()
        {
            string script = Script;
            if (IsSnippet)
            {
                if (ClipboardOrSend)
                {
                    script = string.Format(ClipboardTemplate, script);
                }
                else
                {
                    script = string.Format(SendTemplate, script);
                }
            }
            if (IsHotKey)
            {
                return string.Format(HotKeyTemplate, Trigger, script);
            }
            return string.Format(HotStringTemplate, Trigger, Script);
        }
    }
}
