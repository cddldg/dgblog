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
        url: 'http://up_mp4.t57.cn/2016/5/10m/16/203161624459.m4a',
        cover: 'https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1593421609345&di=c66ed619b1b62eae221a813a036ada65&imgtype=0&src=http%3A%2F%2Fqiniuimg.qingmang.mobi%2Fimage%2Forion%2Fccb9bffe4189240c67b0b6af69494cac_1024_768.jpeg',
        lrc: 'https://cn-south-17-aplayer-46154810.oss.dogecdn.com/hikarunara.lrc',
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
    changeTitle: function (title) {
        document.title = title;
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
    addAplayer: function (songs) {//https://aplayer.js.org/#/zh-Hans/?id=%E5%AE%89%E8%A3%85
        myaplayer.list.add(songs);
        myaplayer.play();
    }
};