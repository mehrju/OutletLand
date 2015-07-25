'use strict';

/**
 * @class
 * @type {{cssClass: string, daysTitleFormat: string, persianDigit: boolean, viewMode: string, position: string, autoClose: boolean, toolbox: boolean, format: boolean, observer: boolean, altField: boolean, altFormat: string, inputDelay: number, viewFormat: string, formatter: formatter, altFieldFormatter: altFieldFormatter, show: show, hide: hide, onShow: onShow, onHide: onHide, onSelect: onSelect, timePicker: {enabled: boolean}, dayPicker: {enabled: boolean}, monthPicker: {enabled: boolean}, yearPicker: {enabled: boolean}}}
 */
var ClassConfig = {

    /**
     * @property persianDigit
     * @type {boolean}
     * @default true
     */
    persianDigit: true,


    /**
     * @property viewMode
     * @type {string}
     * @default day
     */
    viewMode: false,


    /**
     * @property position
     * @type {string|Array}
     * @default auto
     */
    position: "auto",


    /**
     * @property autoClose
     * @type {boolean}
     * @default false
     */
    autoClose: false,


    /**
     * @format format
     * @type {boolean}
     * @default false
     */
    format: false,


    /**
     * @format observer
     * @type {boolean}
     * @default false
     */
    observer: false,


    /**
     * @format altField
     * @type {boolean}
     * @default false
     */
    altField: false,


    /**
     * @format inputDelay
     * @type {number}
     * @default 800
     */
    inputDelay: 800,

    /**
     * @method
     * @param unixDate
     * @returns {*}
     */
    formatter: function (unixDate) {
        var self = this;
        var pdate = new persianDate(unixDate);
        pdate.formatPersian = false;
        return pdate.format(self.format);
    },


    /**
     * @format altField
     * @type {string}
     * @default unix
     */
    altFormat: 'unix',


    /**
     * @method
     * @param unixDate
     * @returns {*}
     */
    altFieldFormatter: function (unixDate) {
        var self = this;
        var thisAltFormat = self.altFormat.toLowerCase();
        if (thisAltFormat === "gregorian" | thisAltFormat === "g") {
            return new Date(unixDate);
        }
        if (thisAltFormat === "unix" | thisAltFormat === "u") {
            return unixDate;
        }
        else {
            return new persianDate(unixDate).format(self.altFormat);
        }
    },


    /**
     * @method
     * @returns {ClassConfig}
     */
    show: function () {
        this.view.fixPosition(this);
        this.element.main.show();
        this.onShow(this);
        this._viewed = true;
        return this;
    },


    /**
     * @method
     * @returns {ClassConfig}
     */
    hide: function () {
        if (this._viewed) {
            this.element.main.hide();
            this.onHide(this);
            this._viewed = false;
        }
        return this;
    },

    /**
     * @method
     * @param self
     */
    destroy: function () {
        this.elmenet.main.remove();
    },


    /**
     * @event
     * @param self
     */
    onShow: function (self) {
    },


    /**
     * @event
     * @param self
     */
    onHide: function (self) {
    },


    /**
     * @event
     * @param unixDate
     */
    onSelect: function (unixDate) {
        return this;
    },

    /**
     * @property navigator
     * @type {boolean}
     * @default true
     */
    navigator: {
        enabled: true,
        text: {
            btnNextText: "<",
            btnPrevText: ">"
        },
        onNext: function (navigator) {
            //log("navigator next ");
        },
        onPrev: function (navigator) {
            //log("navigator prev ");
        },
        onSwitch: function (state) {
            // console.log("navigator switch ");
        }
    },

    /**
     * @property toolbox
     * @type {boolean}
     * @default true
     * @deprecated 0.2.3
     */
    toolbox: {
        enabled: true,
        text: {
            btnToday: "امروز"
        },
        onToday: function (toolbox) {
            //log("toolbox today btn");
        }
    },


    /**
     * @property timePicker
     * @type {object}
     */
    timePicker: {
        enabled: false,
        showSeconds: true,
        showMeridian: true,

        secondStep: 1,
        minuteStep: 1,
        hourStep: 1,

        scrollEnabled: true,
        /**
         * @deprecated 0.3.5
         */
        changeOnScroll: true
    },

    /**
     * @property dayPicker
     * @type {object}
     */
    dayPicker: {
        enabled: true,
        scrollEnabled: true,
        titleFormat: 'YYYY MMMM',
        titleFormatter: function (year, month) {
            if (this.datepicker.persianDigit == false) {
                window.formatPersian = false;
            }
            var titleStr = new persianDate([year, month]).format(this.titleFormat);
            window.formatPersian = true;
            return titleStr
        },
        onSelect: function (selectedDayUnix) {
            //log("daypicker month day :" + selectedDayUnix);
        }

    },

    /**
     * @property monthPicker
     * @type {object}
     */
    monthPicker: {
        enabled: true,
        scrollEnabled: true,
        titleFormat: 'YYYY',
        titleFormatter: function (unix) {
            if (this.datepicker.persianDigit == false) {
                window.formatPersian = false;
            }
            var titleStr = new persianDate(unix).format(this.titleFormat);
            window.formatPersian = true;
            return titleStr;

        },
        onSelect: function (monthIndex) {
            //log("daypicker select day :" + monthIndex);
        }
    },


    /**
     * @property yearPicker
     * @type {object}
     */
    yearPicker: {
        enabled: true,
        scrollEnabled: true,
        titleFormat: 'YYYY',
        titleFormatter: function (year) {
            var remaining = parseInt(year / 12) * 12;
            return remaining + "-" + (remaining + 11);
        },
        onSelect: function (monthIndex) {
            //log("daypicker select Year :" + monthIndex);
        }
    },


    /**
     * if true all pickers hide and just shpw timepicker
     * @property justSelectOnDate
     */
    onlyTimePicker: false,


    /**
     * if true date select just by click on day in month grid
     * @property justSelectOnDate
     */
    justSelectOnDate: true,


    /**
     * set min date on datepicker
     * @property minDate
     */
    minDate: false,


    /**
     * set max date on datepicker
     * @property maxDate
     */
    maxDate: false

//    minDate: 1419242667029,
//    maxDate: false

}