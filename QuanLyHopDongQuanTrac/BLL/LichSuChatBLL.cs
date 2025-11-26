using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LichSuChatBLL
    {
        private readonly DatabaseAccess _dal = new DatabaseAccess();

        public int ThemPhienChatMoi(string maNV, string title)
            => _dal.ThemPhienChatMoi(maNV, title);

        public void SuaTenPhienChat(int sessionId, string newTitle)
            => _dal.SuaTenPhienChat(sessionId, newTitle);

        public void XoaPhienChat(int sessionId)
            => _dal.XoaPhienChat(sessionId);

        public List<ChatSessionDTO> LayPhienTheoTenTK(string maNV)
            => _dal.LayPhienTheoTenTK(maNV);

        public void ThemTinNhan(int sessionId, string senderRole, string senderName, string content)
            => _dal.ThemTinNhan(sessionId, senderRole, senderName, content);

        public List<ChatMessageDbDTO> LayTinNhanTheoPhien(int sessionId)
            => _dal.LayTinNhanTheoPhien(sessionId);
    }
}
