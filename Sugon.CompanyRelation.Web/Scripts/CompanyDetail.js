$(document).ready(
function (e) {
	//$("._cad").append('<div style="background:#FAFBFD; width:100%; height:100%; position:absolute; top:0px; left:0px; z-index:-100;"></div><div class="_left"></div><div class="_right"><div class = "_right_child" style="float:right;"></div></div><div class="_centre"><img class = "jt1" src="jiantou1.png" /><img class = "jt2" src="jiantou3.png" /><div class = "wz"></div></div>');
	// ///////////////////////////////////////////////////////////////////////////////
	//var co_name;
	//var per_cent;
	//for (var i = 0, l = _str1.length; i < l; i++) {
	//	for (var key in _str1[i]) {
	//		if (key == "co_name") {
	//			co_name = _str1[i][key];
	//		};
	//		if (key == "per_cent") {
	//			per_cent = _str1[i][key];
	//		};
	//	}
	//	$("._left").append('<div class = "m_child"><div>'
	//		+ co_name
	//		+ '</div><span class = "l_m_child_per_cent">'
	//		+ per_cent + '</span></div>');
	//}

	//for (var i = 0, l = _str2.length; i < l; i++) {
	//	for (var key in _str2[i]) {
	//		if (key == "co_name") {
	//			co_name = _str2[i][key];
	//		};
	//		if (key == "per_cent") {
	//			per_cent = _str2[i][key];
	//		};
	//	}
	//	$("._right_child").append('<div class = "m_child"><div>'
	//		+ co_name
	//		+ '</div><span class = "r_m_child_per_cent">'
	//		+ per_cent + '</span></div>');
	//}
	//$("._centre").children(".wz").html(_co);

    // //////////////////////////////////////////////////////////////////////////////
    var _with = $("._cad").width() - 100;
	var _le = -_with / 2 + 50;
	 //$("._cad").height($(window).height());
	$("head").append('<style>._centre:before{width:' + _with + 'px;left:' + _le + 'px; border:1px solid #000000;}</style>');
	var ch_with = $("._left").width();
	var m_child_with = $(".m_child").width();
	var m_child_height = $(".m_child").height();
	$("head").append('<style>._left div:before{width:' + ch_with + 'px;left:' + (m_child_with + 2) + 'px;}</style>');
	$("head").append('<style>._right_child div:before{width:' + (ch_with - m_child_with) + 'px;left:' + (-ch_with + m_child_with - 2) + 'px;}</style>');
	///////////////////////////////////////////////////////////////////////////////////////////////////
	var l_h = $("._left").height();
	var r_h = $("._right").height();
	var c_h = $("._cad").height();
	var m_centre_height = $("._centre").height();
	if (l_h > c_h || r_h > c_h) {
		$("._centre").css("margin-top", (c_h - m_centre_height) / 2 + "px");
		// $("._left").css("margin-top",-(c_h-m_child_height)/2+"px");
	    // $("._right").css("margin-top",-(c_h-m_child_height)/2+"px");
	} else if (l_h > r_h && l_h < m_centre_height) {
	    $("._centre").css("margin-top", "2px");
	} else if (l_h > r_h) {
		$("._centre").css("margin-top", (l_h - m_centre_height) / 2 + "px");
		// $("._left").css("margin-top",-(l_h-m_child_height)/2+"px");
	    // $("._right").css("margin-top",-(l_h-m_child_height)/2+"px");
	} else if (l_h <= r_h) {
	    $("._centre").css("margin-top", "2px");
		// $("._left").css("margin-top",-(r_h-m_child_height)/2+"px");
	    // $("._right").css("margin-top",-(r_h-m_child_height)/2+"px");
	}
	///////////////////////////////////////////////////////////////////////////
	var _ma_top = parseInt($("._centre").css("margin-top"));
	if (l_h > r_h) {
		if ((_ma_top + m_centre_height / 2) > r_h) {
			$("._right").height(_ma_top + 71);
		}
	} else if (l_h <= r_h) {
		if ((_ma_top + m_centre_height / 2) > l_h) {
			$("._left").height(_ma_top + 71);
		}
	}
	$("._cad").scroll(function () {
		var top = $("._cad").scrollTop() + _ma_top;
		$("._centre").css({
			"margin-top": top + "px"
		});
		if (l_h > r_h) {
			if ((top + m_centre_height / 2) + 18 > r_h) {
				$("._right").height(top + 70);
				//alert((top + m_centre_height / 2)+":"+r_h);
			} else {
				$("._right").css("height", "");
			}
		} else if (l_h <= r_h) {
			if ((top + m_centre_height / 2) + 20 > l_h) {
				$("._left").height(top + 70);
			} else {
				$("._left").css("height", "");
			}
		}
	});
	$("._left").children(":first").css("background",
			"url(../Content/Images/bg-1-0.png)");
	$("._left").children(":last").css("background",
			"url(../Content/Images/bg-1-2.png)");
	$("._right").children(":first").css("background",
			"url(../Content/Images/bg-2-0.png)");
	$("._right").children(":last").css("background",
			"url(../Content/Images/bg-2-2.png)");
});