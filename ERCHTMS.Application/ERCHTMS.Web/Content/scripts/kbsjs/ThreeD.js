function threeFn() {
    var frame = document.getElementById('ZbMap');
    var sendData = {
        "name": "startPos",
        "data": "取点方法"
    }
    frame.contentWindow.postMessage(sendData, "*");
}

function Iniiframe() {
    //初始化职位json
    $.ajax({
        url: top.contentPath + "/KbsDeviceManage/Kbsdevice/WebGetThreeUrl",
        type: "post",
        dataType: "text",
        async: false,
        success: function (data) {
            $('#ZbMap').attr('src', data + '/gisbcdl/index.html#/home');
            // $('#ZbMap').attr('src', 'http://192.168.1.103:8089/#/home');
        }
    });

    //三维方法注册
    if (ptype == "dzwl") {//pc电子围栏
        window.addEventListener('message', pcElectricFence, false);
    }
    else {
        window.addEventListener('message', receiveMessage, false)
    }

}

function threeDIni() {
    var frame = document.getElementById('ZbMap');
    var sendData = {
        "name": "initPage",
        "data": "getOutPos"
    }
    frame.contentWindow.postMessage(sendData, "*");
}
function receiveMessage(event) {
    var name = event.data.name;
    if (name == "initPage") {
        threeDIni();
    }
    else if (name == "startWork") {
        threeFn();
    } else if (name == "finishedWork") {
        point = event.data.data;
        floorId = event.data.floorID;
    }
}

//pc端电子围栏初始化
function pcElectricFence(event) {
    var name = event.data.name;
    if (name == "initPage") {
        threeDIni1();
    }
    else if (name == "startWork") {
        threeFn1();
    }
    else if (name == "finishedWork") {
        point = event.data.data;
        loorId = event.data.floorID;
        Radius = event.data.radius;
    }
}

//电子围栏
function threeDIni1() {
    var frame = document.getElementById('ZbMap');
    var sendData = {
        "name": "initPage",
        "data": "drawOutDzwl"
    }
    frame.contentWindow.postMessage(sendData, "*");
}

//电子围栏
function threeFn1() {
    var frame = document.getElementById('ZbMap');
    var sendData = {
        "name": "addDZWL",
        "data": { type: pNum, Areacode: { positions: [] }, isDetail: 0, floorID: "" }
    }
    frame.contentWindow.postMessage(sendData, "*");
};












