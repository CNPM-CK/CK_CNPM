using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ChatDTO
{
    public class ChatMessageDTO
    {
        public ChatRole Role { get; set; }
        public string Content { get; set; }
    }
}
