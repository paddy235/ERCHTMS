var containerDiv = document.querySelector(".container");
$(".container").css({ position: "relative" });
// containerDiv.style.height = document.body.clientHeight;
var content = document.querySelector(".content");
console.log(content)
var contentWidth = 1600;
var contentHeight = 1600;

var contentWidthEdge = contentWidth - containerDiv.clientWidth;
var contentHeightEdge = contentHeight - containerDiv.clientHeight;

var start = { x: 0, y: 0 };
var end = { x: 0, y: 0 };

var offset = { x: 0, y: 0 };

var marginLeft = "0px";
var marginRight = "0px";

var flag;
var dify;
var difx;
content.onmousedown = function (ev) {
    var ev = ev || window.event;
    // ev.stopPropagation();
    difx = ev.clientX - content.offsetLeft;
    dify = ev.clientY - content.offsetTop;
    console.log(1)
    ev.preventDefault();
    ev.stopPropagation();
    //#endregion
    flag = true;

}
content.onmousemove = function (ev) {
    var e = ev || window.event;
    if (flag) {
        var content_left = ev.clientX - difx;
        var content_top = ev.clientY - dify;
        content.style.left = content_left + "px";
        content.style.top = content_top + "px";
        // if(ev.clientX==0){
        // 	content.onmousemove = null;
        // 	return false;
        // }
    }
    // return false;
}
containerDiv.onmouseup = function (ev) {
    console.log(3)
    var ev = ev || window.event;
    // console.log(content_top,contentHeightEdge );
    var ev = ev || window.event;

    // e.preventDefault();

    // var ev = ev||event;
    // 	var content_left = ev.clientX - difx;
    // 	var content_top = ev.clientY - dify;
    // 	console.log(content_top,contentHeightEdge );
    // 	// if(content_left<-contentWidthEdge){
    // 	// 	content.onmousemove = null;
    // 	// 	return false;
    // 	// }
    // 	// if(content_top<-contentHeightEdge){
    // 	// 	content.onmousemove = null;
    // 	// 	return false;
    // 	// }
    // 	content.style.left = content_left +"px";
    // 	content.style.top = content_top +"px";
    //    debugger;
    // content.onmousemove = null;
    // content.onmouseup = null;
    flag = false;
}




// // $(".content").on("touchstart",startAction)
// content.addEventListener("mousedown",startAction);
// content.addEventListener("mousemove",move);
// var difx;
// var dify;
// function dragT(e){
// 	var ev = e||window.event;

// 	var userAgent = navigator.userAgent;
// 	var ifFirefox = userAgent.indexOf("Firefox");
// 	if(ifFirefox){
// 		ev.dataTransfer.setData("imginfo",ev.target.id);
// 	}
// 	// console.log(e);

//     difx = ev.clientX - content.offsetLeft;
//     dify = ev.clientY - content.offsetTop;
// }
// function dragmove(e){
// 	debugger;
// 	    var e = e||window.event;
// 		console.log(e.clientX)
// 	    // e.preventDefault();
// 		// e.stopPropagation();
// 		// var ev = e||event;
// 		// var content_left = ev.clientX - difx;
// 		// var content_top = ev.clientY - dify;

// 		// console.log(content_left,content_top)
// 		// // if(content_left<-contentWidthEdge){
// 		// // 	content.onmousemove = null;
// 		// // 	return false;
// 		// // }
// 		// // if(content_top<-contentHeightEdge){
// 		// // 	content.onmousemove = null;
// 		// // 	return false;
// 		// // }
// 		// content.style.left = content_left +"px";
// 		// content.style.top = content_top +"px";
// }
// function drage(e){

// 	var ev = e||window.event;
// 		var content_left = ev.clientX - difx;
// 		var content_top = ev.clientY - dify;
// 		console.log(content_left,content_top)
// 		// if(content_left<-contentWidthEdge){
// 		// 	content.onmousemove = null;
// 		// 	return false;
// 		// }
// 		// if(content_top<-contentHeightEdge){
// 		// 	content.onmousemove = null;
// 		// 	return false;
// 		// }
// 		content.style.left = content_left +"px";
// 		content.style.top = content_top +"px";
// 		e.preventDefault();
// 		e.stopPropagation();
// }
// function dragoverl(e){
// 	var ev = e||window.event;
// 	e.preventDefault();
// }
// function dropsave(e){
// 	var ev = e||window.event;
// 	e.preventDefault();
// 	e.stopPropagation();
// }

var scale = 1;

function zoomimg(event) {
    var delta = 0;
    if (!event) event = window.event;
    console.log(event.wheelDelta)
    if (event.wheelDelta) {
        delta = event.wheelDelta / 120;
        if (window.opera) delta = -delta;
    } else if (event.detail) {
        delta = -event.detail / 3;
    }
    if (delta > 0) {
        scale = scale + 0.1;
        console.log('放大图片的操作');
        if (scale >= 0.6) {
            $(".content>div>div").css("opacity", 1);
        }

    } else if (delta < 0) {
        if (scale < 0.6 && scale >= 0.4) {
            scale = scale - 0.1;
            $(".content>div>div").css("opacity", 0);
        }
        if (scale > 0.4) {
            scale = scale - 0.1;
        }
        console.log('缩小图片的操作');

    }
    content.style.transform = "scale(" + scale + ")"
}

if (window.addEventListener)
    containerDiv.addEventListener('DOMMouseScroll', zoomimg);
containerDiv.onmousewheel = document.onmousewheel = zoomimg;