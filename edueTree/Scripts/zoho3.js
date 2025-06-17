/*!
	Colorbox v1.5.13 - 2014-08-04
	jQuery lightbox and modal window plugin
	(c) 2014 Jack Moore - http://www.jacklmoore.com/colorbox
	license: http://www.opensource.org/licenses/mit-license.php
*/
(function (t, e, i) { function n(i, n, o) { var r = e.createElement(i); return n && (r.id = Z + n), o && (r.style.cssText = o), t(r) } function o() { return i.innerHeight ? i.innerHeight : t(i).height() } function r(e, i) { i !== Object(i) && (i = {}), this.cache = {}, this.el = e, this.value = function (e) { var n; return void 0 === this.cache[e] && (n = t(this.el).attr("data-cbox-" + e), void 0 !== n ? this.cache[e] = n : void 0 !== i[e] ? this.cache[e] = i[e] : void 0 !== X[e] && (this.cache[e] = X[e])), this.cache[e] }, this.get = function (e) { var i = this.value(e); return t.isFunction(i) ? i.call(this.el, this) : i } } function h(t) { var e = W.length, i = (z + t) % e; return 0 > i ? e + i : i } function a(t, e) { return Math.round((/%/.test(t) ? ("x" === e ? E.width() : o()) / 100 : 1) * parseInt(t, 10)) } function s(t, e) { return t.get("photo") || t.get("photoRegex").test(e) } function l(t, e) { return t.get("retinaUrl") && i.devicePixelRatio > 1 ? e.replace(t.get("photoRegex"), t.get("retinaSuffix")) : e } function d(t) { "contains" in y[0] && !y[0].contains(t.target) && t.target !== v[0] && (t.stopPropagation(), y.focus()) } function c(t) { c.str !== t && (y.add(v).removeClass(c.str).addClass(t), c.str = t) } function g(e) { z = 0, e && e !== !1 && "nofollow" !== e ? (W = t("." + te).filter(function () { var i = t.data(this, Y), n = new r(this, i); return n.get("rel") === e }), z = W.index(_.el), -1 === z && (W = W.add(_.el), z = W.length - 1)) : W = t(_.el) } function u(i) { t(e).trigger(i), ae.triggerHandler(i) } function f(i) { var o; if (!G) { if (o = t(i).data(Y), _ = new r(i, o), g(_.get("rel")), !$) { $ = q = !0, c(_.get("className")), y.css({ visibility: "hidden", display: "block", opacity: "" }), L = n(se, "LoadedContent", "width:0; height:0; overflow:hidden; visibility:hidden"), b.css({ width: "", height: "" }).append(L), D = T.height() + k.height() + b.outerHeight(!0) - b.height(), j = C.width() + H.width() + b.outerWidth(!0) - b.width(), A = L.outerHeight(!0), N = L.outerWidth(!0); var h = a(_.get("initialWidth"), "x"), s = a(_.get("initialHeight"), "y"), l = _.get("maxWidth"), f = _.get("maxHeight"); _.w = (l !== !1 ? Math.min(h, a(l, "x")) : h) - N - j, _.h = (f !== !1 ? Math.min(s, a(f, "y")) : s) - A - D, L.css({ width: "", height: _.h }), J.position(), u(ee), _.get("onOpen"), O.add(I).hide(), y.focus(), _.get("trapFocus") && e.addEventListener && (e.addEventListener("focus", d, !0), ae.one(re, function () { e.removeEventListener("focus", d, !0) })), _.get("returnFocus") && ae.one(re, function () { t(_.el).focus() }) } var p = parseFloat(_.get("opacity")); v.css({ opacity: p === p ? p : "", cursor: _.get("overlayClose") ? "pointer" : "", visibility: "visible" }).show(), _.get("closeButton") ? B.html(_.get("close")).appendTo(b) : B.appendTo("<div/>"), w() } } function p() { !y && e.body && (V = !1, E = t(i), y = n(se).attr({ id: Y, "class": t.support.opacity === !1 ? Z + "IE" : "", role: "dialog", tabindex: "-1" }).hide(), v = n(se, "Overlay").hide(), S = t([n(se, "LoadingOverlay")[0], n(se, "LoadingGraphic")[0]]), x = n(se, "Wrapper"), b = n(se, "Content").append(I = n(se, "Title"), R = n(se, "Current"), P = t('<button type="button"/>').attr({ id: Z + "Previous" }), K = t('<button type="button"/>').attr({ id: Z + "Next" }), F = n("button", "Slideshow"), S), B = t('<button type="button"/>').attr({ id: Z + "Close" }), x.append(n(se).append(n(se, "TopLeft"), T = n(se, "TopCenter"), n(se, "TopRight")), n(se, !1, "clear:left").append(C = n(se, "MiddleLeft"), b, H = n(se, "MiddleRight")), n(se, !1, "clear:left").append(n(se, "BottomLeft"), k = n(se, "BottomCenter"), n(se, "BottomRight"))).find("div div").css({ "float": "left" }), M = n(se, !1, "position:absolute; width:9999px; visibility:hidden; display:none; max-width:none;"), O = K.add(P).add(R).add(F), t(e.body).append(v, y.append(x, M))) } function m() { function i(t) { t.which > 1 || t.shiftKey || t.altKey || t.metaKey || t.ctrlKey || (t.preventDefault(), f(this)) } return y ? (V || (V = !0, K.click(function () { J.next() }), P.click(function () { J.prev() }), B.click(function () { J.close() }), v.click(function () { _.get("overlayClose") && J.close() }), t(e).bind("keydown." + Z, function (t) { var e = t.keyCode; $ && _.get("escKey") && 27 === e && (t.preventDefault(), J.close()), $ && _.get("arrowKey") && W[1] && !t.altKey && (37 === e ? (t.preventDefault(), P.click()) : 39 === e && (t.preventDefault(), K.click())) }), t.isFunction(t.fn.on) ? t(e).on("click." + Z, "." + te, i) : t("." + te).live("click." + Z, i)), !0) : !1 } function w() { var e, o, r, h = J.prep, d = ++le; if (q = !0, U = !1, u(he), u(ie), _.get("onLoad"), _.h = _.get("height") ? a(_.get("height"), "y") - A - D : _.get("innerHeight") && a(_.get("innerHeight"), "y"), _.w = _.get("width") ? a(_.get("width"), "x") - N - j : _.get("innerWidth") && a(_.get("innerWidth"), "x"), _.mw = _.w, _.mh = _.h, _.get("maxWidth") && (_.mw = a(_.get("maxWidth"), "x") - N - j, _.mw = _.w && _.w < _.mw ? _.w : _.mw), _.get("maxHeight") && (_.mh = a(_.get("maxHeight"), "y") - A - D, _.mh = _.h && _.h < _.mh ? _.h : _.mh), e = _.get("href"), Q = setTimeout(function () { S.show() }, 100), _.get("inline")) { var c = t(e); r = t("<div>").hide().insertBefore(c), ae.one(he, function () { r.replaceWith(c) }), h(c) } else _.get("iframe") ? h(" ") : _.get("html") ? h(_.get("html")) : s(_, e) ? (e = l(_, e), U = new Image, t(U).addClass(Z + "Photo").bind("error", function () { h(n(se, "Error").html(_.get("imgError"))) }).one("load", function () { d === le && setTimeout(function () { var e; t.each(["alt", "longdesc", "aria-describedby"], function (e, i) { var n = t(_.el).attr(i) || t(_.el).attr("data-" + i); n && U.setAttribute(i, n) }), _.get("retinaImage") && i.devicePixelRatio > 1 && (U.height = U.height / i.devicePixelRatio, U.width = U.width / i.devicePixelRatio), _.get("scalePhotos") && (o = function () { U.height -= U.height * e, U.width -= U.width * e }, _.mw && U.width > _.mw && (e = (U.width - _.mw) / U.width, o()), _.mh && U.height > _.mh && (e = (U.height - _.mh) / U.height, o())), _.h && (U.style.marginTop = Math.max(_.mh - U.height, 0) / 2 + "px"), W[1] && (_.get("loop") || W[z + 1]) && (U.style.cursor = "pointer", U.onclick = function () { J.next() }), U.style.width = U.width + "px", U.style.height = U.height + "px", h(U) }, 1) }), U.src = e) : e && M.load(e, _.get("data"), function (e, i) { d === le && h("error" === i ? n(se, "Error").html(_.get("xhrError")) : t(this).contents()) }) } var v, y, x, b, T, C, H, k, W, E, L, M, S, I, R, F, K, P, B, O, _, D, j, A, N, z, U, $, q, G, Q, J, V, X = { html: !1, photo: !1, iframe: !1, inline: !1, transition: "elastic", speed: 300, fadeOut: 300, width: !1, initialWidth: "600", innerWidth: !1, maxWidth: !1, height: !1, initialHeight: "450", innerHeight: !1, maxHeight: !1, scalePhotos: !0, scrolling: !0, opacity: .9, preloading: !0, className: !1, overlayClose: !0, escKey: !0, arrowKey: !0, top: !1, bottom: !1, left: !1, right: !1, fixed: !1, data: void 0, closeButton: !0, fastIframe: !0, open: !1, reposition: !0, loop: !0, slideshow: !1, slideshowAuto: !0, slideshowSpeed: 2500, slideshowStart: "start slideshow", slideshowStop: "stop slideshow", photoRegex: /\.(gif|png|jp(e|g|eg)|bmp|ico|webp|jxr|svg)((#|\?).*)?$/i, retinaImage: !1, retinaUrl: !1, retinaSuffix: "@2x.$1", current: "image {current} of {total}", previous: "previous", next: "next", close: "close", xhrError: "This content failed to load.", imgError: "This image failed to load.", returnFocus: !0, trapFocus: !0, onOpen: !1, onLoad: !1, onComplete: !1, onCleanup: !1, onClosed: !1, rel: function () { return this.rel }, href: function () { return t(this).attr("href") }, title: function () { return this.title } }, Y = "colorbox", Z = "cbox", te = Z + "Element", ee = Z + "_open", ie = Z + "_load", ne = Z + "_complete", oe = Z + "_cleanup", re = Z + "_closed", he = Z + "_purge", ae = t("<a/>"), se = "div", le = 0, de = {}, ce = function () { function t() { clearTimeout(h) } function e() { (_.get("loop") || W[z + 1]) && (t(), h = setTimeout(J.next, _.get("slideshowSpeed"))) } function i() { F.html(_.get("slideshowStop")).unbind(s).one(s, n), ae.bind(ne, e).bind(ie, t), y.removeClass(a + "off").addClass(a + "on") } function n() { t(), ae.unbind(ne, e).unbind(ie, t), F.html(_.get("slideshowStart")).unbind(s).one(s, function () { J.next(), i() }), y.removeClass(a + "on").addClass(a + "off") } function o() { r = !1, F.hide(), t(), ae.unbind(ne, e).unbind(ie, t), y.removeClass(a + "off " + a + "on") } var r, h, a = Z + "Slideshow_", s = "click." + Z; return function () { r ? _.get("slideshow") || (ae.unbind(oe, o), o()) : _.get("slideshow") && W[1] && (r = !0, ae.one(oe, o), _.get("slideshowAuto") ? i() : n(), F.show()) } }(); t[Y] || (t(p), J = t.fn[Y] = t[Y] = function (e, i) { var n, o = this; if (e = e || {}, t.isFunction(o)) o = t("<a/>"), e.open = !0; else if (!o[0]) return o; return o[0] ? (p(), m() && (i && (e.onComplete = i), o.each(function () { var i = t.data(this, Y) || {}; t.data(this, Y, t.extend(i, e)) }).addClass(te), n = new r(o[0], e), n.get("open") && f(o[0])), o) : o }, J.position = function (e, i) { function n() { T[0].style.width = k[0].style.width = b[0].style.width = parseInt(y[0].style.width, 10) - j + "px", b[0].style.height = C[0].style.height = H[0].style.height = parseInt(y[0].style.height, 10) - D + "px" } var r, h, s, l = 0, d = 0, c = y.offset(); if (E.unbind("resize." + Z), y.css({ top: -9e4, left: -9e4 }), h = E.scrollTop(), s = E.scrollLeft(), _.get("fixed") ? (c.top -= h, c.left -= s, y.css({ position: "fixed" })) : (l = h, d = s, y.css({ position: "absolute" })), d += _.get("right") !== !1 ? Math.max(E.width() - _.w - N - j - a(_.get("right"), "x"), 0) : _.get("left") !== !1 ? a(_.get("left"), "x") : Math.round(Math.max(E.width() - _.w - N - j, 0) / 2), l += _.get("bottom") !== !1 ? Math.max(o() - _.h - A - D - a(_.get("bottom"), "y"), 0) : _.get("top") !== !1 ? a(_.get("top"), "y") : Math.round(Math.max(o() - _.h - A - D, 0) / 2), y.css({ top: c.top, left: c.left, visibility: "visible" }), x[0].style.width = x[0].style.height = "9999px", r = { width: _.w + N + j, height: _.h + A + D, top: l, left: d }, e) { var g = 0; t.each(r, function (t) { return r[t] !== de[t] ? (g = e, void 0) : void 0 }), e = g } de = r, e || y.css(r), y.dequeue().animate(r, { duration: e || 0, complete: function () { n(), q = !1, x[0].style.width = _.w + N + j + "px", x[0].style.height = _.h + A + D + "px", _.get("reposition") && setTimeout(function () { E.bind("resize." + Z, J.position) }, 1), i && i() }, step: n }) }, J.resize = function (t) { var e; $ && (t = t || {}, t.width && (_.w = a(t.width, "x") - N - j), t.innerWidth && (_.w = a(t.innerWidth, "x")), L.css({ width: _.w }), t.height && (_.h = a(t.height, "y") - A - D), t.innerHeight && (_.h = a(t.innerHeight, "y")), t.innerHeight || t.height || (e = L.scrollTop(), L.css({ height: "auto" }), _.h = L.height()), L.css({ height: _.h }), e && L.scrollTop(e), J.position("none" === _.get("transition") ? 0 : _.get("speed"))) }, J.prep = function (i) { function o() { return _.w = _.w || L.width(), _.w = _.mw && _.mw < _.w ? _.mw : _.w, _.w } function a() { return _.h = _.h || L.height(), _.h = _.mh && _.mh < _.h ? _.mh : _.h, _.h } if ($) { var d, g = "none" === _.get("transition") ? 0 : _.get("speed"); L.remove(), L = n(se, "LoadedContent").append(i), L.hide().appendTo(M.show()).css({ width: o(), overflow: _.get("scrolling") ? "auto" : "hidden" }).css({ height: a() }).prependTo(b), M.hide(), t(U).css({ "float": "none" }), c(_.get("className")), d = function () { function i() { t.support.opacity === !1 && y[0].style.removeAttribute("filter") } var n, o, a = W.length; $ && (o = function () { clearTimeout(Q), S.hide(), u(ne), _.get("onComplete") }, I.html(_.get("title")).show(), L.show(), a > 1 ? ("string" == typeof _.get("current") && R.html(_.get("current").replace("{current}", z + 1).replace("{total}", a)).show(), K[_.get("loop") || a - 1 > z ? "show" : "hide"]().html(_.get("next")), P[_.get("loop") || z ? "show" : "hide"]().html(_.get("previous")), ce(), _.get("preloading") && t.each([h(-1), h(1)], function () { var i, n = W[this], o = new r(n, t.data(n, Y)), h = o.get("href"); h && s(o, h) && (h = l(o, h), i = e.createElement("img"), i.src = h) })) : O.hide(), _.get("iframe") ? (n = e.createElement("iframe"), "frameBorder" in n && (n.frameBorder = 0), "allowTransparency" in n && (n.allowTransparency = "true"), _.get("scrolling") || (n.scrolling = "no"), t(n).attr({ src: _.get("href"), name: (new Date).getTime(), "class": Z + "Iframe", allowFullScreen: !0 }).one("load", o).appendTo(L), ae.one(he, function () { n.src = "//about:blank" }), _.get("fastIframe") && t(n).trigger("load")) : o(), "fade" === _.get("transition") ? y.fadeTo(g, 1, i) : i()) }, "fade" === _.get("transition") ? y.fadeTo(g, 0, function () { J.position(0, d) }) : J.position(g, d) } }, J.next = function () { !q && W[1] && (_.get("loop") || W[z + 1]) && (z = h(1), f(W[z])) }, J.prev = function () { !q && W[1] && (_.get("loop") || z) && (z = h(-1), f(W[z])) }, J.close = function () { $ && !G && (G = !0, $ = !1, u(oe), _.get("onCleanup"), E.unbind("." + Z), v.fadeTo(_.get("fadeOut") || 0, 0), y.stop().fadeTo(_.get("fadeOut") || 0, 0, function () { y.hide(), v.hide(), u(he), L.remove(), setTimeout(function () { G = !1, u(re), _.get("onClosed") }, 1) })) }, J.remove = function () { y && (y.stop(), t[Y].close(), y.stop(!1, !0).remove(), v.remove(), G = !1, y = null, t("." + te).removeData(Y).removeClass(te), t(e).unbind("click." + Z).unbind("keydown." + Z)) }, J.element = function () { return t(_.el) }, J.settings = X) })(jQuery, document, window);;/**/
(function ($) {

    Drupal.behaviors.initColorbox = {
        attach: function (context, settings) {
            if (!$.isFunction($.colorbox) || typeof settings.colorbox === 'undefined') {
                return;
            }

            if (settings.colorbox.mobiledetect && window.matchMedia) {
                // Disable Colorbox for small screens.
                var mq = window.matchMedia("(max-device-width: " + settings.colorbox.mobiledevicewidth + ")");
                if (mq.matches) {
                    return;
                }
            }

            $('.colorbox', context)
              .once('init-colorbox')
              .colorbox(settings.colorbox);

            $(context).bind('cbox_complete', function () {
                Drupal.attachBehaviors('#cboxLoadedContent');
            });
        }
    };

})(jQuery);
;/**/
(function ($) {

    Drupal.behaviors.initColorboxLoad = {
        attach: function (context, settings) {
            if (!$.isFunction($.colorbox) || typeof settings.colorbox === 'undefined') {
                return;
            }
            $.urlParams = function (url) {
                var p = {},
                    e,
                    a = /\+/g,  // Regex for replacing addition symbol with a space
                    r = /([^&=]+)=?([^&]*)/g,
                    d = function (s) { return decodeURIComponent(s.replace(a, ' ')); },
                    q = url.split('?');
                while (e = r.exec(q[1])) {
                    e[1] = d(e[1]);
                    e[2] = d(e[2]);
                    switch (e[2].toLowerCase()) {
                        case 'true':
                        case 'yes':
                            e[2] = true;
                            break;
                        case 'false':
                        case 'no':
                            e[2] = false;
                            break;
                    }
                    if (e[1] == 'width') { e[1] = 'innerWidth'; }
                    if (e[1] == 'height') { e[1] = 'innerHeight'; }
                    p[e[1]] = e[2];
                }
                return p;
            };
            $('.colorbox-load', context)
              .once('init-colorbox-load', function () {
                  var params = $.urlParams($(this).attr('href'));
                  $(this).colorbox($.extend({}, settings.colorbox, params));
              });
        }
    };

})(jQuery);
;/**/
jQuery(document).ready(function () {
    jQuery("#loader").delay(3000).fadeOut("slow");
});;/**/
/*!
 * Bootstrap v3.0.2 by @fat and @mdo
 * Copyright 2013 Twitter, Inc.
 * Licensed under http://www.apache.org/licenses/LICENSE-2.0
 *
 * Designed and built with all the love in the world by @mdo and @fat.
 */

if ("undefined" == typeof jQuery) throw new Error("Bootstrap requires jQuery"); +function (a) { "use strict"; function b() { var a = document.createElement("bootstrap"), b = { WebkitTransition: "webkitTransitionEnd", MozTransition: "transitionend", OTransition: "oTransitionEnd otransitionend", transition: "transitionend" }; for (var c in b) if (void 0 !== a.style[c]) return { end: b[c] } } a.fn.emulateTransitionEnd = function (b) { var c = !1, d = this; a(this).one(a.support.transition.end, function () { c = !0 }); var e = function () { c || a(d).trigger(a.support.transition.end) }; return setTimeout(e, b), this }, a(function () { a.support.transition = b() }) }(jQuery), +function (a) { "use strict"; var b = '[data-dismiss="alert"]', c = function (c) { a(c).on("click", b, this.close) }; c.prototype.close = function (b) { function c() { f.trigger("closed.bs.alert").remove() } var d = a(this), e = d.attr("data-target"); e || (e = d.attr("href"), e = e && e.replace(/.*(?=#[^\s]*$)/, "")); var f = a(e); b && b.preventDefault(), f.length || (f = d.hasClass("alert") ? d : d.parent()), f.trigger(b = a.Event("close.bs.alert")), b.isDefaultPrevented() || (f.removeClass("in"), a.support.transition && f.hasClass("fade") ? f.one(a.support.transition.end, c).emulateTransitionEnd(150) : c()) }; var d = a.fn.alert; a.fn.alert = function (b) { return this.each(function () { var d = a(this), e = d.data("bs.alert"); e || d.data("bs.alert", e = new c(this)), "string" == typeof b && e[b].call(d) }) }, a.fn.alert.Constructor = c, a.fn.alert.noConflict = function () { return a.fn.alert = d, this }, a(document).on("click.bs.alert.data-api", b, c.prototype.close) }(jQuery), +function (a) { "use strict"; var b = function (c, d) { this.$element = a(c), this.options = a.extend({}, b.DEFAULTS, d) }; b.DEFAULTS = { loadingText: "loading..." }, b.prototype.setState = function (a) { var b = "disabled", c = this.$element, d = c.is("input") ? "val" : "html", e = c.data(); a += "Text", e.resetText || c.data("resetText", c[d]()), c[d](e[a] || this.options[a]), setTimeout(function () { "loadingText" == a ? c.addClass(b).attr(b, b) : c.removeClass(b).removeAttr(b) }, 0) }, b.prototype.toggle = function () { var a = this.$element.closest('[data-toggle="buttons"]'); if (a.length) { var b = this.$element.find("input").prop("checked", !this.$element.hasClass("active")).trigger("change"); "radio" === b.prop("type") && a.find(".active").removeClass("active") } this.$element.toggleClass("active") }; var c = a.fn.button; a.fn.button = function (c) { return this.each(function () { var d = a(this), e = d.data("bs.button"), f = "object" == typeof c && c; e || d.data("bs.button", e = new b(this, f)), "toggle" == c ? e.toggle() : c && e.setState(c) }) }, a.fn.button.Constructor = b, a.fn.button.noConflict = function () { return a.fn.button = c, this }, a(document).on("click.bs.button.data-api", "[data-toggle^=button]", function (b) { var c = a(b.target); c.hasClass("btn") || (c = c.closest(".btn")), c.button("toggle"), b.preventDefault() }) }(jQuery), +function (a) { "use strict"; var b = function (b, c) { this.$element = a(b), this.$indicators = this.$element.find(".carousel-indicators"), this.options = c, this.paused = this.sliding = this.interval = this.$active = this.$items = null, "hover" == this.options.pause && this.$element.on("mouseenter", a.proxy(this.pause, this)).on("mouseleave", a.proxy(this.cycle, this)) }; b.DEFAULTS = { interval: 5e3, pause: "hover", wrap: !0 }, b.prototype.cycle = function (b) { return b || (this.paused = !1), this.interval && clearInterval(this.interval), this.options.interval && !this.paused && (this.interval = setInterval(a.proxy(this.next, this), this.options.interval)), this }, b.prototype.getActiveIndex = function () { return this.$active = this.$element.find(".item.active"), this.$items = this.$active.parent().children(), this.$items.index(this.$active) }, b.prototype.to = function (b) { var c = this, d = this.getActiveIndex(); return b > this.$items.length - 1 || 0 > b ? void 0 : this.sliding ? this.$element.one("slid", function () { c.to(b) }) : d == b ? this.pause().cycle() : this.slide(b > d ? "next" : "prev", a(this.$items[b])) }, b.prototype.pause = function (b) { return b || (this.paused = !0), this.$element.find(".next, .prev").length && a.support.transition.end && (this.$element.trigger(a.support.transition.end), this.cycle(!0)), this.interval = clearInterval(this.interval), this }, b.prototype.next = function () { return this.sliding ? void 0 : this.slide("next") }, b.prototype.prev = function () { return this.sliding ? void 0 : this.slide("prev") }, b.prototype.slide = function (b, c) { var d = this.$element.find(".item.active"), e = c || d[b](), f = this.interval, g = "next" == b ? "left" : "right", h = "next" == b ? "first" : "last", i = this; if (!e.length) { if (!this.options.wrap) return; e = this.$element.find(".item")[h]() } this.sliding = !0, f && this.pause(); var j = a.Event("slide.bs.carousel", { relatedTarget: e[0], direction: g }); if (!e.hasClass("active")) { if (this.$indicators.length && (this.$indicators.find(".active").removeClass("active"), this.$element.one("slid", function () { var b = a(i.$indicators.children()[i.getActiveIndex()]); b && b.addClass("active") })), a.support.transition && this.$element.hasClass("slide")) { if (this.$element.trigger(j), j.isDefaultPrevented()) return; e.addClass(b), e[0].offsetWidth, d.addClass(g), e.addClass(g), d.one(a.support.transition.end, function () { e.removeClass([b, g].join(" ")).addClass("active"), d.removeClass(["active", g].join(" ")), i.sliding = !1, setTimeout(function () { i.$element.trigger("slid") }, 0) }).emulateTransitionEnd(600) } else { if (this.$element.trigger(j), j.isDefaultPrevented()) return; d.removeClass("active"), e.addClass("active"), this.sliding = !1, this.$element.trigger("slid") } return f && this.cycle(), this } }; var c = a.fn.carousel; a.fn.carousel = function (c) { return this.each(function () { var d = a(this), e = d.data("bs.carousel"), f = a.extend({}, b.DEFAULTS, d.data(), "object" == typeof c && c), g = "string" == typeof c ? c : f.slide; e || d.data("bs.carousel", e = new b(this, f)), "number" == typeof c ? e.to(c) : g ? e[g]() : f.interval && e.pause().cycle() }) }, a.fn.carousel.Constructor = b, a.fn.carousel.noConflict = function () { return a.fn.carousel = c, this }, a(document).on("click.bs.carousel.data-api", "[data-slide], [data-slide-to]", function (b) { var c, d = a(this), e = a(d.attr("data-target") || (c = d.attr("href")) && c.replace(/.*(?=#[^\s]+$)/, "")), f = a.extend({}, e.data(), d.data()), g = d.attr("data-slide-to"); g && (f.interval = !1), e.carousel(f), (g = d.attr("data-slide-to")) && e.data("bs.carousel").to(g), b.preventDefault() }), a(window).on("load", function () { a('[data-ride="carousel"]').each(function () { var b = a(this); b.carousel(b.data()) }) }) }(jQuery), +function (a) { "use strict"; var b = function (c, d) { this.$element = a(c), this.options = a.extend({}, b.DEFAULTS, d), this.transitioning = null, this.options.parent && (this.$parent = a(this.options.parent)), this.options.toggle && this.toggle() }; b.DEFAULTS = { toggle: !0 }, b.prototype.dimension = function () { var a = this.$element.hasClass("width"); return a ? "width" : "height" }, b.prototype.show = function () { if (!this.transitioning && !this.$element.hasClass("in")) { var b = a.Event("show.bs.collapse"); if (this.$element.trigger(b), !b.isDefaultPrevented()) { var c = this.$parent && this.$parent.find("> .panel > .in"); if (c && c.length) { var d = c.data("bs.collapse"); if (d && d.transitioning) return; c.collapse("hide"), d || c.data("bs.collapse", null) } var e = this.dimension(); this.$element.removeClass("collapse").addClass("collapsing")[e](0), this.transitioning = 1; var f = function () { this.$element.removeClass("collapsing").addClass("in")[e]("auto"), this.transitioning = 0, this.$element.trigger("shown.bs.collapse") }; if (!a.support.transition) return f.call(this); var g = a.camelCase(["scroll", e].join("-")); this.$element.one(a.support.transition.end, a.proxy(f, this)).emulateTransitionEnd(350)[e](this.$element[0][g]) } } }, b.prototype.hide = function () { if (!this.transitioning && this.$element.hasClass("in")) { var b = a.Event("hide.bs.collapse"); if (this.$element.trigger(b), !b.isDefaultPrevented()) { var c = this.dimension(); this.$element[c](this.$element[c]())[0].offsetHeight, this.$element.addClass("collapsing").removeClass("collapse").removeClass("in"), this.transitioning = 1; var d = function () { this.transitioning = 0, this.$element.trigger("hidden.bs.collapse").removeClass("collapsing").addClass("collapse") }; return a.support.transition ? (this.$element[c](0).one(a.support.transition.end, a.proxy(d, this)).emulateTransitionEnd(350), void 0) : d.call(this) } } }, b.prototype.toggle = function () { this[this.$element.hasClass("in") ? "hide" : "show"]() }; var c = a.fn.collapse; a.fn.collapse = function (c) { return this.each(function () { var d = a(this), e = d.data("bs.collapse"), f = a.extend({}, b.DEFAULTS, d.data(), "object" == typeof c && c); e || d.data("bs.collapse", e = new b(this, f)), "string" == typeof c && e[c]() }) }, a.fn.collapse.Constructor = b, a.fn.collapse.noConflict = function () { return a.fn.collapse = c, this }, a(document).on("click.bs.collapse.data-api", "[data-toggle=collapse]", function (b) { var c, d = a(this), e = d.attr("data-target") || b.preventDefault() || (c = d.attr("href")) && c.replace(/.*(?=#[^\s]+$)/, ""), f = a(e), g = f.data("bs.collapse"), h = g ? "toggle" : d.data(), i = d.attr("data-parent"), j = i && a(i); g && g.transitioning || (j && j.find('[data-toggle=collapse][data-parent="' + i + '"]').not(d).addClass("collapsed"), d[f.hasClass("in") ? "addClass" : "removeClass"]("collapsed")), f.collapse(h) }) }(jQuery), +function (a) { "use strict"; function b() { a(d).remove(), a(e).each(function (b) { var d = c(a(this)); d.hasClass("open") && (d.trigger(b = a.Event("hide.bs.dropdown")), b.isDefaultPrevented() || d.removeClass("open").trigger("hidden.bs.dropdown")) }) } function c(b) { var c = b.attr("data-target"); c || (c = b.attr("href"), c = c && /#/.test(c) && c.replace(/.*(?=#[^\s]*$)/, "")); var d = c && a(c); return d && d.length ? d : b.parent() } var d = ".dropdown-backdrop", e = "[data-toggle=dropdown]", f = function (b) { a(b).on("click.bs.dropdown", this.toggle) }; f.prototype.toggle = function (d) { var e = a(this); if (!e.is(".disabled, :disabled")) { var f = c(e), g = f.hasClass("open"); if (b(), !g) { if ("ontouchstart" in document.documentElement && !f.closest(".navbar-nav").length && a('<div class="dropdown-backdrop"/>').insertAfter(a(this)).on("click", b), f.trigger(d = a.Event("show.bs.dropdown")), d.isDefaultPrevented()) return; f.toggleClass("open").trigger("shown.bs.dropdown"), e.focus() } return !1 } }, f.prototype.keydown = function (b) { if (/(38|40|27)/.test(b.keyCode)) { var d = a(this); if (b.preventDefault(), b.stopPropagation(), !d.is(".disabled, :disabled")) { var f = c(d), g = f.hasClass("open"); if (!g || g && 27 == b.keyCode) return 27 == b.which && f.find(e).focus(), d.click(); var h = a("[role=menu] li:not(.divider):visible a", f); if (h.length) { var i = h.index(h.filter(":focus")); 38 == b.keyCode && i > 0 && i--, 40 == b.keyCode && i < h.length - 1 && i++, ~i || (i = 0), h.eq(i).focus() } } } }; var g = a.fn.dropdown; a.fn.dropdown = function (b) { return this.each(function () { var c = a(this), d = c.data("dropdown"); d || c.data("dropdown", d = new f(this)), "string" == typeof b && d[b].call(c) }) }, a.fn.dropdown.Constructor = f, a.fn.dropdown.noConflict = function () { return a.fn.dropdown = g, this }, a(document).on("click.bs.dropdown.data-api", b).on("click.bs.dropdown.data-api", ".dropdown form", function (a) { a.stopPropagation() }).on("click.bs.dropdown.data-api", e, f.prototype.toggle).on("keydown.bs.dropdown.data-api", e + ", [role=menu]", f.prototype.keydown) }(jQuery), +function (a) { "use strict"; var b = function (b, c) { this.options = c, this.$element = a(b), this.$backdrop = this.isShown = null, this.options.remote && this.$element.load(this.options.remote) }; b.DEFAULTS = { backdrop: !0, keyboard: !0, show: !0 }, b.prototype.toggle = function (a) { return this[this.isShown ? "hide" : "show"](a) }, b.prototype.show = function (b) { var c = this, d = a.Event("show.bs.modal", { relatedTarget: b }); this.$element.trigger(d), this.isShown || d.isDefaultPrevented() || (this.isShown = !0, this.escape(), this.$element.on("click.dismiss.modal", '[data-dismiss="modal"]', a.proxy(this.hide, this)), this.backdrop(function () { var d = a.support.transition && c.$element.hasClass("fade"); c.$element.parent().length || c.$element.appendTo(document.body), c.$element.show(), d && c.$element[0].offsetWidth, c.$element.addClass("in").attr("aria-hidden", !1), c.enforceFocus(); var e = a.Event("shown.bs.modal", { relatedTarget: b }); d ? c.$element.find(".modal-dialog").one(a.support.transition.end, function () { c.$element.focus().trigger(e) }).emulateTransitionEnd(300) : c.$element.focus().trigger(e) })) }, b.prototype.hide = function (b) { b && b.preventDefault(), b = a.Event("hide.bs.modal"), this.$element.trigger(b), this.isShown && !b.isDefaultPrevented() && (this.isShown = !1, this.escape(), a(document).off("focusin.bs.modal"), this.$element.removeClass("in").attr("aria-hidden", !0).off("click.dismiss.modal"), a.support.transition && this.$element.hasClass("fade") ? this.$element.one(a.support.transition.end, a.proxy(this.hideModal, this)).emulateTransitionEnd(300) : this.hideModal()) }, b.prototype.enforceFocus = function () { a(document).off("focusin.bs.modal").on("focusin.bs.modal", a.proxy(function (a) { this.$element[0] === a.target || this.$element.has(a.target).length || this.$element.focus() }, this)) }, b.prototype.escape = function () { this.isShown && this.options.keyboard ? this.$element.on("keyup.dismiss.bs.modal", a.proxy(function (a) { 27 == a.which && this.hide() }, this)) : this.isShown || this.$element.off("keyup.dismiss.bs.modal") }, b.prototype.hideModal = function () { var a = this; this.$element.hide(), this.backdrop(function () { a.removeBackdrop(), a.$element.trigger("hidden.bs.modal") }) }, b.prototype.removeBackdrop = function () { this.$backdrop && this.$backdrop.remove(), this.$backdrop = null }, b.prototype.backdrop = function (b) { var c = this.$element.hasClass("fade") ? "fade" : ""; if (this.isShown && this.options.backdrop) { var d = a.support.transition && c; if (this.$backdrop = a('<div class="modal-backdrop ' + c + '" />').appendTo(document.body), this.$element.on("click.dismiss.modal", a.proxy(function (a) { a.target === a.currentTarget && ("static" == this.options.backdrop ? this.$element[0].focus.call(this.$element[0]) : this.hide.call(this)) }, this)), d && this.$backdrop[0].offsetWidth, this.$backdrop.addClass("in"), !b) return; d ? this.$backdrop.one(a.support.transition.end, b).emulateTransitionEnd(150) : b() } else !this.isShown && this.$backdrop ? (this.$backdrop.removeClass("in"), a.support.transition && this.$element.hasClass("fade") ? this.$backdrop.one(a.support.transition.end, b).emulateTransitionEnd(150) : b()) : b && b() }; var c = a.fn.modal; a.fn.modal = function (c, d) { return this.each(function () { var e = a(this), f = e.data("bs.modal"), g = a.extend({}, b.DEFAULTS, e.data(), "object" == typeof c && c); f || e.data("bs.modal", f = new b(this, g)), "string" == typeof c ? f[c](d) : g.show && f.show(d) }) }, a.fn.modal.Constructor = b, a.fn.modal.noConflict = function () { return a.fn.modal = c, this }, a(document).on("click.bs.modal.data-api", '[data-toggle="modal"]', function (b) { var c = a(this), d = c.attr("href"), e = a(c.attr("data-target") || d && d.replace(/.*(?=#[^\s]+$)/, "")), f = e.data("modal") ? "toggle" : a.extend({ remote: !/#/.test(d) && d }, e.data(), c.data()); b.preventDefault(), e.modal(f, this).one("hide", function () { c.is(":visible") && c.focus() }) }), a(document).on("show.bs.modal", ".modal", function () { a(document.body).addClass("modal-open") }).on("hidden.bs.modal", ".modal", function () { a(document.body).removeClass("modal-open") }) }(jQuery), +function (a) { "use strict"; var b = function (a, b) { this.type = this.options = this.enabled = this.timeout = this.hoverState = this.$element = null, this.init("tooltip", a, b) }; b.DEFAULTS = { animation: !0, placement: "top", selector: !1, template: '<div class="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>', trigger: "hover focus", title: "", delay: 0, html: !1, container: !1 }, b.prototype.init = function (b, c, d) { this.enabled = !0, this.type = b, this.$element = a(c), this.options = this.getOptions(d); for (var e = this.options.trigger.split(" "), f = e.length; f--;) { var g = e[f]; if ("click" == g) this.$element.on("click." + this.type, this.options.selector, a.proxy(this.toggle, this)); else if ("manual" != g) { var h = "hover" == g ? "mouseenter" : "focus", i = "hover" == g ? "mouseleave" : "blur"; this.$element.on(h + "." + this.type, this.options.selector, a.proxy(this.enter, this)), this.$element.on(i + "." + this.type, this.options.selector, a.proxy(this.leave, this)) } } this.options.selector ? this._options = a.extend({}, this.options, { trigger: "manual", selector: "" }) : this.fixTitle() }, b.prototype.getDefaults = function () { return b.DEFAULTS }, b.prototype.getOptions = function (b) { return b = a.extend({}, this.getDefaults(), this.$element.data(), b), b.delay && "number" == typeof b.delay && (b.delay = { show: b.delay, hide: b.delay }), b }, b.prototype.getDelegateOptions = function () { var b = {}, c = this.getDefaults(); return this._options && a.each(this._options, function (a, d) { c[a] != d && (b[a] = d) }), b }, b.prototype.enter = function (b) { var c = b instanceof this.constructor ? b : a(b.currentTarget)[this.type](this.getDelegateOptions()).data("bs." + this.type); return clearTimeout(c.timeout), c.hoverState = "in", c.options.delay && c.options.delay.show ? (c.timeout = setTimeout(function () { "in" == c.hoverState && c.show() }, c.options.delay.show), void 0) : c.show() }, b.prototype.leave = function (b) { var c = b instanceof this.constructor ? b : a(b.currentTarget)[this.type](this.getDelegateOptions()).data("bs." + this.type); return clearTimeout(c.timeout), c.hoverState = "out", c.options.delay && c.options.delay.hide ? (c.timeout = setTimeout(function () { "out" == c.hoverState && c.hide() }, c.options.delay.hide), void 0) : c.hide() }, b.prototype.show = function () { var b = a.Event("show.bs." + this.type); if (this.hasContent() && this.enabled) { if (this.$element.trigger(b), b.isDefaultPrevented()) return; var c = this.tip(); this.setContent(), this.options.animation && c.addClass("fade"); var d = "function" == typeof this.options.placement ? this.options.placement.call(this, c[0], this.$element[0]) : this.options.placement, e = /\s?auto?\s?/i, f = e.test(d); f && (d = d.replace(e, "") || "top"), c.detach().css({ top: 0, left: 0, display: "block" }).addClass(d), this.options.container ? c.appendTo(this.options.container) : c.insertAfter(this.$element); var g = this.getPosition(), h = c[0].offsetWidth, i = c[0].offsetHeight; if (f) { var j = this.$element.parent(), k = d, l = document.documentElement.scrollTop || document.body.scrollTop, m = "body" == this.options.container ? window.innerWidth : j.outerWidth(), n = "body" == this.options.container ? window.innerHeight : j.outerHeight(), o = "body" == this.options.container ? 0 : j.offset().left; d = "bottom" == d && g.top + g.height + i - l > n ? "top" : "top" == d && g.top - l - i < 0 ? "bottom" : "right" == d && g.right + h > m ? "left" : "left" == d && g.left - h < o ? "right" : d, c.removeClass(k).addClass(d) } var p = this.getCalculatedOffset(d, g, h, i); this.applyPlacement(p, d), this.$element.trigger("shown.bs." + this.type) } }, b.prototype.applyPlacement = function (a, b) { var c, d = this.tip(), e = d[0].offsetWidth, f = d[0].offsetHeight, g = parseInt(d.css("margin-top"), 10), h = parseInt(d.css("margin-left"), 10); isNaN(g) && (g = 0), isNaN(h) && (h = 0), a.top = a.top + g, a.left = a.left + h, d.offset(a).addClass("in"); var i = d[0].offsetWidth, j = d[0].offsetHeight; if ("top" == b && j != f && (c = !0, a.top = a.top + f - j), /bottom|top/.test(b)) { var k = 0; a.left < 0 && (k = -2 * a.left, a.left = 0, d.offset(a), i = d[0].offsetWidth, j = d[0].offsetHeight), this.replaceArrow(k - e + i, i, "left") } else this.replaceArrow(j - f, j, "top"); c && d.offset(a) }, b.prototype.replaceArrow = function (a, b, c) { this.arrow().css(c, a ? 50 * (1 - a / b) + "%" : "") }, b.prototype.setContent = function () { var a = this.tip(), b = this.getTitle(); a.find(".tooltip-inner")[this.options.html ? "html" : "text"](b), a.removeClass("fade in top bottom left right") }, b.prototype.hide = function () { function b() { "in" != c.hoverState && d.detach() } var c = this, d = this.tip(), e = a.Event("hide.bs." + this.type); return this.$element.trigger(e), e.isDefaultPrevented() ? void 0 : (d.removeClass("in"), a.support.transition && this.$tip.hasClass("fade") ? d.one(a.support.transition.end, b).emulateTransitionEnd(150) : b(), this.$element.trigger("hidden.bs." + this.type), this) }, b.prototype.fixTitle = function () { var a = this.$element; (a.attr("title") || "string" != typeof a.attr("data-original-title")) && a.attr("data-original-title", a.attr("title") || "").attr("title", "") }, b.prototype.hasContent = function () { return this.getTitle() }, b.prototype.getPosition = function () { var b = this.$element[0]; return a.extend({}, "function" == typeof b.getBoundingClientRect ? b.getBoundingClientRect() : { width: b.offsetWidth, height: b.offsetHeight }, this.$element.offset()) }, b.prototype.getCalculatedOffset = function (a, b, c, d) { return "bottom" == a ? { top: b.top + b.height, left: b.left + b.width / 2 - c / 2 } : "top" == a ? { top: b.top - d, left: b.left + b.width / 2 - c / 2 } : "left" == a ? { top: b.top + b.height / 2 - d / 2, left: b.left - c } : { top: b.top + b.height / 2 - d / 2, left: b.left + b.width } }, b.prototype.getTitle = function () { var a, b = this.$element, c = this.options; return a = b.attr("data-original-title") || ("function" == typeof c.title ? c.title.call(b[0]) : c.title) }, b.prototype.tip = function () { return this.$tip = this.$tip || a(this.options.template) }, b.prototype.arrow = function () { return this.$arrow = this.$arrow || this.tip().find(".tooltip-arrow") }, b.prototype.validate = function () { this.$element[0].parentNode || (this.hide(), this.$element = null, this.options = null) }, b.prototype.enable = function () { this.enabled = !0 }, b.prototype.disable = function () { this.enabled = !1 }, b.prototype.toggleEnabled = function () { this.enabled = !this.enabled }, b.prototype.toggle = function (b) { var c = b ? a(b.currentTarget)[this.type](this.getDelegateOptions()).data("bs." + this.type) : this; c.tip().hasClass("in") ? c.leave(c) : c.enter(c) }, b.prototype.destroy = function () { this.hide().$element.off("." + this.type).removeData("bs." + this.type) }; var c = a.fn.tooltip; a.fn.tooltip = function (c) { return this.each(function () { var d = a(this), e = d.data("bs.tooltip"), f = "object" == typeof c && c; e || d.data("bs.tooltip", e = new b(this, f)), "string" == typeof c && e[c]() }) }, a.fn.tooltip.Constructor = b, a.fn.tooltip.noConflict = function () { return a.fn.tooltip = c, this } }(jQuery), +function (a) { "use strict"; var b = function (a, b) { this.init("popover", a, b) }; if (!a.fn.tooltip) throw new Error("Popover requires tooltip.js"); b.DEFAULTS = a.extend({}, a.fn.tooltip.Constructor.DEFAULTS, { placement: "right", trigger: "click", content: "", template: '<div class="popover"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>' }), b.prototype = a.extend({}, a.fn.tooltip.Constructor.prototype), b.prototype.constructor = b, b.prototype.getDefaults = function () { return b.DEFAULTS }, b.prototype.setContent = function () { var a = this.tip(), b = this.getTitle(), c = this.getContent(); a.find(".popover-title")[this.options.html ? "html" : "text"](b), a.find(".popover-content")[this.options.html ? "html" : "text"](c), a.removeClass("fade top bottom left right in"), a.find(".popover-title").html() || a.find(".popover-title").hide() }, b.prototype.hasContent = function () { return this.getTitle() || this.getContent() }, b.prototype.getContent = function () { var a = this.$element, b = this.options; return a.attr("data-content") || ("function" == typeof b.content ? b.content.call(a[0]) : b.content) }, b.prototype.arrow = function () { return this.$arrow = this.$arrow || this.tip().find(".arrow") }, b.prototype.tip = function () { return this.$tip || (this.$tip = a(this.options.template)), this.$tip }; var c = a.fn.popover; a.fn.popover = function (c) { return this.each(function () { var d = a(this), e = d.data("bs.popover"), f = "object" == typeof c && c; e || d.data("bs.popover", e = new b(this, f)), "string" == typeof c && e[c]() }) }, a.fn.popover.Constructor = b, a.fn.popover.noConflict = function () { return a.fn.popover = c, this } }(jQuery), +function (a) { "use strict"; function b(c, d) { var e, f = a.proxy(this.process, this); this.$element = a(c).is("body") ? a(window) : a(c), this.$body = a("body"), this.$scrollElement = this.$element.on("scroll.bs.scroll-spy.data-api", f), this.options = a.extend({}, b.DEFAULTS, d), this.selector = (this.options.target || (e = a(c).attr("href")) && e.replace(/.*(?=#[^\s]+$)/, "") || "") + " .nav li > a", this.offsets = a([]), this.targets = a([]), this.activeTarget = null, this.refresh(), this.process() } b.DEFAULTS = { offset: 10 }, b.prototype.refresh = function () { var b = this.$element[0] == window ? "offset" : "position"; this.offsets = a([]), this.targets = a([]); var c = this; this.$body.find(this.selector).map(function () { var d = a(this), e = d.data("target") || d.attr("href"), f = /^#\w/.test(e) && a(e); return f && f.length && [[f[b]().top + (!a.isWindow(c.$scrollElement.get(0)) && c.$scrollElement.scrollTop()), e]] || null }).sort(function (a, b) { return a[0] - b[0] }).each(function () { c.offsets.push(this[0]), c.targets.push(this[1]) }) }, b.prototype.process = function () { var a, b = this.$scrollElement.scrollTop() + this.options.offset, c = this.$scrollElement[0].scrollHeight || this.$body[0].scrollHeight, d = c - this.$scrollElement.height(), e = this.offsets, f = this.targets, g = this.activeTarget; if (b >= d) return g != (a = f.last()[0]) && this.activate(a); for (a = e.length; a--;) g != f[a] && b >= e[a] && (!e[a + 1] || b <= e[a + 1]) && this.activate(f[a]) }, b.prototype.activate = function (b) { this.activeTarget = b, a(this.selector).parents(".active").removeClass("active"); var c = this.selector + '[data-target="' + b + '"],' + this.selector + '[href="' + b + '"]', d = a(c).parents("li").addClass("active"); d.parent(".dropdown-menu").length && (d = d.closest("li.dropdown").addClass("active")), d.trigger("activate") }; var c = a.fn.scrollspy; a.fn.scrollspy = function (c) { return this.each(function () { var d = a(this), e = d.data("bs.scrollspy"), f = "object" == typeof c && c; e || d.data("bs.scrollspy", e = new b(this, f)), "string" == typeof c && e[c]() }) }, a.fn.scrollspy.Constructor = b, a.fn.scrollspy.noConflict = function () { return a.fn.scrollspy = c, this }, a(window).on("load", function () { a('[data-spy="scroll"]').each(function () { var b = a(this); b.scrollspy(b.data()) }) }) }(jQuery), +function (a) { "use strict"; var b = function (b) { this.element = a(b) }; b.prototype.show = function () { var b = this.element, c = b.closest("ul:not(.dropdown-menu)"), d = b.data("target"); if (d || (d = b.attr("href"), d = d && d.replace(/.*(?=#[^\s]*$)/, "")), !b.parent("li").hasClass("active")) { var e = c.find(".active:last a")[0], f = a.Event("show.bs.tab", { relatedTarget: e }); if (b.trigger(f), !f.isDefaultPrevented()) { var g = a(d); this.activate(b.parent("li"), c), this.activate(g, g.parent(), function () { b.trigger({ type: "shown.bs.tab", relatedTarget: e }) }) } } }, b.prototype.activate = function (b, c, d) { function e() { f.removeClass("active").find("> .dropdown-menu > .active").removeClass("active"), b.addClass("active"), g ? (b[0].offsetWidth, b.addClass("in")) : b.removeClass("fade"), b.parent(".dropdown-menu") && b.closest("li.dropdown").addClass("active"), d && d() } var f = c.find("> .active"), g = d && a.support.transition && f.hasClass("fade"); g ? f.one(a.support.transition.end, e).emulateTransitionEnd(150) : e(), f.removeClass("in") }; var c = a.fn.tab; a.fn.tab = function (c) { return this.each(function () { var d = a(this), e = d.data("bs.tab"); e || d.data("bs.tab", e = new b(this)), "string" == typeof c && e[c]() }) }, a.fn.tab.Constructor = b, a.fn.tab.noConflict = function () { return a.fn.tab = c, this }, a(document).on("click.bs.tab.data-api", '[data-toggle="tab"], [data-toggle="pill"]', function (b) { b.preventDefault(), a(this).tab("show") }) }(jQuery), +function (a) { "use strict"; var b = function (c, d) { this.options = a.extend({}, b.DEFAULTS, d), this.$window = a(window).on("scroll.bs.affix.data-api", a.proxy(this.checkPosition, this)).on("click.bs.affix.data-api", a.proxy(this.checkPositionWithEventLoop, this)), this.$element = a(c), this.affixed = this.unpin = null, this.checkPosition() }; b.RESET = "affix affix-top affix-bottom", b.DEFAULTS = { offset: 0 }, b.prototype.checkPositionWithEventLoop = function () { setTimeout(a.proxy(this.checkPosition, this), 1) }, b.prototype.checkPosition = function () { if (this.$element.is(":visible")) { var c = a(document).height(), d = this.$window.scrollTop(), e = this.$element.offset(), f = this.options.offset, g = f.top, h = f.bottom; "object" != typeof f && (h = g = f), "function" == typeof g && (g = f.top()), "function" == typeof h && (h = f.bottom()); var i = null != this.unpin && d + this.unpin <= e.top ? !1 : null != h && e.top + this.$element.height() >= c - h ? "bottom" : null != g && g >= d ? "top" : !1; this.affixed !== i && (this.unpin && this.$element.css("top", ""), this.affixed = i, this.unpin = "bottom" == i ? e.top - d : null, this.$element.removeClass(b.RESET).addClass("affix" + (i ? "-" + i : "")), "bottom" == i && this.$element.offset({ top: document.body.offsetHeight - h - this.$element.height() })) } }; var c = a.fn.affix; a.fn.affix = function (c) { return this.each(function () { var d = a(this), e = d.data("bs.affix"), f = "object" == typeof c && c; e || d.data("bs.affix", e = new b(this, f)), "string" == typeof c && e[c]() }) }, a.fn.affix.Constructor = b, a.fn.affix.noConflict = function () { return a.fn.affix = c, this }, a(window).on("load", function () { a('[data-spy="affix"]').each(function () { var b = a(this), c = b.data(); c.offset = c.offset || {}, c.offsetBottom && (c.offset.bottom = c.offsetBottom), c.offsetTop && (c.offset.top = c.offsetTop), b.affix(c) }) }) }(jQuery);;/**/
/**
 ** Custom JS for Zoho (Manjit)
 */

jQuery(document).ready(function () {
    jQuery('.zoho-child-wrapper').toggle();
    jQuery("#back-top").hide();
    jQuery('.service-links ul,.partners-info,.node-type-product-help .dropBox,.featured-news .views-row .hidden-news').toggle();
    jQuery(".service-links").click(function () {
        jQuery(".service-links ul").slideToggle("slow");
    });
    jQuery(".zoho-parent-toggle").click(function () {
        jQuery(this).parent().find('.zoho-child-wrapper').slideToggle();
        if (jQuery(this).parent().hasClass('open')) {
            jQuery(this).parent().removeClass('open');
        }
        else {
            jQuery(this).parent().siblings().removeClass('open');
            jQuery(this).parent().siblings().find('.zoho-child-wrapper').slideUp('slow');
            jQuery(this).parent().addClass('open');
        }
    });
    jQuery('.featured-news .views-row').click(function () {
        jQuery(this).find('.hidden-news').slideToggle('slow');
    });
    jQuery(".node-type-product-help #faq h4").click(function () {
        var classtoremove = jQuery(this).attr('class');
        if (classtoremove) {
            jQuery(this).next('div').slideToggle('slow');
            jQuery(this).removeClass(classtoremove);
        } else {
            if (jQuery(this).siblings().hasClass('open')) {
                if (jQuery(this).siblings('.open .hidden-news, .open + .ans.dropBox').is(":visible")) {
                    jQuery(this).addClass('open');
                    jQuery(this).next('div').slideToggle('slow');
                } else {
                    var news_toggle = jQuery(this).addClass('open').siblings().removeClass('open');
                    jQuery('.open .hidden-news, .open + .ans.dropBox').slideToggle('slow');
                }
            } else {
                var news_toggle = jQuery(this).addClass('open').siblings().removeClass('open');
                jQuery('.open .hidden-news, .open + .ans.dropBox').slideToggle('slow');
            }
        }
    });
    jQuery('.footer-wrapper a').each(function () {
        if (jQuery(this).hasClass('zoho-and')) {
            jQuery(this).parent().addClass('zoho-and-wrapper');
        }
    });
    jQuery('.zoho-child-wrapper li a').each(function () {
        if (/\/people\/help\//.test(document.URL)) {
            jQuery('.zoho-parent-toggle').remove();
        }
        if (/\/people\/help\//.test(document.URL) || jQuery(this).hasClass('active')) {
            jQuery(this).parent().parent().parent().addClass('open');
        }
    });
    jQuery('.zoho-child-wrapper').each(function () {
        if (/\/people\/help\//.test(document.URL) || jQuery(this).parent().hasClass('open')) {
            jQuery(this).css("display", "block");
        }
    });
    jQuery(".sliding-header-menu").affix({
        offset: {
            top: 120
        }
    });
    jQuery('.countries .names').click(function () {
        var text = jQuery(this).text();
        jQuery('.' + text).show();
    });
    jQuery('.close').click(function () {
        var divtohide = jQuery(this).attr('class').split(' ')[1].split('-')[0];
        jQuery('.' + divtohide).hide();
    })
    // jQuery(window).keydown(function(event) {
    //     if(event.keyCode==27) {
    //                 hide();
    //     }
    // });
    jQuery('.info').click(function () {
        var $nid = jQuery(this).attr('class').split('_')[2];
        jQuery('.expand_view_' + $nid).slideToggle("slow");
    })
    jQuery('.testimonial_addons').each(function () {
        jQuery(this).parent().parent().parent().addClass("testimonial-wrapper");
    });
    jQuery(".dynamic-width:contains('True')").addClass("tick");
    jQuery(".dynamic-width:contains('TRUE')").addClass("tick");
    jQuery(".dynamic-width:contains('False')").addClass("cross");
    jQuery(".dynamic-width:contains('FALSE')").addClass("cross");

    /** Pricing Comparison **/
    jQuery('.pricing-row-wrapper').each(function (i) {
        jQuery(this).children(":first").removeClass("dynamic-width");
        var div_count = jQuery(this).find(".dynamic-width").length;
        var result = 70 / div_count;
        jQuery(this).find(".dynamic-width").css("width", result + "%");
    });
    /** Pricing Comparison end **/

    /** Pricing list tables **/
    var totalsum = 0;
    jQuery('.pricing-blocks .view-pricing .views-row').each(function (key, value) {
        var width = jQuery(this).outerWidth();
        totalsum = totalsum + width;
    });
    jQuery('.pricing-blocks .view-pricing .view-content').css("width", totalsum + "px");
    /** Pricing list tables **/

    /** Universal Color User **/
    var universal_color_user = jQuery(".universal-color-user .field-content").text();
    var universal_color_theme = jQuery(".universal-color-theme .field-item").text();
    var universal_color_quicktabs = jQuery(".universal-color-quicktabs").text();
    var universal_button_color = jQuery(".universal-button-color .field-item").text();
    jQuery(".universal-color-user-1,.cell-empty-row,.pricing-faq .views-field-body a,.view-resourses-listing-page .views-field-nothing .learn-more a").css("color", universal_button_color);
    jQuery(".dynamic-width a,.plan-bottom-wrapper a").css("background-color", universal_button_color);
    jQuery(".products-theme").css("background-color", universal_color_theme);
    jQuery(".quicktabs-wrapper li.active").css("border-right-color", universal_color_quicktabs);
    jQuery(".getstarted-box a,.views-row.testimonial-wrapper .benefit-testimonial-wrapper,#footer-banner-image .get-started a").css("background-color", universal_button_color);
    jQuery(".header-menu-2 li a.active").css("border-bottom-color", universal_button_color);
    jQuery(".plan-wrapper[data-highlight='1'] .plan-top-wrapper").css("border-top-color", universal_button_color);

    /** Universal Color User **/

    jQuery("a[href='#top']").click(function () {
        jQuery("html, body").animate({ scrollTop: 0 }, "slow");
        return false;
    });
    var headerHeight = jQuery('#block-panels-mini-products-header-dropdown-menu').innerHeight();
    jQuery('.look-down').click(function () {
        jQuery("body").animate({
            scrollTop: jQuery('.catagory-mobapps-part').offset().top - headerHeight + 55
        }
        , 1000);
    });
    jQuery('.explore').click(function () {
        jQuery("body").animate({
            scrollTop: jQuery('.zoho-timeline').offset().top - headerHeight + 55
        }
        , 1000);
    });
    var homeheaderheight = jQuery('#block-panels-mini-home-page-sliding-header').innerHeight();
    jQuery('.zoho-home-page .scroll-down .scrolldown-arrow').click(function () {
        jQuery("body").animate({
            scrollTop: jQuery('.pane-home-page-view-panel-pane-1').offset().top - homeheaderheight + 55
        }, 1000);

    });
    jQuery('.view-home-page-view').each(function () {
        var highestBox = 0;
        jQuery('.view-grouping:nth-child(1),.view-grouping:nth-child(2),.view-grouping:nth-child(3)').each(function () {
            if (jQuery(this).height() > highestBox)
                highestBox = jQuery(this).height();
        });
        jQuery('.view-grouping:nth-child(1),.view-grouping:nth-child(2),.view-grouping:nth-child(3)').height(highestBox);
    });
    jQuery('.view-home-page-view').each(function () {
        var highestBox = 0;
        jQuery('.view-grouping:nth-child(4),.view-grouping:nth-child(5),.view-grouping:nth-child(6)').each(function () {
            if (jQuery(this).height() > highestBox)
                highestBox = jQuery(this).height();
        });
        jQuery('.view-grouping:nth-child(4),.view-grouping:nth-child(5),.view-grouping:nth-child(6)').height(highestBox);
    });

    /** Dynamic Pricing blocks height **/
    jQuery('.pricing-view-content-wrapper .pricing-row-wrapper').each(function () {
        var heightofpricingblock = 0;
        jQuery(this).children('.dynamic-width').each(function () {
            if (jQuery(this).height() > heightofpricingblock)
                heightofpricingblock = jQuery(this).height();
        });
        jQuery(this).children('.dynamic-width').height(heightofpricingblock);
    });
    /** Dynamic Pricing blocks height end **/

});

function showhide(showhideDiv, chantxt) {
    var divobj = document.getElementById(showhideDiv);
    if (divobj.style.display == 'none') {
        divobj.style.display = 'block';
        document.getElementById(chantxt).innerHTML = '<span class="up">Up</span>';
    }
    else {
        divobj.style.display = 'none';
        document.getElementById(chantxt).innerHTML = '<span class="down">Down</span>';
    }
}

function expcollps(curr, isexpand) {
    var more_div = (isexpand) ? jQuery(curr) : jQuery(curr).parent().prev();
    var less_div = (isexpand) ? jQuery(curr).next() : jQuery(curr).parent();
    if (isexpand) {
        jQuery('.ldjs_cnt').slideUp(700, function () {
            jQuery('.ldjs_readmr').css('visibility', 'visible');
        });
        less_div.slideDown(700, function () {
            more_div.css('visibility', 'hidden');
        });

    } else {
        less_div.slideUp(700, function () {
            more_div.animate({ 'opacity': 1 }, 700).css('visibility', 'visible');
        });
    }
}

window.onscroll = function () {
    var height = jQuery(window).height();
    var scrollTop = jQuery(window).scrollTop();
    var obj = jQuery(".footer-wrapper");
    if (typeof obj !== 'undefined') {
        var pos = obj.position();
        if (pos && pos.top && (height + scrollTop) > pos.top) {
            jQuery(".service-links").css("position", "absolute");
        }
        else {
            jQuery(".service-links").css("position", "fixed");
        }
    }
    if (jQuery(this).scrollTop() > 200) {
        jQuery('#back-top').fadeIn();
    }
    else {
        jQuery('#back-top').fadeOut();
    }
}
;/**/
(function ($) {
    $(document).ready(function (e) {

        //var insertUsericon = '<a class="user-icon" href="https://accounts.zoho.com/">User Icon</a>'



        /* NEW DESIGN FOR BELOW 767px */
        /*	
            if(window.innerWidth<=767){
                $('.ptmenu').addClass('sticky');	
                $(insertUsericon).insertAfter('.product-nav-links .mobile-menu-icon');
                $('.product-nav-links .menu').css({'height':window.innerHeight-50})
        $('.product-nav-links .menu').append('<li class="featured-apps-part"><div class="more-featured-apps"><h4>Our Featured Products</h4><div class="featured-pro"> <a class="crm" href="/crm/"><span></span>CRM</a> <a class="mail" href="/mail/"> <span></span>Mail</a> <a class="creator" href="/creator/"><span></span>Creator</a> <a class="books" href="/books/"><span></span>Books</a> </div><div class="more-links"><a href="https://www.zoho.com/" class="moreapps">More Zoho products</a></div></div></li>')	
            }
            
        */

        /* JS FOR PLACEHOLDER MOVING EFFECTS STARTS  */



        $('input').on('blur', function () {
            var getTextval = $(this).val()
            if (getTextval == "" || getTextval == null) {

                $(this).parent().removeClass('added-placeholder');
            }
            else { $(this).parent().addClass('added-placeholder') }
        }).on('focus', function () {
            var getTextval = $(this).val()
            if (getTextval == "" || getTextval == null) {

                $(this).parent().removeClass('').addClass('added-placeholder');
            }
            else { $(this).parent().addClass('added-placeholder') }
        });


        var getTextval = $('#emailfield').val();
        if (getTextval == "" || getTextval == null) {
            $('#emailfield').parent().removeClass('added-placeholder');
        }
        else { $('#emailfield').parent().addClass('added-placeholder') }


        /* JS FOR PLACEHOLDER MOVING EFFECTS ENDS */

        getWidth = 0; // set welcome popup width;
        $gotoPage = '';
        productName = ''

        var script = document.createElement('script');
        script.src = "https://accounts.zoho.com/u/info";
        document.getElementsByTagName('head')[0].appendChild(script);



        breaks = false;
        var allapps = "desk,accounts,mdm,appcreator,crmplus,show,writer,wiki,crm,campaigns,contactmanager,forms,sites,salesiq,survey,social,motivator|bugtracker,connect,docs,mail,meeting,projects,showtime,vault,chat|assist,support|books,invoice,subscriptions,expense|people,recruit|creator,reports|"

        var allProduct = ['crm', 'campaigns', 'contactmanager', 'forms', 'sites', 'salesiq', 'survey', 'social', 'assist', 'support', 'bugtracker', 'connect', 'docs', 'mail', 'meeting', 'projects', 'showtime', 'vault', 'chat', 'books', 'invoice', 'subscriptions', 'expense', 'people', 'recruit', 'creator', 'reports', 'motivator', 'accounts', 'appcreator', 'mdm', 'seminar', 'crmplus', 'wiki', 'desk']
        var url = window.location.href.split('/');

        findproduct:
            for (var k = 0; k < url.length; k++) {
                for (var m = 0; m < allProduct.length; m++) {
                    if (allProduct[m] == url[k]) {
                        if (allProduct[m] != 'crm' && allProduct[m] != 'docs') {
                            productName = allProduct[m];
                            break findproduct;
                        }
                        if (allProduct[m] == 'crm') {
                            if (url[k + 1] != 'crmplus') {
                                productName = 'crm'
                            }
                            else {
                                productName = 'crmplus';
                            }
                        }
                        if (allProduct[m] == 'docs') {
                            if ((url[k + 1] != 'writer') || (url[k + 1] != 'writer.html') || (url[k + 1] != 'show') || (url[k + 1] != 'show.html')) {
                                productName = 'docs';
                            }
                            if ((url[k + 1] == 'writer') || (url[k + 1] == 'writer.html')) {
                                productName = 'writer';
                            }
                            if ((url[k + 1] == 'show') || (url[k + 1] == 'show.html')) {
                                productName = 'show';
                            }
                        }
                    }
                }
            }


        addString = "'" + productName + "'"
        resetUrl(productName)
        /* Header footer Common HTML Js */
        var lan = "";
        var href = "";
        var href1 = "";
        var terms = "";
        var privacy = "/privacy.html ";
        var security = "";
        var antispam = "/policy.html ";
        var refound = "";
        var fb = "//www.facebook.com/zoho";
        var tw = "//twitter.com/zoho";
        var linkedin = "<a class='on-in' href='https://www.linkedin.com/company/38373' target='_blank' class='linkedin' title='LinkedIn'>'+Drupal.t('Linkedin')+'</a>";
        var languagecode = ['/es/', '/fr/', '/it/'];
        var lan1 = "";
        var lan2 = "";
        for (var i = 0; i < languagecode.length; i++) {
            if (window.location.href.indexOf(languagecode[i]) > -1) {
                var url_path = window.location.pathname.split('/');
                if (url_path[2] == (languagecode[i].slice(1, -1))) {
                    var lan1 = languagecode[i];
                    lan1 = lan1.slice(1);
                    lan2 = '/' + lan1.slice(0, -1);
                }
                else {
                    var lan = languagecode[i];
                    var lan = lan.slice(0, -1);
                }
            }
        }

        if ((window.location.host) == "www.zoho.jp") {
            var href = "//www.zoho.com";
            var href1 = "//www.zoho.jp";
            var terms = "ofservice";
            var privacy = "//www.zoho.jp/PrivacyPolicy.html";
            var security = "policy";
            var antispam = "//www.zoho.jp/SpamMailPrevention.html";
            var refound = "<li><a href='//www.zoho.jp/refound.html'>ã‚ªãƒ³ãƒ©ã‚¤ãƒ³æ±ºæ¸ˆå¾Œã®ã‚­ãƒ£ãƒ³ã‚»ãƒ«ã«ã¤ã„ã¦ </a></li>";
            var fb = "//www.facebook.com/zohoJapan";
            var tw = "//twitter.com/jpzoho";
            var linkedin = "";
        }
        var header = '<span class="menu-icon">' + Drupal.t('Menu') + '</span><div class="mask-holder hide"> <div class="mobile-menu"> <div class="mobile-menu-wrap"> <ul> <li>' + Drupal.t('Sales & Marketing') + '</li> <li> <a href="' + href1 + '/crm/"><span class="app-icon1"></span>' + Drupal.t('CRM') + '</a></li> <li> <a href="' + href + '/salesinbox/"><span class="app-icon36"></span>' + Drupal.t('SalesInbox') + '</a></li> <li><a href="' + href1 + '/salesiq/"><span class="app-icon6"></span>' + Drupal.t('SalesIQ') + '</a></li> <li> <a href="' + href + '/survey/"><span class="app-icon4"></span>' + Drupal.t('Survey') + '</a></li> <li><a href="' + href1 + '/campaigns/"><span class="app-icon2"></span>' + Drupal.t('Campaigns') + '</a></li> <li><a href="' + href + '/sites/"><span class="app-icon3"></span>' + Drupal.t('Sites') + '</a></li> <li><a href="' + href + '/social/"><span class="app-icon24"></span>' + Drupal.t('Social') + '</a></li> <li><a href="' + href + '/contactmanager/"><span class="app-icon18"></span>' + Drupal.t('ContactManager') + '</a></li> <li><a href="' + href + '/forms/"><span class="app-icon26"></span>' + Drupal.t('Forms') + '</a></li> <li><a href="' + href + '/motivator/"><span class="app-icon30"></span>' + Drupal.t('Motivator') + '</a></li> </ul> <ul> <li>' + Drupal.t('Email & Collaboration') + '</li> <li><a href="' + href + '/mail/"><span class="app-icon10"></span>' + Drupal.t('Mail') + '</a></li> <li> <a href="' + href + '/notebook/"><span class="app-icon35"></span>' + Drupal.t('Notebook') + '</a></li> <li><a href="' + href + '/docs/"><span class="app-icon9a"></span>' + Drupal.t('Docs') + '</a></li> <li> <a href="' + href + '/projects/"><span class="app-icon12"></span>' + Drupal.t('Projects') + '</a></li> <li> <a href="' + href + '/connect/"><span class="app-icon13"></span>' + Drupal.t('Connect') + '</a></li> <li> <a href="' + href + '/bugtracker/"><span class="app-icon8"></span>' + Drupal.t('BugTracker') + '</a></li> <li> <a href="' + href + '/meeting/"><span class="app-icon11"></span>' + Drupal.t('Meeting') + '</a></li> <li> <a href="' + href + '/vault/"><span class="app-icon21"></span>' + Drupal.t('Vault') + '</a></li> <li> <a href="' + href + '/showtime/"><span class="app-icon29"></span>' + Drupal.t('ShowTime') + '</a></li> <li> <a href="' + href + '/chat/"><span class="app-icon31"></span>' + Drupal.t('Chat') + '</a></li> </ul> <ul> <li>' + Drupal.t('IT & Help Desk') + '</li> <li><a href="' + href + '/desk/"><span class="app-icon7"></span>' + Drupal.t('Desk') + '</a></li> <li> <a href="' + href + '/assist/"><span class="app-icon5"></span>' + Drupal.t('Assist') + '</a></li> <li><a href="' + href + '/sdp-ondemand.html"><span class="app-icon32"></span>' + Drupal.t('ServiceDesk Plus') + '</a></li> <li><a href="' + href + '/mdm-cloud.html"><span class="app-icon34"></span>' + Drupal.t('MDM') + '</a></li> </ul> <ul> <li>' + Drupal.t('Finance') + '</li> <li> <a href="' + href + '/books/"><span class="app-icon14"></span>' + Drupal.t('Books') + '</a></li> <li><a href="' + href + '/invoice/"><span class="app-icon15"></span>' + Drupal.t('Invoice') + '</a></li> <li><a href="' + href + '/subscriptions/"><span class="app-icon15a"></span>' + Drupal.t('Subscriptions') + '</a></li> <li><a href="' + href + '/expense/"><span class="app-icon27a"></span>' + Drupal.t('expense') + '</a></li> <li><a href="' + href + '/inventory/"><span class="app-icon28"></span>' + Drupal.t('Inventory') + '</a></li> </ul> <ul> <li>' + Drupal.t('Human Resources') + '</li> <li><a href="' + href + '/recruit/"><span class="app-icon17"></span>' + Drupal.t('Recruit') + '</a></li> <li> <a href="' + href + '/people/"><span class="app-icon16"></span>' + Drupal.t('People') + '</a></li> </ul> <ul> <li>' + Drupal.t('Business Process') + '</li> <li> <a href="' + href + '/creator/"><span class="app-icon19"></span>' + Drupal.t('Creator') + '</a></li> <li><a href="' + href + '/appcreator/"><span class="app-icon33"></span>' + Drupal.t('AppCreator') + '</a></li> <li><a href="' + href + '/reports/"><span class="app-icon20"></span>' + Drupal.t('Reports') + '</a></li> </ul> <p class="mobile-apps-link"><a href="' + href + '/mobile-apps.html">' + Drupal.t('Mobile Apps') + '</a></p> </div> </div></div><ul class="menu"> <li class="sales-marketing"><a href="javascript:;">' + Drupal.t('Sales & Marketing') + '</a> <div class="catagory-dropdown"> <div class="catagory-inner"> <div class="featured-app app-box"> <a class="product" href="' + href1 + '/crm/"><span class="app-icon1"></span>' + Drupal.t('CRM') + '<em>' + Drupal.t(' Close more deals in less time.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/salesinbox/" ><span class="app-icon36"></span>' + Drupal.t('SalesInbox') + '<em>' + Drupal.t('Email for salespeople.') + '</em><small class="new">' + Drupal.t('NEW') + '</small></a> </div> <div class="app-box no-border"><a class="product" href="' + href1 + '/salesiq/" ><span class="app-icon6"></span>' + Drupal.t('SalesIQ') + '<em>' + Drupal.t('Convert website visitors into customers.') + '</em></a> </div> <div class="app-box"> <a class="product" href="' + href + '/survey/" ><span class="app-icon4"></span>' + Drupal.t('Survey') + '<em>' + Drupal.t('Online surveys for every occasion.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href1 + '/campaigns/" ><span class="app-icon2"></span>' + Drupal.t('Campaigns') + '<em>' + Drupal.t('Reach and engage your customers.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href + '/sites/" ><span class="app-icon3"></span>' + Drupal.t('Sites') + '<em>' + Drupal.t('Design mobile friendly websites.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/social/"><span class="app-icon24"></span>' + Drupal.t('Social') + '<em>' + Drupal.t('The all-in-one social media management software.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/contactmanager/" ><span class="app-icon18"></span>' + Drupal.t('ContactManager') + '<em>' + Drupal.t('Absolute contact management.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/forms/" ><span class="app-icon26"></span>' + Drupal.t('Forms') + '<em>' + Drupal.t('Forms for mobile teams.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/motivator/" ><span class="app-icon30"></span>' + Drupal.t('Motivator') + '<em>' + Drupal.t('Sales contests made more engaging.') + '</em></a> </div> </div> </div> </li> <li class="email-collab"><a href="javascript:;" >' + Drupal.t('Email & Collaboration') + '</a> <div class="catagory-dropdown"> <div class="catagory-inner"> <div class="featured-app app-box"> <a class="product" href="' + href + lan + '/mail/' + lan1 + '" ><span class="app-icon10"></span>' + Drupal.t('Mail') + '<em>' + Drupal.t('Business email. Clean and ad-free.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/notebook/" ><span class="app-icon35"></span>' + Drupal.t('Notebook') + '<em>' + Drupal.t('A beautiful home for all your notes.') + '</em><small class="new">' + Drupal.t('NEW') + '</small></a> </div> <div class="app-box"><a class="product" href="' + href + '/docs/" ><span class="app-icon9a"></span>' + Drupal.t('Docs') + '<em>' + Drupal.t(' All your documents in one place.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href + '/projects/" ><span class="app-icon12"></span>' + Drupal.t('Projects') + '<em>' + Drupal.t('Turn every project into a success story.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href + '/connect/" ><span class="app-icon13"></span>' + Drupal.t('Connect') + '<em>' + Drupal.t(' Your social workplace.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/bugtracker/" ><span class="app-icon8"></span>' + Drupal.t('BugTracker') + ' <em>' + Drupal.t(' Fix bugs fast and ship great software.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/meeting/" ><span class="app-icon11"></span>' + Drupal.t('Meeting') + ' <em>' + Drupal.t(' Web conferencing made simple and easy.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/vault/" ><span class="app-icon21"></span>' + Drupal.t('Vault') + ' <em>' + Drupal.t('Online password manager for teams.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/showtime/" ><span class="app-icon29"></span>' + Drupal.t('ShowTime') + '<em>' + Drupal.t('Empowering presenters. Enabling audiences.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/chat/"><span class="app-icon31"></span>' + Drupal.t('Chat') + '<em>' + Drupal.t(' The smart way to work.') + '</em></a> </div> </div> </div> </li> <li class="help-desk" ><a href="javascript:;" >' + Drupal.t('IT & Help Desk') + '</a> <div class="catagory-dropdown"> <div class="catagory-inner"> <div class="featured-app app-box"> <a class="product" href="' + href + '/desk/" ><span class="app-icon7"></span>' + Drupal.t('Desk') + '<em>' + Drupal.t('Put customer service at the heart of your company.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href + '/assist/" ><span class="app-icon5"></span>' + Drupal.t('Assist') + ' <em>' + Drupal.t(' Support remote customers instantly. Manage computers located anywhere.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href + '/sdp-ondemand.html"><span class="app-icon32"></span>' + Drupal.t('ServiceDesk Plus') + '<em> ' + Drupal.t('Offer best-in-class IT support on the cloud.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href + '/mdm-cloud.html"><span class="app-icon34"></span>' + Drupal.t('Mobile Device Management') + '</a> </div>  </div> </div> </li> <li class="finance" ><a href="javascript:;" >' + Drupal.t('Finance') + '</a> <div class="catagory-dropdown"> <div class="catagory-inner"> <div class="featured-app app-box"> <a class="product" href="' + href + '/books/" ><span class="app-icon14"></span>' + Drupal.t('Books') + ' <em>' + Drupal.t(' Smart accounting for growing businesses.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href + '/invoice/" ><span class="app-icon15"></span>' + Drupal.t('Invoice') + ' <em>' + Drupal.t(' Hassle-free invoicing.') + '</em></a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/subscriptions/" ><span class="app-icon15a"></span>' + Drupal.t('Subscriptions') + ' <em>' + Drupal.t('Smarter billing for your subscription business.') + '</em> </a> </div> <div class="app-box no-border"><a class="product" href="' + href + '/expense/" ><span class="app-icon27"></span>' + Drupal.t('Expense') + ' <em>' + Drupal.t('Effortless expense reporting.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href + '/inventory/" ><span class="app-icon28"></span>' + Drupal.t('Inventory') + ' <em>' + Drupal.t('Online inventory and order management.') + '</em></a> </div> </div> </div> </li> <li class="hr" ><a href="javascript:;" >' + Drupal.t('HR') + '</a> <div class="catagory-dropdown"> <div class="catagory-inner"> <div class="featured-app app-box"> <a class="product" href="' + href + '/recruit/" ><span class="app-icon17"></span>' + Drupal.t('Recruit') + ' <em>' + Drupal.t(' Applicant tracking system.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href + '/people/" ><span class="app-icon16"></span>' + Drupal.t('People') + ' <em> ' + Drupal.t('Organize, automate and simplify your HR processes.') + '</em></a> </div> </div> </div> </li> <li class="business-proc" ><a href="javascript:;" >' + Drupal.t('Business Process') + '</a> <div class="catagory-dropdown"> <div class="catagory-inner"> <div class="featured-app app-box"> <a class="product" href="' + href + lan + '/creator/' + lan1 + '" ><span class="app-icon19"></span>' + Drupal.t('Creator') + ' <em> ' + Drupal.t('Build custom apps for your business needs.') + '</em></a> </div> <div class="app-box"><a class="product" href="' + href + '/appcreator/" ><span class="app-icon33"></span>' + Drupal.t('AppCreator') + ' <em>' + Drupal.t(' Take your business mobile.') + '</em><small class="new">' + Drupal.t('NEW') + '</small></a> </div> <div class="app-box"><a class="product" href="' + href + '/reports/" ><span class="app-icon20"></span>' + Drupal.t('Reports') + ' <em>' + Drupal.t(' Online reporting and business intelligence.') + '</em></a> </div> <div class="app-box no-border"><a class="product" target=" /_blank/" href="https://www.site24x7.com/" ><span class="app-icon25"></span>' + Drupal.t('Site24x7') + '<em>' + Drupal.t(' Website monitoring from a global perspective.') + '</em></a> </div> </div> </div> </li></ul><div class="dropdown-mask"></div>';

        var footer = '<ul><li><a href="' + href1 + '/">' + Drupal.t('Zoho Home') + '</a></li><li><a href="' + href1 + '/contact.html">' + Drupal.t('Contact') + '</a></li><li><a href="' + href1 + '/security' + security + '.html">' + Drupal.t('Security') + '</a></li><li><a href="' + href1 + '/ipr-complaints.html">' + Drupal.t('IPR Complaints') + '</a></li><li><a href="' + antispam + '">' + Drupal.t(' Anti-spam Policy') + '</a></li><li><a href="' + href1 + '/terms' + terms + '.html">' + Drupal.t('Terms of Service') + '</a></li><li><a href="' + privacy + '">' + Drupal.t(' Privacy Policy') + '</a></li>' + refound + '</ul><p>' + Drupal.t(' Â© 2016, Zoho Corporation Pvt. Ltd. All Rights Reserved.') + '</p><div class="social-icons"> <a class="facebook" rel="nofollow" href="' + fb + '" target="_blank">' + Drupal.t('FaceBook') + '</a> <a class="twitter" rel="nofollow" href="' + tw + '" target="_blank">' + Drupal.t('Twitter') + '</a> ' + linkedin + '</div><div class="footer-zohologo"> <a href="' + href1 + '/">' + Drupal.t('Zoho') + '</a> </div>';

        //var new_footer ='<ul> <li><a href="/">Zoho Home</a></li><li><a href="/contact.html">Contact</a></li><li><a href="/security.html">Security</a></li> <li><a href="/ipr-complaints.html">IPR Complaints</a></li><li><a href="/policy.html"> Anti-spam Policy</a></li><li><a href="/terms.html">Terms of Service</a></li><li><a href="/privacy.html"> Privacy Policy</a></li></ul><p>Â© 2016, Zoho Corporation Pvt. Ltd. All Rights Reserved.</p><div class="on-social"><a class="on-fb" rel="nofollow" href="//www.facebook.com/zoho" target="_blank" title="FaceBook">FaceBook</a><a class="on-tw" rel="nofollow" href="//twitter.com/zoho" target="_blank" title="Twitter">Twitter</a><a class="on-in" href="https://www.linkedin.com/company/38373" target="_blank" title="LinkedIn">'+Drupal.t('Linkedin')+'</a></div>';

        var new_footer = '<ul> <li><a href="' + href1 + '/">' + Drupal.t('Zoho Home') + '</a></li><li><a href="' + href1 + lan + lan2 + '/contact.html">' + Drupal.t('Contact') + '</a></li><li><a href="' + href1 + lan + lan2 + '/security' + security + '.html">' + Drupal.t('Security') + '</a></li> <li><a href="' + href1 + lan + lan2 + '/ipr-complaints.html">' + Drupal.t('IPR Complaints') + '</a></li><li><a href="' + lan + lan2 + antispam + '"> ' + Drupal.t(' Anti-spam Policy') + '</a></li><li><a href="' + href1 + lan + lan2 + '/terms' + terms + '.html">' + Drupal.t('Terms of Service') + '</a></li><li><a href="' + lan + lan2 + privacy + '">' + Drupal.t(' Privacy Policy') + '</a></li>' + refound + '</ul><p>' + Drupal.t(' Â© 2016, Zoho Corporation Pvt. Ltd. All Rights Reserved.') + '</p><div class="on-social"><a class="on-fb" rel="nofollow" href="' + fb + '" target="_blank" title="FaceBook">' + Drupal.t('FaceBook') + '</a><a class="on-tw" rel="nofollow" href="' + tw + '" target="_blank" title="Twitter">' + Drupal.t('Twitter') + '</a> ' + linkedin + '</div>';
        var zohosignup = {
            "productSignup": [
                {
                    "crm": [
     { "servicename": "ZohoCRM" },
                    ]
                },
 {
     "assist": [
     { "servicename": "ZohoMeeting" },
     ]
 },
 {
     "meeting": [
     { "servicename": "ZohoMeeting" },
     ]
 },
 {
     "bugtracker": [
     { "servicename": "ZohoBugTracker" },
     ]
 },
 {
     "chat": [
     { "servicename": "ZohoChat" },
     ]
 },
 {
     "campaigns": [
     { "servicename": "ZohoCampaigns" },
     ]
 },
 {
     "books": [
     { "servicename": "ZohoBooks" },
     ]
 },
 {
     "connect": [
     { "servicename": "ZohoPulse" },
     ]
 },
 {
     "contactmanager": [
     { "servicename": "ZohoContactManager" },
     ]
 },
 {
     "creator": [
     { "servicename": "ZohoCreator" },
     ]
 },
 {
     "expense": [
     { "servicename": "ZohoExpense" },
     ]
 },
 {
     "subscriptions": [
     { "servicename": "ZohoSubscriptions" },
     ]
 },
 {
     "inventory": [
     { "servicename": "ZohoInventory" },
     ]
 },
 {
     "invoice": [
     { "servicename": "ZohoInvoice" },
     ]
 },
 {
     "docs": [
     { "servicename": "ZohoPC" },
     ]
 },
 {
     "forms": [
     { "servicename": "ZohoForms" },
     ]
 },
 {
     "mail": [
     { "servicename": "VirtualOffice" },
     ]
 },
 {
     "people": [
     { "servicename": "zohopeople" },
     ]
 },
 {
     "projects": [
     { "servicename": "ZohoProjects" },
     ]
 },
 {
     "motivator": [
     { "servicename": "ZohoMotivator" },
     ]
 },
 {
     "recruit": [
     { "servicename": "ZohoRecruit" },
     ]
 },
 {
     "reports": [
     { "servicename": "ZohoReports" },
     ]
 },
 {
     "salesiq": [
     { "servicename": "LiveDesk" },
     ]
 },
 {
     "showtime": [
     { "servicename": "ZohoShowtime" },
     ]
 },
 {
     "social": [
     { "servicename": "ZohoSocial" },
     ]
 },
 {
     "sites": [
     { "servicename": "ZohoSites" },
     ]
 },
 {
     "support": [
     { "servicename": "ZohoSupport" },
     ]
 },
 {
     "desk": [
     { "servicename": "ZohoSupport" },
     ]
 },
 {
     "survey": [
     { "servicename": "ZohoSurvey" },
     ]
 },
 {
     "vault": [
     { "servicename": "ZohoVault" },
     ]
 },

            ]
        }

        getService = getserviceUrl();

        function getserviceUrl() {
            for (var i = 0; i < zohosignup["productSignup"].length; i++) {
                var keys = Object.keys(zohosignup["productSignup"][i]);
                for (var j = 0; j < keys.length; j++) {
                    if (keys[j] == productName) {
                        return [i, j, keys];
                    }

                }
            }
        }





        if (typeof getService != 'undefined') {

            var insertUsericon = '<a class="user-icon" href="https://accounts.zoho.com/login?servicename=' + zohosignup["productSignup"][getService[0]][getService[2][0]][0]["servicename"] + '">User Icon</a>'


        }

        /* NEW DESIGN FOR BELOW 767px */

        if (window.innerWidth <= 767) {
            $('.ptmenu').addClass('sticky');
            $(insertUsericon).insertAfter('.product-nav-links .mobile-menu-icon');
            $('.product-nav-links .menu').css({ 'height': window.innerHeight - 50 })
            $('.product-nav-links .menu').append('<li class="featured-apps-part"><div class="more-featured-apps"><h4>Our Featured Products</h4><div class="featured-pro"> <a class="crm" href="/crm/"><span></span>CRM</a> <a class="mail" href="/mail/"> <span></span>Mail</a> <a class="creator" href="/creator/"><span></span>Creator</a> <a class="books" href="/books/"><span></span>Books</a> </div><div class="more-links"><a href="https://www.zoho.com/" class="moreapps">More Zoho products</a></div></div></li>')
        }




        allapps = allapps.split('|');




        script.onload = urlL;
        var menuLi = $(".product-header-top .menu").children();
        $(menuLi[1]).addClass('selected');

        /* pricing common script */

        var chpriceTarget = $('.view-content');
        var pricplan = $('body').find(chpriceTarget);
        if ((pricplan.length) > 0) {
            var pricplan = $(chpriceTarget).children();
            if (pricplan.length > 3) {
                chpriceTarget.addClass('plane-four-column');
            }
        }




        /* pricing common script */

        /* SCRIPT FOR SMART MOBILE BANNER TO ACCEES MOBILE APPS */

        var userAgent = navigator.userAgent || navigator.vendor || window.opera;

        if (userAgent.match(/iPad/i) || userAgent.match(/iPhone/i) || userAgent.match(/Android/i)) {
            $('.mobile-apps-wrap, .download-apps, .app-section').css('display', 'none');

            var script = document.createElement('script');
            script.src = "https://www.zoho.com/js/zoho-smartbanner.js";
            document.getElementsByTagName('head')[0].appendChild(script);

            setTimeout(function () {
                //var productName='survey'
                var metaName = ['apple-itunes-app', 'google-play-app'];
                var ss = 0;
                var appsIds = [];
                var metaHas = false;
                appTitle = '';
                appIcon = '';
                // Filling mobile app values in JSON 
                var zoho = {
                    "mobileApps": [
                        {
                            "crm": [
                               { "title": "ZOHO CRM" },
                               { "apple": "app-id=444908810" },
                               { "google": "app-id=com.zoho.crm" },
                               { "icon": "http://a2.mzstatic.com/us/r30/Purple3/v4/3c/cc/2e/3ccc2e5b-5eb6-120c-b8a2-f10465a65330/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "mail": [
                               { "title": "ZOHO Mail" },
                               { "apple": "app-id=909262651" },
                               { "google": "app-id=com.zoho.mail" },
                               { "icon": "http://a5.mzstatic.com/us/r30/Purple6/v4/ab/e5/0e/abe50e16-1ccc-db1a-9bae-3ab7f089aa38/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "docs": [
                               { "title": "ZOHO Docs" },
                               { "apple": "app-id=388384804" },
                               { "google": "app-id=com.zoho.docs" },
                               { "icon": "http://a4.mzstatic.com/us/r30/Purple6/v4/0d/60/23/0d602381-ddc5-ae0a-027c-a3ff80376197/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "projects": [
                               { "title": "ZOHO Projects" },
                               { "apple": "app-id=511887920" },
                               { "google": "app-id=com.zoho.projects" },
                               { "icon": "http://a4.mzstatic.com/us/r30/Purple6/v4/da/d7/16/dad71651-eb0b-26f2-02e2-4765d1f4f406/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "creator": [
                               { "title": "ZOHO Creator" },
                               { "apple": "app-id=692005267" },
                               { "google": "app-id=com.zoho.creator.a" },
                               { "icon": "http://a1.mzstatic.com/us/r30/Purple69/v4/2a/76/0f/2a760f54-3b27-1e45-4a5d-f609ee35dab5/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "survey": [
                               { "title": "ZOHO Survey" },
                               { "apple": "app-id=638610965" },
                               { "google": "app-id=com.zoho.survey" },
                               { "icon": "http://a3.mzstatic.com/us/r30/Purple4/v4/65/54/ff/6554ff4e-3d5f-5a6d-70b7-7083494d6c65/icon175x175.png" },
                            ]
                        },
                        {
                            "campaigns": [
                               { "title": "ZOHO Campaigns" },
                               { "apple": "app-id=804728901" },
                               { "google": "app-id=com.zoho.campaigns" },
                               { "icon": "http://a2.mzstatic.com/us/r30/Purple69/v4/3e/6e/6f/3e6e6fc9-64a6-b7b4-74fa-ab88a596134a/icon175x175.png" },
                            ]
                        },
                        {
                            "books": [
                               { "title": "ZOHO Books" },
                               { "apple": "app-id=710446064" },
                               { "google": "app-id=com.zoho.books" },
                               { "icon": "http://a3.mzstatic.com/us/r30/Purple6/v4/73/d4/17/73d41732-7618-b7ec-38d7-af63980c0bf2/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "invoice": [
                               { "title": "ZOHO Invoice" },
                               { "apple": "app-id=413017364" },
                               { "google": "app-id=com.zoho.invoice" },
                               { "icon": "http://a5.mzstatic.com/us/r30/Purple6/v4/00/67/ec/0067ecae-fe81-19d9-07e3-b015c9e9dc25/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "expense": [
                               { "title": "ZOHO Expense" },
                               { "apple": "app-id=966094166" },
                               { "google": "app-id=com.zoho.expense" },
                               { "icon": "http://a3.mzstatic.com/us/r30/Purple6/v4/b1/fd/67/b1fd6750-8a5b-dcd7-e786-6d032a5ada1d/icon175x175.png" },
                            ]
                        },
                        {
                            "inventory": [
                               { "title": "ZOHO Inventory" },
                               { "apple": "app-id=1037960494" },
                               { "google": "app-id=com.zoho.inventory" },
                               { "icon": "http://a1.mzstatic.com/us/r30/Purple6/v4/4e/2e/d2/4e2ed207-54d0-1c40-6f1b-9914d43f8e0d/icon175x175.png" },
                            ]
                        },
                        {
                            "support": [
                               { "title": "ZOHO Support" },
                               { "apple": "app-id=692742510" },
                               { "google": "app-id=com.zoho.support" },
                               { "icon": "http://a2.mzstatic.com/us/r30/Purple3/v4/14/28/c2/1428c239-4630-bf6e-0c86-26bbf4871007/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "desk": [
                               { "title": "ZOHO Desk" },
                               { "apple": "app-id=692742510" },
                               { "google": "app-id=com.zoho.support" },
                               { "icon": "http://a2.mzstatic.com/us/r30/Purple3/v4/14/28/c2/1428c239-4630-bf6e-0c86-26bbf4871007/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "connect": [
                               { "title": "ZOHO Connect" },
                               { "apple": "app-id=650742465" },
                               { "google": "app-id=com.zoho.zohopulse" },
                               { "icon": "http://a5.mzstatic.com/us/r30/Purple69/v4/e6/b4/76/e6b47648-c5ed-8c21-6626-53b18f82d9a3/icon175x175.png" },
                            ]
                        },
                        {
                            "vault": [
                               { "title": "ZOHO Vault" },
                               { "apple": "app-id=634857858" },
                               { "google": "app-id=com.zoho.vault" },
                               { "icon": "http://a1.mzstatic.com/us/r30/Purple69/v4/74/12/de/7412de11-224b-5b9e-2683-cbf551338943/icon175x175.png" },
                            ]
                        },
                        {
                            "social": [
                               { "title": "ZOHO Social" },
                               { "apple": "app-id=906377559" },
                               { "google": "app-id=com.zoho.zohosocial" },
                               { "icon": "http://a1.mzstatic.com/us/r30/Purple69/v4/c7/08/b1/c708b1be-f861-863d-c5df-187966a06dbe/icon175x175.png" },
                            ]
                        },
                        {
                            "people": [
                               { "title": "ZOHO People" },
                               { "apple": "app-id=680525956" },
                               { "google": "app-id=com.zoho.people" },
                               { "icon": "http://a1.mzstatic.com/us/r30/Purple69/v4/ab/04/74/ab04743d-046f-d86b-9337-a626714470c1/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "assist": [
                               { "title": "ZOHO Assist" },
                               { "apple": "app-id=582922932" },
                               { "google": "app-id=com.zoho.assist" },
                               { "icon": "http://a4.mzstatic.com/us/r30/Purple3/v4/e9/9b/80/e99b8063-1034-30d3-ec99-cf7e27930a28/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "contactmanager": [
                               { "title": "ZOHO ContactManager" },
                               { "apple": "app-id=705188406" },
                               { "google": "app-id=com.zoho.cm" },
                               { "icon": "http://a5.mzstatic.com/us/r30/Purple69/v4/d4/d6/7a/d4d67a6d-eeee-8464-5357-9f677e537876/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "forms": [
                               { "title": "ZOHO Forms" },
                               { "apple": "app-id=805565969" },
                               { "google": "app-id=com.zoho.forms.a" },
                               { "icon": "http://a3.mzstatic.com/us/r30/Purple69/v4/0d/41/2a/0d412a87-e4ed-3e68-184d-8264462cfbb2/icon175x175.jpeg" },
                            ]
                        },
                        {
                            "showtime": [
                               { "title": "ZOHO ShoeTime" },
                               { "apple": "app-id=925233601" },
                               { "google": "app-id=com.zoho.showtime.presenter" },
                               { "icon": "http://a3.mzstatic.com/us/r30/Purple69/v4/90/b9/ff/90b9ff29-c423-c2de-b3a2-e396fe3d068c/icon175x175.png" },
                            ]
                        },
                        {
                            "recruit": [
                               { "title": "ZOHO Recruit" },
                               { "apple": "app-id=981522421" },
                               { "google": "app-id=com.zoho.recruit" },
                               { "icon": "http://a4.mzstatic.com/us/r30/Purple69/v4/54/8d/71/548d713b-24d2-acce-eb55-76288492e5d3/icon175x175.png" },
                            ]
                        },
                         {
                             "reports": [
                               { "title": "ZOHO Reports" },
                               { "apple": "app-id=902365462" },
                               { "google": "app-id=com.zoho.reports" },
                               { "icon": "http://a2.mzstatic.com/us/r30/Purple4/v4/cb/01/ce/cb01ce7a-e0f8-ed54-d48a-e973fc61d03d/icon175x175.jpeg" },
                             ]
                         },
                         {
                             "chat": [
                                { "title": "ZOHO Chat" },
                                { "apple": "app-id=1056478397" },
                                { "google": "app-id=com.zoho.chat" },
                                { "icon": "http://a2.mzstatic.com/us/r30/Purple49/v4/f2/52/5b/f2525ba1-0a73-4fcb-68b0-e97986490a27/icon175x175.jpeg" },
                             ]
                         },
                         {
                             "motivator": [
                                { "title": "ZOHO Motivator" },
                                { "apple": "app-id=1091206693" },
                                { "icon": "http://a1.mzstatic.com/us/r30/Purple60/v4/24/a8/41/24a841bb-72e2-1f66-56b0-2c5107a23b0b/icon175x175.png" },
                             ]
                         },
                         {
                             "salesiq": [
                                { "title": "ZOHO SalesIQ" },
                                { "apple": "app-id=1094699978" },
                                { "google": "app-id=com.zoho.salesiq" },
                                { "icon": "http://a4.mzstatic.com/us/r30/Purple69/v4/4a/7c/fb/4a7cfbf6-1885-e25e-85a7-8ec772a5d536/icon175x175.jpeg" },
                             ]
                         },


                    ]
                }

                /* get The data mobile app data from the JSON */
                function getIndex() {
                    for (var i = 0; i < zoho["mobileApps"].length; i++) {
                        var keys = Object.keys(zoho["mobileApps"][i]);
                        for (var j = 0; j < keys.length; j++) {
                            if (keys[j] == productName) {
                                var keys2 = zoho["mobileApps"][i][keys[j]].length;
                                appsIds = [zoho["mobileApps"][i][keys[j]][1]["apple"], zoho["mobileApps"][i][keys[j]][2]["google"]]
                                appendingMetaTag()
                                return [i, j, keys];
                            }

                        }
                    }
                }

                // Appending meta tags script stared.

                function appendingMetaTag() {
                    $('meta').each(function (index, tag) {
                        var content = tag.getAttribute('name');

                        if (content == 'apple-itunes-app' || content == 'google-play-app') {
                            metaHas = true;
                            appTitle = tag.getAttribute('app-title');
                            appIcon = tag.getAttribute('app-icon');

                        }
                    });
                    if (!metaHas) {
                        if (productName == 'survey' || productName == 'inventory' || productName == 'assist') { loopVal = 0 } else { loopVal = 1; }
                        for (var i = 0; i <= loopVal; i++) {
                            var meta = document.createElement('meta');
                            meta.name = metaName[i];
                            meta.content = appsIds[i];
                            document.getElementsByTagName('head')[0].appendChild(meta);
                        }
                    }
                }

                // Appending meta tags script stared.

                getVal = getIndex();    			// getValues 

                // Triggering smart Banner script started.

                if (!metaHas) {
                    //$('.header').addClass('add-smartbanner');
                    var android = location.href.match(/#android$/) || navigator.userAgent.match(/Android/i) != null;
                    if (typeof getVal != 'undefined') {
                        $.smartbanner({
                            title: zoho["mobileApps"][getVal[0]][getVal[2][0]][0]["title"],
                            author: ' ',
                            icon: zoho["mobileApps"][getVal[0]][getVal[2][0]][3]["icon"],
                            force: android ? 'android' : 'ios',
                            daysHidden: 0,
                            daysReminder: 0
                        });
                    }
                }
                else {
                    //$('.header').addClass('add-smartbanner');
                    var android = location.href.match(/#android$/) || navigator.userAgent.match(/Android/i) != null;
                    if (typeof getVal != 'undefined') {
                        $.smartbanner({
                            title: appTitle,
                            author: ' ',
                            icon: appIcon,
                            force: android ? 'android' : 'ios',
                            daysHidden: 0,
                            daysReminder: 0
                        });
                    }
                }

                // Triggering smart Banner script end.

                // Toggling smart Banner script started.

                $('.theme-toggle').on('click', function (e) {
                    e.preventDefault();
                    $.smartbanner('switchType')
                    $(this).text($(this).text() == 'Preview Android' ? 'Preview iOS' : 'Preview Android')
                })
                if (android) $('.theme-toggle').text('Preview iOS');
            }, 1000)
        }

        // Toggling smart Banner script end.


        /* SCRIPT FOR SMART MOBILE BANNER TO ACCESS MOBILE APPS  ENDS */


        function urlL() {
            var getusername = zohouser.DISPLAY_NAME;
            geturL();

            if (getusername) {
                if (productName == 'showtime') {
                    $('.signing').after('<a class="joinatalk" style="display:inline-block; padding: 10px 20px;float:right;color:#333;font-size:12px;font-weight:600;    margin:0;text-transform: uppercase;" href="https://showtime.zoho.com/join" target="_blank">Join a talk</a> ');
                }
            }
            if (!getusername) {
                $('.try-now').addClass('signupbtn-hide').removeClass('signupbtn-show');

                var zsignup_url = "/" + productName + "/";
                var zsignup_text = Drupal.t("Sign Up Now");
                if (productName == "meeting") {
                    zsignup_url += "lp/signup.html?version=freetrial&service=meeting&src=topbar-try";
                }
                else if (productName == "mail") {
                    zsignup_url += "zohomail-pricing.html";
                    zsignup_text = "Get Started";
                }
                else if (productName == "assist") {
                    zsignup_url += "signup.html?version=freetrial";
                }
                else if (productName == "projects") {
                    zsignup_url += "zp-signup.html";
                }
                else if (productName == "salesiq") {
                    zsignup_url += "signup.html?source=Website.Header";
                }
                else if (productName == "wiki") {
                    zsignup_url += "wiki-signup.html";
                }
                else if (productName == 'docs') {
                    zsignup_text = "Get Started Now";
                    zsignup_url += "signup.html";
                }
                else {
                    zsignup_url += "signup.html";
                }
                if (productName != 'show' && productName != 'writer' && productName != 'crmplus' && productName != 'appcreator') {
                    $('.signing').html('<a class="signup" href="' + href + zsignup_url + '">' + zsignup_text + '</a> <a class="signin" href="' + href + '/' + productName + '/login.html">' + Drupal.t("LOGIN") + '</a>');
                }
                if (productName == 'showtime') {
                    $('.signing').after('<a class="joinatalk" style="display:inline-block; padding: 10px 20px;float:right;color:#333;font-size:12px;font-weight:600;    margin:0;text-transform: uppercase;" href="https://showtime.zoho.com/join" target="_blank">Join a talk</a> ');
                    $('.signing').html('<a class="signup" href="' + href + zsignup_url + '">' + zsignup_text + '</a> <a class="signin" href="' + href + '/' + productName + '/login.html">' + Drupal.t("LOGIN") + '</a>');
                }
                if (productName == 'appcreator') { $('.signing').html(' <a class="signin" href="' + href + '/' + productName + '/login.html">' + Drupal.t("LOGIN") + '</a>'); }
                if (productName == 'crmplus') {
                    $('.signing').html('<a class="signup" href="/crm/crmplus/signup.html">Sign Up</a> <a class="signin" href="/crm/crmplus/login.html">' + Drupal.t("LOGIN") + '</a>');
                }
                if (productName == 'show') {
                    $('.signing').html('<a class="signup" href="https://docs.zoho.com/show/signup">Get Started Now</a> <a class="signin" href="https://docs.zoho.com/show/signup?signin=true">' + Drupal.t("LOGIN") + '</a>');
                }
                if (productName == 'writer') {
                    $('.signing').html('<a class="signup" href="https://docs.zoho.com/writer/signup">Get Started Now</a> <a class="signin" href="https://docs.zoho.com/writer/signup?signin=true">' + Drupal.t("LOGIN") + '</a>');
                }
                $('.header .signing .signin').css({ 'opacity': 1 });
                if (window.innerWidth <= 767) {
                    $('.user-icon').css({ 'display': 'block' })
                }
                /****For language based signup ***/

                var langaug = ['es', 'it', 'fr'];
                jQuery(langaug).each(function (intValue, currentElement) {
                    if (window.location.href.indexOf('/' + currentElement + '/') > -1) {
                        var href = '/' + currentElement;
                        var href1 = $('.signing .signup').attr('href');
                        var url_path = window.location.pathname.split('/');
                        if (url_path[2] == currentElement) {
                            jQuery('.signing .signup').attr('href', "/" + url_path[1] + href + "/signup.html");
                            if (url_path[3] == 'crmplus') {
                                jQuery('.signing .signup').attr('href', "/crm/" + currentElement + "/crmplus/signup.html");
                            }
                        }
                        else {
                            jQuery('.signing .signup').attr('href', href + href1);
                        }
                    }
                });
            }
            else {

                function decodeHexString(str) {
                    str = str.replace(/\\x([0-9A-Fa-f]{2})/g, function () { return String.fromCharCode(parseInt(arguments[1], 16)); });
                    str = str.replace(/\\u([\d\w]{4})/gi, function () {
                        return String.fromCharCode(parseInt(arguments[1], 16));
                    });
                    return str;

                }

                var getusername = decodeHexString(zohouser.DISPLAY_NAME);

                $('.mobile-btn').hide();
                $('.try-now').addClass('signupbtn-show').removeClass('signupbtn-hide');

                /* this is for welcome messgae insert */

                $('#navbar').addClass('uheader');

                var getHref = window.location.href;
                if (k2 == 'crm') { k2 = 'CRM'; }
                else if (k2 == 'contactmanager') { k2 = 'ContactManager'; }
                else if (k2 == 'salesiq') { k2 = 'SalesIQ'; }
                else if (k2 == 'bugtracker') { k2 = 'BugTracker'; }
                else if (k2 == 'showtime') { k2 = 'ShowTime'; }
                else if (k2 == 'appcreator') { k2 = 'AppCreator'; }
                else if (k2 == 'crmplus') { k2 = 'CRM Plus'; }
                else if (k2 == 'show') { k2 = 'Docs'; }
                else if (k2 == 'writer') { k2 = 'Docs'; }
                var logOutUrl = 'https://accounts.zoho.com/logout?serviceurl=' + getHref;
                var insertsignIn = '<div class="userinfo-container"><div class="userinfo-details"><div class="zoho-proinner-container"><span id="userinfo-name">' + getusername + '</span><a class="userinfo-access" href=' + $gotoPage + ' target="_blank">' + accessZoho + k2 + '</a></div><div class="userinfo-avatar"><img src=' + $userAvator + ' alt=' + getusername + ' class="userPho"/></div><div class="account-signout-details"><a target="_blank" class="userinfo-account" href="https://accounts.' + k + '.com/u/h#home">' + myAccount + '</a><a id="userinfo-signout" href="#">' + signOut + '</a></div></div></div><a href=' + $gotoPage + ' class="goto-link">' + accessZoho + k2 + '</a>'
                //$('.signing').html(insertsignIn);

                if (window.innerWidth <= 767) {

                    $(insertsignIn).insertAfter('#mini-panel-product_menu .menu-icon-help-header');
                    $('#userinfo-signout').attr("href", logOutUrl);
                }

                else {
                    if ($('body').find('.header').length > 0) {
                        var headIS = $('body').find('.home-bg');
                        if (headIS.length != 1) {
                            $('.signing').html(insertsignIn);
                            $('#userinfo-signout').attr("href", logOutUrl);

                            setTimeout(function () {
                                $('.header').css({ 'overflow': 'visible' });
                                var m = $('.signing .goto-link').innerWidth();
                                var checkProinner = $('body').find('.zoho-proinner-container');
                                if (checkProinner.length != 0) {
                                    var m1 = $('.zoho-proinner-container').css('padding-left').replace('px', '');
                                    getWidth = m + parseInt(m1);
                                }
                            }, 100)
                        }
                    }
                }

                $(".banner-content .getstarted-box").replaceWith("<div class='learn-more'><a href='features.html'>Learn More</a>");
                $('.gr-sign').replaceWith("<a class='gr-sign' href='https://bugtracker.zoho.com/portals.do'>Try Now</a>");
                $('.gr-sign1').replaceWith("<a class='gr-sign1' href='https://bugtracker.zoho.com/portals.do'>Try Now</a>");
                $(".buz .ms").html("<a href='https://bugtracker.zoho.com/portals.do'>Get Started Now</a>");
                if (k2 == 'bugtracker') { $(".three .signup").html("<a href='https://bugtracker.zoho.com/portals.do'>Try Now</a>"); }
                /*For Common*/
                $('.navbar-toggle').css('top', '30px');
            }
            setTimeout(function () {
                if (jQuery('#mini-panel-product_menu').length != 0) {
                    var getTop = $('#mini-panel-product_menu').offset().top;
                    $(".mask-holder,.mobile-menu").css({ 'top': getTop });
                }
            }, 200)

            setTimeout(function () {
                var m = $('.signing .goto-link').innerWidth();
                var checkProinner = $('body').find('.zoho-proinner-container');
                if (checkProinner.length != 0) {
                    var m1 = $('.zoho-proinner-container').css('padding-left').replace('px', '');
                    getWidth = m + parseInt(m1);
                }
            }, 100)

        }
        script.onload = urlL;
        for (i = 0; i < allapps.length; i++) {
            cat = allapps[i].split(',');
            for (j = 0; j < cat.length; j++) {
                if (cat[j] == productName) {
                    $(".header").append(header);
                    if (/\/zoholics\//.test(location.href) == false)
                        $(".common-footer").html(footer);
                    $(".common-links").html(new_footer);
                    var menuLi = $(".header .menu").children();
                    $(menuLi[i]).addClass('selected');
                }
            }

        }
        //}

        /* End of Header footer common HTML Js */

        $('.product-header-top').html($('#mini-panel-product_menu').html());


        function resetUrl(productName) {
            m = window.location.href;
            m = m.replace("http://www.", '');
            m = m.replace("https://www.", '');
            m = m.replace("http://", '');
            m = m.replace("https://", '');
            k = m.split(".com");
            if (k[1] != null) {
                findEs = k[1].split('/');


                if (productName == 'crm' && findEs.indexOf('es') > -1) {
                    accessZoho = 'Accesar Zoho ';
                    myAccount = 'Mi Cuenta';
                    signOut = 'Salir';
                }
                else {
                    accessZoho = 'Access Zoho ';
                    myAccount = 'My Account';
                    signOut = 'Sign Out';
                }
            }
            if (k.length > 1) {
                k = k[0];
                k2 = productName;
            } else {
                k2 = '';
            }




            /* thisis for product homepage link url*/
            $userAvator = 'https://contacts.zoho.com/file?t=user&fs=thumb';
            if (k2 == 'crm' || k2 == 'campaigns' || k2 == 'chat' || k2 == 'accounts' || k2 == 'salesiq' || k2 == 'mail' || k2 == 'docs' || k2 == 'connect' || k2 == 'support' || k2 == 'desk' || k2 == 'books' || k2 == 'invoice' || k2 == 'subscriptions' || k2 == 'expense' || k2 == 'showtime' || k2 == 'forms') {
                $gotoPage = 'https://' + k2 + '.zoho.com'
            }
            else if (k2 == 'projects') { $gotoPage = 'https://projects.zoho.com/portals.do' }
            else if (k2 == 'bugtracker') { $gotoPage = 'https://bugtracker.zoho.com/portals.do' }
            else if (k2 == 'people') { $gotoPage = 'https://people.zoho.com/people' }
            else if (k2 == 'assist') { $gotoPage = 'https://meeting.zoho.com/assist' }
            else if (k2 == 'survey') { $gotoPage = 'https://survey.zoho.com/home.do' }
            else if (k2 == 'sites') { $gotoPage = 'https://sites.zoho.com/index' }
            else if (k2 == 'social') { $gotoPage = 'https://social.zoho.com/Home.do' }
            else if (k2 == 'meeting') { $gotoPage = 'https://meeting.zoho.com/jsp/service-check.jsp?service=meeting' }
            else if (k2 == 'vault') { $gotoPage = 'https://vault.zoho.com/online/main' }
            else if (k2 == 'recruit') { $gotoPage = 'https://recruit.zoho.com/recruit/ShowHomePage.do' }
            else if (k2 == 'creator') { $gotoPage = 'https://creator.zoho.com/home' }
            else if (k2 == 'contactmanager') { $gotoPage = 'https://cm.zoho.com/index.do' }
            else if (k2 == 'motivator') { $gotoPage = 'https://motivator.zoho.com/home.html' }
            else if (k2 == 'crmplus') { $gotoPage = 'https://crmplus.zoho.com' }
            else if (k2 == 'appcreator') { $gotoPage = 'https://appcreator.zoho.com' }
            else if (k2 == 'reports') { $gotoPage = 'https://reports.zoho.com' }
            else if (k2 == 'wiki') { $gotoPage = 'https://wiki.zoho.com' }
            else if (k2 == 'show') { $gotoPage = 'https://docs.zoho.com/' }
            else if (k2 == 'writer') { $gotoPage = 'https://docs.zoho.com/' }
            else if (k2 == 'desk') { $gotoPage = 'https://desk.zoho.com/' }
        }

        //var getusername =  decodeHexString(zohouser.DISPLAY_NAME);

        //$('.product-header-top').html($('.product-header').html());

        $('.product-header-top .menu-icon-help-header > a').addClass('logo').html('ZOHO');


        /* Remove the sites24*7 for zoho.eu*/
        var zohoDomain = document.domain;
        if (zohoDomain == "www.zoho.eu") {
            $('.business-proc .no-border').remove();
            $('.business-proc a[href="/appcreator/"]').parent().remove();
        }



        var nod = $('.signing').clone();
        //$('.product-header-top .product-logo').after(nod);
        $('.product-header-top .product-nav-links .menu').before(nod);
        $(window).scroll(function () {												/*  slide down the menu  */
            slideDown();
        })
        slideDown();

        var clicked = false;
        var navClick = false;
        $('.menu-icon').click(function () {
            if (window.innerWidth <= 550) {
                $('.product-menu').hide();
            }

            if (navClick == false) {
                navClick = true;
                $('.mask-holder').removeClass('hide');
                setTimeout(function () {
                    $('.mobile-menu').css({ 'left': '0%' });
                }, 100)

                setTimeout(function () {
                    $('html,body').css({ 'overflow': 'hidden' })
                }, 0)
            }
            else {
                hide();
            }
        })

        $('.mask-holder').click(function (e) {
            if (e.delegateTarget.className == e.target.className) {
                hide();
            }
        });

        /* this is for adding signout popup */
        gotoSignupnav();

        var checkVisible = $('.product-nav-links .menu').css('display');
        $('.userinfo-container').css({ 'display': 'block' });
        $('body').click(function (e) {


            var getsignupTarget = e.target.className;
            var bottomSignup = e.target.parentNode.className;
            var getTargetId = e.target.parentNode.parentNode.className;


            if (getsignupTarget == 'signup' || bottomSignup == 'get-started') {
                if (window.innerWidth > 767) {
                    if (getAllField.length != 0) {
                        e.preventDefault();
                        gotoSignupnav();
                    }
                }

            }

            if (e.target.className == 'menu') { }
            else if (e.target.className == 'mobile-menu-icon') {

                $('.product-nav-links .menu').css({ 'height': window.innerHeight - 50 });
                if (clicked == false) { clicked = true; $('.menu').slideDown(300); $('body').css({ 'overflow': 'hidden' }); }
                else { clicked = false; $('.product-nav-links .menu').slideUp(300); $('body').css({ 'overflow': 'auto' }); }
            }

            else {

                if (clicked == true) {
                    clicked = false;
                    $('.product-nav-links .menu').slideUp(300);
                    $('html,body').css({ 'overflow': 'auto' })
                }
            }

            if (getTargetId == 'userinfo-details') {
                $('.userinfo-details').addClass("zoho-expand");
                if (getWidth > 285) {
                    $('.userinfo-details.zoho-expand').css({ 'width': getWidth + 10, 'height': '135px' });
                }

            }
            else {

                $('.userinfo-details').removeClass("zoho-expand");
                if (getWidth > 285) {
                    $('.userinfo-details').css({ 'width': '35px', 'height': '35px' });
                    $('.product-header-top .userinfo-details, .product-header-top .userinfo-container').css({ 'height': '43px' });
                }
            }
        })


        $(document).on('keydown', function (e) {
            keys = e.which;
            if (keys == 27) { $('.userinfo-details').removeClass("zoho-expand"); }
        })

        jQuery('.get-started a,.posibilities-bot').click(function (e) {
            if (window.innerWidth > 767) {
                e.preventDefault();
                gotoSignupnav();
            }

        })


        anchorN = $('a');
        aa = [];
        for (i = 0; i < anchorN.length; i++) {
            aa[i] = $(anchorN[i]).attr('href');
            if (aa[i] == "javascript:void(0);") {
                $(anchorN[i]).css({ "cursor": "text" });
            }

        }

        function geturL() {

            var logedUsername = zohouser.DISPLAY_NAME;
            if (logedUsername) {
                $('.header').addClass('uheader');
                if ($("body").find(".main")) {
                    $('.main').addClass('umain');
                    $(document).trigger('umain-trigger');
                }

                if ($("body").find(".main-container-wrapper")) {
                    $('.main-container-wrapper').addClass('umain');
                }

                function decodeHexString(str) {
                    str = str.replace(/\\x([0-9A-Fa-f]{2})/g, function () { return String.fromCharCode(parseInt(arguments[1], 16)); });
                    str = str.replace(/\\u([\d\w]{4})/gi, function () {
                        return String.fromCharCode(parseInt(arguments[1], 16));
                    });
                    return str;
                }
                getusername = decodeHexString(zohouser.DISPLAY_NAME);
            }
        }

        /* this is for goto signup form */
        function gotoSignupnav() {
            getAllField = $('.banner-signup .sgfrm');
            if (getAllField.length != 0) {
                var getTarget = $(getAllField[0]).children();


                /*if(window.innerWidth>767){
                      e.preventDefault();*/
                $('html, body').animate({
                    scrollTop: 0,
                }, 1000, function () {
                    setTimeout(function () {
                        $('.focusin').removeClass('focusin');
                    }, 500)
                })
                setTimeout(function () {
                    for (var i = 0; i < getTarget.length; i++) {
                        var getType = $(getTarget[i]).attr('type');
                        if (getType == 'text' || getType == 'email') {
                            $(getTarget[i]).addClass('focusin')
                            $(getTarget[i]).focus();
                        }
                    }
                }, 1000)
                //}
            }
        }

        /* this is for adding signout popup */
        /* this is for header hover animation */

        var timeout;
        var $getCh;
        var menutimeout;
        var mouseEnter = true;
        var timeDelay = 200;
        $('.dropdown-mask').css({ 'height': $('.catagory-dropdown').innerHeight() })
        $('.header .menu >  li').hover(
            function (e) {

                $this = $(this);
                var bposX = e.clientX;
                timeout = setTimeout(function () {
                    var aposX = e.clientX;
                    if ($('.dropdown-mask').hasClass('showmaskCheck')) {
                        timeDelay = 0;
                        if (bposX == aposX) {
                            $getCh = $this.children()[1];
                            $($getCh).addClass('addMe');
                            $('.catagory-dropdown.addMe').children().addClass('showMe');
                        }
                    }
                }, timeDelay);
            },
            function () {
                clearTimeout(timeout);
                $($getCh).removeClass('addMe');
                $('.catagory-dropdown').children().removeClass('showMe');
            });

        $('.header .menu').hover(function () {
            if ($('.dropdown-mask').hasClass('showmaskCheck')) {
                timeDelay = 0;
            }
            else { timeDelay = 200; }
            menutimeout = setTimeout(function () {
                $('.dropdown-mask').addClass('showmask');
            }, 100);
            $('.dropdown-mask').addClass('showmaskCheck');
        }, function () {
            clearTimeout(menutimeout);
            $('.dropdown-mask').removeClass('showmaskCheck');
            $('.dropdown-mask').removeClass('showmask');
            timeDelay = 200;
        })



        function hide() {
            navClick = false;
            $('.mobile-menu').css({ 'left': '-100%' });
            setTimeout(function () {
                $('.mask-holder').addClass('hide');
            }, 100)

            setTimeout(function () {
                $('html,body').css({ 'overflow': 'auto' });
            }, 400)
        }
    });

    function slideDown(getWidth) {
        $('.userinfo-details').removeClass("zoho-expand");
        if (getWidth > 285) {
            $('.userinfo-details').css({ 'width': 35, 'height': 35 });
            $('.product-header-top .userinfo-details, .product-header-top .userinfo-container').css({ 'height': '43px' });
        }

        if ($(window).scrollTop() > 200) {
            $('.product-header-top').removeClass('slide');
            $('.product-header-top').css({ 'overflow': 'visible' });
        }
        else {
            $('.product-header-top').addClass('slide');
            $('.product-header-top').css({ 'overflow': 'hidden' });
        }
    }


    function intervelStart() {
        setInt = setTimeout(function () {
            count += 1;
            if (count <= 1) {
                $('.banner-part').css({ 'left': 0 - ($(bannerCh[count]).offset().left) })
                $(zSlide[count]).addClass('Zselect').siblings().removeClass('Zselect');
                intervelStart();
            }
            else {
                $('.banner-part').css({ 'left': 0 })
                count = 0;
                $(zSlide[count]).addClass('Zselect').siblings().removeClass('Zselect');
                intervelStart();
            }
        }, speed)
    }
    $(document).ready(function (e) {
        hideFeatureheader();
        $('.navbar-toggle').click(function (e) { e.stopPropagation(); $('.header-menu-2 ul.menu').slideToggle(); });
    });

    $(window).resize(function () {
        if (window.innerWidth <= 767) {
            $('.product-nav-links .menu').css({ 'height': window.innerHeight - 50, 'display': 'none' });
        }
        else {
            $('.product-nav-links .menu').css({ 'height': 'auto', 'top': 'auto', 'display': 'block' });
            $('.product-header-inner .userinfo-container,.product-header-inner .goto-link').remove();
        }
    })


    /* Help Pages responsive */
    $(document).ready(function () {

        if ($('.node-type-product-help .panel-region-sidebar-first').length) {
            $('<div id="helpMenuIconCont"><span id="help-menu">Menu</span></div>').insertBefore('.panel-region-content.crm-help-content');

            addMenu();

            $(window).resize(function (e) {
                addMenu();
                hideFeatureheader()
            });

            $('#help-menu').click(function () {
                $('.node-type-product-help .panel-region-sidebar-first').toggleClass('slide-in');
                $('.node-type-product-help #help-menu').toggleClass('slide-in-menu');
                setTimeout(function () {
                    $('body').toggleClass('overflowHidden');
                }, 500)
            });
        }

        /* add class to benefits*/
        var benefitsCh = $('#benefits ul').children().length;

        if (benefitsCh == 5) {
            $('#benefits').addClass('mid-align');
        }
        if (benefitsCh == 4) {
            $('#benefits').addClass('two-column-box');
        }
        //var setHeight = $(".features-item .cont-part").height()+200;
        //$(".features-item .image-part").css({'max-height':(setHeight)});
    });

    function addMenu() {
        $('.node-type-product-help .panel-region-sidebar-first').is(":visible");
        if (window.innerWidth <= 1000) {
            $('.node-type-product-help .panel-region-sidebar-first').css({ 'height': window.innerHeight - $('.panel-region-sidebar-first').offset().top });
        }
    }

    function hideFeatureheader() {
        if (window.innerWidth < 767) {
            $('#header .menu').css({ 'display': 'none' });
            $('.mobile-menu-icon').css({ 'display': 'block' });
        }
        else {
            $('.mobile-menu-icon').css({ 'display': 'none' });
        }
    }

})(jQuery);

;/**/


jQuery(document).ready(function () {
    setTimeout(function () {
        jQuery('.animation-wrap > div').each(function (i) {
            var t = jQuery(this);
            setTimeout(function () { t.addClass('animated'); }, (i + 1) * 100);
        });
    }, 100)

    /* feature list */
    var getAlllist = jQuery('.features-list').children();
    var gerAlltopval = [];
    for (var i = 0; i < getAlllist.length; i++) {
        gerAlltopval[i] = jQuery(getAlllist[i]).offset().top - window.innerHeight + jQuery('.features-list li').innerHeight();
    }

    jQuery(window).scroll(function () {
        var sTop = jQuery(window).scrollTop();
        for (var i = 0; i < getAlllist.length; i++) {
            if (sTop > gerAlltopval[i]) {
                if (i % 2 == 1) {
                    jQuery(getAlllist[i]).addClass('from-right');
                }
                else {
                    jQuery(getAlllist[i]).addClass('from-left');
                }
            }
        }

        if (sTop > gerAlltopval[getAlllist.length - 1] + 150) {
            jQuery('.bottom-signup,.download-features a').css({ 'opacity': 1 })
        }

        jQuery('.product-header.sub-header').css({ 'top': -76 });
    })


    var clicked = false;
    var checkVisible = jQuery('.product-menu').css('display');
    jQuery('body').click(function (e) {
        if (e.target.className == 'product-menu') {
        }
        else if (e.target.className == 'sub-menu-icon') {
            if (clicked == false) {
                clicked = true;
                jQuery('.product-menu').slideDown(300);
            }
            else {
                clicked = false;
                jQuery('.product-menu').slideUp(300);
            }
        }

        else {
            if (clicked == true) {
                clicked = false;

                jQuery('.product-menu').slideUp(300);
                jQuery('html,body').css({ 'overflow': 'auto' })
            }
        }
    })

    jQuery('.product-menu li').click(function (e) {
        var getClass = jQuery(this).attr('class');
        var getSplit = getClass.split('-');
        if (getSplit[0] == 'crmplus') {
            e.preventDefault();
            var moveTotop = jQuery("section[data=" + getSplit[1] + "]").offset().top;
            jQuery('html, body').animate({
                scrollTop: moveTotop - jQuery('.sub-header').innerHeight(),
            }, 1000, "easeInOutQuad")
        }
    })

    jQuery('.bottom-signup,.signing .signup').click(function (e) {
        e.preventDefault();
        jQuery('html, body').animate({
            scrollTop: 0,
        }, 1000, "easeInOutQuad")
        setTimeout(function () {
            jQuery('.signup-box div:first-child input').focus();
        }, 1000)
    });

});

/***************** ADD ANIMATION SCRIPT *******************/
; (function (jQuery) {
    jQuery.fn.visible = function (partial) {
        var jQueryt = jQuery(this),
		jQueryw = jQuery(window),
		viewTop = jQueryw.scrollTop(),
		viewBottom = viewTop + jQueryw.height(),
		_top = (jQueryt.length > 0) ? (jQueryt.offset().top) + 100 : 0,
		_bottom = _top + jQueryt.height(),
		compareTop = partial === true ? _bottom : _top,
		compareBottom = partial === true ? _top : _bottom;
        return ((compareBottom <= viewBottom) && (compareTop >= viewTop));
    };
})(jQuery);
; (function (jQuery) {
    function animation() {
        if (jQuery(window).width() > 767) {
            jQuery(".content-wrap:not(.animated)").each(function () {
                var jQuerythis = jQuery(this), jQuerydelay = (jQuerythis.attr('data-delay')) ? jQuerythis.attr('data-delay') : 0;
                setTimeout(function () {
                    if (jQuerythis.visible(true)) {
                        jQuerythis.addClass(jQuerythis.attr('data-animation'));
                        jQuerythis.addClass('animated');
                    }
                }, jQuerydelay);
            });
        }
    }
    jQuery(document).ready(function () {
        animation();
    });
    jQuery(window).resize(function () {
        animation();
    });
    jQuery(window).scroll(function () {
        animation();
    });
})(jQuery);
/***************** ADD ANIMATION SCRIPT *******************/
$ = jQuery.noConflict();
/***crmplus**/
jQuery(document).ready(function () {
    var script = document.createElement('script');
    script.src = "https://accounts.zoho.com/u/info";
    document.getElementsByTagName('head')[0].appendChild(script);
    script.onload = urlL;
    var proTi = jQuery('#mini-panel-product_menu .product-title a').text();

    if (proTi == "CRM Plus") {
        var CrmPLUS_URL = window.location.pathname.split('/');
        for (var i = 0; i < CrmPLUS_URL.length; i++) {
            if (CrmPLUS_URL[1] != 'crm') {
                lang = CrmPLUS_URL[1];
                url = "/" + lang + "/crm/crmplus/";
                jQuery('#mini-panel-product_menu .menu-icon-help-header a,#mini-panel-product_menu .product-title a').attr('href', url);
                jQuery('.product-header-top .menu-icon-help-header a,.product-header-top .product-title a').attr('href', url);
            }
            else if (CrmPLUS_URL[1] == 'crm' && CrmPLUS_URL[2] == 'crmplus') {
                jQuery('#mini-panel-product_menu .menu-icon-help-header a,#mini-panel-product_menu .product-title a').attr('href', '/crm/crmplus/');
                jQuery('.product-header-top .menu-icon-help-header a,.product-header-top .product-title a').attr('href', '/crm/crmplus/');
            }
            else if (CrmPLUS_URL[2] !== 'crm' && CrmPLUS_URL[2] !== 'crmplus') {
                lang = CrmPLUS_URL[2];
                url = "/crm/" + lang + "/crmplus/";
                jQuery('#mini-panel-product_menu .menu-icon-help-header a,#mini-panel-product_menu .product-title a').attr('href', url);
                jQuery('.product-header-top .menu-icon-help-header a,.product-header-top .product-title a').attr('href', url);
            }
        }
    }
    else if (proTi == "Show") {
        jQuery('#mini-panel-product_menu .menu-icon-help-header a,#mini-panel-product_menu .product-title a').attr('href', '/docs/show.html');
        jQuery('.product-header-top .menu-icon-help-header a,.product-header-top .product-title a').attr('href', '/docs/show.html');
    }
    else if (proTi == "Writer") {
        jQuery('#mini-panel-product_menu .menu-icon-help-header a,#mini-panel-product_menu .product-title a').attr('href', 'https://www.zoho.com/docs/writer.html');
        jQuery('.product-header-top .menu-icon-help-header a,.product-header-top .product-title a').attr('href', 'https://www.zoho.com/docs/writer.html');
    }
});
function urlL(event) {
    var proTi = jQuery('#mini-panel-product_menu .product-title a').text();
    var getusername = zohouser.DISPLAY_NAME;
    if (proTi == "CRM Plus") {
        if (getusername) {
            event.preventDefault();

            jQuery('.signing a.goto-link').attr('href', 'https://crmplus.zoho.com');
            jQuery('.signing a.goto-link').text('Access Zoho CRM Plus');
            jQuery('.signing .userinfo-container .zoho-proinner-container a').text('Access Zoho CRM Plus');
            jQuery('.signing .userinfo-container .zoho-proinner-container a').attr('href', 'https://crmplus.zoho.com');
        }
        else {
            jQuery('.signing .signin').attr('href', '/crm/crmplus/login.html');
            jQuery('.signing .signup').attr('href', '/crm/crmplus/signup.html')

        }
    }
}
/***crmplus**/
/* Hiding header and footer for project */
jQuery(document).ready(function () {
    try {
        if (window.self !== window.top) {
            if (/\/projects\//.test(location.href) == true) {
                jQuery("#header").hide();
                jQuery(".footer-wrapper").hide();
                jQuery(".tab-search").hide();
                jQuery(".print-help").hide();
            }
        }
    }
    catch (e) { }
});
/****	Sites *****/
jQuery(document).ready(function () {
    var prodGID = jQuery('body').hasClass('og-context-node-8078');
    if (prodGID) {
        jQuery('.only-common .common-links ul li:nth-child(6) a').attr('href', '/sites/terms.html');
        jQuery('.only-common .common-links ul li:nth-child(6) a').text('Terms of Use')
    }


});

/***********************************/
// Watch Video
/***********************************/
/**
jQuery(document).ready(function(){

	if(jQuery('body').find('.watch-video')){
if(jQuery('.z-pouup-wrap').length == 0){
		jQuery('body').append('<div class="z-pouup-wrap" style="display:none;"><div class="z-pouup-wrap-inner"><div class="z-popup-content-wrap"><div class="z-popup-content-inner"><div class="z-popup-content animated fadeInDown"><div id="zoho-video" class="popup-data"></div><div class="popup-close"><img src="https://www.zoho.com/creator/images/close1.png" width="65" height="65" alt="close"></div></div></div></div></div><div class="z-popup-overlay"></div></div>');
	}
}
	var script = document.createElement('script');
	script.src = "https://www.zoho.com/js/jquery.fitvids.js";
	document.getElementsByTagName('head')[0].appendChild(script);

	jQuery(document).on('click', '.watch-video', function(e) {
	    e.preventDefault();
var getPlayvideo=jQuery(this).attr('href');
		var video_src = getPlayvideo+'?&autoplay=1&rel=0&showinfo=0&autohide=1';
		if( /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent) ) {
				window.location.href = video_src;
	        }
else{
	    var $selector = jQuery(this).attr('href'); 

	    jQuery('.z-popup-content-inner .popup-data').prepend('<div class="video-vendor"><iframe width="466" height="262" src="'+jQuery(this).attr('href')+'?rel=0&autoplay=1&showinfo=0&autohide=1" frameborder="0" allowfullscreen ></iframe></div>');
	    
		jQuery('.z-popup-content-inner').fitVids();
		jQuery('.z-pouup-wrap').addClass('z-video-wrap');
	          
	    jQuery('.z-pouup-wrap').fadeIn(300);
	    jQuery('html').css('overflow', 'hidden');
}
	});
	jQuery(document).on('click', '.popup-close, .z-popup-overlay', function(e) {

		e.preventDefault();
		setTimeout(function() {
		  jQuery('.z-pouup-wrap').fadeOut(function() {
		    var $content = jQuery(this).find('.z-popup-content-inner .popup-data');
		    $content.find('.video-vendor').remove();
		    jQuery('.z-pouup-wrap').removeClass('z-video-wrap');
		  });
		  jQuery('html').css('overflow', 'auto');
		}, 100);

	});

	jQuery(document).keyup(function(e) {
		if (e.keyCode === 27 && jQuery('.z-pouup-wrap').is(":visible")) {
		  jQuery('.popup-close').trigger('click');
		}

	});

});*/
/***********************************/
// Watch Video
/***********************************/