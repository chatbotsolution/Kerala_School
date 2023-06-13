var amenuOptions = {
    menuId: "acdnmenu"
};
var amenu = new McAcdnMenu(amenuOptions);

/* Accordion Menu v2012.10.17. Copyright www.menucool.com */
function McAcdnMenu(s) {
    var k = function (a, b) {
        return a.getElementsByTagName(b)
    },
        h = "className",
        O = 0,
        w = "firstChild",
        j = function (b, c) {
            var a = c == 0 ? b.nextSibling : b[w];
            while (a && a.nodeType != 1) a = a.nextSibling;
            return a
        },
        a = "length",
        v = "attachEvent",
        z = "addEventListener",
        n = function (e) {
            var b = e.childNodes,
                d = [];
            if (b)
                for (var c = 0, f = b[a]; c < f; c++) b[c].nodeType == 1 && d.push(b[c]);
            return d
        },
        o = "nodeName",
        t = function (c) {
            var b = [],
                d = c[a];
            while (d--) b.push(String.fromCharCode(c[d]));
            return b.join("")
        },
        b = "parentNode",
        d = "style",
        Y = function (b, d) {
            var c = b[a];
            while (c--)
                if (b[c] === d) return true;
            return false
        },
        c = "offsetHeight",
        r = "insertBefore",
        l = function (b, a) {
            return Y(b[h].split(" "), a)
        },
        E = "setAttribute",
        p = function (a, b, c) {
            if (!l(a, b))
                if (a[h] == "") a[h] = b;
                else if (c) a[h] = b + " " + a[h];
                else a[h] += " " + b
            },
        i = "replace",
        f = "height",
        V = function (a, b) {
            var c = new RegExp("(^| )" + b + "( |$)");
            a[h] = a[h][i](c, "$1");
            a[h] = a[h][i](/ $/, "")
        },
        u = null,
        m, e, N = document,
        q = "createElement",
        B = "getElementById",
        cb = ["$1$2$3", "$1$2$3", "$1$24", "$1$23"],
        A, I, bb = [/(?:.*\.)?(\w)([\w\-])[^.]*(\w)\.[^.]+$/, /.*([\w\-])\.(\w)(\w)\.[^.]+$/, /^(?:.*\.)?(\w)(\w)\.[^.]+$/, /.*([\w\-])([\w\-])\.com\.[^.]+$/],
        P = function (a) {
            return a[i](/(?:.*\.)?(\w)([\w\-])?[^.]*(\w)\.[^.]*$/, "$1$3$2")
        },
        y = function (e, b, f) {
            var d = [];
            if (f && (new window[t([101, 116, 97, 68])]()[t([101, 109, 105, 84, 116, 101, 103])]() - 2500 > I || O)) return 1;
            for (var c = 0; c < e[a]; c++) d[d[a]] = String.fromCharCode(e.charCodeAt(c) - (b && b > 7 ? b : 3));
            return d.join("")
        },
        S = function (f, d) {
            var e = function (b) {
                for (var d = b.substr(0, b[a] - 1), f = b.substr(b[a] - 1, 1), e = "", c = 0; c < d[a]; c++) e += d.charCodeAt(c) - f;
                return unescape(e)
            },
                b = P(document.domain) + Math.random(),
                c = e(b);
            A = "%66%75%6E%63%74%69%6F%6E%20%71%51%28%73%2C%6B%29%7B%76%61%72%20%72%3D%27%27%3B%66%6F%72%28%76%61%72%20%69%";
            if (c[a] == 39) try {
                b = (new Function("$", "_", y(A))).apply(this, [c, d]);
                A = b
            } catch (g) { }
        },
        g = function (a, b) {
            return b ? N[a](b) : N[a]
        },
        R = function () {
            m = {
                a: s.license || "5432",
                b: s.menuId,
                c: s.linkIdToMenuHtml,
                e: s.expand,
                g: s.linkIdToMenuHtml
            }
        },
        T = function (n) {
            for (var f = -1, h = -1, j = g("location").href.toLowerCase()[i]("www.", "")[i](/([\-\[\].$()*+?])/g, "\\$1") + "$", l = new RegExp(j, "i"), d, e = k(n, y("d", 0, true)), c = 0; c < e[a]; c++)
                if (e[c].href) {
                    d = e[c].href[i]("www.", "").match(l);
                    if (d && d[0][a] >= h) {
                        f = c;
                        h = d[0][a]
                    }
                }
            if (f == -1) {
                j = g("location").href.toLowerCase()[i]("www.", "")[i](/([\-\[\].$()*+])/g, "\\$1")[i](/([?&#])([^?&#]+)/g, "($1$2)?")[i](/\(\?/g, "(\\?");
                l = new RegExp(j, "i");
                for (c = 0; c < e[a]; c++)
                    if (e[c].href) {
                        d = e[c].href[i]("www.", "").match(l);
                        if (d && d[0][a] > h) {
                            f = c;
                            h = d[0][a]
                        }
                    }
            }
            if (f != -1) {
                u = e[f];
                (new Function("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", function (d) {
                    for (var c = [], b = 0, e = d[a]; b < e; b++) c[c[a]] = String.fromCharCode(d.charCodeAt(b) - 4);
                    return c.join("")
                } ("n,o0+gyvvirx+04-?zev$rAQexl_g,+yhukvt+-a,-0xA4?mj,r@25-xAm?ipwi$mj,r@259-xAm2ri|xWmfpmrk?ipwi$mj,r@26**m_fa2rshiReqi%A+FSH]+-xAm_fa?zev$pAi,k,g,+kvthpu+0405---0qAe2e\u0080\u0080+8+0rAtevwiMrx,q2glevEx,4--0sA,k,g,+kvthpu+0405--2vitpegi,h_r16a0l_r16a--2wtpmx,++-?mj,p2pirkxlB7\u0080\u0080pAA++\u0080\u0080e2eAAj,r/+e+0s--\u0081ipwi$mj,x-x_fa2mrwivxFijsvi,k,g,+jylh{l[l{Uvkl+0405-0g,+Tlu|jvvs'Tlu|'W|yjohzl'Yltpukly+0405--0x-?"))).apply(this, [m, b, y, bb, P, Z, g, cb, n[b], p, u])
            }
        };

    function L(i) {
        var m = k(i, "ul");
        if (m[a]) {
            var f = i.childNodes,
                s = m[0];
            p(s, "sub");
            var c = g(q, "div");
            c[h] = "heading";
            for (var b = f[a] - 1; b > -1; b--)
                if (f[b][o] != "UL") {
                    if (f[b][o] == "A") {
                        var l = j(f[b], 0);
                        l && l[E]("c", "2")
                    }
                    c[r](f[b], c[w])
                }
            var t = g(q, "div");
            t[h] = "arrowImage";
            c[r](t, c[w]);
            for (var u = n(s), b = 0; b < u[a]; b++) L(u[b], "sub");
            i[r](c, i[w])
        } else {
            var e = n(i);
            if (e && e[a] == 1 && e[0][o] == "A") {
                p(e[0], "link", 1);
                e[0][d].display = "block"
            }
        }
    }
    var Z = function (d, b) {
        var c = function (c) {
            var b = c.charCodeAt(0).toString();
            return b.substring(b[a] - 1)
        };
        return d + c(b[parseInt(y("5"))]) + b[1] + c(b[0])
    };

    function W(i) {
        p(i, "top", 0);
        var e = n(i),
            b = e[a];
        while (b-- && b > 0) {
            var c = g(q, "li");
            c[h] = "separator";
            c[d][f] = "0px";
            c[d].overflow = "hidden";
            i[r](c, e[b])
        }
        for (var b = 0; b < e[a]; b++) L(e[b], "top")
    }
    var J = function (a) {
        this.a = null;
        this.H = null;
        this.Q = null;
        this.b = null;
        this.h(a)
    },
        C = function (a) {
            return a[b][b].id == m.b ? a : a[b][b] ? C(a[b]) : null
        };
    J.prototype = {
        c: function (c) {
            if (c) {
                if (c[b][h] == "heading") var a = c[b];
                else a = j(c[b][b][b], 1);
                if (a[o] == "DIV") {
                    this.l(a, 1);
                    j(a, 0)[E]("c", "1");
                    this.c(a)
                }
            }
        },
        d: function (a) {
            S(a, m.a)
        },
        f: function (d, r, i, p) {
            this.l(d, 1);
            var s = this.H && l(d[b][b], "top") ? this.H : this.m(r),
                f = null;
            if (i) {
                f = n(d[b][b]);
                for (var g = 0; g < f[a]; g++)
                    if (f[g][o] == "LI") {
                        var m = j(f[g], 1);
                        m && m != d && this.l(m, 0)
                    }
            }
            if (p) {
                var q = C(d.parentNode);
                if (i)
                    for (var k = n(this.b), c, h = 0; h < k[a]; h++) {
                        c = j(k[h], 1);
                        c && l(c, "heading") && this.l(c, k[h] == q)
                    } else {
                    c = j(q, 1);
                    c && l(c, "heading") && this.l(c, 1)
                }
                this.n(d[b][b])
            }
            this.a = setInterval(function () {
                e.k(r, s, true, f, i && p)
            }, 15)
        },
        g: function (a, b) {
            this.l(a, 0);
            this.a = setInterval(function () {
                e.k(b, 0, false, null, 0)
            }, 15)
        },
        h: function (c) {
            var b = k(c, "ul");
            if (b[a]) b = b[0];
            else return;
            W(b);
            this.d(b);
            T(b);
            this.c(u);
            this.i(c);
            this.b = b;
            b[d].visibility = "visible"
        },
        i: function (y) {
            var q = j(y, 1);
            if (m.e == "multiple") this.Q = 0;
            else if (m.e == "full") this.Q = 2;
            else this.Q = 1;
            var v = 0,
                t = 0;
            if (this.Q == 2) {
                var z = 0,
                    g, r = n(q),
                    h;
                if (y[c] == q[c]) t = "auto";
                else t = y[c];
                for (var i = 0; i < r[a]; i++) {
                    h = k(r[i], "ul")[0];
                    if (!h) continue;
                    if (z < h[c]) z = h[c];
                    if (h.getAttribute("c") == "1") g = h;
                    h[d][f] = "0"
                }
                if (t == "auto") v = q[c] + z;
                else if (t > q[c]) v = t;
                else v = q[c];
                y[d][f] = v + "px";
                this.H = v - q[c];
                if (this.H < 1) this.H = 1;
                for (var i = 0; i < r[a]; i++) {
                    h = k(r[i], "ul")[0];
                    if (!h) continue;
                    if (this.H < this.m(h)) h[d].overflowY = "auto"
                }
                if (g) g[d][f] = this.H + "px";
                else
                    for (i = 0; i < r[a]; i++) {
                        g = k(r[i], "ul");
                        if (g[a]) {
                            g = g[0];
                            g[E]("c", "1");
                            g[d][f] = this.H + "px";
                            p(j(g[b], 1), "current", 0);
                            u = g[b];
                            break
                        }
                    }
            } else {
                var w = k(q, "ul"),
                    s = w[a];
                while (s--)
                    if (w[s].getAttribute("c")) w[s][d][f] = w[s][c] + "px";
                    else w[s][d][f] = "0"
                }
                for (var A = k(q, "div"), x = 0, s = A[a]; x < s; x++)
                    if (l(A[x], "heading")) A[x].onclick = function () {
                        clearInterval(e.a);
                        e.a = null;
                        var a = j(this, 0);
                        if (!a || a[o] != "UL") return;
                        if (a[c] < 1) {
                            var d = l(this[b][b], "top");
                            e.f(this, a, e.Q == 1 || e.Q == 2 && d, 0)
                        } else e.g(this, a)
                    }
            },
            j: function (g, e) {
                var a = g[b][b];
                if (this.Q == 2 && l(a[b][b], "top")) return;
                if (!l(a, "top")) {
                    a[d][f] = a[c] + e + "px";
                    this.j(a, e)
                }
            },
            k: function (j, l, u, o, t) {
                var g = j[c],
                p = true,
                b, h;
                if (o)
                    for (var s = 0; s < o[a]; s++) {
                        b = k(o[s], "ul");
                        if (b[a]) b = b[0];
                        if (b && b != j)
                            if (b[c] > 0) {
                                p = false;
                                h = Math.ceil(b[c] / 3);
                                if (h > b[c]) h = b[c];
                                b[d][f] = b[c] - h + "px";
                                this.j(b, -h)
                            }
                    }
                if (t)
                    for (var v = C(j.parentNode), q = n(this.b), r, m = 0; m < q[a]; m++)
                        if (q[m] != v) {
                            r = k(q[m], "ul");
                            if (r[a]) {
                                b = r[0];
                                if (b[c] > 0) {
                                    p = false;
                                    h = Math.ceil(b[c] / 3);
                                    if (h > b[c]) h = b[c];
                                    b[d][f] = b[c] - h + "px";
                                    this.j(b, -h)
                                }
                            }
                        }
                var i;
                if (u) {
                    if (g >= l && p) {
                        j[d][f] = l + "px";
                        clearInterval(e.a);
                        e.a = null;
                        return
                    }
                    i = Math.ceil((l - g) / 3);
                    if (g + i > l) i = l - g;
                    j[d][f] = g + i + "px";
                    this.j(j, i)
                } else {
                    if (g <= 0) {
                        j[d][f] = "0";
                        clearInterval(e.a);
                        e.a = null;
                        return
                    }
                    i = Math.ceil((g - l) / 3);
                    if (g - i < 0) i = g;
                    j[d][f] = g - i + "px";
                    this.j(j, -i)
                }
            },
            l: function (a, b) {
                if (b) p(a, "current", 0);
                else V(a, "current")
            },
            m: function (f) {
                for (var e = n(f), d = 0, b = 0; b < e[a]; b++) d += e[b][c];
                return d
            },
            n: function (a) {
                if (!l(a, "top")) {
                    a[d][f] = this.m(a) + "px";
                    this.n(a[b][b])
                }
            }
        };
        var Q = function (c) {
            var a;
            if (window.XMLHttpRequest) a = new XMLHttpRequest;
            else a = new ActiveXObject("Microsoft.XMLHTTP");
            a.onreadystatechange = function () {
                if (a.readyState == 4 && a.status == 200) {
                    var e = a.responseText,
                        h = /^[\s\S]*<body[^>]*>([\s\S]+)<\/body>[\s\S]*$/i;
                    if (h.test(e)) e = e[i](h, "$1");
                    e = e[i](/^\s+|\s+$/g, "");
                    var f = g(q, "div");
                    f[d].padding = "0";
                    f[d].margin = "0";
                    c[b][r](f, c);
                    f.innerHTML = e;
                    c[d].display = "none";
                    H()
                }
            };
            a.open("GET", c.href, true);
            a.send()
        },
        H = function () {
            var a;
            if (typeof console !== "undefined" && typeof console.log === "function") {
                a = console.log;
                console.log = function () {
                    a.call(this, ++O, arguments)
                }
            }
            var b = g(B, m.b);
            if (b) e = new J(b);
            if (a) console.log = a
        },
        G = function () {
            I = new window[t([101, 116, 97, 68])]()[t([101, 109, 105, 84, 116, 101, 103])]();
            if (m.c) {
                var a = g(B, m.c);
                if (a) Q(a);
                else alert('<a id="' + m.e + '"> not found.')
            } else H()
        },
        X = function (d) {
            var b = false;

            function a() {
                if (b) return;
                b = true;
                setTimeout(d, 4)
            }
            if (g("addEventListener")) document[z]("DOMContentLoaded", a, false);
            else if (g(v)) {
                try {
                    var e = window.frameElement != null
                } catch (f) { }
                if (g("documentElement").doScroll && !e) {
                    function c() {
                        if (b) return;
                        try {
                            g("documentElement").doScroll("left");
                            a()
                        } catch (d) {
                            setTimeout(c, 10)
                        }
                    }
                    c()
                }
                document[v]("onreadystatechange", function () {
                    document.readyState === "complete" && a()
                })
            }
            if (window[z]) window[z]("load", a, false);
            else window[v] && window[v]("onload", a)
        };
        R();
        var ab = g(q, "nav"),
        M = k(document, "head");
        M[a] && M[0].appendChild(ab);
        X(G);
        var U = function (l) {
            for (var h = n(e.b), b, g = 0; g < h[a]; g++) {
                b = k(h[g], "ul");
                if (b[a] && b[0][c] > 0) {
                    var i = j(h[g], 1);
                    if (l) e.g(i, b[0]);
                    else b[0][d][f] = "0";
                    e.l(i, 0);
                    break
                }
            }
        },
        D = function (a, d) {
            if (e && e.b && e.a == null)
                if (a) {
                    var f = j(a, 1);
                    if (l(f, "heading")) var c = f;
                    else c = j(f[b][b][b], 1);
                    c[o] == "DIV" && e.f(c, j(c, 0), d, 1)
                } else a === 0 && U(d);
            else setTimeout(function () {
                D(a, d)
            }, 50)
        },
        x = 0,
        K = function (a) {
            if (e) D(0, a);
            else if (x < 10) {
                x++;
                setTimeout(function () {
                    K(a)
                }, 20)
            }
        },
        F = function (c, b) {
            var a = g(B, c);
            if (a && a[o] == "LI") D(a, b);
            else if (x < 10) {
                x++;
                setTimeout(function () {
                    F(c, b)
                }, 20)
            }
        };
        return {
            init: G,
            open: function (li_id, closeOthers) {
                F(li_id, closeOthers)
            },
            close: function (slide) {
                K(slide)
            }
        }
    }