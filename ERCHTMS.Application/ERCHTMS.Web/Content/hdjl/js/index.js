$(function () {
	//待办事项
    var mySwiper1 = new Swiper('.swiper-container1', {
        loop: true,
        // 如果需要前进后退按钮
        nextButton: '.swiper-button-next',
        prevButton: '.swiper-button-prev',
    })
    //违章曝光
    var mySwiper4 = new Swiper('.swiper-container4', {
		loop: true,
		slidesPerView: 3,
		spaceBetween: 30,
		autoplay: 3000,
        // 如果需要分页器
        pagination: '.swiper-pagination',
        paginationClickable: true
    })
    //工作预警
    var mySwiper2 = new Swiper('.swiper-container2', {
        loop: true,
        // 如果需要分页器
        pagination: '.swiper-pagination',
        paginationClickable: true
    })
    

    $('.sec4_list li').on('click', function () {
        var index = $(this).index();
        $(this).addClass('active').siblings().removeClass('active');
    })
    $('.secTit_list li').on('click', function () {
        var index = $(this).index();
        $(this).addClass('active').siblings().removeClass('active');
    })

    //表格滚动条
    var scrollParam = {
        autohidemode:true,
        cursorcolor:"#d0ebfc",
        cursorwidth:"5px",
        cursorborder:"none",
        cursorborderradius:"2.5px"
    }
    $('.table1-tbody').niceScroll(scrollParam);
})


$(function () {
    var swiper = {
        swiper0: null,
        swiper1: null,
        swiper2: null,
        swiper3: null,
        swiper4: null,
    }
    swiper.swiper0 = new Swiper('.zk-swiper0', {
        loop: true,
        // 如果需要分页器
        pagination: '.swiper-pagination',
        paginationClickable: true
    })
    $('.secTit_list li').click(function () {
        var index = $(this).index()
        if (index === 0) {
            $('.zk-swiper-wrap').show()
            $('.my-table').hide()
        } else {
            $('.zk-swiper-wrap').hide()
            $('.my-table').show()
        }
    })

    $(".side-nav li").click(function () {
        var index = $(this).index()
        $(this).addClass('active').siblings().removeClass('active')
        $('.zk-swiper' + index).show().siblings('.zk-swiper').hide()
        if (!(swiper['swiper' + index])) {
            swiper['swiper' + index] = new Swiper('.zk-swiper' + index, {
                loop: true,
                // 如果需要分页器
                pagination: '.swiper-pagination',
                paginationClickable: true
            })
        }
    })
})


