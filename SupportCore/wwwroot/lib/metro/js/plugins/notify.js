var NotifyDefaultConfig = {
    container: null,
    width: 220,
    timeout: METRO_TIMEOUT,
    duration: METRO_ANIMATION_DURATION,
    distance: "100vh",
    animation: "swing",
    onClick: Metro.noop,
    onClose: Metro.noop,
    onShow: Metro.noop,
    onAppend: Metro.noop,
    onNotifyCreate: Metro.noop

};

var Notify = {

    container: null,

    options: {
    },

    notifies: [],

    setup: function(options){
        var body = $("body"), container;
        this.options = $.extend({}, NotifyDefaultConfig, options);

        if (Notify.container === null) {
            Notify.container = Notify._createContainer();
        }

        return this;
    },

    reset: function(){
        var reset_options = {
            width: 220,
            timeout: METRO_TIMEOUT,
            duration: METRO_ANIMATION_DURATION,
            distance: "100vh",
            animation: "linear"
        };
        this.options = $.extend({}, NotifyDefaultConfig, reset_options);
    },

    _createContainer: function(){

        var container = $("<div>").addClass("notify-container");
        $("body").prepend(container);

        return container;
    },

    create: function(message, title, options){
        var notify, that = this, o = this.options;
        var m, t;

        if (Utils.isNull(options)) {
            options = {};
        }

        if (!Utils.isValue(message)) {
            return false;
        }

        notify = $("<div>").addClass("notify");
        notify.css({
            width: o.width
        });

        if (title) {
            t = $("<div>").addClass("notify-title").html(title);
            notify.prepend(t);
        }
        m = $("<div>").addClass("notify-message").html(message);
        m.appendTo(notify);

        // Set options
        /*
        * keepOpen, cls, width, callback
        * */
        if (options !== undefined) {
            if (options.cls !== undefined) {
                notify.addClass(options.cls);
            }
            if (options.width !== undefined) {
                notify.css({
                    width: options.width
                });
            }
        }

        notify.on(Metro.events.click, function(){
            Utils.exec(Utils.isValue(options.onClick) ? options.onClick : o.onClick, null, this);
            that.kill($(this), Utils.isValue(options.onClose) ? options.onClose : o.onClose);
        });

        // Show
        if (Notify.container === null) {
            Notify.container = Notify._createContainer();
        }
        notify.appendTo(Notify.container);

        notify.hide(0, function(){

            Utils.exec(Utils.isValue(options.onAppend) ? options.onAppend : o.onAppend, null, notify[0]);

            notify.css({
                marginTop: Utils.isValue(options.distance) ? options.distance : o.distance
            }).fadeIn(100, function(){
                var duration = Utils.isValue(options.duration) ? options.duration : o.duration;
                var animation = Utils.isValue(options.animation) ? options.animation : o.animation;

                notify.animate({
                    marginTop: ".25rem"
                }, duration, animation, function(){

                    Utils.exec(o.onNotifyCreate, null, this);

                    if (options !== undefined && options.keepOpen === true) {
                    } else {
                        setTimeout(function(){
                            that.kill(notify, Utils.isValue(options.onClose) ? options.onClose : o.onClose);
                        }, o.timeout);
                    }

                    Utils.exec(Utils.isValue(options.onShow) ? options.onShow : o.onShow, null, notify[0]);

                });
            });
        });
    },

    kill: function(notify, callback){
        var that = this, o = this.options;
        notify.off(Metro.events.click);
        notify.fadeOut(o.duration, function(){
            Utils.exec(Utils.isValue(callback) ? callback : that.options.onClose, null, notify[0]);
            notify.remove();
        });
    },

    killAll: function(){
        var that = this;
        var notifies = $(".notify");
        $.each(notifies, function(){
            that.kill($(this));
        });
    }
};

Metro['notify'] = Notify.setup();