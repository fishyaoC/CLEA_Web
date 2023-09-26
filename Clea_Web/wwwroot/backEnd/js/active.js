$(document).ready(function () {
    // 查找所有具有"active"类的菜单项元素并移除该类
    $('.nav-item.active').removeClass('active');
    $('.nav-link.active').removeClass('active');

    // 获取当前页面的URL路径
    var currentUrl = window.location.pathname;

    // 查找菜单项中与当前页面URL匹配的项并添加"active"类
    $('.nav-link[href="' + currentUrl + '"]').closest('.nav-item').addClass('active-highlight');
    $('.nav-link[href="' + currentUrl + '"]').closest('.nav-link').addClass('active');
});