//---------------------------------------------
//$(function () {
//    var option4 = {
//        title: {
//            text: '标签统计',
//            subtext: '540',
//            subtextStyle: {
//                color: '#333',
//                fontSize:22,
//                fontWeight:'bold'
                
//            },
//            textStyle: {
//                color: '#333',
//                fontWeight: 'bold',
//                fontSize: 16
//            },
//            x: 'center',
//            y: '41%'
//        },
//        tooltip : {
//            trigger: 'item',
//            formatter: "{b}<br/> {c} ({d}%)"
//        },
//        legend:{
//            show:false
//        },
//        grid: {
//            top: '8%',
//            containLabel: true
//        },
        
//        calculable : true,
//        series : [
//            {
//                name:'',
//                type:'pie',
//                radius:['55%','85%'],
//                sort : 'ascending',     // for funnel
//                data:[
//                    {value:256, name:'管理人员'},
//                    {value:100, name:'班组员工'},
//                    {value:50, name:'外包人员'},
//                    {value:5, name:'临时人员'},
//                    {value:200, name:'场内车辆'},
//                    {value:200, name:'临时车辆'}
//                ],
//                label: {
//                   show:false
//                },
//                itemStyle:{
//                    normal:{
//                        color:function(params){
//                            var colorList1 = ['#3aa0ff','#36cbcb','#4dcb73','#fad337','#f2637b','#975fe4']
//                            return colorList1[params.dataIndex]
//                        },
//                    },
                   
//                }
//            }
//        ]
//    }
    
//    var chart4 = echarts.init(document.getElementById('chart4'));
//    chart4.setOption(option4);
     
//});

    //---------------------------------------------右边滑动显示--------------------------------------

    $(function () {
        $('.leftDownImg').on('click',function () {
            var hasShow = $('.lg').hasClass('show');
            if (hasShow) {
                $('.lg').removeClass('show');//.addClass('hide');
                $('#btn_left_lg').addClass('active');
            } else {
                $('.lg').removeClass('hide').addClass('show');
                $('#btn_left_lg').removeClass('active');
            }
            
        })
    });