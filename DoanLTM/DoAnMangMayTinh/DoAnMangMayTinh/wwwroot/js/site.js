// ===== GLOBAL FUNCTIONS =====

// Show notification toast
function showNotification(message, type = 'info', duration = 5000) {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    notification.style.cssText = `
        top: 20px;
        right: 20px;
        z-index: 9999;
        min-width: 300px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    `;
    
    const icon = type === 'success' ? 'fa-check-circle' : 
                 type === 'warning' ? 'fa-exclamation-triangle' : 
                 type === 'danger' ? 'fa-times-circle' : 'fa-info-circle';
    
    notification.innerHTML = `
        <i class="fas ${icon} me-2"></i>
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(notification);
    
    // Auto remove after duration
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, duration);
}

// Loading spinner
function showLoading(element) {
    const spinner = document.createElement('div');
    spinner.className = 'loading';
    spinner.innerHTML = '<div class="spinner-border spinner-border-sm" role="status"></div>';
    element.appendChild(spinner);
    element.disabled = true;
}

function hideLoading(element) {
    const spinner = element.querySelector('.loading');
    if (spinner) {
        spinner.remove();
    }
    element.disabled = false;
}

// Confirm dialog
function confirmAction(message, callback) {
    if (confirm(message)) {
        callback();
    }
}

// Format date
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('vi-VN');
}

// Format currency
function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}

// ===== TABLE ENHANCEMENTS =====

// Initialize data tables
function initializeDataTable(tableId, options = {}) {
    const table = document.getElementById(tableId);
    if (!table) return;

    // Add search functionality
    const searchInput = document.createElement('input');
    searchInput.type = 'text';
    searchInput.className = 'form-control mb-3';
    searchInput.placeholder = 'Tìm kiếm...';
    table.parentNode.insertBefore(searchInput, table);

    searchInput.addEventListener('keyup', function() {
        const searchTerm = this.value.toLowerCase();
        const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');
        
        for (let row of rows) {
            const text = row.textContent.toLowerCase();
            row.style.display = text.includes(searchTerm) ? '' : 'none';
        }
    });

    // Add hover effects
    const rows = table.getElementsByTagName('tbody')[0].getElementsByTagName('tr');
    for (let row of rows) {
        row.addEventListener('mouseenter', function() {
            this.style.backgroundColor = 'rgba(102, 126, 234, 0.05)';
        });
        
        row.addEventListener('mouseleave', function() {
            this.style.backgroundColor = '';
        });
    }
}

// ===== FORM ENHANCEMENTS =====

// Auto-save form data
function autoSaveForm(formId, key) {
    const form = document.getElementById(formId);
    if (!form) return;

    const inputs = form.querySelectorAll('input, textarea, select');
    
    // Load saved data
    const savedData = localStorage.getItem(key);
    if (savedData) {
        const data = JSON.parse(savedData);
        inputs.forEach(input => {
            if (data[input.name]) {
                input.value = data[input.name];
            }
        });
    }

    // Save on input change
    inputs.forEach(input => {
        input.addEventListener('input', function() {
            const formData = {};
            inputs.forEach(inp => {
                formData[inp.name] = inp.value;
            });
            localStorage.setItem(key, JSON.stringify(formData));
        });
    });

    // Clear on successful submit
    form.addEventListener('submit', function() {
        localStorage.removeItem(key);
    });
}

// Form validation
function validateForm(formId) {
    const form = document.getElementById(formId);
    if (!form) return false;

    let isValid = true;
    const inputs = form.querySelectorAll('input[required], textarea[required], select[required]');
    
    inputs.forEach(input => {
        if (!input.value.trim()) {
            input.classList.add('is-invalid');
            isValid = false;
        } else {
            input.classList.remove('is-invalid');
        }
    });

    return isValid;
}

// ===== CHART FUNCTIONS =====

// Create simple bar chart
function createBarChart(canvasId, data, options = {}) {
    const canvas = document.getElementById(canvasId);
    if (!canvas) return;

    const ctx = canvas.getContext('2d');
    const { labels, values, colors } = data;

    const maxValue = Math.max(...values);
    const barWidth = canvas.width / labels.length - 10;
    const barHeight = canvas.height - 40;

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    labels.forEach((label, index) => {
        const x = index * (barWidth + 10) + 5;
        const height = (values[index] / maxValue) * barHeight;
        const y = canvas.height - height - 20;

        // Draw bar
        ctx.fillStyle = colors[index] || '#667eea';
        ctx.fillRect(x, y, barWidth, height);

        // Draw label
        ctx.fillStyle = '#2c3e50';
        ctx.font = '12px Arial';
        ctx.textAlign = 'center';
        ctx.fillText(label, x + barWidth / 2, canvas.height - 5);
        ctx.fillText(values[index], x + barWidth / 2, y - 5);
    });
}

// ===== UTILITY FUNCTIONS =====

// Debounce function
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

// Throttle function
function throttle(func, limit) {
    let inThrottle;
    return function() {
        const args = arguments;
        const context = this;
        if (!inThrottle) {
            func.apply(context, args);
            inThrottle = true;
            setTimeout(() => inThrottle = false, limit);
        }
    }
}

// Copy to clipboard
function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(() => {
        showNotification('Đã sao chép vào clipboard!', 'success');
    }).catch(() => {
        showNotification('Không thể sao chép!', 'danger');
    });
}

// ===== ANIMATION FUNCTIONS =====

// Fade in animation
function fadeIn(element, duration = 300) {
    element.style.opacity = '0';
    element.style.display = 'block';
    
    let start = null;
    function animate(timestamp) {
        if (!start) start = timestamp;
        const progress = timestamp - start;
        const opacity = Math.min(progress / duration, 1);
        
        element.style.opacity = opacity;
        
        if (progress < duration) {
            requestAnimationFrame(animate);
        }
    }
    requestAnimationFrame(animate);
}

// Slide down animation
function slideDown(element, duration = 300) {
    element.style.height = '0';
    element.style.overflow = 'hidden';
    element.style.display = 'block';
    
    const targetHeight = element.scrollHeight;
    let start = null;
    
    function animate(timestamp) {
        if (!start) start = timestamp;
        const progress = timestamp - start;
        const height = Math.min((progress / duration) * targetHeight, targetHeight);
        
        element.style.height = height + 'px';
        
        if (progress < duration) {
            requestAnimationFrame(animate);
        } else {
            element.style.height = 'auto';
        }
    }
    requestAnimationFrame(animate);
}

// ===== INITIALIZATION =====

document.addEventListener('DOMContentLoaded', function() {
    // Initialize tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize popovers
    const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Add smooth scrolling to all links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // Add loading states to buttons
    document.querySelectorAll('button[type="submit"]').forEach(button => {
        button.addEventListener('click', function() {
            if (this.form && validateForm(this.form.id)) {
                showLoading(this);
            }
        });
    });

    // Auto-save forms
    document.querySelectorAll('form[data-autosave]').forEach(form => {
        const key = form.getAttribute('data-autosave');
        autoSaveForm(form.id, key);
    });

    // Initialize data tables
    document.querySelectorAll('table[data-table]').forEach(table => {
        initializeDataTable(table.id);
    });

    // Add copy functionality to code blocks
    document.querySelectorAll('code').forEach(code => {
        code.addEventListener('click', function() {
            copyToClipboard(this.textContent);
        });
        code.style.cursor = 'pointer';
        code.title = 'Click để sao chép';
    });

    // Add keyboard shortcuts
    document.addEventListener('keydown', function(e) {
        // Ctrl/Cmd + S to save
        if ((e.ctrlKey || e.metaKey) && e.key === 's') {
            e.preventDefault();
            const saveButton = document.querySelector('button[type="submit"]');
            if (saveButton) {
                saveButton.click();
            }
        }

        // Ctrl/Cmd + K to search
        if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
            e.preventDefault();
            const searchInput = document.querySelector('input[type="search"], #searchInput');
            if (searchInput) {
                searchInput.focus();
            }
        }
    });

    // Add intersection observer for animations
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('fade-in-up');
            }
        });
    }, observerOptions);

    document.querySelectorAll('.dashboard-card, .content-card').forEach(card => {
        observer.observe(card);
    });
});

// ===== EXPORT FUNCTIONS =====

// Export table to CSV
function exportTableToCSV(tableId, filename = 'export.csv') {
    const table = document.getElementById(tableId);
    if (!table) return;

    let csv = [];
    const rows = table.querySelectorAll('tr');
    
    for (let row of rows) {
        const cols = row.querySelectorAll('td, th');
        const rowData = [];
        
        for (let col of cols) {
            rowData.push('"' + col.textContent.replace(/"/g, '""') + '"');
        }
        
        csv.push(rowData.join(','));
    }

    const csvContent = csv.join('\n');
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    
    if (link.download !== undefined) {
        const url = URL.createObjectURL(blob);
        link.setAttribute('href', url);
        link.setAttribute('download', filename);
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}

// Export table to PDF (requires jsPDF library)
function exportTableToPDF(tableId, filename = 'export.pdf') {
    // This would require jsPDF library to be included
    showNotification('Tính năng xuất PDF sẽ được triển khai sau!', 'info');
}

// ===== REAL-TIME UPDATES =====

let signalRConnection = null;

function initializeSignalR() {
    try {
        signalRConnection = new signalR.HubConnectionBuilder()
            .withUrl("/lichthiHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        signalRConnection
            .start()
            .then(() => {
                console.log("SignalR connected");
                showNotification("Kết nối thời gian thực đã được thiết lập", "success");
            })
            .catch((err) => {
                console.error("Kết nối SignalR thất bại:", err);
                showNotification("Kết nối thời gian thực bị lỗi", "danger");
                setTimeout(initializeSignalR, 5000);
            });

        // Nhận thông điệp từ server
        signalRConnection.on("ReceiveNotification", (message) => {
            showNotification(message, "info");
        });

        signalRConnection.on("ReceiveRealTimeUpdate", (data) => {
            handleRealTimeUpdate(data);
        });

    } catch (error) {
        console.error("Lỗi khi khởi tạo SignalR:", error);
    }
}


function handleRealTimeUpdate(data) {
    switch (data.type) {
        case 'notification':
            showNotification(data.message, data.level || 'info');
            break;
        case 'data_update':
            if (data.target) {
                const element = document.getElementById(data.target);
                if (element) {
                    element.innerHTML = data.content;
                }
            }
            break;
        case 'user_activity':
            updateUserActivity(data);
            break;
    }
}

function updateUserActivity(data) {
    const activityElement = document.getElementById('userActivity');
    if (activityElement) {
        activityElement.innerHTML = data.html;
    }
}

if (typeof WebSocket !== 'undefined') {
    initializeWebSocket();
}

function showToast(message) {
    const container = document.getElementById("toast-container");

    const toast = document.createElement("div");
    toast.className = "toast align-items-center text-white bg-primary border-0 show";
    toast.role = "alert";
    toast.ariaLive = "assertive";
    toast.ariaAtomic = "true";
    toast.style.minWidth = "250px";
    toast.style.marginTop = "10px";
    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
        </div>
    `;

    container.appendChild(toast);

    setTimeout(() => {
        toast.classList.remove("show");
        toast.classList.add("hide");
        setTimeout(() => toast.remove(), 500);
    }, 5000);
}