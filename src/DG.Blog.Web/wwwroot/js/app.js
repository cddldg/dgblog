var func = window.func || {}, editor;
const myaplayer = new APlayer({
    container: document.getElementById('aplayer'),
    autoplay: true,
    loop: 'all',
    order: 'list',
    lrcType: 1,
    fixed: true,
    audio: [{
        name: '直到世界的尽头',
        artist: '上杉升 (うえすぎ しょう)',
        url: 'https://s-bj-1636-dldg.oss.dogecdn.com/glgs-zdsjjt.mp3',
        cover: 'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1593421609345&di=c66ed619b1b62eae221a813a036ada65&imgtype=0&src=http%3A%2F%2Fqiniuimg.qingmang.mobi%2Fimage%2Forion%2Fccb9bffe4189240c67b0b6af69494cac_1024_768.jpeg',
        lrc: '[00:02.21]直到世界尽头' +
            '[00:05.60]作词：上杉升 作曲：叶山たけし' +
            '[00:08.60]演唱：上杉升' +
            '[00:11.50]' +
            '[00:38.57]大都会に 仆はもう一人で' +
            '[00:47.79]投げ舍てられた 空カンのようだ' +
            '[00:55.05]互いのすべてを' +
            '[00:59.52]知りつくすまでが 爱ならば' +
            '[01:06.84]いっそ 永久に眠ろうか…' +
            '[01:12.79]世界が终わるまでは' +
            '[01:17.57]离れる事もない' +
            '[01:22.65]そう愿っていた 几千の夜と' +
            '[01:31.38]戾らない时だけが' +
            '[01:35.90]何故辉いては' +
            '[01:41.13]やつれ切った' +
            '[01:43.83]心までも 坏す…' +
            '[01:49.89]はかなき想い…' +
            '[01:54.79]このTragedy Night' +
            '[02:00.82]' +
            '[02:06.54]そして人は' +
            '[02:09.66]形(こたえ)を求めて' +
            '[02:15.59]かけがえのない 何かを失う' +
            '[02:22.83]欲望だらけの 街じゃ 夜空の' +
            '[02:31.98]星屑も 仆らを 灯せない' +
            '[02:40.53]世界が终わる前に' +
            '[02:45.35]闻かせておくれよ' +
            '[02:50.53]满开の花が' +
            '[02:54.95]似合いのCatastrophe' +
            '[02:59.16]谁もが望みながら' +
            '[03:03.80]永远を信じない' +
            '[03:08.92]…なのに きっと' +
            '[03:11.79]明日を梦见てる' +
            '[03:17.66]はかなき日 と' +
            '[03:22.56]このTragedy Night' +
            '[03:28.54]' +
            '[03:50.03]世界が终わるまでは' +
            '[03:54.57]离れる事もない' +
            '[03:59.71]そう愿っていた 几千の夜と' +
            '[04:08.39]戾らない时だけが' +
            '[04:12.88]何故辉いては' +
            '[04:18.12]やつれ切った' +
            '[04:20.86]心までも 坏す…' +
            '[04:26.85]はかなき想い…' +
            '[04:31.82]このTragedy Night' +
            '[04:36.38]このTragedy Night' +
            '[04:43.32]',
        theme: '#ebd0c2'
    }]
});

(function (l) {
    if (l.search) {
        var q = {};
        l.search.slice(1).split('&').forEach(function (v) {
            var a = v.split('=');
            q[a[0]] = a.slice(1).join('=').replace(/~and~/g, '&');
        });
        if (q.p !== undefined) {
            window.history.replaceState(null, null,
                l.pathname.slice(0, -1) + (q.p || '') +
                (q.q ? ('?' + q.q) : '') +
                l.hash
            );
        }
    }
}(window.location));

func = {
    changeTitle: function (title, keyworks) {
        document.title = title;
        if (keyworks) {
            var metas = document.getElementsByTagName("meta");
            for (var i = 0; i < metas.length; i++) {
                if (metas[i].name == "keywords") {
                    metas[i].content = 'dldg,huang,hym,blog,cd,成都,抵拢倒拐,个人博客,.netcore,C#,' + keyworks;
                    return;
                }
            }
        }
    },

    setStorage: function (name, value) {
        localStorage.setItem(name, value);
    },
    getStorage: function (name) {
        return localStorage.getItem(name);
    },
    switchTheme: function () {
        var currentTheme = this.getStorage('theme') || 'Light';
        var isDark = currentTheme === 'Dark';

        if (isDark) {
            document.querySelector('body').classList.add('dark-theme');
        } else {
            document.querySelector('body').classList.remove('dark-theme');
        }
    },
    setDarkTheme: function () {
        document.querySelector('body').classList.add('dark-theme');
    },
    switchEditorTheme: function () {
        editor.setTheme(localStorage.editorTheme || 'default');
        editor.setEditorTheme(localStorage.editorTheme === 'dark' ? 'pastel-on-dark' : 'default');
        editor.setPreviewTheme(localStorage.editorTheme || 'default');
    },
    renderEditor: async function () {
        await this._loadScript('./editor.md/lib/zepto.min.js').then(function () {
            func._loadScript('./editor.md/editormd.js').then(function () {
                editor = editormd("editor", {
                    width: "100%",
                    height: 700,
                    path: './editor.md/lib/',
                    codeFold: true,
                    saveHTMLToTextarea: true,
                    emoji: true,
                    atLink: false,
                    emailLink: false,
                    theme: localStorage.editorTheme || 'default',
                    editorTheme: localStorage.editorTheme === 'dark' ? 'pastel-on-dark' : 'default',
                    previewTheme: localStorage.editorTheme || 'default',
                    toolbarIcons: function () {
                        return ["bold", "del", "italic", "quote", "ucwords", "uppercase", "lowercase", "h1", "h2", "h3", "h4", "h5", "h6", "list-ul", "list-ol", "hr", "link", "image", "code", "preformatted-text", "code-block", "table", "datetime", "html-entities", "emoji", "watch", "preview", "fullscreen", "clear", "||", "save"]
                    },
                    toolbarIconsClass: {
                        save: "fa-check"
                    },
                    toolbarHandlers: {
                        save: function () {
                            func._shoowBox();
                        }
                    },
                    onload: function () {
                        this.addKeyMap({
                            "Ctrl-S": function () {
                                func._shoowBox();
                            }
                        });
                    }
                });
            });
        });
    },
    renderMarkdown: async function () {
        await this._loadScript('./editor.md/lib/zepto.min.js').then(function () {
            func._loadScript('./editor.md/lib/marked.min.js').then(function () {
                func._loadScript('./editor.md/lib/prettify.min.js').then(function () {
                    func._loadScript('./editor.md/editormd.js').then(function () {
                        editormd.markdownToHTML("content");
                    });
                });
            });
        });
    },
    _shoowBox: function () {
        DotNet.invokeMethodAsync('DG.Blog.Web', 'showbox');
    },
    _loadScript: async function (url) {
        let response = await fetch(url);
        var js = await response.text();
        eval(js);
    },
    addAplayer: function (resongs, cls, msg) {//https://aplayer.js.org/#/zh-Hans/?id=%E5%AE%89%E8%A3%85
        if (cls && cls === true) {
            myaplayer.list.clear();
        }
        myaplayer.list.add(resongs);
        if (cls && cls === true) {
            myaplayer.notice('🎵歌曲切换成功了哦🎵', 2000, 0.8);
        }
        if (msg && msg !== '') {
            myaplayer.notice('🎵' + msg + '🎵', 2000, 0.8);
        }
        myaplayer.play();
    }
};