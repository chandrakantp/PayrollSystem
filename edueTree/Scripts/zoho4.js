function doMonthYearConv() {
    if ($(".year") && $(".year").hasClass('active')) {
        jQuery.each(jQuery('.price'), function (key, value) {
            var s = value.children[1];
            var ps = s.innerHTML;
            var p = parseInt(ps);
            if (ps.indexOf(".") != -1) {
                p = parseFloat(ps);
            }
            value.children[1].innerHTML = p / 10;
        });
    }
    else if ($(".mon") && $(".mon").hasClass('active')) {
        jQuery.each(jQuery('.price'), function (key, value) {
            var s = value.children[1];
            var ps = s.innerHTML;
            var p = parseInt(ps);
            if (ps.indexOf(".") != -1) {
                p = parseFloat(ps);
            }
            value.children[1].innerHTML = p * 10;
        });
    }
}

function perEmpPricing() {
    var currency = '';
    var payperiod = '';
    var amount = '';
    if (jQuery('.year').hasClass('active')) {
        payperiod = 'year';
    } else {
        payperiod = 'month';
    }

    if (jQuery('.cusd span.two').hasClass('action')) {
        currency = '₹';
    } else if (jQuery('.cusd span.one').hasClass('action')) {
        currency = '$';
    }
    console.log("payperiod " + payperiod);
    Tooltip();
    if (payperiod == 'year' && currency == '$') {
        amount = 10;
    } else if (payperiod == 'month' && currency == '$') {
        amount = 1;
    } else if (payperiod == 'year' && currency == '₹') {
        amount = 600;
    } else if (payperiod == 'month' && currency == '₹') {
        amount = 60;
    }
    //$('.product-block:last-child .detail span:last-child').text(currency+amount+'/'+payperiod+'/'+'additional employee.');
    $('.product-block:last-child .detail span:last-child').text('More than 100 Employees');
}
function currencyChang() {
    var hosNam = window.location.hostname.split('.')[2];
    if (hosNam == "eu") {
        var lastEle = $(".product-block:last-child .detail span:last-child");
        lastEle.text('More than 100 Employees');
        jQuery('.main-box .price span:first-child').html('€');
        $(lastEle).hover(function () {
            if ($('.pricing-tab .active').index() == 0) {
                $('.price-tooltip').remove();
                lastEle.append('<em class="price-tooltip"></em>');
                $('.price-tooltip').text('€2/month/additional employee.');
            }
            if ($('.pricing-tab .active').index() == 1) {
                $('.price-tooltip').remove();
                lastEle.append('<em class="price-tooltip"></em>');
                $('.price-tooltip').text('€20/year/additional employee.');
            }
        });
        $(lastEle).mouseleave(function () {
            $('.price-tooltip').remove();
        });
    }
}
jQuery(document).ready(function (e) {

    //

    perEmpPricing();
    currencyChang();

    $(".year").click(function () {
        if (!jQuery('.year').hasClass('active')) {
            doMonthYearConv();
            jQuery('.mon').removeClass('active');
            jQuery('.year').addClass('active');
            $(".detail span:first-child").html("Per year");
            var curr = $(".cusd .action").html();
            //console.log(curr);
            if (CountryCode == "IN") {
                if (curr == '₹') {
                    //$(".product-block:last-child .detail span:last-child").html(curr+"1200/year/additional employee.");
                    $('.product-block:last-child .detail span:last-child').text('More than 100 Employees');
                }
                else {
                    //$(".product-block:last-child .detail span:last-child").html(curr+"20/year/additional employee.");
                    $('.product-block:last-child .detail span:last-child').text('More than 100 Employees');
                }
            }
            else if (CountryCode == "US") {
                // $(".product-block:last-child .detail span:last-child").html("$20/year/additional employee.");
                $('.product-block:last-child .detail span:last-child').text('More than 100 Employees');
            }
            $($(".detail span:first-child")[0]).html("Forever");
        }
    });
    $(".mon").click(function () {
        if (!jQuery('.mon').hasClass('active')) {
            doMonthYearConv();
            jQuery('.year').removeClass('active');
            jQuery('.mon').addClass('active');
            $(".detail span:first-child").html("Per month");
            var curr = $(".cusd .action").html();
            if (CountryCode == "IN") {
                if (curr == '₹') {
                    //$(".product-block:last-child .detail span:last-child").html(curr+"120/month/additional employee.");
                    $('.product-block:last-child .detail span:last-child').text('More than 100 Employees');
                }
                else {
                    //$(".product-block:last-child .detail span:last-child").html(curr+"2/month/additional employee.");
                    $('.product-block:last-child .detail span:last-child').text('More than 100 Employees');
                }
            }
            else if (CountryCode == "US") {
                //$(".product-block:last-child .detail span:last-child").html("$2/month/additional employee.");
                $('.product-block:last-child .detail span:last-child').text('More than 100 Employees');
            }
            $($(".detail span:first-child")[0]).html("Forever");
        }

    });

    /**** Add class to the pricing column start ****/
    var priceWrap = jQuery('body').find('.pricing-wrap');
    var overallHeight = 0;
    if (priceWrap.length > 0) {
        var foundColumn = jQuery('.product-block')
        var priAdd = 'pricing-box-' + foundColumn.length;
        jQuery('.pricing-wrap').addClass(priAdd);
        jQuery(foundColumn).each(function (index) {
            if (overallHeight < jQuery(this).innerHeight()) {
                overallHeight = jQuery(this).innerHeight();
            }
        });
        setPlanWidth();
    }

    jQuery(window).resize(function () {
        setPlanWidth();
    })

    function setPlanWidth() {
        if (window.innerWidth >= 768) {
            jQuery(foundColumn).css({ 'min-height': overallHeight + 15 });
        }
        else {
            jQuery(foundColumn).css({ 'min-height': 'inherit' });
        }
    }
});


/*************QUery Parameter*********/


function Tooltip() {
    $('.price-tooltip').remove();
    var lastEle = $(".product-block:last-child .detail span:last-child");
    lastEle.text('More than 100 Employees');
    //  lastEle.append('<span class="price-tooltip"></span>');
    if (document.domain != 'www.zoho.eu') {
        if (CountryCode == "IN") {
            if ($('.cusd .action').index() == 1) { //console.log('₹')
                $(lastEle).hover(function () {
                    if ($('.pricing-tab .active').index() == 0) {
                        $('.price-tooltip').remove();
                        lastEle.append('<em class="price-tooltip"></em>');
                        $('.price-tooltip').text('₹120/month/additional employee.');
                    }
                    if ($('.pricing-tab .active').index() == 1) {
                        $('.price-tooltip').remove();
                        lastEle.append('<em class="price-tooltip"></em>');
                        $('.price-tooltip').text('₹1200/year/additional employee.');
                    }
                });
                $(lastEle).mouseleave(function () {
                    $('.price-tooltip').remove();
                });
            }

            else { //console.log('$')           
                $(lastEle).hover(function () {
                    if ($('.pricing-tab .active').index() == 0) {
                        $('.price-tooltip').remove();
                        lastEle.append('<em class="price-tooltip"></em>');
                        $('.price-tooltip').text('$2/month/additional employee.');
                    }
                    if ($('.pricing-tab .active').index() == 1) {
                        $('.price-tooltip').remove();
                        lastEle.append('<em class="price-tooltip"></em>');
                        $('.price-tooltip').text('$20/year/additional employee.');
                    }
                });
                $(lastEle).mouseleave(function () {
                    $('.price-tooltip').remove();
                });
            }

        }
    }
    if (CountryCode == "US") {
        $(lastEle).hover(function () {
            if ($('.pricing-tab .active').index() == 0) {
                $('.price-tooltip').remove();
                lastEle.append('<em class="price-tooltip"></em>');
                $('.price-tooltip').text('$2/month/additional employee.');
            }
            if ($('.pricing-tab .active').index() == 1) {
                $('.price-tooltip').remove();
                lastEle.append('<em class="price-tooltip"></em>');
                $('.price-tooltip').text('$20/year/additional employee.');
            }
        });
        $(lastEle).mouseleave(function () {
            $('.price-tooltip').remove();
        });
    }
}