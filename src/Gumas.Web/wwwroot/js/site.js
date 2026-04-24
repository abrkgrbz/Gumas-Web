/**
 * GÜMAŞ Otomotiv - Site JavaScript
 * Premium Web Experience
 */

(function () {
    'use strict';

    // ========================================
    // Back to Top Button
    // ========================================
    const backToTopBtn = document.getElementById('backToTop');

    if (backToTopBtn) {
        window.addEventListener('scroll', function () {
            if (window.scrollY > 300) {
                backToTopBtn.classList.add('visible');
            } else {
                backToTopBtn.classList.remove('visible');
            }
        });

        backToTopBtn.addEventListener('click', function () {
            window.scrollTo({
                top: 0,
                behavior: 'smooth'
            });
        });
    }

    // ========================================
    // Navbar Scroll Effect
    // ========================================
    const mainNav = document.querySelector('.main-nav');

    if (mainNav) {
        window.addEventListener('scroll', function () {
            if (window.scrollY > 50) {
                mainNav.style.background = 'rgba(10, 10, 10, 0.98)';
                mainNav.style.boxShadow = '0 4px 20px rgba(0, 0, 0, 0.3)';
            } else {
                mainNav.style.background = 'var(--bg-dark)';
                mainNav.style.boxShadow = 'none';
            }
        });
    }

    // ========================================
    // Scroll Animations (Intersection Observer)
    // ========================================
    const animateElements = document.querySelectorAll('.animate-fade-up, .animate-fade-right, .animate-fade-left, .animate-scale');

    if (animateElements.length > 0) {
        const observerOptions = {
            root: null,
            rootMargin: '0px',
            threshold: 0.1
        };

        const observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.style.animationPlayState = 'running';
                    observer.unobserve(entry.target);
                }
            });
        }, observerOptions);

        animateElements.forEach(function (el) {
            el.style.animationPlayState = 'paused';
            observer.observe(el);
        });
    }

    // ========================================
    // Counter Animation
    // ========================================
    const counters = document.querySelectorAll('.stat-number[data-count]');

    if (counters.length > 0) {
        const counterObserver = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    const counter = entry.target;
                    const target = parseInt(counter.getAttribute('data-count'));
                    const duration = 2000;
                    const step = target / (duration / 16);
                    let current = 0;

                    const updateCounter = function () {
                        current += step;
                        if (current < target) {
                            counter.textContent = Math.floor(current).toLocaleString('tr-TR');
                            requestAnimationFrame(updateCounter);
                        } else {
                            counter.textContent = target.toLocaleString('tr-TR') + '+';
                        }
                    };

                    updateCounter();
                    counterObserver.unobserve(counter);
                }
            });
        }, { threshold: 0.5 });

        counters.forEach(function (counter) {
            counterObserver.observe(counter);
        });
    }

    // ========================================
    // Smooth Scroll for Anchor Links
    // ========================================
    document.querySelectorAll('a[href^="#"]').forEach(function (anchor) {
        anchor.addEventListener('click', function (e) {
            const href = this.getAttribute('href');
            if (href !== '#' && href.length > 1) {
                e.preventDefault();
                const target = document.querySelector(href);
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    });

    // ========================================
    // Mobile Menu Close on Link Click
    // ========================================
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    const navbarCollapse = document.querySelector('.navbar-collapse');

    navLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            if (navbarCollapse && navbarCollapse.classList.contains('show')) {
                const bsCollapse = bootstrap.Collapse.getInstance(navbarCollapse);
                if (bsCollapse) {
                    bsCollapse.hide();
                }
            }
        });
    });

    // ========================================
    // Image Lazy Loading Enhancement
    // ========================================
    const lazyImages = document.querySelectorAll('img[loading="lazy"]');

    if ('loading' in HTMLImageElement.prototype) {
        // Native lazy loading supported
        lazyImages.forEach(function (img) {
            img.addEventListener('load', function () {
                img.classList.add('loaded');
            });
        });
    } else {
        // Fallback for older browsers
        const imageObserver = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    const img = entry.target;
                    img.src = img.dataset.src || img.src;
                    img.classList.add('loaded');
                    imageObserver.unobserve(img);
                }
            });
        });

        lazyImages.forEach(function (img) {
            imageObserver.observe(img);
        });
    }

    // ========================================
    // Form Validation Enhancement
    // ========================================
    const forms = document.querySelectorAll('form');

    forms.forEach(function (form) {
        form.addEventListener('submit', function (e) {
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn && form.checkValidity()) {
                submitBtn.disabled = true;
                submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Gönderiliyor...';
            }
        });
    });

    // ========================================
    // Product Image Zoom Effect
    // ========================================
    const productImages = document.querySelectorAll('.product-image img, .main-image img');

    productImages.forEach(function (img) {
        img.addEventListener('mouseenter', function () {
            this.style.cursor = 'zoom-in';
        });
    });

    // ========================================
    // Tooltip Initialization (if Bootstrap)
    // ========================================
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    if (tooltipTriggerList.length > 0 && typeof bootstrap !== 'undefined') {
        tooltipTriggerList.forEach(function (tooltipTriggerEl) {
            new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    // ========================================
    // Active Nav Link Highlight
    // ========================================
    const currentPath = window.location.pathname;
    const navLinksAll = document.querySelectorAll('.navbar-nav .nav-link');

    navLinksAll.forEach(function (link) {
        const href = link.getAttribute('href');
        if (href === currentPath || (currentPath === '/' && href === '/')) {
            link.classList.add('active');
        } else if (href !== '/' && currentPath.startsWith(href)) {
            link.classList.add('active');
        }
    });

    // ========================================
    // Console Welcome Message
    // ========================================
    console.log('%c GÜMAŞ Otomotiv ', 'background: #E31E24; color: white; font-size: 20px; font-weight: bold; padding: 10px 20px; border-radius: 5px;');
    console.log('%c Ağır Vasıta Yedek Parça Tedarikçisi ', 'color: #666; font-size: 12px;');

})();
