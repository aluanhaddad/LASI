/// <reference path="../../../typings/jquery/jquery.d.ts"/>
/// <reference path="../lasi.ts"/>
(function (app) {
    app.enableActiveHighlighting = (function () {
        'use strict';
        var onReady = function () {
            var phrasalTextSpans = $('span.phrase'),
                highlightClass = 'active-phrase-highlight',
                recolor = function () {
                    phrasalTextSpans.each(function () {
                        $(this).removeClass(highlightClass);
                    });
                    $(this).addClass(highlightClass);
                };
            phrasalTextSpans.click(recolor);
            phrasalTextSpans.on('contextmenu', recolor);

            // bootstrap requires that tooltips be manually enabled. The data-toggle="tooltip" attributes set on each element
            // have no effect without this or an equivalent call. By setting container to 'body', we allow the contents of the 
            // tooltip to overflow its container. This is generally close to the desired behavior as it is difficult to predict width
            // and this gives good flexibility. There is probably a cleaner and more precise/obvious way of doing this, change to that if discovered.
            (<any>$('[data-toggle="tooltip"]')).tooltip({
                delay: 250,
                container: 'body'
            });
            // TODO: look into fixing tooltips on elements containing a line break or remove such breaks.
        };
        $(onReady);
        return onReady;
    } ());
} (LASI));
 