//'use strict';

//angular.module('ap.fotorama', [])

//    .value('apFotoramaConfig', {
//        //width:'100%', и проч. настройки по умолчанию
//        //Имена полей в фотораме и приложении
//        id: 'id',      //имя поля с id картинки
//        thumb: 'thumb',   //имя поля с миниатюрой
//        img: 'img',     //имя поля с изображением
//        full: 'full',    //имя поля с оригиналом
//        caption: 'caption', //имя с заголовком
//        active: 'active',  //указатель активной фотки
//        domain: '',        //'http://tamtakoe.ru/uploader/' //для кроссдоменных запросов
//        //Колбеки событий
//        show: null,
//        showend: null,
//        fullscreenenter: null,
//        fullscreenexit: null,
//        loadvideo: null,
//        unloadvideo: null,
//        stagetap: null,
//        ready: null,
//        error: null,
//        load: null,
//        stopautoplay: null,
//        startautoplay: null
//    })
//    .directive('apFotorama', ['apFotoramaConfig', function (apFotoramaConfig) {
//        return {
//            require: '?ngModel',
//            link: function (scope, element, attrs, ngModel) {

//                var opts = {},
//                    collection,
//                    events = 'show showend fullscreenenter fullscreenexit loadvideo unloadvideo stagetap ready error load stopautoplay startautoplay'.split(' ');

//                angular.extend(opts, apFotoramaConfig);

//                element.bind('fotorama:showend', function (e, fotorama, extra) {
//                    if (collection !== undefined && typeof scope[attrs.ngModel] === 'object') {
//                        //Записываем активную фотку в модель
//                        setActive(collection.activeIndex);

//                        scope.$$phase || scope.$apply(); //Не всегда срабатывает в первый раз, если бы не было начального переключения на фотку
//                    }
//                });

//                //Преобразование массивов данных в массивы, эквивалентные внутреннему массиву Фоторамы
//                function makeFotoramaArray(res, update) {
//                    var n = typeof res === 'object' ? res.length : 0,
//                        activeIndex;

//                    for (var i = 0, nn = n, arr = [], ci; i < nn; i++) {
//                        if (res[i].id !== undefined) {
//                            ci = arr.push({}) - 1;
//                            arr[ci].id = res[i][opts.id];
//                            arr[ci].thumb = res[i][opts.thumb] !== undefined ? opts.domain + res[i][opts.thumb] : res[i][opts.thumb];
//                            arr[ci].img = res[i][opts.img] !== undefined ? opts.domain + res[i][opts.img] : res[i][opts.thumb];
//                            arr[ci].full = res[i][opts.full] !== undefined ? opts.domain + res[i][opts.full] : res[i][opts.thumb];
//                            arr[ci].html = res[i][opts.html];
//                            arr[ci].caption = res[i][opts.caption];

//                            if (res[ci][opts.active]) activeIndex = ci;

//                            if (update && collection.data) collection.splice(i, 1, arr[i]);

//                        } else {
//                            n--;
//                        }
//                    }
//                    return { arr: arr, arrLength: n, activeIndex: activeIndex }
//                }

//                function setActive(index) {
//                    index = index === undefined && this !== undefined && this.$index !== undefined ? this.$index : index;

//                    for (var i = 0, n = scope[attrs.ngModel].length; i < n; i++) {
//                        scope[attrs.ngModel][i][opts.active] = index == i ? true : false;
//                    }
//                }
//                scope.setActive = setActive;

//                scope.$watch(attrs.ngModel, function (newVal, oldVal) {

//                    //Если модель изменилась, синхронизируем с ней Фотораму
//                    if (oldVal !== newVal) {

//                        var oKeys = {}, nKeys = {}, i, oi, temp;

//                        var oArr = collection.data ? collection.data : [];
//                        var o = oArr.length,
//                            oldActiveIndex = collection.activeIndex;

//                        temp = makeFotoramaArray(newVal);
//                        var nArr = temp.arr,
//                            n = temp.arrLength,
//                            activeIndex = temp.activeIndex;

//                        if (o) {

//                            //Алгоритм преобразования массива фотографий в соответствии с моделью с минимальным количеством перестановок. http://plnkr.co/edit/clW0aVqzaisUkUo44EOL?p=preview
//                            for (i = 0; i < o; i++) {
//                                oKeys[oArr[i].id] = i;
//                            }
//                            for (i = 0; i < n; i++) {
//                                nKeys[nArr[i].id] = i;
//                            }
//                            for (i = 0, oi = 0; i < n; i++, oi++) {

//                                if (oArr[oi] === undefined) oArr[oi] = { id: null };

//                                if (nArr[i].id !== oArr[oi].id) {

//                                    //Добавление нового элемента  
//                                    if (oKeys[nArr[i].id] === undefined) {

//                                        collection.splice(i, 0, nArr[i]);
//                                        oi--;
//                                        //console.log('+add')
//                                    }

//                                    //Удаление элемента
//                                    if (oi >= 0 && nKeys[oArr[oi].id] === undefined) {

//                                        if (oldActiveIndex == oi && activeIndex === undefined) activeIndex = i !== n ? i : i - 2; //меняем активную фотку, если она была удалена, на следующую/предыдущую
//                                        collection.splice(i, 1);
//                                        i--;
//                                        //console.log('-del')
//                                    }

//                                    //Смена позиции
//                                    if (i >= 0 && oKeys[nArr[i].id] !== undefined && nKeys[oArr[oi].id] !== undefined) {

//                                        if ((oKeys[nArr[i].id] - oKeys[oArr[oi].id]) > (nKeys[oArr[oi].id] - nKeys[nArr[i].id])) {

//                                            collection.splice(i, 0, nArr[i]);
//                                            delete nKeys[nArr[i].id];
//                                            oi--;
//                                            //console.log('add')
//                                        } else {

//                                            collection.splice(i, 1);
//                                            delete oKeys[oArr[oi].id];
//                                            i--;
//                                            //console.log('del')
//                                        }
//                                    }
//                                } else if (nArr[i].img !== oArr[oi].img) {

//                                    //Замена картинки
//                                    collection.splice(i, 1, nArr[i]);
//                                    //console.log('change')
//                                }
//                            }

//                            //Удаляем оставшиеся в конце элементы и меняем активную фотку, если она была удалена, на предыдущую
//                            if (o > oi) collection.splice(n, o - (oi));
//                            if (oldActiveIndex >= n && activeIndex === undefined) activeIndex = n - 1;

//                        } else if (n) {
//                            //Если фоток не было, то инициализируем фотораму заново и обновляем настройки, т.к. они сбрасываются
//                            collection.setOptions(opts).load(nArr).setOptions(scope[attrs.apFotorama]);
//                            //console.log('load data')
//                        }

//                        //Переключаемся на активную фотку
//                        if (n) activeIndex !== undefined ? collection.show(activeIndex) : collection.show(0);
//                    }

//                }, true);

//                //Смотрим изменение настроек
//                scope.$watch(attrs.apFotorama, function (newVal, oldVal) {
//                    angular.extend(opts, apFotoramaConfig, newVal);
//                    collection.setOptions(opts)

//                    //Устанавливаем события
//                    //TODO: Уничтожение старых событий
//                    angular.forEach(events, function (event) {
//                        if (typeof opts[event] === 'function') {
//                            element.bind('fotorama:' + event, function (e, fotorama, extra) {
//                                opts[event](e, extra);
//                            });
//                        }
//                    });

//                    if (newVal.thumb || newVal.img || newVal.full) {
//                        //Обновляем фотораму, если поменялись имена картинок
//                        makeFotoramaArray(scope[attrs.ngModel], true);
//                    }
//                    //collection.setOptions(newVal);
//                }, true);

//                // Создаем фотораму
//                collection = element.fotorama(opts).data('fotorama');

//                //Копируем в настройки значения по умолчанию
//                scope[attrs.apFotorama] = angular.extend({}, collection.options, scope[attrs.apFotorama]);
//            }
//        };
//    }
//    ]);


/*!
 * Fotorama 4.5.1 | http://fotorama.io/license/
 */
! function (a, b, c, d, e) {
    "use strict";

    function f(a) {
        var b = "bez_" + d.makeArray(arguments).join("_").replace(".", "p");
        if ("function" != typeof d.easing[b]) {
            var c = function (a, b) {
                var c = [null, null],
                    d = [null, null],
                    e = [null, null],
                    f = function (f, g) {
                        return e[g] = 3 * a[g], d[g] = 3 * (b[g] - a[g]) - e[g], c[g] = 1 - e[g] - d[g], f * (e[g] + f * (d[g] + f * c[g]))
                    }, g = function (a) {
                        return e[0] + a * (2 * d[0] + 3 * c[0] * a)
                    }, h = function (a) {
                        for (var b, c = a, d = 0; ++d < 14 && (b = f(c, 0) - a, !(Math.abs(b) < .001)) ;) c -= b / g(c);
                        return c
                    };
                return function (a) {
                    return f(h(a), 1)
                }
            };
            d.easing[b] = function (b, d, e, f, g) {
                return f * c([a[0], a[1]], [a[2], a[3]])(d / g) + e
            }
        }
        return b
    }

    function g() { }

    function h(a, b, c) {
        return Math.max(isNaN(b) ? -1 / 0 : b, Math.min(isNaN(c) ? 1 / 0 : c, a))
    }

    function i(a) {
        return a.match(/ma/) && a.match(/-?\d+(?!d)/g)[a.match(/3d/) ? 12 : 4]
    }

    function j(a) {
        return Ec ? +i(a.css("transform")) : +a.css("left").replace("px", "")
    }

    function k(a, b) {
        var c = {};
        return Ec ? c.transform = "translate3d(" + (a + (b ? .001 : 0)) + "px,0,0)" : c.left = a, c
    }

    function l(a) {
        return {
            "transition-duration": a + "ms"
        }
    }

    function m(a, b) {
        return +String(a).replace(b || "px", "") || e
    }

    function n(a) {
        return /%$/.test(a) && m(a, "%")
    }

    function o(a, b) {
        return n(a) / 100 * b || m(a)
    }

    function p(a) {
        return (!!m(a) || !!m(a, "%")) && a
    }

    function q(a, b, c, d) {
        return (a - (d || 0)) * (b + (c || 0))
    }

    function r(a, b, c, d) {
        return -Math.round(a / (b + (c || 0)) - (d || 0))
    }

    function s(a) {
        var b = a.data();
        if (!b.tEnd) {
            var c = a[0],
                d = {
                    WebkitTransition: "webkitTransitionEnd",
                    MozTransition: "transitionend",
                    OTransition: "oTransitionEnd otransitionend",
                    msTransition: "MSTransitionEnd",
                    transition: "transitionend"
                };
            c.addEventListener(d[mc.prefixed("transition")], function (a) {
                b.tProp && a.propertyName.match(b.tProp) && b.onEndFn()
            }, !1), b.tEnd = !0
        }
    }

    function t(a, b, c, d) {
        var e, f = a.data();
        f && (f.onEndFn = function () {
            e || (e = !0, clearTimeout(f.tT), c())
        }, f.tProp = b, clearTimeout(f.tT), f.tT = setTimeout(function () {
            f.onEndFn()
        }, 1.5 * d), s(a))
    }

    function u(a, b, c) {
        if (a.length) {
            var d = a.data();
            Ec ? (a.css(l(0)), d.onEndFn = g, clearTimeout(d.tT)) : a.stop();
            var e = v(b, function () {
                return j(a)
            });
            return a.css(k(e, c)), e
        }
    }

    function v() {
        for (var a, b = 0, c = arguments.length; c > b && (a = b ? arguments[b]() : arguments[b], "number" != typeof a) ; b++);
        return a
    }

    function w(a, b) {
        return Math.round(a + (b - a) / 1.5)
    }

    function x() {
        return x.p = x.p || ("https:" === c.protocol ? "https://" : "http://"), x.p
    }

    function y(a) {
        var c = b.createElement("a");
        return c.href = a, c
    }

    function z(a, b) {
        if ("string" != typeof a) return a;
        a = y(a);
        var c, d;
        if (a.host.match(/youtube\.com/) && a.search) {
            if (c = a.search.split("v=")[1]) {
                var e = c.indexOf("&"); -1 !== e && (c = c.substring(0, e)), d = "youtube"
            }
        } else a.host.match(/youtube\.com|youtu\.be/) ? (c = a.pathname.replace(/^\/(embed\/|v\/)?/, "").replace(/\/.*/, ""), d = "youtube") : a.host.match(/vimeo\.com/) && (d = "vimeo", c = a.pathname.replace(/^\/(video\/)?/, "").replace(/\/.*/, ""));
        return c && d || !b || (c = a.href, d = "custom"), c ? {
            id: c,
            type: d,
            s: a.search.replace(/^\?/, "")
        } : !1
    }

    function A(a, b, c) {
        var e, f, g = a.video;
        return "youtube" === g.type ? (f = x() + "img.youtube.com/vi/" + g.id + "/default.jpg", e = f.replace(/\/default.jpg$/, "/hqdefault.jpg"), a.thumbsReady = !0) : "vimeo" === g.type ? d.ajax({
            url: x() + "vimeo.com/api/v2/video/" + g.id + ".json",
            dataType: "jsonp",
            success: function (d) {
                a.thumbsReady = !0, B(b, {
                    img: d[0].thumbnail_large,
                    thumb: d[0].thumbnail_small
                }, a.i, c)
            }
        }) : a.thumbsReady = !0, {
            img: e,
            thumb: f
        }
    }

    function B(a, b, c, e) {
        for (var f = 0, g = a.length; g > f; f++) {
            var h = a[f];
            if (h.i === c && h.thumbsReady) {
                var i = {
                    videoReady: !0
                };
                i[Uc] = i[Wc] = i[Vc] = !1, e.splice(f, 1, d.extend({}, h, i, b));
                break
            }
        }
    }

    function C(a) {
        function b(a, b, e) {
            var f = a.children("img").eq(0),
                g = a.attr("href"),
                h = a.attr("src"),
                i = f.attr("src"),
                j = b.video,
                k = e ? z(g, j === !0) : !1;
            k ? g = !1 : k = j, c(a, f, d.extend(b, {
                video: k,
                img: b.img || g || h || i,
                thumb: b.thumb || i || h || g
            }))
        }

        function c(a, b, c) {
            var e = c.thumb && c.img !== c.thumb,
                f = m(c.width || a.attr("width")),
                g = m(c.height || a.attr("height"));
            d.extend(c, {
                width: f,
                height: g,
                thumbratio: R(c.thumbratio || m(c.thumbwidth || b && b.attr("width") || e || f) / m(c.thumbheight || b && b.attr("height") || e || g))
            })
        }
        var e = [];
        return a.children().each(function () {
            var a = d(this),
                f = Q(d.extend(a.data(), {
                    id: a.attr("id")
                }));
            if (a.is("a, img")) b(a, f, !0);
            else {
                if (a.is(":empty")) return;
                c(a, null, d.extend(f, {
                    html: this,
                    _html: a.html()
                }))
            }
            e.push(f)
        }), e
    }

    function D(a) {
        return 0 === a.offsetWidth && 0 === a.offsetHeight
    }

    function E(a) {
        return !d.contains(b.documentElement, a)
    }

    function F(a, b, c) {
        a() ? b() : setTimeout(function () {
            F(a, b)
        }, c || 100)
    }

    function G(a) {
        c.replace(c.protocol + "//" + c.host + c.pathname.replace(/^\/?/, "/") + c.search + "#" + a)
    }

    function H(a, b, c) {
        var d = a.data(),
            e = d.measures;
        if (e && (!d.l || d.l.W !== e.width || d.l.H !== e.height || d.l.r !== e.ratio || d.l.w !== b.w || d.l.h !== b.h || d.l.m !== c)) {
            var f = e.width,
                g = e.height,
                i = b.w / b.h,
                j = e.ratio >= i,
                k = "scaledown" === c,
                l = "contain" === c,
                m = "cover" === c;
            j && (k || l) || !j && m ? (f = h(b.w, 0, k ? f : 1 / 0), g = f / e.ratio) : (j && m || !j && (k || l)) && (g = h(b.h, 0, k ? g : 1 / 0), f = g * e.ratio), a.css({
                width: Math.ceil(f),
                height: Math.ceil(g),
                marginLeft: Math.floor(-f / 2),
                marginTop: Math.floor(-g / 2)
            }), d.l = {
                W: e.width,
                H: e.height,
                r: e.ratio,
                w: b.w,
                h: b.h,
                m: c
            }
        }
        return !0
    }

    function I(a, b) {
        var c = a[0];
        c.styleSheet ? c.styleSheet.cssText = b : a.html(b)
    }

    function J(a, b, c) {
        return b === c ? !1 : b >= a ? "left" : a >= c ? "right" : "left right"
    }

    function K(a, b, c, d) {
        if (!c) return !1;
        if (!isNaN(a)) return a - (d ? 0 : 1);
        for (var e, f = 0, g = b.length; g > f; f++) {
            var h = b[f];
            if (h.id === a) {
                e = f;
                break
            }
        }
        return e
    }

    function L(a, b, c) {
        c = c || {}, a.each(function () {
            var a, e = d(this),
                f = e.data();
            f.clickOn || (f.clickOn = !0, d.extend(X(e, {
                onStart: function (b) {
                    a = b, (c.onStart || g).call(this, b)
                },
                onMove: c.onMove || g,
                onTouchEnd: c.onTouchEnd || g,
                onEnd: function (c) {
                    c.moved || b.call(this, a)
                }
            }), {
                noMove: !0
            }))
        })
    }

    function M(a, b) {
        return '<div class="' + a + '">' + (b || "") + "</div>"
    }

    function N(a) {
        for (var b = a.length; b;) {
            var c = Math.floor(Math.random() * b--),
                d = a[b];
            a[b] = a[c], a[c] = d
        }
        return a
    }

    function O(a) {
        return "[object Array]" == Object.prototype.toString.call(a) && d.map(a, function (a) {
            return d.extend({}, a)
        })
    }

    function P(a, b) {
        Ac.scrollLeft(a).scrollTop(b)
    }

    function Q(a) {
        if (a) {
            var b = {};
            return d.each(a, function (a, c) {
                b[a.toLowerCase()] = c
            }), b
        }
    }

    function R(a) {
        if (a) {
            var b = +a;
            return isNaN(b) ? (b = a.split("/"), +b[0] / +b[1] || e) : b
        }
    }

    function S(a, b) {
        a.preventDefault(), b && a.stopPropagation()
    }

    function T(a) {
        return a ? ">" : "<"
    }

    function U(a, b) {
        var c = a.data(),
            e = Math.round(b.pos),
            f = function () {
                c.sliding = !1, (b.onEnd || g)()
            };
        "undefined" != typeof b.overPos && b.overPos !== b.pos && (e = b.overPos, f = function () {
            U(a, d.extend({}, b, {
                overPos: b.pos,
                time: Math.max(Nc, b.time / 2)
            }))
        });
        var h = d.extend(k(e, b._001), b.width && {
            width: b.width
        });
        c.sliding = !0, Ec ? (a.css(d.extend(l(b.time), h)), b.time > 10 ? t(a, "transform", f, b.time) : f()) : a.stop().animate(h, b.time, Xc, f)
    }

    function V(a, b, c, e, f, h) {
        var i = "undefined" != typeof h;
        if (i || (f.push(arguments), Array.prototype.push.call(arguments, f.length), !(f.length > 1))) {
            a = a || d(a), b = b || d(b);
            var j = a[0],
                k = b[0],
                l = "crossfade" === e.method,
                m = function () {
                    if (!m.done) {
                        m.done = !0;
                        var a = (i || f.shift()) && f.shift();
                        a && V.apply(this, a), (e.onEnd || g)(!!a)
                    }
                }, n = e.time / (h || 1);
            c.removeClass(Kb + " " + Jb), a.stop().addClass(Kb), b.stop().addClass(Jb), l && k && a.fadeTo(0, 0), a.fadeTo(l ? n : 0, 1, l && m), b.fadeTo(n, 0, m), j && l || k || m()
        }
    }

    function W(a) {
        var b = (a.touches || [])[0] || a;
        a._x = b.pageX, a._y = b.clientY, a._now = d.now()
    }

    function X(c, e) {
        function f(a) {
            return n = d(a.target), v.checked = q = r = t = !1, l || v.flow || a.touches && a.touches.length > 1 || a.which > 1 || wc && wc.type !== a.type && yc || (q = e.select && n.is(e.select, u)) ? q : (p = "touchstart" === a.type, r = n.is("a, a *", u), o = v.control, s = v.noMove || v.noSwipe || o ? 16 : v.snap ? 0 : 4, W(a), m = wc = a, xc = a.type.replace(/down|start/, "move").replace(/Down/, "Move"), (e.onStart || g).call(u, a, {
                control: o,
                $target: n
            }), l = v.flow = !0, void ((!p || v.go) && S(a)))
        }

        function h(a) {
            if (a.touches && a.touches.length > 1 || Kc && !a.isPrimary || xc !== a.type || !l) return l && i(), void (e.onTouchEnd || g)();
            W(a);
            var b = Math.abs(a._x - m._x),
                c = Math.abs(a._y - m._y),
                d = b - c,
                f = (v.go || v.x || d >= 0) && !v.noSwipe,
                h = 0 > d;
            p && !v.checked ? (l = f) && S(a) : (S(a), (e.onMove || g).call(u, a, {
                touch: p
            })), !t && Math.sqrt(Math.pow(b, 2) + Math.pow(c, 2)) > s && (t = !0), v.checked = v.checked || f || h
        }

        function i(a) {
            (e.onTouchEnd || g)();
            var b = l;
            v.control = l = !1, b && (v.flow = !1), !b || r && !v.checked || (a && S(a), yc = !0, clearTimeout(zc), zc = setTimeout(function () {
                yc = !1
            }, 1e3), (e.onEnd || g).call(u, {
                moved: t,
                $target: n,
                control: o,
                touch: p,
                startEvent: m,
                aborted: !a || "MSPointerCancel" === a.type
            }))
        }

        function j() {
            v.flow || setTimeout(function () {
                v.flow = !0
            }, 10)
        }

        function k() {
            v.flow && setTimeout(function () {
                v.flow = !1
            }, Mc)
        }
        var l, m, n, o, p, q, r, s, t, u = c[0],
            v = {};
        return Kc ? (u[Jc]("MSPointerDown", f, !1), b[Jc]("MSPointerMove", h, !1), b[Jc]("MSPointerCancel", i, !1), b[Jc]("MSPointerUp", i, !1)) : (u[Jc] && (u[Jc]("touchstart", f, !1), u[Jc]("touchmove", h, !1), u[Jc]("touchend", i, !1), b[Jc]("touchstart", j, !1), b[Jc]("touchend", k, !1), b[Jc]("touchcancel", k, !1), a[Jc]("scroll", k, !1)), c.on("mousedown", f), Bc.on("mousemove", h).on("mouseup", i)), c.on("click", "a", function (a) {
            v.checked && S(a)
        }), v
    }

    function Y(a, b) {
        function c(c, d) {
            A = !0, j = l = c._x, q = c._now, p = [
                [q, j]
            ], m = n = D.noMove || d ? 0 : u(a, (b.getPos || g)(), b._001), (b.onStart || g).call(B, c)
        }

        function e(a, b) {
            s = D.min, t = D.max, v = D.snap, x = a.altKey, A = z = !1, y = b.control, y || C.sliding || c(a)
        }

        function f(d, e) {
            D.noSwipe || (A || c(d), l = d._x, p.push([d._now, l]), n = m - (j - l), o = J(n, s, t), s >= n ? n = w(n, s) : n >= t && (n = w(n, t)), D.noMove || (a.css(k(n, b._001)), z || (z = !0, e.touch || Kc || a.addClass(Zb)), (b.onMove || g).call(B, d, {
                pos: n,
                edge: o
            })))
        }

        function i(e) {
            if (!D.noSwipe || !e.moved) {
                A || c(e.startEvent, !0), e.touch || Kc || a.removeClass(Zb), r = d.now();
                for (var f, i, j, k, o, q, u, w, y, z = r - Mc, C = null, E = Nc, F = b.friction, G = p.length - 1; G >= 0; G--) {
                    if (f = p[G][0], i = Math.abs(f - z), null === C || j > i) C = f, k = p[G][1];
                    else if (C === z || i > j) break;
                    j = i
                }
                u = h(n, s, t);
                var H = k - l,
                    I = H >= 0,
                    J = r - C,
                    K = J > Mc,
                    L = !K && n !== m && u === n;
                v && (u = h(Math[L ? I ? "floor" : "ceil" : "round"](n / v) * v, s, t), s = t = u), L && (v || u === n) && (y = -(H / J), E *= h(Math.abs(y), b.timeLow, b.timeHigh), o = Math.round(n + y * E / F), v || (u = o), (!I && o > t || I && s > o) && (q = I ? s : t, w = o - q, v || (u = q), w = h(u + .03 * w, q - 50, q + 50), E = Math.abs((n - w) / (y / F)))), E *= x ? 10 : 1, (b.onEnd || g).call(B, d.extend(e, {
                    moved: e.moved || K && v,
                    pos: n,
                    newPos: u,
                    overPos: w,
                    time: E
                }))
            }
        }
        var j, l, m, n, o, p, q, r, s, t, v, x, y, z, A, B = a[0],
            C = a.data(),
            D = {};
        return D = d.extend(X(b.$wrap, {
            onStart: e,
            onMove: f,
            onTouchEnd: b.onTouchEnd,
            onEnd: i,
            select: b.select
        }), D)
    }

    function Z(a, b) {
        var c, e, f, h = a[0],
            i = {
                prevent: {}
            };
        return h[Jc] && h[Jc](Lc, function (a) {
            var h = a.wheelDeltaY || -1 * a.deltaY || 0,
                j = a.wheelDeltaX || -1 * a.deltaX || 0,
                k = Math.abs(j) > Math.abs(h),
                l = T(0 > j),
                m = e === l,
                n = d.now(),
                o = Mc > n - f;
            e = l, f = n, k && i.ok && (!i.prevent[l] || c) && (S(a, !0), c && m && o || (b.shift && (c = !0, clearTimeout(i.t), i.t = setTimeout(function () {
                c = !1
            }, Oc)), (b.onEnd || g)(a, b.shift ? l : j)))
        }, !1), i
    }

    function $() {
        d.each(d.Fotorama.instances, function (a, b) {
            b.index = a
        })
    }

    function _(a) {
        d.Fotorama.instances.push(a), $()
    }

    function ab(a) {
        d.Fotorama.instances.splice(a.index, 1), $()
    }
    var bb = "fotorama",
        cb = "fullscreen",
        db = bb + "__wrap",
        eb = db + "--css2",
        fb = db + "--css3",
        gb = db + "--video",
        hb = db + "--fade",
        ib = db + "--slide",
        jb = db + "--no-controls",
        kb = db + "--no-shadows",
        lb = db + "--pan-y",
        mb = db + "--rtl",
        nb = db + "--only-active",
        ob = db + "--no-captions",
        pb = db + "--toggle-arrows",
        qb = bb + "__stage",
        rb = qb + "__frame",
        sb = rb + "--video",
        tb = qb + "__shaft",
        ub = bb + "__grab",
        vb = bb + "__pointer",
        wb = bb + "__arr",
        xb = wb + "--disabled",
        yb = wb + "--prev",
        zb = wb + "--next",
        Ab = bb + "__nav",
        Bb = Ab + "-wrap",
        Cb = Ab + "__shaft",
        Db = Ab + "--dots",
        Eb = Ab + "--thumbs",
        Fb = Ab + "__frame",
        Gb = Fb + "--dot",
        Hb = Fb + "--thumb",
        Ib = bb + "__fade",
        Jb = Ib + "-front",
        Kb = Ib + "-rear",
        Lb = bb + "__shadow",
        Mb = Lb + "s",
        Nb = Mb + "--left",
        Ob = Mb + "--right",
        Pb = bb + "__active",
        Qb = bb + "__select",
        Rb = bb + "--hidden",
        Sb = bb + "--fullscreen",
        Tb = bb + "__fullscreen-icon",
        Ub = bb + "__error",
        Vb = bb + "__loading",
        Wb = bb + "__loaded",
        Xb = Wb + "--full",
        Yb = Wb + "--img",
        Zb = bb + "__grabbing",
        $b = bb + "__img",
        _b = $b + "--full",
        ac = bb + "__dot",
        bc = bb + "__thumb",
        cc = bc + "-border",
        dc = bb + "__html",
        ec = bb + "__video",
        fc = ec + "-play",
        gc = ec + "-close",
        hc = bb + "__caption",
        ic = bb + "__caption__wrap",
        jc = bb + "__spinner",
        kc = d && d.fn.jquery.split(".");
    if (!kc || kc[0] < 1 || 1 == kc[0] && kc[1] < 8) throw "Fotorama requires jQuery 1.8 or later and will not run without it.";
    var lc = {}, mc = function (a, b, c) {
        function d(a) {
            r.cssText = a
        }

        function e(a, b) {
            return typeof a === b
        }

        function f(a, b) {
            return !!~("" + a).indexOf(b)
        }

        function g(a, b) {
            for (var d in a) {
                var e = a[d];
                if (!f(e, "-") && r[e] !== c) return "pfx" == b ? e : !0
            }
            return !1
        }

        function h(a, b, d) {
            for (var f in a) {
                var g = b[a[f]];
                if (g !== c) return d === !1 ? a[f] : e(g, "function") ? g.bind(d || b) : g
            }
            return !1
        }

        function i(a, b, c) {
            var d = a.charAt(0).toUpperCase() + a.slice(1),
                f = (a + " " + u.join(d + " ") + d).split(" ");
            return e(b, "string") || e(b, "undefined") ? g(f, b) : (f = (a + " " + v.join(d + " ") + d).split(" "), h(f, b, c))
        }
        var j, k, l, m = "2.6.2",
            n = {}, o = b.documentElement,
            p = "modernizr",
            q = b.createElement(p),
            r = q.style,
            s = ({}.toString, " -webkit- -moz- -o- -ms- ".split(" ")),
            t = "Webkit Moz O ms",
            u = t.split(" "),
            v = t.toLowerCase().split(" "),
            w = {}, x = [],
            y = x.slice,
            z = function (a, c, d, e) {
                var f, g, h, i, j = b.createElement("div"),
                    k = b.body,
                    l = k || b.createElement("body");
                if (parseInt(d, 10))
                    for (; d--;) h = b.createElement("div"), h.id = e ? e[d] : p + (d + 1), j.appendChild(h);
                return f = ["&#173;", '<style id="s', p, '">', a, "</style>"].join(""), j.id = p, (k ? j : l).innerHTML += f, l.appendChild(j), k || (l.style.background = "", l.style.overflow = "hidden", i = o.style.overflow, o.style.overflow = "hidden", o.appendChild(l)), g = c(j, a), k ? j.parentNode.removeChild(j) : (l.parentNode.removeChild(l), o.style.overflow = i), !!g
            }, A = {}.hasOwnProperty;
        l = e(A, "undefined") || e(A.call, "undefined") ? function (a, b) {
            return b in a && e(a.constructor.prototype[b], "undefined")
        } : function (a, b) {
            return A.call(a, b)
        }, Function.prototype.bind || (Function.prototype.bind = function (a) {
            var b = this;
            if ("function" != typeof b) throw new TypeError;
            var c = y.call(arguments, 1),
                d = function () {
                    if (this instanceof d) {
                        var e = function () { };
                        e.prototype = b.prototype;
                        var f = new e,
                            g = b.apply(f, c.concat(y.call(arguments)));
                        return Object(g) === g ? g : f
                    }
                    return b.apply(a, c.concat(y.call(arguments)))
                };
            return d
        }), w.csstransforms3d = function () {
            var a = !!i("perspective");
            return a
        };
        for (var B in w) l(w, B) && (k = B.toLowerCase(), n[k] = w[B](), x.push((n[k] ? "" : "no-") + k));
        return n.addTest = function (a, b) {
            if ("object" == typeof a)
                for (var d in a) l(a, d) && n.addTest(d, a[d]);
            else {
                if (a = a.toLowerCase(), n[a] !== c) return n;
                b = "function" == typeof b ? b() : b, "undefined" != typeof enableClasses && enableClasses && (o.className += " " + (b ? "" : "no-") + a), n[a] = b
            }
            return n
        }, d(""), q = j = null, n._version = m, n._prefixes = s, n._domPrefixes = v, n._cssomPrefixes = u, n.testProp = function (a) {
            return g([a])
        }, n.testAllProps = i, n.testStyles = z, n.prefixed = function (a, b, c) {
            return b ? i(a, b, c) : i(a, "pfx")
        }, n
    }(a, b),
        nc = {
            ok: !1,
            is: function () {
                return !1
            },
            request: function () { },
            cancel: function () { },
            event: "",
            prefix: ""
        }, oc = "webkit moz o ms khtml".split(" ");
    if ("undefined" != typeof b.cancelFullScreen) nc.ok = !0;
    else
        for (var pc = 0, qc = oc.length; qc > pc; pc++)
            if (nc.prefix = oc[pc], "undefined" != typeof b[nc.prefix + "CancelFullScreen"]) {
                nc.ok = !0;
                break
            } nc.ok && (nc.event = nc.prefix + "fullscreenchange", nc.is = function () {
                switch (this.prefix) {
                    case "":
                        return b.fullScreen;
                    case "webkit":
                        return b.webkitIsFullScreen;
                    default:
                        return b[this.prefix + "FullScreen"]
                }
            }, nc.request = function (a) {
                return "" === this.prefix ? a.requestFullScreen() : a[this.prefix + "RequestFullScreen"]()
            }, nc.cancel = function () {
                return "" === this.prefix ? b.cancelFullScreen() : b[this.prefix + "CancelFullScreen"]()
            });
    var rc, sc = {
        lines: 12,
        length: 5,
        width: 2,
        radius: 7,
        corners: 1,
        rotate: 15,
        color: "rgba(128, 128, 128, .75)",
        hwaccel: !0
    }, tc = {
        top: "auto",
        left: "auto",
        className: ""
    };
    ! function (a, b) {
        rc = b()
    }(this, function () {
        function a(a, c) {
            var d, e = b.createElement(a || "div");
            for (d in c) e[d] = c[d];
            return e
        }

        function c(a) {
            for (var b = 1, c = arguments.length; c > b; b++) a.appendChild(arguments[b]);
            return a
        }

        function d(a, b, c, d) {
            var e = ["opacity", b, ~~(100 * a), c, d].join("-"),
                f = .01 + c / d * 100,
                g = Math.max(1 - (1 - a) / b * (100 - f), a),
                h = m.substring(0, m.indexOf("Animation")).toLowerCase(),
                i = h && "-" + h + "-" || "";
            return o[e] || (p.insertRule("@" + i + "keyframes " + e + "{0%{opacity:" + g + "}" + f + "%{opacity:" + a + "}" + (f + .01) + "%{opacity:1}" + (f + b) % 100 + "%{opacity:" + a + "}100%{opacity:" + g + "}}", p.cssRules.length), o[e] = 1), e
        }

        function f(a, b) {
            var c, d, f = a.style;
            for (b = b.charAt(0).toUpperCase() + b.slice(1), d = 0; d < n.length; d++)
                if (c = n[d] + b, f[c] !== e) return c;
            return f[b] !== e ? b : void 0
        }

        function g(a, b) {
            for (var c in b) a.style[f(a, c) || c] = b[c];
            return a
        }

        function h(a) {
            for (var b = 1; b < arguments.length; b++) {
                var c = arguments[b];
                for (var d in c) a[d] === e && (a[d] = c[d])
            }
            return a
        }

        function i(a) {
            for (var b = {
                x: a.offsetLeft,
                y: a.offsetTop
            }; a = a.offsetParent;) b.x += a.offsetLeft, b.y += a.offsetTop;
            return b
        }

        function j(a, b) {
            return "string" == typeof a ? a : a[b % a.length]
        }

        function k(a) {
            return "undefined" == typeof this ? new k(a) : void (this.opts = h(a || {}, k.defaults, q))
        }

        function l() {
            function b(b, c) {
                return a("<" + b + ' xmlns="urn:schemas-microsoft.com:vml" class="spin-vml">', c)
            }
            p.addRule(".spin-vml", "behavior:url(#default#VML)"), k.prototype.lines = function (a, d) {
                function e() {
                    return g(b("group", {
                        coordsize: k + " " + k,
                        coordorigin: -i + " " + -i
                    }), {
                        width: k,
                        height: k
                    })
                }

                function f(a, f, h) {
                    c(m, c(g(e(), {
                        rotation: 360 / d.lines * a + "deg",
                        left: ~~f
                    }), c(g(b("roundrect", {
                        arcsize: d.corners
                    }), {
                        width: i,
                        height: d.width,
                        left: d.radius,
                        top: -d.width >> 1,
                        filter: h
                    }), b("fill", {
                        color: j(d.color, a),
                        opacity: d.opacity
                    }), b("stroke", {
                        opacity: 0
                    }))))
                }
                var h, i = d.length + d.width,
                    k = 2 * i,
                    l = 2 * -(d.width + d.length) + "px",
                    m = g(e(), {
                        position: "absolute",
                        top: l,
                        left: l
                    });
                if (d.shadow)
                    for (h = 1; h <= d.lines; h++) f(h, -2, "progid:DXImageTransform.Microsoft.Blur(pixelradius=2,makeshadow=1,shadowopacity=.3)");
                for (h = 1; h <= d.lines; h++) f(h);
                return c(a, m)
            }, k.prototype.opacity = function (a, b, c, d) {
                var e = a.firstChild;
                d = d.shadow && d.lines || 0, e && b + d < e.childNodes.length && (e = e.childNodes[b + d], e = e && e.firstChild, e = e && e.firstChild, e && (e.opacity = c))
            }
        }
        var m, n = ["webkit", "Moz", "ms", "O"],
            o = {}, p = function () {
                var d = a("style", {
                    type: "text/css"
                });
                return c(b.getElementsByTagName("head")[0], d), d.sheet || d.styleSheet
            }(),
            q = {
                lines: 12,
                length: 7,
                width: 5,
                radius: 10,
                rotate: 0,
                corners: 1,
                color: "#000",
                direction: 1,
                speed: 1,
                trail: 100,
                opacity: .25,
                fps: 20,
                zIndex: 2e9,
                className: "spinner",
                top: "auto",
                left: "auto",
                position: "relative"
            };
        k.defaults = {}, h(k.prototype, {
            spin: function (b) {
                this.stop();
                var c, d, e = this,
                    f = e.opts,
                    h = e.el = g(a(0, {
                        className: f.className
                    }), {
                        position: f.position,
                        width: 0,
                        zIndex: f.zIndex
                    }),
                    j = f.radius + f.length + f.width;
                if (b && (b.insertBefore(h, b.firstChild || null), d = i(b), c = i(h), g(h, {
                    left: ("auto" == f.left ? d.x - c.x + (b.offsetWidth >> 1) : parseInt(f.left, 10) + j) + "px",
                    top: ("auto" == f.top ? d.y - c.y + (b.offsetHeight >> 1) : parseInt(f.top, 10) + j) + "px"
                })), h.setAttribute("role", "progressbar"), e.lines(h, e.opts), !m) {
                    var k, l = 0,
                        n = (f.lines - 1) * (1 - f.direction) / 2,
                        o = f.fps,
                        p = o / f.speed,
                        q = (1 - f.opacity) / (p * f.trail / 100),
                        r = p / f.lines;
                    ! function s() {
                        l++;
                        for (var a = 0; a < f.lines; a++) k = Math.max(1 - (l + (f.lines - a) * r) % p * q, f.opacity), e.opacity(h, a * f.direction + n, k, f);
                        e.timeout = e.el && setTimeout(s, ~~(1e3 / o))
                    }()
                }
                return e
            },
            stop: function () {
                var a = this.el;
                return a && (clearTimeout(this.timeout), a.parentNode && a.parentNode.removeChild(a), this.el = e), this
            },
            lines: function (b, e) {
                function f(b, c) {
                    return g(a(), {
                        position: "absolute",
                        width: e.length + e.width + "px",
                        height: e.width + "px",
                        background: b,
                        boxShadow: c,
                        transformOrigin: "left",
                        transform: "rotate(" + ~~(360 / e.lines * i + e.rotate) + "deg) translate(" + e.radius + "px,0)",
                        borderRadius: (e.corners * e.width >> 1) + "px"
                    })
                }
                for (var h, i = 0, k = (e.lines - 1) * (1 - e.direction) / 2; i < e.lines; i++) h = g(a(), {
                    position: "absolute",
                    top: 1 + ~(e.width / 2) + "px",
                    transform: e.hwaccel ? "translate3d(0,0,0)" : "",
                    opacity: e.opacity,
                    animation: m && d(e.opacity, e.trail, k + i * e.direction, e.lines) + " " + 1 / e.speed + "s linear infinite"
                }), e.shadow && c(h, g(f("#000", "0 0 4px #000"), {
                    top: "2px"
                })), c(b, c(h, f(j(e.color, i), "0 0 1px rgba(0,0,0,.1)")));
                return b
            },
            opacity: function (a, b, c) {
                b < a.childNodes.length && (a.childNodes[b].style.opacity = c)
            }
        });
        var r = g(a("group"), {
            behavior: "url(#default#VML)"
        });
        return !f(r, "transform") && r.adj ? l() : m = f(r, "animation"), k
    });
    var uc, vc, wc, xc, yc, zc, Ac = d(a),
        Bc = d(b),
        Cc = "quirks" === c.hash.replace("#", ""),
        Dc = mc.csstransforms3d,
        Ec = Dc && !Cc,
        Fc = Dc || "CSS1Compat" === b.compatMode,
        Gc = nc.ok,
        Hc = navigator.userAgent.match(/Android|webOS|iPhone|iPad|iPod|BlackBerry|Windows Phone/i),
        Ic = !Ec || Hc,
        Jc = "addEventListener",
        Kc = navigator.msPointerEnabled,
        Lc = "onwheel" in b.createElement("div") ? "wheel" : b.onmousewheel !== e ? "mousewheel" : "DOMMouseScroll",
        Mc = 250,
        Nc = 300,
        Oc = 1400,
        Pc = 5e3,
        Qc = 2,
        Rc = 64,
        Sc = 500,
        Tc = 333,
        Uc = "$stageFrame",
        Vc = "$navDotFrame",
        Wc = "$navThumbFrame",
        Xc = f([.1, 0, .25, 1]),
        Yc = 99999,
        Zc = {
            width: null,
            minwidth: null,
            maxwidth: "100%",
            height: null,
            minheight: null,
            maxheight: null,
            ratio: null,
            margin: Qc,
            glimpse: 0,
            nav: "dots",
            navposition: "bottom",
            navwidth: null,
            thumbwidth: Rc,
            thumbheight: Rc,
            thumbmargin: Qc,
            thumbborderwidth: Qc,
            allowfullscreen: !1,
            fit: "contain",
            transition: "slide",
            clicktransition: null,
            transitionduration: Nc,
            captions: !0,
            hash: !1,
            startindex: 0,
            loop: !1,
            autoplay: !1,
            stopautoplayontouch: !0,
            keyboard: !1,
            arrows: !0,
            click: !0,
            swipe: !0,
            trackpad: !0,
            shuffle: !1,
            direction: "ltr",
            shadows: !0,
            spinner: null
        }, $c = {
            left: !0,
            right: !0,
            down: !1,
            up: !1,
            space: !1,
            home: !1,
            end: !1
        };
    jQuery.Fotorama = function (a, e) {
        function f() {
            d.each(pd, function (a, b) {
                if (!b.i) {
                    b.i = ce++;
                    var c = z(b.video, !0);
                    if (c) {
                        var d = {};
                        b.video = c, b.img || b.thumb ? b.thumbsReady = !0 : d = A(b, pd, $d), B(pd, {
                            img: d.img,
                            thumb: d.thumb
                        }, b.i, $d)
                    }
                }
            })
        }

        function g(a) {
            return Pd[a] || $d.fullScreen
        }

        function i(a) {
            var b = "keydown." + bb,
                c = "keydown." + bb + _d,
                d = "resize." + bb + _d;
            a ? (Bc.on(c, function (a) {
                var b, c;
                td && 27 === a.keyCode ? (b = !0, fd(td, !0, !0)) : ($d.fullScreen || e.keyboard && !$d.index) && (27 === a.keyCode ? (b = !0, $d.cancelFullScreen()) : a.shiftKey && 32 === a.keyCode && g("space") || 37 === a.keyCode && g("left") || 38 === a.keyCode && g("up") ? c = "<" : 32 === a.keyCode && g("space") || 39 === a.keyCode && g("right") || 40 === a.keyCode && g("down") ? c = ">" : 36 === a.keyCode && g("home") ? c = "<<" : 35 === a.keyCode && g("end") && (c = ">>")), (b || c) && S(a), c && $d.show({
                    index: c,
                    slow: a.altKey,
                    user: !0
                })
            }), $d.index || Bc.off(b).on(b, "textarea, input, select", function (a) {
                !vc.hasClass(cb) && a.stopPropagation()
            }), Ac.on(d, $d.resize)) : (Bc.off(c), Ac.off(d))
        }

        function j(b) {
            b !== j.f && (b ? (a.html("").addClass(bb + " " + ae).append(ge).before(ee).before(fe), _($d)) : (ge.detach(), ee.detach(), fe.detach(), a.html(de.urtext).removeClass(ae), ab($d)), i(b), j.f = b)
        }

        function n() {
            pd = $d.data = pd || O(e.data) || C(a), qd = $d.size = pd.length, !od.ok && e.shuffle && N(pd), f(), ze = y(ze), qd && j(!0)
        }

        function s() {
            var a = 2 > qd || td;
            Ce.noMove = a || Id, Ce.noSwipe = a || !e.swipe, !Md && ie.toggleClass(ub, !Ce.noMove && !Ce.noSwipe), Kc && ge.toggleClass(lb, !Ce.noSwipe)
        }

        function t(a) {
            a === !0 && (a = ""), e.autoplay = Math.max(+a || Pc, 1.5 * Ld)
        }

        function w() {
            function a(a, c) {
                b[a ? "add" : "remove"].push(c)
            }
            $d.options = e = Q(e), Id = "crossfade" === e.transition || "dissolve" === e.transition, Cd = e.loop && (qd > 2 || Id) && (!Md || "slide" !== Md), Ld = +e.transitionduration || Nc, Od = "rtl" === e.direction, Pd = d.extend({}, e.keyboard && $c, e.keyboard);
            var b = {
                add: [],
                remove: []
            };
            qd > 1 ? (Dd = e.nav, Fd = "top" === e.navposition, b.remove.push(Qb), me.toggle(!!e.arrows)) : (Dd = !1, me.hide()), ec(), sd = new rc(d.extend(sc, e.spinner, tc, {
                direction: Od ? -1 : 1
            })), yc(), zc(), e.autoplay && t(e.autoplay), Jd = m(e.thumbwidth) || Rc, Kd = m(e.thumbheight) || Rc, De.ok = Fe.ok = e.trackpad && !Ic, s(), Xc(e, [Be]), Ed = "thumbs" === Dd, Ed ? (lc(qd, "navThumb"), rd = re, Zd = Wc, I(ee, d.Fotorama.jst.style({
                w: Jd,
                h: Kd,
                b: e.thumbborderwidth,
                m: e.thumbmargin,
                s: _d,
                q: !Fc
            })), oe.addClass(Eb).removeClass(Db)) : "dots" === Dd ? (lc(qd, "navDot"), rd = qe, Zd = Vc, oe.addClass(Db).removeClass(Eb)) : (Dd = !1, oe.removeClass(Eb + " " + Db)), Dd && (Fd ? ne.insertBefore(he) : ne.insertAfter(he), qc.nav = !1, qc(rd, pe, "nav")), Gd = e.allowfullscreen, Gd ? (te.appendTo(he), Hd = Gc && "native" === Gd) : (te.detach(), Hd = !1), a(Id, hb), a(!Id, ib), a(!e.captions, ob), a(Od, mb), a("always" !== e.arrows, pb), Nd = e.shadows && !Ic, a(!Nd, kb), ge.addClass(b.add.join(" ")).removeClass(b.remove.join(" ")), Ae = d.extend({}, e)
        }

        function x(a) {
            return 0 > a ? (qd + a % qd) % qd : a >= qd ? a % qd : a
        }

        function y(a) {
            return h(a, 0, qd - 1)
        }

        function D(a) {
            return Cd ? x(a) : y(a)
        }

        function W(a) {
            return a > 0 || Cd ? a - 1 : !1
        }

        function X(a) {
            return qd - 1 > a || Cd ? a + 1 : !1
        }

        function $() {
            Ce.min = Cd ? -1 / 0 : -q(qd - 1, Be.w, e.margin, wd), Ce.max = Cd ? 1 / 0 : -q(0, Be.w, e.margin, wd), Ce.snap = Be.w + e.margin
        }

        function Ib() {
            Ee.min = Math.min(0, Be.nw - pe.width()), Ee.max = 0, pe.toggleClass(ub, !(Ee.noMove = Ee.min === Ee.max))
        }

        function Jb(a, b, c) {
            if ("number" == typeof a) {
                a = new Array(a);
                var e = !0
            }
            return d.each(a, function (a, d) {
                if (e && (d = a), "number" == typeof d) {
                    var f = pd[x(d)];
                    if (f) {
                        var g = "$" + b + "Frame",
                            h = f[g];
                        c.call(this, a, d, f, h, g, h && h.data())
                    }
                }
            })
        }

        function Kb(a, b, c, d) {
            (!Qd || "*" === Qd && d === Bd) && (a = p(e.width) || p(a) || Sc, b = p(e.height) || p(b) || Tc, $d.resize({
                width: a,
                ratio: e.ratio || c || a / b
            }, 0, d === Bd ? !0 : "*"))
        }

        function Lb(a, b, c, f, g) {
            Jb(a, b, function (a, h, i, j, k, l) {
                function m(a) {
                    var b = x(h);
                    Zc(a, {
                        index: b,
                        src: v,
                        frame: pd[b]
                    })
                }

                function n() {
                    s.remove(), d.Fotorama.cache[v] = "error", i.html && "stage" === b || !w || w === v ? (!v || i.html || q ? "stage" === b && (j.trigger("f:load").removeClass(Vb + " " + Ub).addClass(Wb), m("load"), Kb()) : (j.trigger("f:error").removeClass(Vb).addClass(Ub), m("error")), l.state = "error", !(qd > 1 && pd[h] === i) || i.html || i.deleted || i.video || q || (i.deleted = !0, $d.splice(h, 1))) : (i[u] = v = w, Lb([h], b, c, f, !0))
                }

                function o() {
                    d.Fotorama.measures[v] = t.measures = d.Fotorama.measures[v] || {
                        width: r.width,
                        height: r.height,
                        ratio: r.width / r.height
                    }, Kb(t.measures.width, t.measures.height, t.measures.ratio, h), s.off("load error").addClass($b + (q ? " " + _b : "")).prependTo(j), H(s, c || Be, f || i.fit || e.fit), d.Fotorama.cache[v] = l.state = "loaded", setTimeout(function () {
                        j.trigger("f:load").removeClass(Vb + " " + Ub).addClass(Wb + " " + (q ? Xb : Yb)), "stage" === b && m("load")
                    }, 5)
                }

                function p() {
                    var a = 10;
                    F(function () {
                        return !Xd || !a-- && !Ic
                    }, function () {
                        o()
                    })
                }
                if (j) {
                    var q = $d.fullScreen && i.full && i.full !== i.img && !l.$full && "stage" === b;
                    if (!l.$img || g || q) {
                        var r = new Image,
                            s = d(r),
                            t = s.data();
                        l[q ? "$full" : "$img"] = s;
                        var u = "stage" === b ? q ? "full" : "img" : "thumb",
                            v = i[u],
                            w = q ? null : i["stage" === b ? "thumb" : "img"];
                        if ("navThumb" === b && (j = l.$wrap), !v) return void n();
                        d.Fotorama.cache[v] ? ! function y() {
                            "error" === d.Fotorama.cache[v] ? n() : "loaded" === d.Fotorama.cache[v] ? setTimeout(p, 0) : setTimeout(y, 100)
                        }() : (d.Fotorama.cache[v] = "*", s.on("load", p).on("error", n)), l.state = "", r.src = v
                    }
                }
            })
        }

        function Zb(a) {
            ye.append(sd.spin().el).appendTo(a)
        }

        function ec() {
            ye.detach(), sd && sd.stop()
        }

        function kc() {
            var a = $d.activeFrame[Uc];
            a && !a.data().state && (Zb(a), a.on("f:load f:error", function () {
                a.off("f:load f:error"), ec()
            }))
        }

        function lc(a, b) {
            Jb(a, b, function (a, c, f, g, h, i) {
                g || (g = f[h] = ge[h].clone(), i = g.data(), i.data = f, "stage" === b ? (f.html && d('<div class="' + dc + '"></div>').append(f._html ? d(f.html).removeAttr("id").html(f._html) : f.html).appendTo(g), e.captions && f.caption && d(M(hc, M(ic, f.caption))).appendTo(g), f.video && g.addClass(sb).append(ve.clone()), je = je.add(g)) : "navDot" === b ? qe = qe.add(g) : "navThumb" === b && (i.$wrap = g.children(":first"), re = re.add(g), f.video && g.append(ve.clone())))
            })
        }

        function mc(a, b, c) {
            return a && a.length && H(a, b, c)
        }

        function oc(a) {
            Jb(a, "stage", function (a, b, c, f, g, h) {
                if (f) {
                    He[Uc][x(b)] = f.css(d.extend({
                        left: Id ? 0 : q(b, Be.w, e.margin, wd)
                    }, Id && l(0))), E(f[0]) && (f.appendTo(ie), fd(c.$video));
                    var i = c.fit || e.fit;
                    mc(h.$img, Be, i), mc(h.$full, Be, i)
                }
            })
        }

        function pc(a, b) {
            if ("thumbs" === Dd && !isNaN(a)) {
                var c = -a,
                    e = -a + Be.nw;
                re.each(function () {
                    var a = d(this),
                        f = a.data(),
                        g = f.eq,
                        h = {
                            h: Kd
                        }, i = "cover";
                    h.w = f.w, f.l + f.w < c || f.l > e || mc(f.$img, h, i) || b && Lb([g], "navThumb", h, i)
                })
            }
        }

        function qc(a, b, c) {
            if (!qc[c]) {
                var f = "nav" === c && Ed,
                    g = 0;
                b.append(a.filter(function () {
                    for (var a, b = d(this), c = b.data(), e = 0, f = pd.length; f > e; e++)
                        if (c.data === pd[e]) {
                            a = !0, c.eq = e;
                            break
                        }
                    return a || b.remove() && !1
                }).sort(function (a, b) {
                    return d(a).data().eq - d(b).data().eq
                }).each(function () {
                    if (f) {
                        var a = d(this),
                            b = a.data(),
                            c = Math.round(Kd * b.data.thumbratio) || Jd;
                        b.l = g, b.w = c, a.css({
                            width: c
                        }), g += c + e.thumbmargin
                    }
                })), qc[c] = !0
            }
        }

        function wc(a) {
            return a - Ie > Be.w / 3
        }

        function xc(a) {
            return !(Cd || ze + a && ze - qd + a || td)
        }

        function yc() {
            ke.toggleClass(xb, xc(0)), le.toggleClass(xb, xc(1))
        }

        function zc() {
            De.ok && (De.prevent = {
                "<": xc(0),
                ">": xc(1)
            })
        }

        function Cc(a) {
            var b, c, d = a.data();
            return Ed ? (b = d.l, c = d.w) : (b = a.position().left, c = a.width()), {
                c: b + c / 2,
                min: -b + 10 * e.thumbmargin,
                max: -b + Be.w - c - 10 * e.thumbmargin
            }
        }

        function Dc(a) {
            var b = $d.activeFrame[Zd].data();
            U(se, {
                time: .9 * a,
                pos: b.l,
                width: b.w - 2 * e.thumbborderwidth
            })
        }

        function Hc(a) {
            var b = pd[a.guessIndex][Zd];
            if (b) {
                var c = Ee.min !== Ee.max,
                    d = c && Cc($d.activeFrame[Zd]),
                    e = c && (a.keep && Hc.l ? Hc.l : h((a.coo || Be.nw / 2) - Cc(b).c, d.min, d.max)),
                    f = c && h(e, Ee.min, Ee.max),
                    g = .9 * a.time;
                U(pe, {
                    time: g,
                    pos: f || 0,
                    onEnd: function () {
                        pc(f, !0)
                    }
                }), ed(oe, J(f, Ee.min, Ee.max)), Hc.l = e
            }
        }

        function Jc() {
            Lc(Zd), Ge[Zd].push($d.activeFrame[Zd].addClass(Pb))
        }

        function Lc(a) {
            for (var b = Ge[a]; b.length;) b.shift().removeClass(Pb)
        }

        function Oc(a) {
            var b = He[a];
            d.each(vd, function (a, c) {
                delete b[x(c)]
            }), d.each(b, function (a, c) {
                delete b[a], c.detach()
            })
        }

        function Qc(a) {
            wd = xd = ze;
            var b = $d.activeFrame,
                c = b[Uc];
            c && (Lc(Uc), Ge[Uc].push(c.addClass(Pb)), a || $d.show.onEnd(!0), u(ie, 0, !0), Oc(Uc), oc(vd), $(), Ib())
        }

        function Xc(a, b) {
            a && d.each(b, function (b, c) {
                c && d.extend(c, {
                    width: a.width || c.width,
                    height: a.height,
                    minwidth: a.minwidth,
                    maxwidth: a.maxwidth,
                    minheight: a.minheight,
                    maxheight: a.maxheight,
                    ratio: R(a.ratio)
                })
            })
        }

        function Zc(b, c) {
            a.trigger(bb + ":" + b, [$d, c])
        }

        function _c() {
            clearTimeout(ad.t), Xd = 1, e.stopautoplayontouch ? $d.stopAutoplay() : Ud = !0
        }

        function ad() {
            e.stopautoplayontouch || (bd(), cd()), ad.t = setTimeout(function () {
                Xd = 0
            }, Nc + Mc)
        }

        function bd() {
            Ud = !(!td && !Vd)
        }

        function cd() {
            if (clearTimeout(cd.t), !e.autoplay || Ud) return void ($d.autoplay && ($d.autoplay = !1, Zc("stopautoplay")));
            $d.autoplay || ($d.autoplay = !0, Zc("startautoplay"));
            var a = ze,
                b = $d.activeFrame[Uc].data();
            F(function () {
                return b.state || a !== ze
            }, function () {
                cd.t = setTimeout(function () {
                    Ud || a !== ze || $d.show(Cd ? T(!Od) : x(ze + (Od ? -1 : 1)))
                }, e.autoplay)
            })
        }

        function dd() {
            $d.fullScreen && ($d.fullScreen = !1, Gc && nc.cancel(be), vc.removeClass(cb), uc.removeClass(cb), a.removeClass(Sb).insertAfter(fe), Be = d.extend({}, Wd), fd(td, !0, !0), kd("x", !1), $d.resize(), Lb(vd, "stage"), P(Sd, Rd), Zc("fullscreenexit"))
        }

        function ed(a, b) {
            Nd && (a.removeClass(Nb + " " + Ob), b && !td && a.addClass(b.replace(/^|\s/g, " " + Mb + "--")))
        }

        function fd(a, b, c) {
            b && (ge.removeClass(gb), td = !1, s()), a && a !== td && (a.remove(), Zc("unloadvideo")), c && (bd(), cd())
        }

        function gd(a) {
            ge.toggleClass(jb, a)
        }

        function hd(a) {
            if (!Ce.flow) {
                var b = a ? a.pageX : hd.x,
                    c = b && !xc(wc(b)) && e.click;
                hd.p === c || !Id && e.swipe || !he.toggleClass(vb, c) || (hd.p = c, hd.x = b)
            }
        }

        function id(a) {
            clearTimeout(id.t), e.clicktransition && e.clicktransition !== e.transition ? (Md = e.transition, $d.setOptions({
                transition: e.clicktransition
            }), id.t = setTimeout(function () {
                $d.show(a)
            }, 10)) : $d.show(a)
        }

        function jd(a, b) {
            var c = a.target,
                f = d(c);
            f.hasClass(fc) ? $d.playVideo() : c === ue ? $d[($d.fullScreen ? "cancel" : "request") + "FullScreen"]() : td ? c === xe && fd(td, !0, !0) : b ? gd() : e.click && id({
                index: a.shiftKey || T(wc(a._x)),
                slow: a.altKey,
                user: !0
            })
        }

        function kd(a, b) {
            Ce[a] = Ee[a] = b
        }

        function ld(a, b) {
            var c = d(this).data().eq;
            id({
                index: c,
                slow: a.altKey,
                user: !0,
                coo: a._x - oe.offset().left,
                time: b
            })
        }

        function md() {
            if (n(), w(), !md.i) {
                md.i = !0;
                var a = e.startindex;
                (a || e.hash && c.hash) && (Bd = K(a || c.hash.replace(/^#/, ""), pd, 0 === $d.index || a, a)), ze = wd = xd = yd = Bd = D(Bd) || 0
            }
            if (qd) {
                if (nd()) return;
                td && fd(td, !0), vd = [], Oc(Uc), $d.show({
                    index: ze,
                    time: 0,
                    reset: md.ok
                }), $d.resize()
            } else $d.destroy();
            md.ok = !0
        }

        function nd() {
            return !nd.f === Od ? (nd.f = Od, ze = qd - 1 - ze, $d.reverse(), !0) : void 0
        }

        function od() {
            od.ok || (od.ok = !0, Zc("ready"))
        }
        uc = uc || d("html"), vc = vc || d("body");
        var pd, qd, rd, sd, td, ud, vd, wd, xd, yd, zd, Ad, Bd, Cd, Dd, Ed, Fd, Gd, Hd, Id, Jd, Kd, Ld, Md, Nd, Od, Pd, Qd, Rd, Sd, Td, Ud, Vd, Wd, Xd, Yd, Zd, $d = this,
            _d = d.now(),
            ae = bb + _d,
            be = a[0],
            ce = 1,
            de = a.data(),
            ee = d("<style></style>"),
            fe = d(M(Rb)),
            ge = d(M(db)),
            he = d(M(qb)).appendTo(ge),
            ie = (he[0], d(M(tb)).appendTo(he)),
            je = d(),
            ke = d(M(wb + " " + yb)),
            le = d(M(wb + " " + zb)),
            me = ke.add(le).appendTo(he),
            ne = d(M(Bb)),
            oe = d(M(Ab)).appendTo(ne),
            pe = d(M(Cb)).appendTo(oe),
            qe = d(),
            re = d(),
            se = (ie.data(), pe.data(), d(M(cc)).appendTo(pe)),
            te = d(M(Tb)),
            ue = te[0],
            ve = d(M(fc)),
            we = d(M(gc)).appendTo(he),
            xe = we[0],
            ye = d(M(jc)),
            ze = !1,
            Ae = {}, Be = {}, Ce = {}, De = {}, Ee = {}, Fe = {}, Ge = {}, He = {}, Ie = 0,
            Je = [];
        ge[Uc] = d(M(rb)), ge[Wc] = d(M(Fb + " " + Hb, M(bc))), ge[Vc] = d(M(Fb + " " + Gb, M(ac))), Ge[Uc] = [], Ge[Wc] = [], Ge[Vc] = [], He[Uc] = {}, ge.addClass(Ec ? fb : eb), de.fotorama = this, $d.startAutoplay = function (a) {
            return $d.autoplay ? this : (Ud = Vd = !1, t(a || e.autoplay), cd(), this)
        }, $d.stopAutoplay = function () {
            return $d.autoplay && (Ud = Vd = !0, cd()), this
        }, $d.show = function (a) {
            var b;
            "object" != typeof a ? (b = a, a = {}) : b = a.index, b = ">" === b ? xd + 1 : "<" === b ? xd - 1 : "<<" === b ? 0 : ">>" === b ? qd - 1 : b, b = isNaN(b) ? K(b, pd, !0) : b, b = "undefined" == typeof b ? ze || 0 : b, $d.activeIndex = ze = D(b), zd = W(ze), Ad = X(ze), vd = [ze, zd, Ad], xd = Cd ? b : ze;
            var c = Math.abs(yd - xd),
                d = v(a.time, function () {
                    return Math.min(Ld * (1 + (c - 1) / 12), 2 * Ld)
                }),
                f = a.overPos;
            a.slow && (d *= 10), $d.activeFrame = ud = pd[ze], fd(td, ud.i !== pd[x(wd)].i), lc(vd, "stage"), oc(Ic ? [xd] : [xd, W(xd), X(xd)]), kd("go", !0), a.reset || Zc("show", {
                user: a.user,
                time: d
            }), Ud = !0;
            var g = $d.show.onEnd = function (b) {
                if (!g.ok) {
                    if (g.ok = !0, b || Qc(!0), !a.reset && (Zc("showend", {
                        user: a.user
                    }), !b && Md && Md !== e.transition)) return $d.setOptions({
                        transition: Md
                    }), void (Md = !1);
                    kc(), Lb(vd, "stage"), kd("go", !1), zc(), hd(), bd(), cd()
                }
            };
            if (Id) {
                var i = ud[Uc],
                    j = ze !== yd ? pd[yd][Uc] : null;
                V(i, j, je, {
                    time: d,
                    method: e.transition,
                    onEnd: g
                }, Je)
            } else U(ie, {
                pos: -q(xd, Be.w, e.margin, wd),
                overPos: f,
                time: d,
                onEnd: g,
                _001: !0
            }); if (yc(), Dd) {
                Jc();
                var k = y(ze + h(xd - yd, -1, 1));
                Hc({
                    time: d,
                    coo: k !== ze && a.coo,
                    guessIndex: "undefined" != typeof a.coo ? k : ze,
                    keep: a.reset
                }), Ed && Dc(d)
            }
            return Td = "undefined" != typeof yd && yd !== ze, yd = ze, e.hash && Td && !$d.eq && G(ud.id || ze + 1), this
        }, $d.requestFullScreen = function () {
            return Gd && !$d.fullScreen && (Rd = Ac.scrollTop(), Sd = Ac.scrollLeft(), P(0, 0), kd("x", !0), Wd = d.extend({}, Be), a.addClass(Sb).appendTo(vc.addClass(cb)), uc.addClass(cb), fd(td, !0, !0), $d.fullScreen = !0, Hd && nc.request(be), $d.resize(), Lb(vd, "stage"), kc(), Zc("fullscreenenter")), this
        }, $d.cancelFullScreen = function () {
            return Hd && nc.is() ? nc.cancel(b) : dd(), this
        }, b.addEventListener && b.addEventListener(nc.event, function () {
            !pd || nc.is() || td || dd()
        }, !1), $d.resize = function (a) {
            if (!pd) return this;
            Xc($d.fullScreen ? {
                width: "100%",
                maxwidth: null,
                minwidth: null,
                height: "100%",
                maxheight: null,
                minheight: null
            } : Q(a), [Be, $d.fullScreen || e]);
            var b = arguments[1] || 0,
                c = arguments[2],
                d = Be.width,
                f = Be.height,
                g = Be.ratio,
                i = Ac.height() - (Dd ? oe.height() : 0);
            return p(d) && (ge.addClass(nb).css({
                width: d,
                minWidth: Be.minwidth || 0,
                maxWidth: Be.maxwidth || Yc
            }), d = Be.W = Be.w = ge.width(), Be.nw = Dd && o(e.navwidth, d) || d, e.glimpse && (Be.w -= Math.round(2 * (o(e.glimpse, d) || 0))), ie.css({
                width: Be.w,
                marginLeft: (Be.W - Be.w) / 2
            }), f = o(f, i), f = f || g && d / g, f && (d = Math.round(d), f = Be.h = Math.round(h(f, o(Be.minheight, i), o(Be.maxheight, i))), he.stop().animate({
                width: d,
                height: f
            }, b, function () {
                ge.removeClass(nb)
            }), Qc(), Dd && (oe.stop().animate({
                width: Be.nw
            }, b), Hc({
                guessIndex: ze,
                time: b,
                keep: !0
            }), Ed && qc.nav && Dc(b)), Qd = c || !0, od())), Ie = he.offset().left, this
        }, $d.setOptions = function (a) {
            return d.extend(e, a), md(), this
        }, $d.shuffle = function () {
            return pd && N(pd) && md(), this
        }, $d.destroy = function () {
            return $d.cancelFullScreen(), $d.stopAutoplay(), pd = $d.data = null, j(), vd = [], Oc(Uc), this
        }, $d.playVideo = function () {
            var a = $d.activeFrame,
                b = a.video,
                c = ze;
            return "object" == typeof b && a.videoReady && (Hd && $d.fullScreen && $d.cancelFullScreen(), F(function () {
                return !nc.is() || c !== ze
            }, function () {
                c === ze && (a.$video = a.$video || d(d.Fotorama.jst.video(b)), a.$video.appendTo(a[Uc]), ge.addClass(gb), td = a.$video, s(), Zc("loadvideo"))
            })), this
        }, $d.stopVideo = function () {
            return fd(td, !0, !0), this
        }, he.on("mousemove", hd), Ce = Y(ie, {
            onStart: _c,
            onMove: function (a, b) {
                ed(he, b.edge)
            },
            onTouchEnd: ad,
            onEnd: function (a) {
                ed(he);
                var b = (Kc && !Yd || a.touch) && e.arrows && "always" !== e.arrows;
                if (a.moved || b && a.pos !== a.newPos && !a.control) {
                    var c = r(a.newPos, Be.w, e.margin, wd);
                    $d.show({
                        index: c,
                        time: Id ? Ld : a.time,
                        overPos: a.overPos,
                        user: !0
                    })
                } else a.aborted || a.control || jd(a.startEvent, b)
            },
            _001: !0,
            timeLow: 1,
            timeHigh: 1,
            friction: 2,
            select: "." + Qb + ", ." + Qb + " *",
            $wrap: he
        }), Ee = Y(pe, {
            onStart: _c,
            onMove: function (a, b) {
                ed(oe, b.edge)
            },
            onTouchEnd: ad,
            onEnd: function (a) {
                function b() {
                    Hc.l = a.newPos, bd(), cd(), pc(a.newPos, !0)
                }
                if (a.moved) a.pos !== a.newPos ? (Ud = !0, U(pe, {
                    time: a.time,
                    pos: a.newPos,
                    overPos: a.overPos,
                    onEnd: b
                }), pc(a.newPos), Nd && ed(oe, J(a.newPos, Ee.min, Ee.max))) : b();
                else {
                    var c = a.$target.closest("." + Fb, pe)[0];
                    c && ld.call(c, a.startEvent)
                }
            },
            timeLow: .5,
            timeHigh: 2,
            friction: 5,
            $wrap: oe
        }), De = Z(he, {
            shift: !0,
            onEnd: function (a, b) {
                _c(), ad(), $d.show({
                    index: b,
                    slow: a.altKey
                })
            }
        }), Fe = Z(oe, {
            onEnd: function (a, b) {
                _c(), ad();
                var c = u(pe) + .25 * b;
                pe.css(k(h(c, Ee.min, Ee.max))), Nd && ed(oe, J(c, Ee.min, Ee.max)), Fe.prevent = {
                    "<": c >= Ee.max,
                    ">": c <= Ee.min
                }, clearTimeout(Fe.t), Fe.t = setTimeout(function () {
                    pc(c, !0)
                }, Mc), pc(c)
            }
        }), ge.hover(function () {
            setTimeout(function () {
                Xd || (Yd = !0, gd(!Yd))
            }, 0)
        }, function () {
            Yd && (Yd = !1, gd(!Yd))
        }), L(me, function (a) {
            S(a), id({
                index: me.index(this) ? ">" : "<",
                slow: a.altKey,
                user: !0
            })
        }, {
            onStart: function () {
                _c(), Ce.control = !0
            },
            onTouchEnd: ad
        }), d.each("load push pop shift unshift reverse sort splice".split(" "), function (a, b) {
            $d[b] = function () {
                return pd = pd || [], "load" !== b ? Array.prototype[b].apply(pd, arguments) : arguments[0] && "object" == typeof arguments[0] && arguments[0].length && (pd = O(arguments[0])), md(), $d
            }
        }), md()
    }, d.fn.fotorama = function (b) {
        return this.each(function () {
            var c = this,
                e = d(this),
                f = e.data(),
                g = f.fotorama;
            g ? g.setOptions(b) : F(function () {
                return !D(c)
            }, function () {
                f.urtext = e.html(), new d.Fotorama(e, d.extend({}, Zc, a.fotoramaDefaults, b, f))
            })
        })
    }, d.Fotorama.instances = [], d.Fotorama.cache = {}, d.Fotorama.measures = {}, d = d || {}, d.Fotorama = d.Fotorama || {}, d.Fotorama.jst = d.Fotorama.jst || {}, d.Fotorama.jst.style = function (a) {
        {
            var b, c = "";
            lc.escape
        }
        return c += ".fotorama" + (null == (b = a.s) ? "" : b) + " .fotorama__nav--thumbs .fotorama__nav__frame{\npadding:" + (null == (b = a.m) ? "" : b) + "px;\nheight:" + (null == (b = a.h) ? "" : b) + "px}\n.fotorama" + (null == (b = a.s) ? "" : b) + " .fotorama__thumb-border{\nheight:" + (null == (b = a.h - a.b * (a.q ? 0 : 2)) ? "" : b) + "px;\nborder-width:" + (null == (b = a.b) ? "" : b) + "px;\nmargin-top:" + (null == (b = a.m) ? "" : b) + "px}"
    }, d.Fotorama.jst.video = function (a) {
        function b() {
            c += d.call(arguments, "")
        }
        var c = "",
            d = (lc.escape, Array.prototype.join);
        return c += '<div class="fotorama__video"><iframe src="', b(("youtube" == a.type ? "http://youtube.com/embed/" + a.id + "?autoplay=1" : "vimeo" == a.type ? "http://player.vimeo.com/video/" + a.id + "?autoplay=1&badge=0" : a.id) + (a.s && "custom" != a.type ? "&" + a.s : "")), c += '" frameborder="0" allowfullscreen></iframe></div>'
    }, d(function () {
        d("." + bb + ':not([data-auto="false"])').fotorama()
    })
}(window, document, location, "undefined" != typeof jQuery && jQuery);