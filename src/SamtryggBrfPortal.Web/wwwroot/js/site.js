// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Initialize Bootstrap tooltips
document.addEventListener('DOMContentLoaded', function() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize Bootstrap popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });
});

// Function to show confirmation dialog
function confirmAction(message, callback) {
    if (confirm(message)) {
        callback();
    }
}

// Function to format dates in Swedish format
function formatDate(dateString) {
    if (!dateString) return '';
    const date = new Date(dateString);
    return date.toLocaleDateString('sv-SE', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit'
    });
}

// Function to format currency in Swedish format
function formatCurrency(amount) {
    if (amount === undefined || amount === null) return '';
    return new Intl.NumberFormat('sv-SE', {
        style: 'currency',
        currency: 'SEK',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    }).format(amount);
}

// Simple notification system
const notificationSystem = {
    show: function(message, type = 'info', duration = 5000) {
        const container = document.getElementById('notification-container');
        if (!container) {
            // Create container if it doesn't exist
            const newContainer = document.createElement('div');
            newContainer.id = 'notification-container';
            newContainer.style.position = 'fixed';
            newContainer.style.top = '20px';
            newContainer.style.right = '20px';
            newContainer.style.zIndex = '9999';
            document.body.appendChild(newContainer);
            
            // Use the newly created container
            this.show(message, type, duration);
            return;
        }
        
        // Create notification element
        const notification = document.createElement('div');
        notification.className = `alert alert-${type} alert-dismissible fade show`;
        notification.role = 'alert';
        notification.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        `;
        
        // Add notification to container
        container.appendChild(notification);
        
        // Auto-close after duration
        setTimeout(() => {
            try {
                notification.classList.remove('show');
                setTimeout(() => {
                    container.removeChild(notification);
                }, 150);
            } catch (e) {
                // Notification might have been closed by user
            }
        }, duration);
    },
    
    info: function(message, duration) {
        this.show(message, 'info', duration);
    },
    
    success: function(message, duration) {
        this.show(message, 'success', duration);
    },
    
    warning: function(message, duration) {
        this.show(message, 'warning', duration);
    },
    
    error: function(message, duration) {
        this.show(message, 'danger', duration);
    }
};