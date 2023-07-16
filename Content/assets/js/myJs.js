/*
$(document).on('click', 'ul li', function () {
    var x = this.children[0].href;
    sessionStorage.setItem('page', x);
});
*/

$(document).ready(function () {
    // Function to handle adding/removing active class
    function handleActiveClass() {
        // Lấy URL hiện tại
        var currentUrl = window.location.href;

        // Xóa lớp 'active' khỏi tất cả các mục điều hướng
        $('#navMenu li').removeClass('active');

        // Lặp qua từng mục điều hướng
        $('#navMenu li').each(function () {
            // Lấy href của mục điều hướng
            var navLink = $(this).find('a').attr('href');

            // Kiểm tra nếu URL hiện tại chứa href của mục điều hướng
            if (currentUrl.indexOf(navLink) !== -1) {
                // Thêm lớp 'active' cho mục điều hướng
                $(this).addClass('active');
            }
        });
    }

    // Gọi hàm handleActiveClass khi trang web được tải lại
    handleActiveClass();

    // Gọi hàm handleActiveClass khi URL thay đổi (sử dụng sự kiện hashchange)
    $(window).on('popstate', function () {
        handleActiveClass();
    });

    // Gọi hàm handleActiveClass khi mục điều hướng được nhấp chuột
    $('#navMenu li').on('click', function () {
        // Xóa lớp 'active' khỏi tất cả các mục điều hướng
        $('#navMenu li').removeClass('active');

        // Thêm lớp 'active' cho mục điều hướng được nhấp chuột
        $(this).addClass('active');
    });
});

$(function () {
    $.ajaxSetup({ cache: false });
    $('body').on('click', '.pagination a', function (event) {
        event.preventDefault();
        var url = $(this).attr('href');
        $('#post-list').load(url);
    });
});

$("#search-form").submit(function (e) {
    e.preventDefault();
    var form = $(this);
    var url = form.attr("action");
    var searchTerm = form.find("#searchTerm").val();
    var searchType = form.find("#searchType").val();
    $.ajax({
        type: "POST",
        url: url,
        data: {
            searchTerm: searchTerm,
            searchType: searchType
        },
        success: function (result) {
            $("#post-list").html(result);
            handleActiveClass();
        },
        error: function () {
            alert("An error occurred while processing your request.");
        }
    });
});
