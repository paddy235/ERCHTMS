var data7 = [{
	name: '一级风险',
	value: 5
}, {
	name: '二级风险',
	value: 20
}, {
	name: '三级风险',
	value: 45
}]
var color = ['#d42129', '#eaa309', '#0060e1']
var option = {
	legend: {
		selectedMode: true,
		top: 40,
		left:100,
		orient: 'vertical',
		x:'left',
		itemGap:20,
		textStyle: {
			fontSize: 14,
			color: '#ffffff',
			textAlign: 'left'

		},
		data: ['一级风险', '二级风险', '三级风险'],

	},

	series: [{
		type: 'pie',
		hoverAnimation: true,
		color: color,
		label: {
			normal: {
				show: true,
				position: 'absolute',

				color: '#ffffff'
			}
		},
		labelLine: {
			normal: {
				show: true,
				length: 30,
				length2: 15,
				lineStyle: {

				}
			}
		},
		data: data7
	}]
}
var chart = echarts.init(document.getElementById('chart7'));
chart.setOption(option);