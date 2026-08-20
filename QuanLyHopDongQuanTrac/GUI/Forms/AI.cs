using BLL;
using BLL.Speech;
using DTO;
using DTO.ChatDTO;
using GUI.Common;
using GUI.Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;


namespace GUI.Forms
{
    public partial class AI : UserControl
    {
        private readonly AIPollutionChatBLL _chatBll = new AIPollutionChatBLL();
        private readonly List<ChatMessageDTO> _history = new List<ChatMessageDTO>();
        private readonly LichSuChatBLL lichSuChat = new LichSuChatBLL();
        private int maPhienHienTai = 0;
        private VoiceRecorder _recorder;
        private WhisperService _whisper;
        private string _wavPath;
        private bool _ready = false;

        private int textBoxMinHeight;
        private int textBoxMaxHeight;
        private int lineHeight;
        private bool _isUpdatingLayout = false;


        public AI()
        {
            InitializeComponent();

            this.Load += AI_Load;

            textBox1.TextChanged += textBox1_TextChanged;
            panelInput.Resize += panelInput_Resize;

            btnSend.Click += btnSend_Click;
            btnMic.Click += btnMic_Click;
            textBox1.KeyDown += textBox1_KeyDown;

            this.DoubleBuffered = true;

            flowLayoutPanelChat.Paint += flowLayoutPanelChat_Paint;
        }

        private void flowLayoutPanelChat_Paint(object sender, PaintEventArgs e)
        {
            var logo = Properties.Resources.greenlogo;
            if (logo == null) return;

            var panel = (FlowLayoutPanel)sender;
            var g = e.Graphics;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Viewport hiện tại (vùng đang nhìn thấy)
            int vw = panel.ClientSize.Width;
            int vh = panel.ClientSize.Height;

            // AutoScrollPosition: khi cuộn xuống, X/Y là số âm
            var scroll = panel.AutoScrollPosition;

            // 👉 Đặt logo luôn ở giữa *viewport*, không phải giữa toàn bộ content
            int x = -scroll.X + (vw - logo.Width) / 2;
            int y = -scroll.Y + (vh - logo.Height) / 2;

            ColorMatrix matrix = new ColorMatrix();
            matrix.Matrix33 = 0.08f; // độ mờ

            using (var attrs = new ImageAttributes())
            {
                attrs.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(
                    logo,
                    new Rectangle(x, y, logo.Width, logo.Height),
                    0, 0, logo.Width, logo.Height,
                    GraphicsUnit.Pixel,
                    attrs
                );
            }
        }




        private async void AI_Load(object sender, EventArgs e)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string modelPath = Path.Combine(baseDir, "Model", "ggml-tiny.bin");
            _wavPath = Path.Combine(baseDir, "TempAudio", "search.wav");

            _recorder = new VoiceRecorder(_wavPath);
            var iatService = IATService.TryCreateFromConfiguration();
            _whisper = new WhisperService(modelPath, iatService);
            btnMic.Enabled = false;

            try
            {
                await _whisper.InitAsync();
                _ready = true;
                btnMic.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khởi tạo Whisper:\n\n" + ex.ToString(),
                    "Whisper Init Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }

            textBox1.PlaceholderText = "Nhập câu hỏi....";

            panelInput.Padding = new Padding(20, 25, 10, 10);
            panelInput.BackColor = Color.White;

            textBox1.Multiline = true;
            textBox1.WordWrap = true;
            textBox1.ScrollBars = ScrollBars.None;
            textBox1.AutoSize = false;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Dock = DockStyle.None;
            textBox1.BackColor = panelInput.BackColor;

            lineHeight = TextRenderer.MeasureText("A", textBox1.Font).Height;

            textBoxMinHeight = Math.Max(45, lineHeight + 4);
            textBoxMaxHeight = lineHeight * 5 + 4;
            textBox1.Height = textBoxMinHeight;

            int spacingBetweenButtons = 6;
            int spacingTextboxToButtons = 12;
            int rightExtraMargin = 15;

            int sendWidth = btnSend.Width;
            int micWidth = btnMic.Width;

            btnSend.Left = panelInput.ClientSize.Width
                           - panelInput.Padding.Right
                           - sendWidth
                           - rightExtraMargin;

            btnMic.Left = btnSend.Left
                          - spacingBetweenButtons
                          - micWidth;

            // 2. Textbox bên trái
            textBox1.Left = panelInput.Padding.Left;
            textBox1.Top = panelInput.Padding.Top;

            int rightLimitForTextbox = btnMic.Left - spacingTextboxToButtons;
            int newTextWidth = rightLimitForTextbox - textBox1.Left;
            if (newTextWidth < 50) newTextWidth = 50;
            textBox1.Width = newTextWidth;


            UpdateLayoutKeepBottom();

            //// ====== FLOW LAYOUT CHAT ======
            //flowLayoutPanelChat.HorizontalScroll.Maximum = 0;
            //flowLayoutPanelChat.HorizontalScroll.Visible = false;
            //flowLayoutPanelChat.HorizontalScroll.Enabled = false;

            InitChatUI();
            LoadSessionListForCurrentUser();

            maPhienHienTai = 0;
            //var tenTK = SessionStore.Current?.UserName;  
            //if (string.IsNullOrEmpty(tenTK)) tenTK = "";

            //string defaultTitle = "Phiên chat " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            //maPhienHienTai = lichSuChat.ThemPhienChatMoi(tenTK, defaultTitle);



        }
        private void InitChatUI()
        {
            flowLayoutPanelChat.WrapContents = false;
            flowLayoutPanelChat.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelChat.AutoScroll = true;
            flowLayoutPanelHistory.WrapContents = false;
            flowLayoutPanelHistory.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelHistory.AutoScroll = true;


            flowLayoutPanelChat.BorderStyle = BorderStyle.None;
            flowLayoutPanelChat.Padding = new Padding(0, 5, 0, 5);

            flowLayoutPanelChat.BackColor = Color.Transparent;

            // Double-buffer
            typeof(FlowLayoutPanel).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty
                | System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic,
                null,
                flowLayoutPanelChat,
                new object[] { true }
            );

            flowLayoutPanelChat.HorizontalScroll.Maximum = 0;
            flowLayoutPanelChat.HorizontalScroll.Visible = false;
            flowLayoutPanelChat.HorizontalScroll.Enabled = false;

            flowLayoutPanelChat.VerticalScroll.Visible = false;
            flowLayoutPanelChat.VerticalScroll.Enabled = false;

            flowLayoutPanelChat.AutoScroll = true;
            flowLayoutPanelHistory.Padding = new Padding(0);
            flowLayoutPanelHistory.Margin = new Padding(0);



            //// 🔒 Mỗi lần layout là tắt ngang
            //flowLayoutPanelChat.SizeChanged += (s, e) =>
            //{
            //    int outerWidth = flowLayoutPanelChat.ClientSize.Width - 20;
            //    if (outerWidth < 200) outerWidth = 200;

            //    foreach (Control ctrl in flowLayoutPanelChat.Controls)
            //    {
            //        ctrl.MaximumSize = new Size(outerWidth, 0);
            //        ctrl.Width = outerWidth;   // gọi Resize -> RepositionBubble()
            //    }
            //};

        }

       

        private void AddMessage(string senderName, string message, bool isUser)
        {
            // 1. Nếu chưa có phiên → tạo mới
            if (maPhienHienTai == 0)
            {
                string tenTK = SessionStore.Current?.UserName ?? "";

                // Dùng nội dung tin nhắn đầu tiên làm title (cắt ngắn)
                string title = message;
                if (string.IsNullOrWhiteSpace(title))
                {
                    title = "Phiên chat " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                }
                else if (title.Length > 40)
                {
                    title = title.Substring(0, 40) + "...";
                }

                maPhienHienTai = lichSuChat.ThemPhienChatMoi(tenTK, title);

                // 🔹 Reload lại danh sách bên trái để thấy session mới
                LoadSessionListForCurrentUser();
            }

            // 2. Lưu tin nhắn vào DB
            if (maPhienHienTai > 0)
            {
                string role = isUser ? "User" : "Assistant";
                lichSuChat.ThemTinNhan(maPhienHienTai, role, senderName, message);
            }

            // 3. Cập nhật history cho AI
            _history.Add(new ChatMessageDTO
            {
                Role = isUser ? ChatRole.User : ChatRole.Assistant,
                Content = message
            });

            // 4. Vẽ bubble
            AddMessageToFlow(senderName, message, isUser);
        }




        private void UpdateLayoutKeepBottom()
        {
            if (_isUpdatingLayout) return;
            _isUpdatingLayout = true;

            try
            {
                panelInput.SuspendLayout();

                // 3. Panel cao theo content (KHÔNG đụng tới Top, để Dock=Bottom tự xử lý)
                int contentHeight = Math.Max(textBox1.Height, Math.Max(btnSend.Height, btnMic.Height));
                int newPanelHeight = contentHeight
                                     + panelInput.Padding.Top
                                     + panelInput.Padding.Bottom;

                if (newPanelHeight < 0) newPanelHeight = 0;

                panelInput.Height = newPanelHeight;

                // 4. Căn nút theo đáy panelInput
                int buttonBottom = panelInput.Height - panelInput.Padding.Bottom;
                int upOffset = 5;

                btnSend.Top = buttonBottom - btnSend.Height - upOffset;
                btnMic.Top = buttonBottom - btnMic.Height - upOffset;

                panelInput.ResumeLayout();
            }
            finally
            {
                _isUpdatingLayout = false;
            }

        }
        private void LoadSessionListForCurrentUser()
        {
            flowLayoutPanelHistory.Controls.Clear();

            var tenTK = SessionStore.Current?.UserName;

            // TẠM THỜI: nếu chưa có tên TK vẫn load hết cho dễ test
            var sessions = lichSuChat.LayPhienTheoTenTK(tenTK);

            foreach (var s in sessions)
            {
                var item = CreateSessionItemControl(s);
                flowLayoutPanelHistory.Controls.Add(item);
            }
        }


        private void AttachHoverRecursive(Control root, ModernPanel4Goc bubble)
        {
            root.MouseEnter += (s, e) =>
            {
                if (!bubble.IsActive)
                {
                    bubble.BackColorFill = bubble.HoverColor;
                    bubble.Invalidate();
                }
            };

            root.MouseLeave += (s, e) =>
            {
                if (!bubble.IsActive)
                {
                    bubble.BackColorFill = bubble.DefaultFill;
                    bubble.Invalidate();
                }
            };

            foreach (Control child in root.Controls)
            {
                AttachHoverRecursive(child, bubble);
            }
        }

        private void MakeRoundIconButton(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = Color.Transparent;
            btn.TextAlign = ContentAlignment.MiddleCenter;

            btn.Resize += (s, e) =>
            {
                var path = new GraphicsPath();
                path.AddEllipse(0, 0, btn.Width - 1, btn.Height - 1);
                btn.Region = new Region(path);
            };

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = Color.FromArgb(230, 230, 230);
            };
            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = Color.Transparent;
            };
        }


        private Control CreateSessionItemControl(ChatSessionDTO session)
        {
            // Panel BỌC NGOÀI: full chiều ngang của flow
            var outer = new Panel
            {
                Height = 80,
                Width = 230,  // sát viền phải
                Margin = new Padding(0, 6, 0, 6),
                Padding = new Padding(4, 0, 4, 0), // 👉 chỉnh số này để tăng/giảm khoảng cách trái/phải
                BackColor = Color.Transparent
            };


            // BUBBLE thực tế
            var bubble = new ModernPanel4Goc
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12, 8, 12, 8),
                BorderRadius = 12,
                BackColorFill = Color.White,
                DefaultFill = Color.White,
                BorderColor = Color.FromArgb(230, 230, 230),
                DrawTextOnPanel = false,
                Tag = session.MaPhien,
                HoverColor = Color.FromArgb(174, 153, 68),
                ActiveColor = Color.FromArgb(174, 153, 68)
            };

            var lblTitle = new Label
            {
                Text = session.TenPhienChat,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true,
                Padding = new Padding(6, 0, 6, 0),
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold)
            };

            int size = 26;

            var btnDelete = new Button
            {
                Text = "🗑",
                Width = size,
                Height = size,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                TabStop = false,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI Emoji", 12f, FontStyle.Bold)
            };

            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnDelete.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnDelete.ForeColor = Color.Black;
            btnDelete.TabStop = false;

            var btnRename = new Button
            {
                Text = "✎",
                Width = size,
                Height = size,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                TabStop = false,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI Emoji", 12f, FontStyle.Bold)
            };
            btnRename.FlatAppearance.BorderSize = 0;
            btnRename.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnRename.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnRename.ForeColor = Color.Black;
            btnRename.TabStop = false;

            MakeRoundIconButton(btnDelete);
            MakeRoundIconButton(btnRename);

            // THÊM CONTROL VÀO BUBBLE (giữ thứ tự này để Dock đúng)
            bubble.Controls.Add(lblTitle);
            bubble.Controls.Add(btnRename);
            bubble.Controls.Add(btnDelete);

            AttachHoverRecursive(bubble, bubble);

            // ==== Click đổi active + load phiên ====
            void LoadThis(object _, EventArgs __)
            {
                foreach (Control ctrl in flowLayoutPanelHistory.Controls)
                {
                    if (ctrl is Panel p && p.Controls.Count > 0 && p.Controls[0] is ModernPanel4Goc mp)
                    {
                        mp.SetActive(false);
                    }
                }

                bubble.SetActive(true);
                LoadSessionToChat(session.MaPhien);
            }

            bubble.Click += LoadThis;
            lblTitle.Click += LoadThis;
            btnRename.Click += (s, e) =>
            {
                string newTitle = Microsoft.VisualBasic.Interaction.InputBox(
                    "Nhập tên mới cho lịch sử chat:",
                    "Đổi tên lịch sử chat",
                    session.TenPhienChat
                );
                if (!string.IsNullOrWhiteSpace(newTitle))
                {
                    lichSuChat.SuaTenPhienChat(session.MaPhien, newTitle);
                    lblTitle.Text = newTitle;
                }
            };
            btnDelete.Click += (s, e) =>
            {
                if (session.MaPhien == maPhienHienTai)
                {
                    MessageBox.Show("Không thể xóa phiên hiện tại!");
                    return;
                }

                var confirm = MessageBox.Show(
                    "Xóa lịch sử chat này?",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    lichSuChat.XoaPhienChat(session.MaPhien);
                    flowLayoutPanelHistory.Controls.Remove(outer);
                }
            };

            // GẮN BUBBLE VÀO OUTER, rồi trả về OUTER cho FlowLayoutPanel
            outer.Controls.Add(bubble);
            return outer;
        }


        private void LoadSessionToChat(int sessionId)
        {
            maPhienHienTai = sessionId;

            // Xóa UI hiện tại
            flowLayoutPanelChat.Controls.Clear();
            _history.Clear();

            var messages = lichSuChat.LayTinNhanTheoPhien(sessionId);

            // ORDER BY OrderIndex trong DAL/BLL
            foreach (var msg in messages)
            {
                bool isUser = msg.VaiTro == "User";
                AddMessageToFlow(msg.TenNguoiGui, msg.NoiDung, isUser);

                // đồng bộ lại _history để AI hiểu context nếu bạn chat tiếp
                _history.Add(new ChatMessageDTO
                {
                    Role = isUser ? ChatRole.User : ChatRole.Assistant,
                    Content = msg.NoiDung
                });
            }

            // cuộn xuống cuối
            if (flowLayoutPanelChat.Controls.Count > 0)
            {
                var last = flowLayoutPanelChat.Controls[flowLayoutPanelChat.Controls.Count - 1];
                flowLayoutPanelChat.ScrollControlIntoView(last);
            }
        }



        private void panelInput_Resize(object sender, EventArgs e)
        {
            UpdateLayoutKeepBottom();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (lineHeight <= 0)
                lineHeight = TextRenderer.MeasureText("A", textBox1.Font).Height;

            int lineCount = textBox1.GetLineFromCharIndex(textBox1.TextLength) + 1;
            int newHeight = lineCount * lineHeight + 4;

            if (newHeight < textBoxMinHeight)
                newHeight = textBoxMinHeight;

            if (newHeight > textBoxMaxHeight)
            {
                newHeight = textBoxMaxHeight;
                textBox1.ScrollBars = ScrollBars.Vertical;
            }
            else
            {
                textBox1.ScrollBars = ScrollBars.None;
            }

            if (textBox1.Height != newHeight)
            {
                textBox1.Height = newHeight;
                UpdateLayoutKeepBottom();
            }
        }

        // ========== GỬI CHAT (BUTTON SEND) ==========
        private async void btnSend_Click(object sender, EventArgs e)
        {
            string userText = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(userText))
                return;

            userText = NormalizeNewlines(userText);

            // ChatAsync sẽ tự nối userText vào request, nên chỉ truyền lịch sử
            // trước tin nhắn hiện tại để tránh gửi cùng một nội dung hai lần.
            var historyBeforeCurrentMessage = new List<ChatMessageDTO>(_history);

            // 🔹 Thêm tin nhắn người dùng (kèm tạo session nếu chưa có)
            AddMessage("Bạn", userText, isUser: true);

            textBox1.Clear();
            btnSend.Enabled = false;

            try
            {
                var result = await _chatBll.ChatAsync(historyBeforeCurrentMessage, userText);
                string aiText = NormalizeNewlines(result.ReplyText);

                await Task.Delay(300);

                // 🔹 Lưu luôn tin nhắn AI vào DB + UI + _history
                AddMessage("ECOS Trợ Lý", aiText, isUser: false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gọi AI: " + ex.Message);
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }


        // Enter để gửi – Shift+Enter để xuống dòng
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true;
                btnSend.PerformClick();
            }
        }

        private string NormalizeNewlines(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            return text.Replace("\r\n", "\n")
                       .Replace("\r", "\n")
                       .Replace("\n", Environment.NewLine);
        }

        private Control CreateMessageBubble(string senderName, string message, bool isUser)
        {
            int outerWidth = 1370; 


            var outerPanel = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Margin = isUser ? new Padding(20, 5, 0, 5)   
                                : new Padding(10, 5, 20, 5), 
                Padding = new Padding(0),
                BackColor = Color.Transparent
            };
            //outerPanel.BorderStyle = BorderStyle.FixedSingle;
            outerPanel.MaximumSize = new Size(outerWidth, 0);
            outerPanel.Width = outerWidth;

            int maxBubbleWidth = (int)(outerWidth * (isUser ? 0.55 : 0.75));

            var bubble = new ModernPanel4Goc
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BorderRadius = 18,
                DrawTextOnPanel = false,

                Padding = isUser
                    ? new Padding(40, 16, 40, 16)   // (Left, Top, Right, Bottom)
                    : new Padding(30, 12, 30, 12),

                BackColor = Color.Transparent,
                MaximumSize = new Size(maxBubbleWidth, 0),
                Margin = new Padding(0)
            };

            if (isUser)
            {
                bubble.Anchor = AnchorStyles.Right;
                bubble.BackColorFill = Color.FromArgb(198, 235, 198);
                bubble.BorderColor = Color.FromArgb(153, 210, 153);
            }
            else
            {
                bubble.BackColorFill = Color.FromArgb(235, 235, 235);
                bubble.BorderColor = Color.FromArgb(200, 200, 200);
            }

            var fontSender = new Font("Segoe UI Semibold", 12f, FontStyle.Bold);
            var fontMessage = new Font("Segoe UI Semibold", 12f, FontStyle.Bold);

            var lblSender = new Label
            {
                AutoSize = true,
                Font = fontSender,
                Text = senderName,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 2)
            };

            if (!string.IsNullOrEmpty(message))
            {
                message = message.Replace("\r\n", "\n")
                                 .Replace("\r", "\n")
                                 .Replace("\n", Environment.NewLine);
            }

            var lblMsg = new Label
            {
                AutoSize = true,
                Font = fontMessage,
                Text = message,
                BackColor = Color.Transparent,
                MaximumSize = new Size(maxBubbleWidth - 20, 0),
                Margin = new Padding(0)
            };

            var inner = new Panel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = isUser
                    ? new Padding(14, 0, 5, 0)   // user: cách trái/phải 14px
                    : new Padding(10, 0, 5, 0),  // AI: 10px
                Margin = new Padding(0),
                BackColor = Color.Transparent
            };
            if (isUser)
            {
                inner.RightToLeft = RightToLeft.Yes;
                lblSender.TextAlign = ContentAlignment.MiddleRight;
                lblMsg.TextAlign = ContentAlignment.MiddleRight;
            }

            lblSender.Padding = new Padding(10, 5, 0, 0);
            lblMsg.Padding = new Padding(10, 5, 0, 0);

            inner.Controls.Add(lblSender);
            lblMsg.Location = new Point(0, lblSender.Bottom + 2);
            inner.Controls.Add(lblMsg);

            bubble.Controls.Add(inner);
            bubble.PerformLayout();

            int paddingEdgeUser = 5;
            int paddingEdgeAI = 10;

            void RepositionBubble()
            {
                if (isUser)
                {
                    bubble.Left = outerPanel.Width - bubble.Width - paddingEdgeUser;
                }
                else
                {
                    bubble.Left = paddingEdgeAI;
                }
                bubble.Top = 0;
            }

            RepositionBubble();
            outerPanel.Resize += (s, e) => RepositionBubble();

            outerPanel.Controls.Add(bubble);
            return outerPanel;
        }

        private void AddMessageToFlow(string senderName, string message, bool isUser)
        {
            var bubble = CreateMessageBubble(senderName, message, isUser);
            flowLayoutPanelChat.Controls.Add(bubble);

            flowLayoutPanelChat.ScrollControlIntoView(bubble);
        }


        private async void btnMic_Click(object sender, EventArgs e)
        {
            if (!_ready) return;

            if (!_recorder.IsRecording)
            {
                try
                {
                    _recorder.Start();
                    btnMic.ButtonImage = Properties.Resources.microphone_hoatdong;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể ghi âm: " + ex.Message);
                }
            }
            else
            {
                btnMic.Enabled = false;
                btnMic.ButtonImage = Properties.Resources.microphone;

                _recorder.Stop();
                await System.Threading.Tasks.Task.Delay(300);

                try
                {
                    string text;
                    try
                    {
                        Debug.WriteLine("này nó hoạt động á cha ơi");
                        text = await _whisper.TranscribeIFlytekAsync(_wavPath);
                    }
                    catch
                    {
                        text = await _whisper.TranscribeAsync(_wavPath);
                    }

                    if (string.IsNullOrWhiteSpace(text))
                    {
                        MessageBox.Show("Không nghe được nội dung");
                        return;
                    }

                    textBox1.ForeColor = Color.FromArgb(64, 64, 64);
                    textBox1.Text = text.Trim();
                    textBox1.SelectionStart = textBox1.Text.Length;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi nhận dạng giọng nói: " + ex.Message);
                }
                finally
                {
                    btnMic.Enabled = true;
                }
            }
        }
    }
}
