﻿$(document).ready(function () {
   
    DatHang = function (MaSP) {
        var SL = $('#txtSL').val();
        if (isNaN(SL) == true) {
            SL = 1;
        }
        $.ajax({
            url: "/GioHang/ThemGioHang",
            data: { iMaSP: MaSP, SL: SL },
            type: "POST",
            success: function (result) {
                var TongSL = 0;
                var TongTien = 0;
                $.each(result, function (i, item) {
                    TongSL += item.SoLuong;
                    TongTien += item.TongTien
                });
                $('#cart > a  > span').html(TongSL + " Sản Phẩm - " + TongTien + "VND")
                alert("Thêm Sản Phẩm Thành Công")
            },
        });
        return false;
    };
    CapNhat = (function (MaSP) {
        SL = $("#" + MaSP).val();
        if (isNaN(SL) == true) {
            SL = 1;
        }
        var TongTien1 = 0;
        var TongSL1 = 0
        $.ajax({
            url: "/GioHang/ThemGioHang",
            data: { iMaSP: MaSP, SL: SL },
            type: "POST",
            success: function (result) {
                var item = "";
                $('.Cart-info tbody').empty();
                $.each(result, function (i, item) {
                    var R = "<tr>"
                    + "<td><a href='/ChiTietSP?MaSP=@item.MaSPham'><img src='/Images/laptop/" + item.AnhSP + "' style='width:100px' /></a></td>"
                    + "<td>" + item.TenSP + "</td>"
                    + "<td><input type='text' id='" + item.MaSP + "' class='txtSLGH' value=" + item.SoLuong + " /><a class='icon icon-upload btn-Update' onclick='CapNhat(" + item.MaSP + ")' aria-hidden='true'></a><a  class='icon icon-trash-o btn-Delete' aria-hidden='true' onclick=' XoaSP(" + item.MaSP + ")'></a></td>"
                    + "<td>" + item.GiaSP + "</td>"
                    + "<td>" + item.TongTien + "</td>"
                    + "</tr>"
                    TongTien1 += item.TongTien;
                    TongSL1 += item.SoLuong;
                    $('.Cart-info tbody').append(R);
                });
                $('.TongTien > h2 > span').html(TongTien1)
                $('#cart > a  > span').html(TongSL1 + " Sản Phẩm - " + TongTien1 + "VND")
            },
        });
        return false;
    });
    XoaSP = (function (MaSP) {
        $.ajax({
            url: "/GioHang/XoaSP",
            data: { iMaSP: MaSP },
            type: "POST",
            success: function (result) {
                var item = "";
                var TongTien = 0;
                var TongSL2 = 0;
                $('.Cart-info tbody').empty();
                $.each(result, function (i, item) {
                    var R = "<tr>"
                    + "<td><a href='/DetailTui/ChiTietSP?MaSP=@item.MaSPham'><img src='/images/hinhtui/" + item.AnhSP + "' style='width:100px' /></a></td>"
                    + "<td>" + item.TenSP + "</td>"
                    + "<td><input type='text' id='" + item.MaSP + "' class='txtSLGH' value=" + item.SoLuong + " /><a class='icon icon-upload btn-Update' onclick='CapNhat(" + item.MaSP + ")' aria-hidden='true'></a><a  class='icon icon-trash-o btn-Delete' aria-hidden='true'onclick=' XoaSP(" + item.MaSP + ")'></a></td>"
                    + "<td>" + item.GiaSP + "</td>"
                    + "<td>" + item.TongTien + "</td>"
                    + "</tr>"
                    TongTien += item.TongTien
                    TongSL2 += item.SoLuong
                    $('.Cart-info tbody').append(R);

                });
                $('.TongTien > h2 > span').html(TongTien)
                $('#cart > a  > span').html(TongSL2 + " Sản Phẩm - " + TongTien + "VND")
            },
        });
        return false;
    });
    tai = function () {
        alert("Load lại")
    };
    //$('#thanhtoan').off('click').on('click', function () {
    //    window.location.href = "/thanh-toan";
    //});
});
var SL = 1;
function Tru() {
    SL = document.getElementById("txtSL").value;
    if (isNaN(SL) == true) {
        SL = 1;
    }
    SL--;
    if (SL < 1) {
        SL = 1;
    }
    document.getElementById("txtSL").value = SL;
};
function Cong() {
    SL = document.getElementById("txtSL").value;
    if (isNaN(SL) == true) {
        SL = 0;
    }
    SL++;
    document.getElementById("txtSL").value = SL;
};