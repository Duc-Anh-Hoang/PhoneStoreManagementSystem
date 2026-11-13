/**
 * =============================================================================
 * SAMAMOBILE - CUSTOMER LIVE CHAT WIDGET
 * Tích hợp Live Chat tư vấn trực tuyến giữa Khách hàng và Admin/CSKH qua SignalR
 * =============================================================================
 */

(function () {
  'use strict';

  const API_BASE = (window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1')
    ? 'http://localhost:5132'
    : window.location.origin;

  const CHAT_HUB_URL = `${API_BASE}/hubs/chat`;
  const isInPagesDir = window.location.pathname.includes('/pages/');
  const IMG_PREFIX = isInPagesDir ? '../images' : 'images';
  const STORAGE_KEY_SESSION = 'samamobile_chat_session';
  const STORAGE_KEY_IS_OPEN = 'samamobile_chat_open';

  let chatConnection = null;
  let activeSessionId = null;
  let isChatOpen = false;
  let isConnecting = false;
  let unreadCount = 0;

  // Âm thanh thông báo tin nhắn mới nhẹ nhàng
  function playChatChime() {
    try {
      const AudioCtx = window.AudioContext || window.webkitAudioContext;
      if (!AudioCtx) return;
      const ctx = new AudioCtx();
      if (ctx.state === 'suspended') ctx.resume().catch(() => {});
      const now = ctx.currentTime;
      const osc1 = ctx.createOscillator();
      const osc2 = ctx.createOscillator();
      const gain = ctx.createGain();

      osc1.type = 'sine';
      osc2.type = 'triangle';
      osc1.frequency.setValueAtTime(587.33, now); // D5
      osc1.frequency.exponentialRampToValueAtTime(880.00, now + 0.15); // A5

      osc2.frequency.setValueAtTime(880.00, now + 0.05);
      osc2.frequency.exponentialRampToValueAtTime(1174.66, now + 0.25); // D6

      gain.gain.setValueAtTime(0.12, now);
      gain.gain.exponentialRampToValueAtTime(0.001, now + 0.35);

      osc1.connect(gain);
      osc2.connect(gain);
      gain.connect(ctx.destination);

      osc1.start(now);
      osc2.start(now + 0.05);
      osc1.stop(now + 0.35);
      osc2.stop(now + 0.35);
    } catch (e) {}
  }

  // Khởi tạo HTML cho Chat Widget trên giao diện User
  function injectChatWidgetHTML() {
    if (document.getElementById('samamobile-chat-widget-root')) return;

    const root = document.createElement('div');
    root.id = 'samamobile-chat-widget-root';
    root.className = 'fixed right-4 sm:right-6 bottom-4 sm:bottom-6 z-50 select-none font-sans';

    root.innerHTML = `
      <!-- Cụm Nút Mascot Chat Nổi (Floating Mascot Chat) -->
      <div id="sama-chat-fab-container" class="relative flex items-center justify-end">
        <!-- Speech Bubble Mời Chào của Mascot Sama (Style 3D) -->
        <div id="sama-chat-tooltip" class="hidden sm:block mr-2 z-50 cursor-pointer animate-in fade-in slide-in-from-right-4 duration-300 select-none group">
          <div class="relative inline-block hover:scale-105 transition-transform duration-200" onclick="window.SamaChat.toggleChat()">
            <picture>
              <source srcset="${IMG_PREFIX}/chat-mascot-bubble-1-clean.webp" type="image/webp">
              <img src="${IMG_PREFIX}/chat-mascot-bubble-1-clean.png" alt="Cần tư vấn điện thoại? Chat ngay!" class="h-13 sm:h-14 w-auto drop-shadow-md">
            </picture>
            <!-- Nút đóng (X) trong suốt đặt ngay góc phải bubble -->
            <button type="button" onclick="event.stopPropagation(); document.getElementById('sama-chat-tooltip').remove();" class="absolute top-1.5 right-2 w-5 h-5 rounded-full flex items-center justify-center text-slate-400 hover:text-slate-700 hover:bg-slate-200/50 text-[11px] transition-colors cursor-pointer" title="Đóng lời chào">✕</button>
          </div>
        </div>

        <!-- Nút Tròn Mascot 3D SamaMobile -->
        <button type="button" id="btn-toggle-chat-widget" onclick="window.SamaChat.toggleChat()" class="relative w-15 h-15 sm:w-16 sm:h-16 flex items-center justify-center hover:scale-110 active:scale-95 transition-all duration-200 cursor-pointer group" title="Chat tư vấn trực tuyến với SamaMobile">
          <!-- Hình ảnh Mascot mở khi chưa bật hộp thoại -->
          <div id="sama-chat-icon-open" class="w-full h-full relative flex items-center justify-center">
            <!-- Mascot mặc định (mắt mở vẫy tay) -->
            <picture class="w-full h-full block group-hover:hidden transition-all filter drop-shadow-[0_8px_16px_rgba(0,102,255,0.35)]">
              <source srcset="${IMG_PREFIX}/chat-mascot-btn-1-clean.webp" type="image/webp">
              <img src="${IMG_PREFIX}/chat-mascot-btn-1-clean.png" alt="SamaMobile Live Chat" class="w-full h-full object-contain">
            </picture>

            <!-- Mascot khi hover (mỉm cười nháy mắt vui vẻ) -->
            <picture class="w-full h-full hidden group-hover:block transition-all filter drop-shadow-[0_10px_20px_rgba(0,102,255,0.5)]">
              <source srcset="${IMG_PREFIX}/chat-mascot-btn-2-clean.webp" type="image/webp">
              <img src="${IMG_PREFIX}/chat-mascot-btn-2-clean.png" alt="SamaMobile Live Chat" class="w-full h-full object-contain">
            </picture>
          </div>

          <!-- Nút X đóng khi chat box đang mở -->
          <div id="sama-chat-icon-close" class="hidden w-13 h-13 sm:w-14 sm:h-14 rounded-full bg-[#0066FF] hover:bg-[#0052cc] text-white flex items-center justify-center shadow-xl shadow-blue-600/40 transition-transform duration-300">
            <i class="fa-solid fa-xmark text-2xl"></i>
          </div>

          <!-- Badge đếm tin chưa đọc -->
          <span id="sama-chat-unread-badge" class="hidden absolute -top-1 -left-1 px-2 py-0.5 rounded-full bg-rose-500 text-white text-[10px] font-black border-2 border-white shadow-md animate-pulse z-10">0</span>
        </button>
      </div>

      <!-- Cửa Sổ Hộp Thoại Chat (Chat Box Popup) -->
      <div id="sama-chat-box" class="hidden fixed sm:absolute right-0 bottom-16 sm:bottom-18 w-[calc(100vw-2rem)] sm:w-[380px] max-w-[420px] h-[520px] max-h-[82vh] bg-white rounded-3xl shadow-2xl border border-slate-200/90 overflow-hidden flex flex-col z-50 animate-in fade-in slide-in-from-bottom-6 duration-200">
        
        <!-- HEADER CỬA SỔ CHAT -->
        <div class="px-4 py-3 bg-gradient-to-r from-[#0066FF] to-[#0048ba] text-white flex items-center justify-between shadow-xs flex-shrink-0">
          <div class="flex items-center gap-2.5 min-w-0">
            <div class="relative w-9 h-9 rounded-2xl bg-white/20 border border-white/30 flex items-center justify-center flex-shrink-0 overflow-hidden">
              <img src="${IMG_PREFIX}/chat-mascot-head.png" alt="SamaMobile CSKH" class="w-7 h-7 object-contain">
              <span class="absolute bottom-0 right-0 w-2.5 h-2.5 rounded-full bg-emerald-400 border border-white"></span>
            </div>
            <div class="min-w-0">
              <h4 class="font-bold text-sm tracking-tight truncate flex items-center gap-1.5">
                <span>SamaMobile CSKH</span>
                <span class="text-[9px] font-medium bg-white/20 px-1.5 py-0.2 rounded text-white">24/7</span>
              </h4>
              <p id="sama-chat-header-subtitle" class="text-[11px] text-blue-100 font-normal truncate">Tư vấn viên sẵn sàng hỗ trợ bạn</p>
            </div>
          </div>

          <div class="flex items-center gap-1">
            <button type="button" onclick="window.SamaChat.minimizeChat()" class="w-7 h-7 rounded-lg text-white/80 hover:text-white hover:bg-white/10 flex items-center justify-center text-xs transition-colors cursor-pointer" title="Thu nhỏ">
              <i class="fa-solid fa-minus"></i>
            </button>
            <button type="button" onclick="window.SamaChat.endCurrentChat()" class="w-7 h-7 rounded-lg text-white/80 hover:text-rose-200 hover:bg-rose-500/20 flex items-center justify-center text-xs transition-colors cursor-pointer" title="Kết thúc phiên chat">
              <i class="fa-solid fa-power-off"></i>
            </button>
            <button type="button" onclick="window.SamaChat.toggleChat()" class="w-7 h-7 rounded-lg text-white/80 hover:text-white hover:bg-white/10 flex items-center justify-center text-xs transition-colors cursor-pointer" title="Đóng">
              <i class="fa-solid fa-xmark"></i>
            </button>
          </div>
        </div>

        <!-- MÀN HÌNH KHỞI TẠO (KHI CHƯA CÓ PHIÊN CHAT NÀO MỞ) -->
        <div id="sama-chat-init-view" class="p-5 flex-1 overflow-y-auto space-y-4 flex flex-col justify-center bg-slate-50/50">
          <div class="text-center space-y-2">
            <div class="w-14 h-14 mx-auto rounded-2xl bg-blue-50 text-[#0066FF] flex items-center justify-center text-2xl shadow-inner border border-blue-100">
              <i class="fa-solid fa-comments"></i>
            </div>
            <h5 class="font-bold text-sm text-slate-800">Xin chào bạn! 👋</h5>
            <p class="text-xs text-slate-500 leading-relaxed">Vui lòng để lại tên hoặc số điện thoại để tư vấn viên phục vụ bạn tốt nhất và tiện tra cứu đơn hàng.</p>
          </div>

          <form id="sama-chat-init-form" onsubmit="window.SamaChat.startChat(event)" class="space-y-3 bg-white p-4 rounded-2xl border border-slate-200 shadow-2xs">
            <div>
              <label class="block text-[11px] font-semibold text-slate-600 mb-1">Họ và tên:</label>
              <input type="text" id="sama-chat-input-name" placeholder="Ví dụ: Nguyễn Văn A" class="w-full px-3 py-2 rounded-xl border border-slate-200 text-xs text-slate-800 focus:outline-none focus:border-[#0066FF] focus:ring-1 focus:ring-[#0066FF] font-medium placeholder:text-slate-400">
            </div>

            <div>
              <label class="block text-[11px] font-semibold text-slate-600 mb-1">Số điện thoại (để nhận diện):</label>
              <input type="tel" id="sama-chat-input-phone" placeholder="Ví dụ: 0912345678" class="w-full px-3 py-2 rounded-xl border border-slate-200 text-xs text-slate-800 focus:outline-none focus:border-[#0066FF] focus:ring-1 focus:ring-[#0066FF] font-medium placeholder:text-slate-400">
            </div>

            <button type="submit" id="sama-chat-btn-start" class="w-full py-2.5 rounded-xl bg-[#0066FF] hover:bg-[#0052cc] text-white font-bold text-xs shadow-md shadow-blue-600/20 transition-all flex items-center justify-center gap-1.5 cursor-pointer">
              <span>Bắt đầu trò chuyện ngay</span>
              <i class="fa-solid fa-arrow-right text-[10px]"></i>
            </button>
          </form>

          <!-- Gợi ý nhanh -->
          <div class="space-y-1.5 text-center">
            <span class="text-[11px] text-slate-400 font-medium">Bạn có thể hỏi về:</span>
            <div class="flex flex-wrap justify-center gap-1.5">
              <button type="button" onclick="window.SamaChat.quickFillAndStart('Tư vấn mua iPhone 16 Pro Max')" class="px-2.5 py-1 rounded-full bg-white border border-slate-200 text-[11px] text-slate-600 hover:border-blue-300 hover:text-[#0066FF] transition-all cursor-pointer shadow-2xs">
                📱 iPhone 16 Pro Max
              </button>
              <button type="button" onclick="window.SamaChat.quickFillAndStart('Thu cũ đổi mới trợ giá bao nhiêu?')" class="px-2.5 py-1 rounded-full bg-white border border-slate-200 text-[11px] text-slate-600 hover:border-blue-300 hover:text-[#0066FF] transition-all cursor-pointer shadow-2xs">
                🔄 Thu cũ đổi mới
              </button>
              <button type="button" onclick="window.SamaChat.quickFillAndStart('Tra cứu tiến độ giao hàng đơn hàng')" class="px-2.5 py-1 rounded-full bg-white border border-slate-200 text-[11px] text-slate-600 hover:border-blue-300 hover:text-[#0066FF] transition-all cursor-pointer shadow-2xs">
                📦 Tra cứu đơn
              </button>
            </div>
          </div>
        </div>

        <!-- MÀN HÌNH NỘI DUNG CUỘC TRÒ CHUYỆN (ACTIVE CHAT VIEW) -->
        <div id="sama-chat-active-view" class="hidden flex-1 overflow-hidden flex flex-col bg-slate-50">
          
          <!-- Thanh trạng thái phiên chat nhỏ -->
          <div class="px-3.5 py-1.5 bg-blue-50/80 border-b border-blue-100/80 flex items-center justify-between text-[11px] text-slate-600 flex-shrink-0">
            <div class="flex items-center gap-1.5 min-w-0">
              <span id="sama-chat-session-status-dot" class="w-2 h-2 rounded-full bg-amber-500 animate-pulse"></span>
              <span id="sama-chat-session-status-text" class="font-semibold text-slate-700 truncate">Đang chờ nhân viên kết nối...</span>
            </div>
            <span id="sama-chat-session-code" class="font-mono text-[10px] text-blue-700 bg-white px-1.5 py-0.5 rounded border border-blue-200 font-semibold">-</span>
          </div>

          <!-- Danh sách bong bóng tin nhắn (Scrollable) -->
          <div id="sama-chat-messages-container" class="flex-1 p-3.5 overflow-y-auto space-y-2.5 scroll-smooth text-xs">
            <!-- Tin nhắn được render động vào đây -->
          </div>

          <!-- Hàng gợi ý nhanh (Chips) -->
          <div id="sama-chat-quick-chips" class="px-3 py-1.5 bg-white border-t border-slate-100 flex items-center gap-1.5 overflow-x-auto no-scrollbar flex-shrink-0">
            <button type="button" onclick="window.SamaChat.sendQuickMessage('Cho mình hỏi cửa hàng có hỗ trợ trả góp 0% không?')" class="px-2.5 py-1 rounded-full bg-slate-100 hover:bg-blue-50 hover:text-[#0066FF] text-slate-600 text-[11px] font-medium whitespace-nowrap transition-colors cursor-pointer">
              💳 Trả góp 0%?
            </button>
            <button type="button" onclick="window.SamaChat.sendQuickMessage('Shop kiểm tra giúp mình đơn hàng mới đặt với ạ')" class="px-2.5 py-1 rounded-full bg-slate-100 hover:bg-blue-50 hover:text-[#0066FF] text-slate-600 text-[11px] font-medium whitespace-nowrap transition-colors cursor-pointer">
              📦 Kiểm tra đơn hàng
            </button>
            <button type="button" onclick="window.SamaChat.sendQuickMessage('Chính sách bảo hành và đổi trả của bên mình thế nào?')" class="px-2.5 py-1 rounded-full bg-slate-100 hover:bg-blue-50 hover:text-[#0066FF] text-slate-600 text-[11px] font-medium whitespace-nowrap transition-colors cursor-pointer">
              🛡️ Chính sách bảo hành
            </button>
          </div>

          <!-- KHUNG SOẠN TIN NHẮN (INPUT BAR) -->
          <div class="p-2.5 bg-white border-t border-slate-200/90 flex-shrink-0">
            <form id="sama-chat-send-form" onsubmit="window.SamaChat.sendMessage(event)" class="flex items-center gap-2">
              <input type="text" id="sama-chat-message-input" placeholder="Nhập tin nhắn..." autocomplete="off" class="flex-1 px-3.5 py-2.5 rounded-2xl bg-slate-100 focus:bg-white border border-transparent focus:border-[#0066FF] focus:outline-none text-xs text-slate-800 placeholder:text-slate-400 font-medium transition-all">
              
              <button type="submit" id="sama-chat-btn-send" class="w-9 h-9 rounded-xl bg-[#0066FF] hover:bg-[#0052cc] text-white flex items-center justify-center text-xs shadow-md transition-all active:scale-90 cursor-pointer flex-shrink-0" title="Gửi tin nhắn">
                <i class="fa-solid fa-paper-plane"></i>
              </button>
            </form>
          </div>

        </div>

      </div>
    `;

    document.body.appendChild(root);
  }

  // Khởi tạo và kết nối SignalR Hub cho Chat
  async function initChatSignalR() {
    if (chatConnection) return chatConnection;

    if (typeof signalR === 'undefined') {
      await loadSignalRLibrary();
    }

    try {
      chatConnection = new signalR.HubConnectionBuilder()
        .withUrl(CHAT_HUB_URL, {
          skipNegotiation: false,
          transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
        })
        .withAutomaticReconnect([0, 2000, 5000, 10000, 20000])
        .configureLogging(signalR.LogLevel.Warning)
        .build();

      // 1. Nhận tin nhắn mới (từ Agent, Bot hoặc phản hồi của chính mình)
      chatConnection.on('ReceiveMessage', function (msg) {
        if (!msg) return;
        appendMessageBubble(msg);
        
        // Nếu widget đang đóng hoặc thu nhỏ và tin nhắn từ CSKH hoặc BOT -> Phát chuông + tăng badge
        if (!isChatOpen && msg.nguoiGui !== 'CUSTOMER') {
          unreadCount++;
          updateUnreadBadge();
          playChatChime();
        }
      });

      // 2. Nhân viên CSKH tiếp nhận phiên chat
      chatConnection.on('SessionAssigned', function (data) {
        const sub = document.getElementById('sama-chat-header-subtitle');
        const statusDot = document.getElementById('sama-chat-session-status-dot');
        const statusText = document.getElementById('sama-chat-session-status-text');

        if (sub) sub.textContent = `Đang chat với: ${data.hoTenNV || 'Tư vấn viên Sama'}`;
        if (statusDot) {
          statusDot.className = 'w-2 h-2 rounded-full bg-emerald-500';
        }
        if (statusText) {
          statusText.textContent = `Tư vấn viên ${data.hoTenNV || ''} đã tham gia cuộc trò chuyện`;
        }

        appendSystemNotice(`🎉 Nhân viên <strong>${data.hoTenNV || 'CSKH'}</strong> đã tiếp nhận phiên chat và đang phản hồi bạn.`);
        playChatChime();
      });

      // 3. Phiên chat kết thúc
      chatConnection.on('SessionClosed', function () {
        const sub = document.getElementById('sama-chat-header-subtitle');
        const statusDot = document.getElementById('sama-chat-session-status-dot');
        const statusText = document.getElementById('sama-chat-session-status-text');

        if (sub) sub.textContent = 'Phiên tư vấn đã kết thúc';
        if (statusDot) {
          statusDot.className = 'w-2 h-2 rounded-full bg-slate-400';
        }
        if (statusText) {
          statusText.textContent = 'Phiên chat đã kết thúc. Cảm ơn bạn!';
        }

        appendSystemNotice('🔒 Phiên tư vấn đã được đóng. Cảm ơn bạn đã liên hệ với SamaMobile!');
      });

      await chatConnection.start();

      // Nếu đã có phiên chat từ trước, tự động join room của phiên đó
      if (activeSessionId) {
        await chatConnection.invoke('JoinSession', activeSessionId);
      }

      return chatConnection;
    } catch (err) {
      return null;
    }
  }

  function loadSignalRLibrary() {
    return new Promise((resolve) => {
      if (typeof signalR !== 'undefined') return resolve();
      const s = document.createElement('script');
      s.src = 'https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/8.0.7/signalr.min.js';
      s.onload = () => resolve();
      s.onerror = () => resolve();
      document.head.appendChild(s);
    });
  }

  // Tải lịch sử tin nhắn của phiên chat hiện tại
  async function loadSessionMessages(sessionId) {
    if (!sessionId) return;
    try {
      const res = await fetch(`${API_BASE}/api/chat/sessions/${encodeURIComponent(sessionId)}/messages`);
      if (res.ok) {
        const result = await res.json();
        if (result && result.success && Array.isArray(result.data)) {
          const container = document.getElementById('sama-chat-messages-container');
          if (container) container.innerHTML = '';
          result.data.forEach(m => appendMessageBubble(m));
          scrollToBottom();
        }
      }
    } catch (e) {}
  }

  // Kiểm tra phiên chat còn hợp lệ không
  async function verifySession(sessionId) {
    if (!sessionId) return false;
    try {
      const res = await fetch(`${API_BASE}/api/chat/sessions/${encodeURIComponent(sessionId)}`);
      if (res.ok) {
        const result = await res.json();
        if (result && result.success && result.data) {
          return result.data.trangThai !== 'CLOSED';
        }
      }
      return false;
    } catch (e) {
      return false;
    }
  }

  // Hiển thị tin nhắn vào danh sách
  function appendMessageBubble(msg) {
    const container = document.getElementById('sama-chat-messages-container');
    if (!container) return;

    // Tránh render trùng lặp nếu tin nhắn đã có
    if (msg.maTinNhan && container.querySelector(`[data-msg-id="${msg.maTinNhan}"]`)) {
      return;
    }

    const isCustomer = msg.nguoiGui === 'CUSTOMER';
    const isBot = msg.nguoiGui === 'BOT';
    const timeStr = msg.thoiGian ? new Date(msg.thoiGian).toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' }) : '';

    const el = document.createElement('div');
    if (msg.maTinNhan) el.setAttribute('data-msg-id', msg.maTinNhan);

    if (isBot) {
      // Tin nhắn từ BOT tự động (SamaBot AI)
      el.className = 'flex items-start gap-2 max-w-[92%] sm:max-w-[85%] animate-in fade-in duration-200';
      el.innerHTML = `
        <div class="w-7 h-7 rounded-full bg-blue-50 flex items-center justify-center text-xs flex-shrink-0 mt-0.5 border border-blue-200 shadow-2xs overflow-hidden p-0.5">
          <img src="${IMG_PREFIX}/chat-mascot-head.png" alt="SamaBot" class="w-full h-full object-contain">
        </div>
        <div class="flex-1 min-w-0">
          <div class="text-[10px] text-slate-500 font-semibold mb-0.5 flex items-center gap-1">
            <span>SamaBot AI</span>
            <span class="text-[9px] px-1 rounded bg-blue-100 text-[#0066FF] font-semibold">Tự động</span>
          </div>
          <div class="p-3 rounded-2xl rounded-tl-xs bg-white text-slate-800 border border-slate-200/90 shadow-2xs leading-relaxed text-xs">
            ${escapeHtml(msg.noiDung)}
          </div>
          <div class="text-[9px] text-slate-400 mt-1 pl-1">${timeStr}</div>
        </div>
      `;
    } else if (isCustomer) {
      // Tin nhắn của Khách hàng (bên phải, màu xanh)
      el.className = 'flex flex-col items-end max-w-[85%] ml-auto animate-in fade-in duration-200';
      el.innerHTML = `
        <div class="p-3 rounded-2xl rounded-tr-xs bg-[#0066FF] text-white shadow-xs leading-relaxed text-xs">
          ${escapeHtml(msg.noiDung)}
        </div>
        <div class="text-[9px] text-slate-400 mt-1 pr-1 flex items-center gap-1">
          <span>${timeStr}</span>
          <i class="fa-solid fa-check text-[9px] text-[#0066FF]"></i>
        </div>
      `;
    } else {
      // Tin nhắn từ Nhân viên CSKH (bên trái, màu trắng viền xanh)
      el.className = 'flex items-start gap-2 max-w-[92%] sm:max-w-[85%] animate-in fade-in duration-200';
      el.innerHTML = `
        <div class="w-7 h-7 rounded-full bg-blue-100 text-[#0066FF] flex items-center justify-center text-xs flex-shrink-0 mt-0.5 border border-blue-200 shadow-2xs">
          <i class="fa-solid fa-user-tie"></i>
        </div>
        <div class="flex-1 min-w-0">
          <div class="text-[10px] text-slate-500 font-semibold mb-0.5 flex items-center gap-1">
            <span>Tư vấn viên</span>
            <span class="text-[9px] px-1 rounded bg-blue-100 text-[#0066FF]">CSKH</span>
          </div>
          <div class="p-3 rounded-2xl rounded-tl-xs bg-white text-slate-800 border border-blue-200/90 shadow-2xs leading-relaxed text-xs">
            ${escapeHtml(msg.noiDung)}
          </div>
          <div class="text-[9px] text-slate-400 mt-1 pl-1">${timeStr}</div>
        </div>
      `;
    }

    container.appendChild(el);
    scrollToBottom();
  }

  // Thêm thông báo hệ thống vào khung chat
  function appendSystemNotice(html) {
    const container = document.getElementById('sama-chat-messages-container');
    if (!container) return;
    const notice = document.createElement('div');
    notice.className = 'my-2 p-2 rounded-xl bg-slate-100/90 text-slate-600 text-[11px] text-center border border-slate-200/80 leading-normal animate-in fade-in duration-200';
    notice.innerHTML = html;
    container.appendChild(notice);
    scrollToBottom();
  }

  function scrollToBottom() {
    const container = document.getElementById('sama-chat-messages-container');
    if (container) {
      setTimeout(() => {
        container.scrollTop = container.scrollHeight;
      }, 50);
    }
  }

  function updateUnreadBadge() {
    const badge = document.getElementById('sama-chat-unread-badge');
    if (badge) {
      if (unreadCount > 0) {
        badge.textContent = unreadCount > 9 ? '9+' : unreadCount;
        badge.classList.remove('hidden');
      } else {
        badge.classList.add('hidden');
      }
    }
  }

  function escapeHtml(str) {
    if (!str) return '';
    return str
      .replace(/&/g, '&amp;')
      .replace(/</g, '&lt;')
      .replace(/>/g, '&gt;')
      .replace(/"/g, '&quot;')
      .replace(/'/g, '&#039;')
      .replace(/\n/g, '<br>');
  }

  // Khởi tạo thông tin người dùng từ localStorage nếu đã đăng nhập
  function prefillUserInfo() {
    try {
      const rawUser = localStorage.getItem('cellphones_auth_user') || localStorage.getItem('auth_user');
      if (rawUser) {
        const u = JSON.parse(rawUser);
        const nameInput = document.getElementById('sama-chat-input-name');
        const phoneInput = document.getElementById('sama-chat-input-phone');
        if (nameInput && (u.hoTen || u.name)) nameInput.value = u.hoTen || u.name;
        if (phoneInput && u.sdt) phoneInput.value = u.sdt;
      }
    } catch (e) {}
  }

  // CÔNG KHAI API RA WINDOW ĐỂ GỌI TRỰC TIẾP
  window.SamaChat = {
    toggleChat: function () {
      const chatBox = document.getElementById('sama-chat-box');
      const iconOpen = document.getElementById('sama-chat-icon-open');
      const iconClose = document.getElementById('sama-chat-icon-close');
      const tooltip = document.getElementById('sama-chat-tooltip');

      if (!chatBox) return;

      isChatOpen = !chatBox.classList.contains('hidden') ? false : true;

      if (isChatOpen) {
        chatBox.classList.remove('hidden');
        if (iconOpen) iconOpen.classList.add('hidden');
        if (iconClose) iconClose.classList.remove('hidden');
        if (tooltip) tooltip.classList.add('hidden');

        // Xóa badge chưa đọc khi mở
        unreadCount = 0;
        updateUnreadBadge();

        // Kết nối SignalR nếu chưa
        initChatSignalR();

        // Focus ô nhập tin nhắn
        const input = document.getElementById('sama-chat-message-input');
        if (input && !document.getElementById('sama-chat-active-view')?.classList.contains('hidden')) {
          setTimeout(() => input.focus(), 200);
        }
        scrollToBottom();
      } else {
        chatBox.classList.add('hidden');
        if (iconOpen) iconOpen.classList.remove('hidden');
        if (iconClose) iconClose.classList.add('hidden');
      }
    },

    minimizeChat: function () {
      this.toggleChat();
    },

    // Bắt đầu phiên chat mới
    startChat: async function (e) {
      if (e) e.preventDefault();
      const nameInput = document.getElementById('sama-chat-input-name');
      const phoneInput = document.getElementById('sama-chat-input-phone');
      const btnStart = document.getElementById('sama-chat-btn-start');

      const hoTen = nameInput?.value?.trim() || 'Khách hàng';
      const sdt = phoneInput?.value?.trim() || '';

      if (btnStart) {
        btnStart.disabled = true;
        btnStart.innerHTML = '<i class="fa-solid fa-spinner fa-spin mr-1.5"></i> Đang kết nối...';
      }

      try {
        const res = await fetch(`${API_BASE}/api/chat/sessions`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ hoTen, sdt })
        });

        if (res.ok) {
          const json = await res.json();
          if (json && json.success && json.data) {
            const session = json.data;
            activeSessionId = session.maPhienChat;
            try { localStorage.setItem(STORAGE_KEY_SESSION, activeSessionId); } catch (e) {}

            // Chuyển sang màn hình Chat Active
            this.showActiveChatView(session);

            // Kết nối SignalR và gia nhập phòng của phiên
            const conn = await initChatSignalR();
            if (conn) {
              await conn.invoke('JoinSession', activeSessionId);
            }

            // Tải lại lịch sử tin nhắn
            await loadSessionMessages(activeSessionId);
          }
        } else {
          alert('Không thể khởi tạo phiên chat. Vui lòng thử lại!');
        }
      } catch (err) {
        alert('Lỗi kết nối máy chủ chat: ' + err.message);
      } finally {
        if (btnStart) {
          btnStart.disabled = false;
          btnStart.innerHTML = '<span>Bắt đầu trò chuyện ngay</span> <i class="fa-solid fa-arrow-right text-[10px]"></i>';
        }
      }
    },

    // Gợi ý điền nhanh và bắt đầu chat
    quickFillAndStart: function (text) {
      const form = document.getElementById('sama-chat-init-form');
      if (form) {
        this.startChat().then(() => {
          setTimeout(() => {
            this.sendQuickMessage(text);
          }, 800);
        });
      }
    },

    showActiveChatView: function (session) {
      const initView = document.getElementById('sama-chat-init-view');
      const activeView = document.getElementById('sama-chat-active-view');
      const sessionCode = document.getElementById('sama-chat-session-code');
      const sub = document.getElementById('sama-chat-header-subtitle');
      const statusText = document.getElementById('sama-chat-session-status-text');
      const statusDot = document.getElementById('sama-chat-session-status-dot');

      if (initView) initView.classList.add('hidden');
      if (activeView) activeView.classList.remove('hidden');

      if (sessionCode) sessionCode.textContent = session.maPhienChat;
      if (sub) {
        sub.textContent = session.trangThai === 'ACTIVE'
          ? 'Đang có nhân viên hỗ trợ trực tiếp'
          : 'Đã gửi yêu cầu tới tư vấn viên...';
      }
      if (statusText) {
        statusText.textContent = session.trangThai === 'ACTIVE'
          ? 'Tư vấn viên đang kết nối'
          : 'Đang xếp hàng chờ nhân viên...';
      }
      if (statusDot) {
        statusDot.className = session.trangThai === 'ACTIVE'
          ? 'w-2 h-2 rounded-full bg-emerald-500'
          : 'w-2 h-2 rounded-full bg-amber-500 animate-pulse';
      }

      const input = document.getElementById('sama-chat-message-input');
      if (input) setTimeout(() => input.focus(), 200);
    },

    // Gửi tin nhắn từ người dùng
    sendMessage: async function (e) {
      if (e) e.preventDefault();
      const input = document.getElementById('sama-chat-message-input');
      const noiDung = input?.value?.trim();
      if (!noiDung || !activeSessionId) return;

      input.value = '';

      try {
        const conn = await initChatSignalR();
        if (conn && conn.state === signalR.HubConnectionState.Connected) {
          await conn.invoke('SendMessage', activeSessionId, 'CUSTOMER', noiDung, null);
        } else {
          // Fallback nếu SignalR chưa sẵn sàng
          appendMessageBubble({
            maTinNhan: Date.now(),
            maPhienChat: activeSessionId,
            nguoiGui: 'CUSTOMER',
            noiDung: noiDung,
            thoiGian: new Date().toISOString()
          });
        }
      } catch (err) {
        // Fallback hiển thị tin nhắn tạm
        appendMessageBubble({
          maTinNhan: Date.now(),
          maPhienChat: activeSessionId,
          nguoiGui: 'CUSTOMER',
          noiDung: noiDung,
          thoiGian: new Date().toISOString()
        });
      }
    },

    sendQuickMessage: function (text) {
      if (!text) return;
      const input = document.getElementById('sama-chat-message-input');
      if (input) input.value = text;
      this.sendMessage();
    },

    // Kết thúc phiên chat hiện tại
    endCurrentChat: async function () {
      if (!activeSessionId) return;
      if (!confirm('Bạn có chắc muốn kết thúc phiên tư vấn hiện tại?')) return;

      try {
        await fetch(`${API_BASE}/api/chat/sessions/${encodeURIComponent(activeSessionId)}/close`, {
          method: 'POST'
        });
      } catch (e) {}

      localStorage.removeItem(STORAGE_KEY_SESSION);
      activeSessionId = null;

      const initView = document.getElementById('sama-chat-init-view');
      const activeView = document.getElementById('sama-chat-active-view');
      const msgsContainer = document.getElementById('sama-chat-messages-container');

      if (initView) initView.classList.remove('hidden');
      if (activeView) activeView.classList.add('hidden');
      if (msgsContainer) msgsContainer.innerHTML = '';
      
      const sub = document.getElementById('sama-chat-header-subtitle');
      if (sub) sub.textContent = 'Tư vấn viên sẵn sàng hỗ trợ bạn';
    }
  };

  // Khởi chạy khi DOM sẵn sàng
  document.addEventListener('DOMContentLoaded', async () => {
    injectChatWidgetHTML();
    prefillUserInfo();

    // Kiểm tra xem khách đã có phiên chat cũ đang mở dở không
    try {
      const savedSession = localStorage.getItem(STORAGE_KEY_SESSION);
      if (savedSession) {
        const isValid = await verifySession(savedSession);
        if (isValid) {
          activeSessionId = savedSession;
          window.SamaChat.showActiveChatView({ maPhienChat: savedSession, trangThai: 'WAITING' });
          await initChatSignalR();
          await loadSessionMessages(savedSession);
        } else {
          localStorage.removeItem(STORAGE_KEY_SESSION);
        }
      }
    } catch (e) {}
  });

})();
