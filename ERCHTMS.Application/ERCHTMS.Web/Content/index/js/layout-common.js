var win,bs = {};


if( top === window ){
    win = window;
}else{
    win = parent.window;
}

bs.layer = win.layer;

$(function(){
    /*响应式切换*/
    $('.bs-aside > .bs-toggle').click(function(){
        $('.bs-warp').toggleClass('fold')
    });

    /*表格单选*/
    $('[data-toggle="radio"]').delegate('input[type="checkbox"]', 'click', function(e){
        e.stopPropagation();
        var $table = $(this).parents('table');
        $table.find('input[type="checkbox"]').prop('checked',false);
        $(this).prop('checked',true);
    })
    $('[data-toggle="radio"]').delegate('tr','click',function(){
        var $p = $(this).parents('table');
        var $input = $(this).find('input[type="checkbox"]');
        var flag = $input.prop('checked');
        $p.find('input[type="checkbox"]').prop('checked',false);
        $input.prop('checked',!flag);
    })

    /*新增tab页面*/
    $(document).delegate('[data-target="iframe"]','click',function(){
        createIframe(this);
    })

    /*tab页面切换*/
    $('.bs-nav-table').delegate('li','click',function(){
        var index = $(this).index();
        $(this).addClass('active').siblings('li').removeClass('active');
        $('#frame-box iframe').contents().scrollTop(0);
        $('#frame-box > div').eq(index).fadeIn().siblings('div').hide();
    })

    /*点击按钮移除*/
    $('.bs-nav-table').delegate('.remove', 'click', function(e){
        e.stopPropagation();
        var index = $(this).parent('li').index();
        removeIframe(index);
    })

    /*打开操作轨迹*/
    $('#trail').click(function(){
        $(this).next().slideDown();
    })

    /*关闭操作轨迹*/
    $('#close-trail').click(function(){
        $(this).parents('.trail-content').slideUp();
    })

})


/*tab页移除*/
function removeIframe(index){
    debugger;
    var $frameBox,$bsNavTable;
    if( top === window ){
        $frameBox = $('#frame-box');
        $bsNavTable = $('.bs-nav-table');
    }else{
        $frameBox = $('#frame-box', parent.window.document);
        $bsNavTable = $('.bs-nav-table', parent.window.document);
    }
    index = index || $bsNavTable.find('li.active').index();
    $bsNavTable.find('li:eq(' + index + ')').remove().end().find('li:eq('+ (index - 1) +')').addClass('active');
    $frameBox.children('div:eq('+ index +')').remove().end().children('div:eq(' + ( index - 1 ) + ')').show();
}


/*新增tab页面*/
function createIframe(dom){
    var $ul, $frameBox
    var title = $(dom).data('title') || 'tab页面';
    var href = $(dom).data('href');
    var index = -1;
    if( !dom ){
        throw new Error('没有dom对象！！！');
    }
    if( !href ){
        throw new Error('data-href属性未定义！！！');
    }
    if( window === top ){
        $ul = $('.bs-nav-table ul');
        $frameBox = $('#frame-box');
    }else{
        $ul = $('.bs-nav-table ul',parent.window.document);
        $frameBox = $('#frame-box',parent.window.document);
    }

    $ul.find('li').removeClass('active');
    $frameBox.children('div').hide();

    $ul.find('li').each(function(i,content){
        if( href === $(this).data('url') ){
            index = i;
            return false;
        }
    })

    if( index !== -1 ){
        $ul.find('li').eq(index).addClass('active');
        $frameBox.children('div').eq($ul.find('li').length - 1).fadeIn().siblings('div').hide();
    }else{
        var li = '<li class="active" style="display: none;" data-url="'+ href +'">' +
            ' <span>' + title + '</span>' +
            '<button class="remove"></button>' +
            '</li>';
        $ul.append(li);
        $ul.find('li:last-child').fadeIn();
        var iframe = '<div style="display: none;"><iframe frameborder="0" border="0" width="100%" height="100%" src="'+ href +'"></iframe></div>'
        $frameBox.append(iframe);
        $frameBox.children('div:last-child').fadeIn();
    }

}

